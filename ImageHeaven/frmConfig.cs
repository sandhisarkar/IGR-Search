using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using LItems;
//using AForge.Imaging;
//using AForge;
//using AForge.Imaging.Filters;
//using System.Drawing.Bitmap;
//using System.Drawing.Graphics;
//using Graphics.DrawImage;

namespace ImageHeaven
{
    public partial class frmConfig : Form
    {
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
        INIFile ini = new INIFile();

        public frmConfig()
        {
            InitializeComponent();

            exMailLog.SetNextLogger(exTxtLog);

            find_state_name();
        }
        private void find_state_name()
        {
            if (File.Exists(iniPath) == true)
            {
                string stName = ini.ReadINI("PATHCONF", "PATH", string.Empty, iniPath);

                if (stName.ToString().Trim() == null || stName.ToString().Trim() == "\0")
                {
                    deTextBox1.Text = string.Empty;
                }
                else
                {
                    deTextBox1.Text = stName.ToString();
                }
            }
        }
        private void frmConfig_Load(object sender, EventArgs e)
        {
            find_path_name();
        }
        private void find_path_name()
        {
            if (File.Exists(iniPath) == true)
            {
                string stName = ini.ReadINI("PATHCONF", "PATH", string.Empty, iniPath);

                if (stName.ToString().Trim() == null || stName.ToString().Trim() == "\0")
                {
                    //MessageBox.Show("Pdf path is not set");

                    //this.Close();
                    deTextBox1.Text = string.Empty;
                }
                else
                {
                    deTextBox1.Text = stName.ToString();
                }
            }
        }
        private void deButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButtonSave_Click(object sender, EventArgs e)
        {
            if (deTextBox1.Text == "" || deTextBox1.Text == null || deTextBox1.Text == string.Empty || string.IsNullOrEmpty(deTextBox1.Text))
            {
                MessageBox.Show(this, "You cannot save blank pdf path ", "Error ! ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string stName = deTextBox1.Text.Trim();
                if (File.Exists(iniPath) == true)
                {
                    int i = ini.WriteINI("PATHCONF", "PATH", stName, iniPath);

                    if (i > 0)
                    {
                        MessageBox.Show(this, "Pdf path is ready to use ", "Success !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            DialogResult res = fldDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                deTextBox1.Text = fldDlg.SelectedPath;
                
            }
            else
            {
                deTextBox1.Text = string.Empty;
            }
        }
    }
}
