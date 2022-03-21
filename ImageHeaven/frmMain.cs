using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.wfe;
using NovaNet.Utils;
using System.Data.Odbc;
using System.Data;
using LItems;
using System.Reflection;
using nControls;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Globalization;

namespace ImageHeaven
{
    public partial class frmMain : Form
    {
        static wItem wi;
        //NovaNet.Utils.dbCon dbcon;
        frmMain mainForm;
        OdbcConnection sqlCon = null;
        private Credentials crd = new Credentials();
        static int colorMode;
        dbCon dbcon;
        //
        NovaNet.Utils.GetProfile pData;
        NovaNet.Utils.ChangePassword pCPwd;
        NovaNet.Utils.Profile p;
        public static NovaNet.Utils.IntrRBAC rbc;
        private short logincounter;
        //

        public static string projectName = null;
        public static string batchName = null;
        public static string boxNumber = null;
        public static string projectVal = null;
        public static string batchVal = null;

        public frmMain(OdbcConnection pCon)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //


            InitializeComponent();
            sqlCon = pCon;

            logincounter = 0;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            ImageHeaven.Program.Logout = false;
        }

        public frmMain()
        {
            InitializeComponent();
        }
        void getData(ref NovaNet.Utils.Profile prmp)
        {
            int i;
            p = prmp;
            for (i = 1; i <= 2; i++)
            {
                if (rbc.authenticate(p.UserId, p.Password) == false)
                {
                    if (logincounter == 2)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        logincounter++;
                        GetChallenge ogc = new GetChallenge(getData);
                        ogc.ShowDialog(this);
                    }
                }
                else
                {
                    if (rbc.CheckUserIsLogged(p.UserId))
                    {

                        p = rbc.getProfile();
                        crd = rbc.getCredentials(p);
                        if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                        {
                            rbc.LockedUser(p.UserId, crd.created_dttm);
                        }
                        break;
                    }
                    else
                    {
                        p.UserId = null;
                        p.UserName = null;
                        GetChallenge ogc = new GetChallenge(getData);
                        AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
                        this.Text = "B'Zer" + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;
                        ogc.ShowDialog(this);
                    }
                }
            }
        }
        // Used for password change
        void getCPwd(ref NovaNet.Utils.Profile prmpwd)
        {
            p = prmpwd;
            rbc.changePassword(p.UserId, p.UserName, p.Password);
        }
        // Used for add new user
        void getnwusrData(ref NovaNet.Utils.Profile prmp)
        {
            p = prmp;
            if (rbc.addUser(p.UserId, p.UserName, p.Role_des, p.Password) == false)
            {
                AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
                nwUsr.ShowDialog(this);
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            int k;
            dbcon = new NovaNet.Utils.dbCon();


           // dbcon = new NovaNet.Utils.dbCon();
           // sqlCon.Open();


            if (sqlCon.State == ConnectionState.Open)
            {
                pData = getData;
                pCPwd = getCPwd;
                rbc = new NovaNet.Utils.RBAC(sqlCon, dbcon, pData, pCPwd);
                //string test = sqlCon.Database;
                GetChallenge gc = new GetChallenge(getData);
                gc.ShowDialog(this);
                ///get credential for the logged user
                crd = rbc.getCredentials(p);
               
               
            }

            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "B'Zer" + "           Version: " + assemName.Version.ToString() + "    Database name: " + sqlCon.Database.ToString() + "    Logged in user: " + crd.userName;
           
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = null;
            sqlCon.Close();


            sqlCon.Open();
            logoutToolStripMenuItem.Visible = true;
            logoutToolStripMenuItem.Enabled = true;



            frmMain_Load(sender, e);

        }

        private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImport frm = new frmImport(sqlCon,crd);
            frm.ShowDialog(this);
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm sfrm = new SearchForm(sqlCon);
            sfrm.ShowDialog(this);
        }

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDashboard frm = new frmDashboard(sqlCon);
            frm.ShowDialog(this);
        }

        private void requestSubmitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRequest rfrm = new frmRequest(sqlCon);
            rfrm.ShowDialog(this);
        }

        private void searchRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearchReq frm = new frmSearchReq(sqlCon);
            frm.ShowDialog(this);
        }

        private void pathConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmConfig frm = new frmConfig();
            frm.ShowDialog(this);
        }
    }
}
