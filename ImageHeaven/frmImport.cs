using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using NovaNet.Utils;
using System.IO;

namespace ImageHeaven
{
    public partial class frmImport : Form
    {
        // static int csv_count, pdf_count;

        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);

        string iniPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + "IhConfiguration.ini";
        INIFile ini = new INIFile();

        OdbcConnection sqlCon;

        Credentials crd = new Credentials();
        string csv_path = string.Empty;
        string pdf_path = string.Empty;

        List<string> csvList = new List<string>();
        List<string> pdfList = new List<string>();
        static string db_name, file_name;

        public OdbcDataAdapter adapter = null;

        public static string[] headerLables;

        public static string stName;

        public frmImport()
        {
            InitializeComponent();
        }

        public frmImport(OdbcConnection prmSql, Credentials prmCrd)
        {
     
            InitializeComponent();

            sqlCon = prmSql;

            crd = prmCrd;
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            deButton2.Enabled = false;

            find_path_name();
        }
        private void find_path_name()
        {
            if (File.Exists(iniPath) == true)
            {
                stName = ini.ReadINI("PATHCONF", "PATH", string.Empty, iniPath);

                if (stName.ToString().Trim() == null || stName.ToString().Trim() == "\0")
                {
                    MessageBox.Show("Pdf path is not set");

                    this.Close();

                }
                else
                {
                    deTextBox2.Text = stName.ToString();
                }
            }
        }
        private void deButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            DialogResult res = fldDlg.ShowDialog();
           // fldDlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                csvList.Clear();
                pdfList.Clear();
                listBox1.Items.Clear();
                listBox2.Items.Clear();

                csv_path = fldDlg.SelectedPath + "\\csv";
                pdf_path = fldDlg.SelectedPath + "\\Images";
                if (Directory.Exists(csv_path) && Directory.Exists(pdf_path))
                {
                    string[] get_csv = Directory.GetFiles(csv_path);
                    foreach (string str in get_csv)
                    {
                        if (Path.GetExtension(str).ToLower() == ".csv")
                        {
                            csvList.Add(str);
                            listBox1.Items.Add(Path.GetFileName(str));
                        }

                    }
                    string[] get_pdf = Directory.GetFiles(pdf_path);
                    foreach (string str in get_pdf)
                    {
                        if (Path.GetExtension(str).ToLower() == ".pdf")
                        {
                            pdfList.Add(str);
                            listBox2.Items.Add(Path.GetFileName(str));
                        }

                    }
                    if (csvList.Count > 0 && pdfList.Count > 0)
                    {
                        deTextBox1.Text = fldDlg.SelectedPath;
                        // string directory_name = Path.GetDirectoryName(deTextBox1.Text);
                        deButton2.Enabled = true;
                    }
                    else
                    {
                        deTextBox1.Text = string.Empty;
                        // string directory_name = Path.GetDirectoryName(deTextBox1.Text);
                        deButton2.Enabled = false;
                        csv_path = string.Empty;
                        pdf_path = string.Empty;
                        MessageBox.Show(this, "Please select proper folder", "Selection!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        deButton1.Focus();
                        return;
                    }
                }
                else
                {
                    deTextBox1.Text = string.Empty;
                    // string directory_name = Path.GetDirectoryName(deTextBox1.Text);
                    deButton2.Enabled = false;
                    csv_path = string.Empty;
                    pdf_path = string.Empty;
                    MessageBox.Show(this, "Please select proper folder", "Selection!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    deButton1.Focus();
                    return;
                }
                
            }
        }
        public void LoadData(string path)               //Load Data to Gridview
        {
            try
            {
                DataTable dt = new DataTable();
                string[] lines = File.ReadAllLines(path);

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (lines.Length > 0)
                {
                    string first_line = lines[0];
                    headerLables = first_line.Split(',');
                    foreach (string strheader in headerLables)
                    {
                        dt.Columns.Add(new DataColumn(strheader));
                    }
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] strData = lines[i].Split(',');


                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string strheader in headerLables)
                        {
                            dr[strheader] = strData[columnIndex++].Trim('"');
                            //dr[strheader] = dr[strheader].re

                            Application.DoEvents();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        dt.Rows.Add(dr);
                        dataGridView1.DataSource = dt;

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    dataGridView1.DataSource = dt;
                    dataGridView1.AllowUserToAddRows = false;

                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        public System.Data.DataTable checkDeed(string do_code, string ro, string book, string year, string no)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            string sql = "Select * from deed_details where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql,sqlCon);
           
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public System.Data.DataTable checkDeedException(string do_code, string ro, string book, string year, string no, string ex)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            string sql = "Select * from deed_details_exception where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and exception = '"+ex+"'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }

        public DataTable checkPerson(string do_code, string ro, string book, string year, string no, string item)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from index_of_name where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable checkPersonException(string do_code, string ro, string book, string year, string no, string item, string exc)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from index_of_name_exception where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "' and exception = '"+exc+"'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable check_prop_out_wb(string do_code, string ro, string book, string year, string no, string item)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from index_of_property_out_wb where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable check_other_khatian(string do_code, string ro, string book, string year, string no, string item)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from tbl_other_khatian where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable check_other_plots(string do_code, string ro, string book, string year, string no, string item)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from tblother_plots where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable checkProperty(string do_code, string ro, string book, string year, string no, string item)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from index_of_property where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public DataTable checkPropertyException(string do_code, string ro, string book, string year, string no, string item, string exc)
        {
            DataTable dt = new DataTable();

            string sql = "Select * from index_of_property_exception where District_code = '" + do_code + "' and RO_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' and deed_no = '" + no + "' and item_no = '" + item + "' and exception = '" + exc + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);

            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(dt);

            return dt;
        }
        public string getEx(string ex_Code)
        {
            string get_ex = "select Exception_name from tblexception where exception_code = '" + ex_Code + "'";
            OdbcCommand cmd1 = new OdbcCommand(get_ex, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            DataTable dt = new DataTable();
            sqlAdap.Fill(dt);
            string ex_details = dt.Rows[0][0].ToString();
            return ex_details;

        }
        public DataTable CheckImg(string do_code, string ro, string book, string year, string no)
        {
            DataTable dt = new DataTable();
            string sql_img = "select * from tbl_img where district_code = '" + do_code + "' and  ro_code = '" + ro + "' and  book = '" + book + "' and year = '"+year+"' and deed_no = '" + no + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql_img, sqlCon);
            OdbcDataAdapter sqladap = new OdbcDataAdapter(cmd1);
            sqladap.Fill(dt);
            return dt;

        }

        public DataTable CheckLot(string lotno)
        {
            DataTable dt = new DataTable();
            string sql_img = "select * from tbl_lot where media_no = '" + lotno + "' ";
            OdbcCommand cmd1 = new OdbcCommand(sql_img, sqlCon);
            OdbcDataAdapter sqladap = new OdbcDataAdapter(cmd1);
            sqladap.Fill(dt);
            return dt;
        }
 
        private void deButton2_Click(object sender, EventArgs e)
        {
            deButton2.Enabled = false;

            Application.DoEvents();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            string media_name = Path.GetFileName(deTextBox1.Text).ToUpper().Trim();

            if(CheckLot(media_name).Rows.Count > 0)
            {
                MessageBox.Show(this,"This Media Name already exists...","Error !",MessageBoxButtons.OK,MessageBoxIcon.Error);
                deButton1.Enabled = true;
                deButton2.Enabled = true;
                return;
            }


            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                //string csv_path = textBox1.Text + "\\" + listBox1.Items[i].ToString();
                db_name = Path.GetFileNameWithoutExtension(csv_path + "\\" + listBox1.Items[i].ToString());
                int a = db_name.IndexOf('_');
                db_name = db_name.Remove(0, a + 1);

                dataGridView1.DataSource = null;
                //listBox1.SelectedIndex = i;
                LoadData(csv_path + "\\" + listBox1.Items[i].ToString());


                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //   store_data(csv_path);

                //check 1
                if (db_name.ToLower() == "deed_details")
                {
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Please wait while updating deed details");
                    //statusStrip1.ForeColor = Color.Black;
                    

                    //cell value check
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        string serial_no = deed_no;
                        string serial_year = year;
                        string trans_major = dataGridView1.Rows[j].Cells[7].Value.ToString();
                        string trans_minor = dataGridView1.Rows[j].Cells[8].Value.ToString();
                        string vol = dataGridView1.Rows[j].Cells[9].Value.ToString();
                        string pg_from = dataGridView1.Rows[j].Cells[10].Value.ToString();
                        string pg_to = dataGridView1.Rows[j].Cells[11].Value.ToString();
                        string date_c = dataGridView1.Rows[j].Cells[12].Value.ToString();
                        string date_del = dataGridView1.Rows[j].Cells[13].Value.ToString();
                        string remarks = dataGridView1.Rows[j].Cells[14].Value.ToString();
                        string doc_type = dataGridView1.Rows[j].Cells[15].Value.ToString();
                        string hold = string.Empty;
                        string ex_type = string.Empty;

                        if (headerLables.Length == 18)
                        {
                            hold = dataGridView1.Rows[j].Cells[16].Value.ToString();
                            ex_type = dataGridView1.Rows[j].Cells[17].Value.ToString();

                        }
                        else
                        {
                            hold = "N";
                            ex_type = dataGridView1.Rows[j].Cells[16].Value.ToString();


                        }

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (checkDeed(do_code, ro, book, year, deed_no).Rows.Count == 0)
                        {
                            string sql_insert = "insert into deed_details (district_code, ro_code, book, deed_year, deed_no, serial_no, serial_year, tran_maj_code, tran_min_code, volume_no, page_from, page_to, date_of_completion, date_of_delivery, deed_remarks, scan_doc_type, hold, created_by, created_dttm)" 
                                +"values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + serial_no + "', '" + serial_year + "', '" + trans_major + "', '" + trans_minor + "', '" + vol + "', '" + pg_from + "', '" + pg_to + "', '" + date_c + "', '" + date_del + "', '" + remarks + "', '" + doc_type + "', '" + hold + "','"+crd.created_by+"','"+crd.created_dttm+"')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            int k = insert_cmd.ExecuteNonQuery();
                            if (k > 0)
                            {
                                //commitBol = true;
                            }
                            else
                            {
                                //commitBol = false;
                            }
                        }


                        string[] split = ex_type.Split(new string[] { ";" }, StringSplitOptions.None);
                        int srl_no = 0;
                        foreach (string exc in split)
                        {

                            if (exc == null || exc == "")
                            {
                            }
                            else
                            {
                                //statusStrip1.Items.Clear();


                                if (checkDeedException(do_code, ro, book, year, deed_no, exc).Rows.Count == 0)
                                {
                                    string sqlex_insert = "insert into deed_details_exception (district_code, ro_code, book, deed_year, deed_no, srl_no, exception, details) values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + (srl_no++) + "', '" + exc + "', '" + getEx(exc) + "')";
                                    OdbcCommand ex_cmd = new OdbcCommand();
                                    ex_cmd.Connection = sqlCon;
                                    ex_cmd.CommandText = sqlex_insert;
                                    ex_cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        //statusStrip1.Items.Clear();
                        //statusStrip1.Items.Add("Please wait while updating deed details");

                    }
                   
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Deed details updated successfully");
                    //statusStrip1.ForeColor = Color.Black;
                }
                //check 2
                else if (db_name.ToLower() == "index_of_name")
                {
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Please wait while updating person details");
                    //statusStrip1.ForeColor = Color.Black;

                  
                    //cell value check
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {

                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        //string serial_no = deed_no;
                        //string serial_year = year;
                        string item = dataGridView1.Rows[j].Cells[5].Value.ToString();
                        string initial = dataGridView1.Rows[j].Cells[6].Value.ToString();
                        string f_name = dataGridView1.Rows[j].Cells[7].Value.ToString();
                        string l_name = dataGridView1.Rows[j].Cells[8].Value.ToString();
                        string party_code = dataGridView1.Rows[j].Cells[9].Value.ToString();
                        string admit = dataGridView1.Rows[j].Cells[10].Value.ToString();
                        string address = dataGridView1.Rows[j].Cells[11].Value.ToString();
                        string add_dist_code = dataGridView1.Rows[j].Cells[12].Value.ToString();
                        string add_dist_name = dataGridView1.Rows[j].Cells[13].Value.ToString();
                        string add_ps_code = dataGridView1.Rows[j].Cells[14].Value.ToString();
                        string add_ps_name = dataGridView1.Rows[j].Cells[15].Value.ToString();
                        string father_mother = dataGridView1.Rows[j].Cells[16].Value.ToString();
                        string rel_code = dataGridView1.Rows[j].Cells[17].Value.ToString();
                        string rel = dataGridView1.Rows[j].Cells[18].Value.ToString();
                        string occ = dataGridView1.Rows[j].Cells[19].Value.ToString();
                        string relg = dataGridView1.Rows[j].Cells[20].Value.ToString();
                        string more = dataGridView1.Rows[j].Cells[21].Value.ToString();
                        string pin = dataGridView1.Rows[j].Cells[22].Value.ToString();
                        string city = dataGridView1.Rows[j].Cells[23].Value.ToString();
                        string other_p = dataGridView1.Rows[j].Cells[24].Value.ToString();
                        string linked = dataGridView1.Rows[j].Cells[25].Value.ToString();
                        string ex_type = dataGridView1.Rows[j].Cells[26].Value.ToString();

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (checkPerson(do_code, ro, book, year, deed_no, item).Rows.Count == 0)
                        {
                            string sql_insert = "insert into index_of_name (district_code, ro_code, book, deed_year, deed_no, item_no, initial_name, first_name, last_name, party_code, admit_code, address, address_district_code, address_district_name, address_ps_code, address_ps_name, father_mother, rel_code, relation, occupation_code, religion_code, more, pin, city, other_party_code, linked_to,created_by,created_dttm)" 
                                                +"values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '" + initial + "', '" + f_name + "', '" + l_name + "', '" + party_code + "', '" + admit + "', '" + address + "', '" + add_dist_code + "', '" + add_dist_name + "', '" + add_ps_code + "', '" + add_ps_name + "', '" + father_mother + "', '" + rel_code + "', '" + rel + "', '" + occ + "', '" + relg + "', '" + more + "', '" + pin + "', '" + city + "', '" + other_p + "', '" + linked + "','" + crd.created_by + "','" + crd.created_dttm + "')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            int k = insert_cmd.ExecuteNonQuery();
                            if (k > 0)
                            {
                                //commitBol = true;
                            }
                            else
                            {
                                //commitBol = false;
                            }
                        }
                        string[] split = ex_type.Split(new string[] { ";" }, StringSplitOptions.None);
                        int srl_no = 0;

                        foreach (string exc in split)
                        {
                            Application.DoEvents();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            if (exc == null || exc == "")
                            {
                            }
                            else
                            {
                               // statusStrip1.Items.Clear();
                                //statusStrip1.Items.Add("Please wait while updating person details exception");
                                //statusStrip1.ForeColor = Color.Black;
                               
                                if (checkPersonException(do_code, ro, book, year, deed_no, item, exc).Rows.Count == 0)
                                {
                                    string sqlex_insert = "insert into index_of_name_exception (district_code, ro_code, book, deed_year, deed_no, item_no, srl_no, exception, details) values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '"+item+"', '" + (srl_no++) + "', '" + exc + "', '" + getEx(exc) + "')";
                                    OdbcCommand ex_cmd = new OdbcCommand();
                                    ex_cmd.Connection = sqlCon;
                                    ex_cmd.CommandText = sqlex_insert;
                                    ex_cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        //statusStrip1.Items.Clear();
                        //statusStrip1.Items.Add("Please wait while updating person details");
                        //statusStrip1.ForeColor = Color.Black;

                      
                    }
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Person details updated successfully");
                    //statusStrip1.ForeColor = Color.Black;
                 
                }
                else if (db_name.ToLower() == "index_of_property")
                {
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Please wait while updating property details");
                    //statusStrip1.ForeColor = Color.Black;

                  
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        //string serial_no = deed_no;
                        //string serial_year = year;
                        string item = dataGridView1.Rows[j].Cells[5].Value.ToString();
                        string prop_dist_code = dataGridView1.Rows[j].Cells[6].Value.ToString();
                        string prop_ro_code = dataGridView1.Rows[j].Cells[8].Value.ToString();
                        string ps_code = dataGridView1.Rows[j].Cells[9].Value.ToString();
                        string moucode = dataGridView1.Rows[j].Cells[11].Value.ToString();
                        string area_type = dataGridView1.Rows[j].Cells[13].Value.ToString();
                        string gp_muni_corp_code = dataGridView1.Rows[j].Cells[14].Value.ToString();
                        string ward = dataGridView1.Rows[j].Cells[16].Value.ToString();
                        string holding = dataGridView1.Rows[j].Cells[17].Value.ToString();
                        string premises = dataGridView1.Rows[j].Cells[18].Value.ToString();
                        string road_code = dataGridView1.Rows[j].Cells[19].Value.ToString();
                        string plot_code_type = dataGridView1.Rows[j].Cells[20].Value.ToString();
                        string road = dataGridView1.Rows[j].Cells[21].Value.ToString();
                        string plot_no = dataGridView1.Rows[j].Cells[22].Value.ToString();
                        string bata_no = dataGridView1.Rows[j].Cells[23].Value.ToString();
                        string khatian_type = dataGridView1.Rows[j].Cells[24].Value.ToString();
                        string khatian_no = dataGridView1.Rows[j].Cells[25].Value.ToString();
                        string bata_khatian_no = dataGridView1.Rows[j].Cells[26].Value.ToString();
                        string property_type = dataGridView1.Rows[j].Cells[27].Value.ToString();
                        string land_area_Acre = dataGridView1.Rows[j].Cells[28].Value.ToString();
                        string land_area_bigha = dataGridView1.Rows[j].Cells[29].Value.ToString();
                        string land_area_decimal = dataGridView1.Rows[j].Cells[30].Value.ToString();
                        string land_area_katha = dataGridView1.Rows[j].Cells[31].Value.ToString();
                        string land_area_chatak = dataGridView1.Rows[j].Cells[32].Value.ToString();
                        string land_area_sqfeet = dataGridView1.Rows[j].Cells[33].Value.ToString();
                        string structure_area_in_sqfeet = dataGridView1.Rows[j].Cells[34].Value.ToString();
                        string ref_ps = dataGridView1.Rows[j].Cells[35].Value.ToString();
                        string ref_mouza = dataGridView1.Rows[j].Cells[36].Value.ToString();
                        string jl_no = dataGridView1.Rows[j].Cells[37].Value.ToString();
                        string other_plots = dataGridView1.Rows[j].Cells[38].Value.ToString();
                        string other_khatian = dataGridView1.Rows[j].Cells[39].Value.ToString();
                        string land_type = dataGridView1.Rows[j].Cells[40].Value.ToString();
                        string refjl_no = dataGridView1.Rows[j].Cells[41].Value.ToString();
                        string ex_type = dataGridView1.Rows[j].Cells[42].Value.ToString();

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (checkProperty(do_code, ro, book, year, deed_no, item).Rows.Count == 0)
                        {
                            string sql_insert = "insert into index_of_property (district_code, ro_code, book, deed_year, deed_no, item_no, property_district_code, property_ro_code, ps_code, " +
                                                "moucode, area_type, gp_muni_corp_code, ward, holding, premises, road_code, plot_code_type, road, plot_no, bata_no, khatian_type, khatian_no, bata_khatian_no, " +
                                                "property_type, land_area_acre, land_area_bigha, land_area_decimal, land_area_katha, land_area_chatak, land_area_sqfeet, structure_area_in_sqfeet, ref_ps," +
                                                "ref_mouza, jl_no, other_plots, other_khatian, land_type, refjl_no,created_by,created_dttm) " +
                                                 "values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '" + prop_dist_code + "', '" + prop_ro_code + "'" +
                                                 ", '" + ps_code + "', '" + moucode + "', '" + area_type + "', '" + gp_muni_corp_code + "', '" + ward + "', '" + holding + "', '" + premises + "'" +
                                                 ", '" + road_code + "', '" + plot_code_type + "', '" + road + "', '" + plot_no + "', '" + bata_no + "', '" + khatian_type + "', '" + khatian_no + "', '" + bata_khatian_no + "', '" + property_type + "'" +
                                                 ", '" + land_area_Acre + "', '" + land_area_bigha + "', '" + land_area_decimal + "', '" + land_area_katha + "', '" + land_area_chatak + "', '" + land_area_sqfeet + "'" +
                                                 ", '" + structure_area_in_sqfeet + "', '" + ref_ps + "', '" + ref_mouza + "', '" + jl_no + "', '" + other_plots + "', '" + other_khatian + "', '" + land_type + "', '" + refjl_no + "','" + crd.created_by + "','" + crd.created_dttm + "')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            insert_cmd.ExecuteNonQuery();
                             
                        }

                        string[] split = ex_type.Split(new string[] { ";" }, StringSplitOptions.None);
                        int srl_no = 0;

                        foreach (string exc in split)
                        {
                            Application.DoEvents();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            if (exc == null || exc == "")
                            {
                            }
                            else
                            {
                                //statusStrip1.Items.Clear();
                                //statusStrip1.Items.Add("Please wait while updating property details exception");
                                //statusStrip1.ForeColor = Color.Black;
                                if (checkPropertyException(do_code, ro, book, year, deed_no, item, exc).Rows.Count == 0)
                                {
                                    srl_no = srl_no + 1;
                                    string sqlex_insert = "insert into index_of_property_exception (district_code, ro_code, book, deed_year, deed_no, item_no, srl_no, exception, details) values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '" + srl_no + "', '" + exc + "', '" + getEx(exc) + "')";
                                    OdbcCommand ex_cmd = new OdbcCommand();
                                    ex_cmd.Connection = sqlCon;
                                    ex_cmd.CommandText = sqlex_insert;
                                    ex_cmd.ExecuteNonQuery();
                                }
                            }
                        }
                       
                        //statusStrip1.Items.Clear();
                        //statusStrip1.Items.Add("Please wait while updating property details");
                        //statusStrip1.ForeColor = Color.Black;

                    }
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Property details updated successfully");
                    //statusStrip1.ForeColor = Color.Black;
                  
                }
                else if (db_name.ToLower() == "index_of_property_out_wb")
                {
                    ///statusStrip1.Items.Clear();
              
                    //statusStrip1.Items.Add("Please wait while updating out of wb property details");
                    //statusStrip1.ForeColor = Color.Black;
                    //cell value check
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        //string serial_no = deed_no;
                        //string serial_year = year;
                        string item = dataGridView1.Rows[j].Cells[5].Value.ToString();
                        string prop_country_code = dataGridView1.Rows[j].Cells[6].Value.ToString();
                        string prop_state_code = dataGridView1.Rows[j].Cells[7].Value.ToString();
                        string prop_dist_code = dataGridView1.Rows[j].Cells[8].Value.ToString();
                        string thana = dataGridView1.Rows[j].Cells[9].Value.ToString();
                        string moucode = dataGridView1.Rows[j].Cells[10].Value.ToString();
                        string plot_code_type = dataGridView1.Rows[j].Cells[11].Value.ToString();
                        string plot_no = dataGridView1.Rows[j].Cells[12].Value.ToString();
                        string khatian_type = dataGridView1.Rows[j].Cells[13].Value.ToString();
                        string khatian_no = dataGridView1.Rows[j].Cells[14].Value.ToString();
                        string land_use = dataGridView1.Rows[j].Cells[15].Value.ToString();
                        string prop_type = dataGridView1.Rows[j].Cells[16].Value.ToString();
                        string area_acre = dataGridView1.Rows[j].Cells[17].Value.ToString();
                        string local_body_type = dataGridView1.Rows[j].Cells[18].Value.ToString();
                        string other_details = dataGridView1.Rows[j].Cells[19].Value.ToString();
                        string area_bigha = dataGridView1.Rows[j].Cells[20].Value.ToString();
                        string area_decimal = dataGridView1.Rows[j].Cells[21].Value.ToString();
                        string area_katha = dataGridView1.Rows[j].Cells[22].Value.ToString();
                        string area_chatak = dataGridView1.Rows[j].Cells[23].Value.ToString();
                        string area_sqf = dataGridView1.Rows[j].Cells[24].Value.ToString();
                        string area_sqfeet = dataGridView1.Rows[j].Cells[25].Value.ToString();
                        string total_area_decimal = dataGridView1.Rows[j].Cells[26].Value.ToString();
                        string struct_sqfeet = dataGridView1.Rows[j].Cells[27].Value.ToString();

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (check_prop_out_wb(do_code, ro, book, year, deed_no, item).Rows.Count == 0)
                        {
                            string sql_insert = "insert into index_of_property_out_wb (district_code, ro_code, book, deed_year, deed_no, item_no, property_country_code, property_state_code, property_district_code, thana, moucode, plot_code_type, plot_no, khatian_type, khatian_no, land_use, property_type, area_acre, local_body_type, other_details, area_bigha, area_decimal, area_katha, area_chatak, area_sqf,area_sqfeet, total_area_decimal, struct_sqfeet, created_by,created_dttm)" + 
                                                "values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '" + prop_country_code + "', '" + prop_state_code + "', '" + prop_dist_code + "', '" + thana + "', '" + moucode + "', '" + plot_code_type + "', '" + plot_no + "', '" + khatian_type + "', '" + khatian_no + "', '" + land_use + "', '" + prop_type + "', '" + area_acre + "', '" + local_body_type + "', '" + other_details + "', '" + area_bigha + "', '" + area_decimal + "', '" + area_katha + "', '" + area_chatak + "', '" + area_sqf + "','"+ area_sqfeet + "', '" + total_area_decimal + "', '"+struct_sqfeet+"', '" + crd.created_by + "','" + crd.created_dttm + "')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            int k = insert_cmd.ExecuteNonQuery();
                            if (k > 0)
                            {
                                //commitBol = true;
                            }
                            else
                            {
                                //commitBol = false;
                            }
                        }

                    }

                  
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Out of wb property details updated successfully");
                    //statusStrip1.ForeColor = Color.Black;
                }
                else if (db_name.ToLower() == "other_khatian")
                {
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Please wait while updating other khatian");
                    //statusStrip1.ForeColor = Color.Black;
                
                    //cell value check
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        //string serial_no = deed_no;
                        //string serial_year = year;
                        string item = dataGridView1.Rows[j].Cells[5].Value.ToString();
                        string other_khatian = dataGridView1.Rows[j].Cells[6].Value.ToString();

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (check_other_khatian(do_code, ro, book, year, deed_no, item).Rows.Count == 0)
                        {
                            string sql_insert = "insert into tbl_other_khatian  (district_code, ro_code, book, deed_year, deed_no, item_no, other_khatian)" +
                                                "values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '"+other_khatian+"')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            insert_cmd.ExecuteNonQuery();
                            
                        }
                    }
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Other khatian updated successfully");
                    //statusStrip1.ForeColor = Color.Black;

                   
                }

                else if (db_name.ToLower() == "other_plots")
                {
                   // statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Please wait while updating other plots");
                    //statusStrip1.ForeColor = Color.Black;
                   
                    //cell value check
                    for (int j = 0; j < dataGridView1.Rows.Count; j++)
                    {
                        string do_code = dataGridView1.Rows[j].Cells[0].Value.ToString();
                        string ro = dataGridView1.Rows[j].Cells[1].Value.ToString();
                        string book = dataGridView1.Rows[j].Cells[2].Value.ToString();
                        string year = dataGridView1.Rows[j].Cells[3].Value.ToString();
                        string deed_no = dataGridView1.Rows[j].Cells[4].Value.ToString();
                        //string serial_no = deed_no;
                        //string serial_year = year;
                        string item = dataGridView1.Rows[j].Cells[5].Value.ToString();
                        string other_plots = dataGridView1.Rows[j].Cells[6].Value.ToString();

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (check_other_plots(do_code, ro, book, year, deed_no, item).Rows.Count == 0)
                        {
                            string sql_insert = "insert into tblother_plots  (district_code, ro_code, book, deed_year, deed_no, item_no, other_plots)" +
                                                "values('" + do_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + item + "', '" + other_plots + "')";
                            OdbcCommand insert_cmd = new OdbcCommand();
                            insert_cmd.Connection = sqlCon;
                            //sqlCmd.Transaction = trans;
                            insert_cmd.CommandText = sql_insert;
                            insert_cmd.ExecuteNonQuery();

                        }
                    }
                   
                    //statusStrip1.Items.Clear();
                    //statusStrip1.Items.Add("Other plots updated successfully");
                    //statusStrip1.ForeColor = Color.Black;
                }


                dataGridView1.DataSource = null;
                
                
            }

            Application.DoEvents();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            string copy_path = deTextBox2.Text.Trim();
            if (!Directory.Exists(copy_path))
            {
                Directory.CreateDirectory(copy_path);
            }

            string lot_name = Path.GetFileName(deTextBox1.Text);
            if (!Directory.Exists(copy_path + "\\" + lot_name))
            {
                Directory.CreateDirectory(copy_path + "\\" + lot_name);
               
            }

            if (!Directory.Exists(copy_path + "\\" + lot_name + "\\Images"))
            {
                Directory.CreateDirectory(copy_path + "\\" + lot_name + "\\Images");
            }

              
            for (int j = 0; j < pdfList.Count; j++)
            {

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                string pdf_name = Path.GetFileNameWithoutExtension(pdfList[j].ToString());
                string dist_code = pdf_name.Substring(0,2);
                string ro = pdf_name.Substring(2, 2);
                string book = pdf_name.Substring(4, 1);
                string year = pdf_name.Substring(5, 4);
                string deed_no = pdf_name.Substring(9, pdf_name.Length-9);

                string dest_path = copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString());
                dest_path = dest_path.Replace("\\", "\\\\");

                if (CheckImg(dist_code, ro, book, year, deed_no).Rows.Count == 0)
                {
                    //string dest_path = copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString());
                    //dest_path = dest_path.Replace("\\", "\\\\");
                    string insert_img = "insert into tbl_img (District_code, ro_code, book, year, deed_no, pdf_path, pdf_name) values('" + dist_code + "', '" + ro + "', '" + book + "', '" + year + "', '" + deed_no + "', '" + dest_path + "', '" + pdf_name+".pdf" + "')";
                    OdbcCommand insert_cmd = new OdbcCommand();
                    insert_cmd.Connection = sqlCon;
                    //sqlCmd.Transaction = trans;
                    insert_cmd.CommandText = insert_img;
                    insert_cmd.ExecuteNonQuery();

                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (!File.Exists(copy_path + "\\" + lot_name + "\\Images"+"\\"+Path.GetFileName(pdfList[j].ToString())))
                    {
                        File.Copy(pdfList[j].ToString(), copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString()),true);
                    }


                }
                else
                {
                    string pdfPath = CheckImg(dist_code, ro, book, year, deed_no).Rows[0][5].ToString();
                    string pdfName = CheckImg(dist_code, ro, book, year, deed_no).Rows[0][6].ToString();

                    if (pdfPath == "" && pdfName == "")
                    {

                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (!File.Exists(copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString())))
                        {
                            File.Copy(pdfList[j].ToString(), copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString()), true);
                        }


                        //update
                        string update_str = "update tbl_img set pdf_path = '" + dest_path + "',"+
                            "pdf_name = '" + pdf_name +".pdf" + "' where District_code = '" + dist_code + "' and ro_code = '"+ro+"' and book '"+book+"' and year = '"+year+"' and deed_no = '"+deed_no+"'";

                        OdbcCommand insert_cmd = new OdbcCommand();
                        insert_cmd.Connection = sqlCon;
                        //sqlCmd.Transaction = trans;
                        insert_cmd.CommandText = update_str;
                        insert_cmd.ExecuteNonQuery();



                    }
                    else
                    {
                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        if (!File.Exists(pdfPath))
                        {
                            File.Copy(pdfList[j].ToString(), copy_path + "\\" + lot_name + "\\Images" + "\\" + Path.GetFileName(pdfList[j].ToString()), true);
                        }
                    }

                }

            }

            insertLot(media_name);

            MessageBox.Show(this, "Data imported successfully", "B`Zer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            deButton2.Enabled = true;
        }

        public void insertLot(string lotno)
        {
            string insert_lot = "insert into tbl_lot (media_no) values('" + lotno + "')";
            OdbcCommand insert_cmd = new OdbcCommand();
            insert_cmd.Connection = sqlCon;
            //sqlCmd.Transaction = trans;
            insert_cmd.CommandText = insert_lot;
            insert_cmd.ExecuteNonQuery();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (csv_path != "")
            {
                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                dataGridView1.DataSource = null;
                LoadData(csv_path + "\\" + listBox1.SelectedItem.ToString());
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
