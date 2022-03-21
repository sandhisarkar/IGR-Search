using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;
using AcroPDFLib;
using AxAcroPDFLib;

namespace ImageHeaven
{
    public partial class frmPDFViewer : Form
    {
        OdbcConnection sqlCon;
        public string pdf_path = string.Empty;
        public string pdf_name = string.Empty;

        public frmPDFViewer()
        {
            InitializeComponent();
        }

        public frmPDFViewer(OdbcConnection prmCon, string prmPath, string prmFile)
        {
            InitializeComponent();
            pdf_path = prmPath;
            pdf_name = prmFile;

        }

        private void frmPDFViewer_Load(object sender, EventArgs e)
        {
            //string excePath = Path.GetDirectoryName(Application.ExecutablePath);
            if(pdf_path != null || pdf_path != "")
            {
                if (File.Exists(pdf_path))
                {
                    string filePdf = pdf_path;
                    string name = pdf_name;
                    axAcroPDF1.src = name;
                    axAcroPDF1.Name = name;
                    axAcroPDF1.LoadFile(pdf_path);
                }
                else
                {
                    MessageBox.Show(this, "Pdf not available...", "Not Found !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            
        }
    }
}
