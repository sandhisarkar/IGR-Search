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
    public partial class SearchForm : Form
    {
        static string root_path = "C:\\Export";
        static string csvFolder = string.Empty, folder_name = string.Empty, csvFolder1 = string.Empty;
        OdbcConnection sqlCon;

        public int checkedCount=0;

        public string district = string.Empty;
        public string ro = string.Empty;
        public string book_type = string.Empty;
        public string deed_year = string.Empty;


        DataTable deedEx = new DataTable();
        DataTable NameEx = new DataTable();
        DataTable PropEx = new DataTable();
        DataTable CSVPropEx = new DataTable();
        DataTable CSVPropEx1 = new DataTable();
        DataTable PlotEx = new DataTable();
        DataTable KhatianEx = new DataTable(); 

        public SearchForm()
        {
            InitializeComponent();
        }
        public SearchForm(OdbcConnection conn)
        {
            InitializeComponent();

            sqlCon = conn;
            
            this.Text = "Search and generate (CSV, PDF)";

            init();
        }


        public void init()
        {
            DataTable deed_dt = new DataTable();
            string str = "select district_code, ro_code, book, deed_year, deed_no, volume_no, page_from, page_to, scan_doc_type, date_of_completion, date_of_delivery, addl_pages, hold, hold_reason, deed_remarks from deed_details";
            OdbcCommand cmd1 = new OdbcCommand(str, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            DataTable dt = new DataTable();
            sqlAdap.Fill(deed_dt);


            if (deed_dt.Rows.Count > 0)
            {
                dgvbatch.DataSource = deed_dt;
                dgvbatch.ColumnHeadersVisible = true;
                dgvbatch.Columns[0].Width = 25;
            }
            else
            {
                dgvbatch.DataSource = null;
                dgvbatch.ColumnHeadersVisible = false;
                
            }
        }

        
        
        public AutoCompleteStringCollection GetSuggestions(string tblName, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct " + fldName + " from " + tblName;
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }

        public AutoCompleteStringCollection GetSuggestions1(string tblName1,string tblName2, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct a." + fldName + " from " + tblName1 + " a, "+tblName2+" b where a.district_code = b.district_code ";
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }

        public AutoCompleteStringCollection GetSuggestions2(string tblName1, string tblName2, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct a." + fldName + " from " + tblName1 + " a, " + tblName2 + " b where a.district_code = b.district_code";
            DataSet ds = new DataSet();
            OdbcCommand cmd = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter odap = new OdbcDataAdapter(cmd);
            odap.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    x.Add(ds.Tables[0].Rows[i][0].ToString().Trim());
                }
            }
            //x.Add("Others");
            //x.Add("NA");
            return x;
        }
        private void SearchForm_Load(object sender, EventArgs e)
        {

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions1("district", "deed_details", "district_name");
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox2.AutoCompleteCustomSource = GetSuggestions2("ro_master", "deed_details", "ro_name");
            this.deTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;


            this.deTextBox3.AutoCompleteCustomSource = GetSuggestions("deed_details", "book");
            this.deTextBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox4.AutoCompleteCustomSource = GetSuggestions("deed_details", "deed_year");
            this.deTextBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox5.AutoCompleteCustomSource = GetSuggestions("deed_details", "deed_no");
            this.deTextBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox5.AutoCompleteSource = AutoCompleteSource.CustomSource;

            
            
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            //dgvbatch.DataSource = null;
            
            string search_str = "select district_code, ro_code, book, deed_year, deed_no, volume_no, page_from, page_to, scan_doc_type, date_of_completion, date_of_delivery, addl_pages, hold, hold_reason from deed_details where ";
            if (deTextBox1.Text.Trim() == "" && deTextBox2.Text.Trim() == "" && deTextBox3.Text.Trim() == "" && deTextBox4.Text.Trim() == "" && deTextBox5.Text.Trim() == "")
            {
                //MessageBox.Show(this, "No search field is entered");
                dgvbatch.DataSource = null;
                dgvbatch.Columns[0].Visible = false;
                return;
            }
            else
            {
               
                if (!(deTextBox1.Text.Trim() == ""))
                {
                    search_str = search_str + "district_code = (select dis_code from district_master where dis_name = '" + deTextBox1.Text + "')  and ";

                }
                if (!(deTextBox2.Text.Trim() == ""))
                {
                    
                    search_str = search_str + " Ro_code = (select ro_code from ro_master where ro_name = '" + deTextBox2.Text + "') and ";

                }
                if (!(deTextBox3.Text.Trim() == ""))
                {
                    search_str = search_str + " book = '" + deTextBox3.Text + "' and ";

                }
                if (!(deTextBox4.Text.Trim() == ""))
                {
                    search_str = search_str + " deed_year = '" + deTextBox4.Text + "' and ";

                }
                if (!(deTextBox5.Text.Trim() == ""))
                {
                    search_str = search_str + " deed_no = '" + deTextBox5.Text + "' and ";
                }

            }
            search_str = search_str.Substring(0, search_str.Length - 4);
            OdbcCommand cmd1 = new OdbcCommand(search_str, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            DataTable dt = new DataTable();
            sqlAdap.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                //MessageBox.Show(this, "No data found", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvbatch.DataSource = null;
                dgvbatch.ColumnHeadersVisible = false;
            }
            else
            {
                dgvbatch.DataSource = dt;
                dgvbatch.ColumnHeadersVisible = true;
                dgvbatch.Columns[0].Visible = true;
               // dgvbatch.Columns[0].Width = 25;
            }

        }

        private void SearchForm_KeyUp(object sender, KeyEventArgs e)
        {
            //refresh
            if(e.KeyCode == Keys.F5)
            {
                init();
            }

            //form close
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void deCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (deCheckBox1.Checked == true)
            {
                deCheckBox2.Checked = false;
                if (dgvbatch.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = false;
                    }
                }
                
            }
        }

        private void deCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (deCheckBox2.Checked == true)
            {
                deCheckBox1.Checked = false;
                if (dgvbatch.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = false;
                    }
                }
                else
                {
                    for (int i = 0; i < dgvbatch.Rows.Count; i++)
                    {
                        dgvbatch.Rows[i].Cells[0].Value = true;
                    }
                }
            }
        }

        private void dgvbatch_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                checkedCount = 0;
                for (int i = 0; i < dgvbatch.Rows.Count; i++)
                {
                       if(Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value) == true)
                       {
                           checkedCount++;
                       }
                }
                if(checkedCount > 0)
                {
                    cmsDeeds.Show(Cursor.Position);
                }
                //if (dgvbatch.Rows.FocusedItem.Bounds.Contains(e.Location) == true)
                //{
                //    cmsDeeds.Show(Cursor.Position);
                //}
            } 
        }
        public void WriteToCSV(DataTable dt, string path, string exception)
        {
           
            //StringBuilder sw = new StringBuilder();
            string csv = string.Empty;
            if (new FileInfo(path).Length == 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    csv += dt.Columns[i].ToString() + ',';
                    
                    //sw.Append(dt.Columns[i].ToString());
                    //sw.Append(",");
                }
                csv += "Exception";
                csv += "\r\n";
                //sw.Append("Exception");
                //sw.Append(Environment.NewLine);
            }
            

            foreach (DataRow dr in dt.Rows)
            {
                for(int j = 0; j<dt.Columns.Count; j++)
                {
                    csv += dr[j].ToString() + ',';
                    //sw.Append(dr[j].ToString());
                    //sw.Append(",");
                }
            }
            csv += exception;
            //sw.Append(exception);
            //sw.Append(Environment.NewLine);
            //if (new FileInfo(path).Length == 0)
            //    File.WriteAllText(path, sw.ToString());
            //else
            //    File.AppendAllText(path, sw.ToString());
            
           

        }
        public void GetDeed(string no)
        {

            //if (checkDuplicate() == true)
            //{
            //    folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
            //    csvFolder = root_path + "\\" + folder_name + "\\csv";
            //    if (!Directory.Exists(root_path + "\\" + folder_name))
            //    {
            //        Directory.CreateDirectory(root_path + "\\" + folder_name);
            //        Directory.CreateDirectory(csvFolder);
            //    }
            //}
            //folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
            //csvFolder = root_path + "\\" + folder_name + "\\csv";
            //if(!Directory.Exists(csvFolder))
            //    Directory.CreateDirectory(csvFolder);
            string deed_csv = csvFolder + "\\" + folder_name + "_Deed_Details.csv";
            if (!File.Exists(deed_csv))
                File.Create(deed_csv);

            DataTable deed_dt = new DataTable();
            string sql = "select district_code, ro_code, book, deed_year, deed_no, serial_no, serial_year, tran_maj_code, tran_min_code, volume_no, page_from, page_to, date_of_completion, date_of_delivery, deed_remarks, scan_doc_type, hold" 
                        +" from deed_details where district_code = '" + district + "' and ro_code = '" + ro + "' and book = '" + book_type + "' and deed_year = '" + deed_year + "' and deed_no = '" + no + "'";
            OdbcCommand cmd1 = new OdbcCommand(sql, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            sqlAdap.Fill(deed_dt);

            DataTable ex_dt = new DataTable();
            string ex_sql = "select exception from deed_details_exception where district_code = '" + district + "' and ro_code = '" + ro + "' and book = '" + book_type + "' and deed_year = '" + deed_year + "' and deed_no = '" + no + "'";
            OdbcCommand cmd2 = new OdbcCommand(ex_sql, sqlCon);
            OdbcDataAdapter sqlAdap1 = new OdbcDataAdapter(cmd2);
            sqlAdap1.Fill(ex_dt);
            string exception = string.Empty;
            if (ex_dt.Rows.Count == 1)
            {
                exception = ex_dt.Rows[0][0].ToString();
            }
            if (ex_dt.Rows.Count > 1)
            {
                for (int k = 0; k < ex_dt.Rows.Count; k++)
                {
                    exception = exception + ex_dt.Rows[k][0].ToString();
                    if ((ex_dt.Rows[k][0].ToString() != ""))
                        exception = exception + ";";

                }
                exception = exception.Substring(0, exception.Length - 1);
            }
            WriteToCSV(deed_dt, deed_csv, exception);
        }


        bool checkDuplicate()
        {
            int checkfilecount = 0;

            bool retVal = true;

            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {

                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value) == true)
                {
                    if (checkfilecount == 0)
                    {
                        district = dgvbatch.Rows[i].Cells[1].Value.ToString();
                        ro = dgvbatch.Rows[i].Cells[2].Value.ToString();
                        book_type = dgvbatch.Rows[i].Cells[3].Value.ToString();
                        deed_year = dgvbatch.Rows[i].Cells[4].Value.ToString();
                    }
                    else
                    {
                        if (district != dgvbatch.Rows[i].Cells[1].Value.ToString())
                        {
                            MessageBox.Show("District for All Records must me unique" + dgvbatch.Rows[i].Cells[1].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            retVal =  false;
                            break;
                        }
                        if (ro != dgvbatch.Rows[i].Cells[2].Value.ToString())
                        {
                            MessageBox.Show("RO for All Records must me unique " + dgvbatch.Rows[i].Cells[2].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            retVal = false;
                            break;
                        }
                        if (book_type != dgvbatch.Rows[i].Cells[3].Value.ToString())
                        {
                            MessageBox.Show("Book Type for All Records must me unique,Remove Folder: " + dgvbatch.Rows[i].Cells[4].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            retVal = false;
                            break;
                        }
                        if (deed_year != dgvbatch.Rows[i].Cells[4].Value.ToString())
                        {
                            MessageBox.Show("Year must me unique " + dgvbatch.Rows[i].Cells[4].Value.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            retVal = false;
                            break;
                        }
                    }
                    checkfilecount++;
                }

                
            }

            //folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
            //csvFolder = root_path + "\\" + folder_name + "\\csv";
            //if (!Directory.Exists(csvFolder))
            //    Directory.CreateDirectory(csvFolder);
            return retVal;
        }
        public DataTable GetAllDeedEX(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            DataSet ds = new DataSet();
            string exception = null;
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,a.Serial_No,a.Serial_Year,a.tran_maj_code,a.tran_min_code,a.Volume_No,a.Page_From,a.Page_To,a.Date_of_Completion,a.Date_of_Delivery,replace(replace(replace(a.Deed_Remarks,'\t',''),'\n',''),'\r','') as Deed_Remarks,a.Scan_doc_type,a.hold as Exception from deed_details a,deed_details_exception b where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "'  and a.deed_no = '" + deed_no + "' and a.district_code = b.district_code and a.Ro_code = b.ro_code and a.book = b.book and a.deed_year =b.deed_year and a.deed_no = b.deed_no";
            sqlStr = "select District_Code,RO_Code,Book,Deed_year,Deed_no,Serial_No,Serial_Year,tran_maj_code,tran_min_code,Volume_No,Page_From,Page_To,Date_of_Completion,Date_of_Delivery,replace(replace(replace(Deed_Remarks,'\t',''),'\n',''),'\r','') as Deed_Remarks,Scan_doc_type,hold from deed_details where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);

                sqlStr = "select exception from deed_details_exception where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        exception = exception + ds.Tables[0].Rows[j][0].ToString() + ";";
                    }
                }
                else
                {
                    exception = "";
                }
                dsImage.Tables[0].Columns.Add("Exception_Type");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    dsImage.Tables[0].Rows[i]["Exception_Type"] = exception.TrimEnd(';');
                }
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
              
                
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();

            return dsImage.Tables[0];
        }
        public DataTable GetAllNameEX1(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            DataSet ds = new DataSet();
            string exception = null;
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;
            //
            //sqlStr = "select a.District_Code,a.ro_code,a.book,a.deed_year,a.deed_no,a.item_no,a.initial_name,a.first_name,a.last_name,a.party_code,a.admit_code,replace(replace(replace(replace(a.Address,'\t',''),'\n',''),'\r',''),'\"','') as Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,a.occupation_code,a.religion_code,a.more,a.pin,a.city,a.other_party_code,a.linked_to,b.exception from index_of_name a,index_of_name_exception b where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' and a.district_code = b.district_code and a.Ro_code = b.Ro_code and a.book = b.book and a.deed_year = b.deed_year and a.deed_no = b.deed_no";
            sqlStr = "select District_Code,ro_code,book,deed_year,deed_no,item_no,initial_name,first_name,last_name,party_code,admit_code,replace(replace(replace(replace(Address,'\t',''),'\n',''),'\r',''),'\"','') as Address,Address_district_code,Address_district_name,Address_ps_code,Address_ps_name,Father_mother,Rel_code,Relation,occupation_code,religion_code,more,pin,city,other_party_code,linked_to from index_of_name where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,b.district_name,c.ro_name,a.Item_no,a.initial_name,a.First_name,a.Last_name,D.EC_NAME,a.Admit_code,a.Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,e.occupation_name,f.religion_name from index_of_name a,district b, ro_master c,party_CODE d,occupation e,religion f where a.District_Code = b.district_code  and a.ro_code = c.ro_code  and a.party_code = d.ec_code  and a.occupation_code = e.occupation_code and a.religion_code = f.religion_code and a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
                string itm_no = dsImage.Tables[0].Rows[0][5].ToString();
                sqlStr = "select exception from index_of_name_exception where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "' and item_no = '" + itm_no + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        exception = exception + ds.Tables[0].Rows[j][0].ToString() + ";";
                    }
                }
                else
                {
                    exception = "";
                }
                dsImage.Tables[0].Columns.Add("Exception_Type");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    dsImage.Tables[0].Rows[i]["Exception_Type"] = exception.TrimEnd(';');
                }
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
              
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }
        public DataTable GetcsvAllPropEX1(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            string newdbConStr = sqlCon.ConnectionString;
            //newdbConStr = newdbConStr.Replace("3.51", "5.1");
            newdbConStr = newdbConStr.Replace("root;", "root;PASSWORD=root;");
            OdbcConnection newdbCon = new OdbcConnection(newdbConStr);
            newdbCon.Open();
            DataSet dsImage = new DataSet();
            string exception = null;
            DataSet ds = new DataSet();

            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,a.Item_no,a.Property_district_code,trim(b.district_name) as district_name,a.Property_ro_code, " +
                     "a.ps_code,trim(d.ps_name) as ps_name,a.moucode,trim(e.eng_mouname) as mouja,a.Area_type,a.GP_Muni_Corp_Code,a.GP_Muni_Corp_Code as GP_Muni_Name,a.Ward,a.Holding,a.Premises,a.road_code," +
                     " a.Plot_code_type,a.Road,a.Plot_No,a.Bata_No,a.Khatian_type,a.khatian_No,a.bata_khatian_no," +
                     " a.property_type,a.Land_Area_acre,a.Land_Area_bigha,a.Land_Area_decimal," +
                     " a.Land_Area_katha,a.Land_Area_chatak,a.Land_Area_sqfeet," +
                     " a.Structure_area_in_sqFeet,a.ref_ps,a.ref_mouza," +
                     " a.jl_no,a.other_plots,a.other_khatian,a.land_type,a.refjl_no from" +
                     " index_of_property a left outer join district b on a.Property_district_code = b.district_code" +
                     " left outer join ro_master c on a.Property_district_code = c.district_code and a.Property_ro_code = c.ro_code" +
                     " left outer join ps d on a.Property_district_code = d.district_code and a.ps_code = d.ps_code" +
                     " left outer join moucode e on a.Property_district_code = e.district_code and a.ps_code = e.ps_code and a.moucode = e.moucode" +
                     " where a.district_code = '" + Do_code + "' and a.ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' ";

            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, newdbCon);
                sqlAdap.Fill(dsImage);
                string itm_no = dsImage.Tables[0].Rows[0][5].ToString();
                if (dsImage.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                    {
                        if (dsImage.Tables[0].Rows[i][13].ToString() != "")
                        {
                            dsImage.Tables[0].Rows[i][15] = getmuniCorpValue(dsImage.Tables[0].Rows[i][13].ToString(), dsImage.Tables[0].Rows[i][14].ToString(), dsImage.Tables[0].Rows[i][6].ToString(), dsImage.Tables[0].Rows[i][9].ToString()).Tables[0].Rows[0][0].ToString();
                        }
                    }
                }
                sqlStr = "select exception from index_of_property_exception where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "' and item_no = '" + itm_no + "'";
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        exception = exception + ds.Tables[0].Rows[j][0].ToString() + ";";

                    }
                }
                else
                {
                    exception = "";
                }
                dsImage.Tables[0].Columns.Add("Exception_Type");
                for (int i = 0; i < dsImage.Tables[0].Rows.Count; i++)
                {
                    dsImage.Tables[0].Rows[i]["Exception_Type"] = exception.TrimEnd(';');
                }
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
               
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }
        public DataTable GetAlloutsideWBDeedEX(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            sqlStr = "select district_code,ro_code,book,deed_year,deed_no,item_no,property_country_code,property_state_code,Property_district_code,thana,moucode,Plot_code_type,Plot_No,Khatian_type,khatian_No,land_use,property_type,area_acre,local_body_type,other_details,area_bigha,area_decimal,area_katha,area_chatak,area_sqf,area_sqfeet,total_area_decimal,struct_sqfeet from index_of_property_out_wb  where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            // sqlStr = "select * from index_of_property_out_wb a where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "'";
            // sqlStr = "select District_Code,RO_Code,Book,Deed_year,Deed_no,Serial_No,Serial_Year,tran_maj_code,tran_min_code,Volume_No,Page_From,Page_To,Date_of_Completion,Date_of_Delivery,replace(replace(replace(Deed_Remarks,'\t',''),'\n',''),'\r','') as Deed_Remarks,Scan_doc_type,hold as Exception from deed_details where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();

            return dsImage.Tables[0];
        }
        public DataSet getmuniCorpValue(string areaType, string gpCode, string disCode, string psCode)
        {
            DataSet ds = new DataSet();
            string sqlStr = string.Empty;
            OdbcDataAdapter sqlAdap = null;
            try
            {
                if (areaType == "M" || areaType == "C")
                {
                    sqlStr = "select trim(municipality_name) as municipality_name from municipality where district_code = '" + disCode + "' and municipality_code ='" + gpCode + "'";
                }
                else if (areaType == "G")
                {
                    sqlStr = "select trim(gp_desc) as gp_desc from gram_panchayat where district_code = '" + disCode + "' and ps_code = '" + psCode + "' and gp_code ='" + gpCode + "'";
                }

                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(ds);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
               
            }
            return ds;
        }
        public DataTable GetAllOtherKhatian(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select * from tbl_other_khatian where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            sqlStr = "select distinct a.* from tbl_other_khatian a,index_of_property b where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' and a.district_code = b.district_code and a.ro_code = b.ro_code and a.book = b.book and a.deed_year = b.deed_year and a.deed_no = b.deed_no and b.property_type = 'FL'";
            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,b.district_name,c.ro_name,a.Item_no,a.initial_name,a.First_name,a.Last_name,D.EC_NAME,a.Admit_code,a.Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,e.occupation_name,f.religion_name from index_of_name a,district b, ro_master c,party_CODE d,occupation e,religion f where a.District_Code = b.district_code  and a.ro_code = c.ro_code  and a.party_code = d.ec_code  and a.occupation_code = e.occupation_code and a.religion_code = f.religion_code and a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
                
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }
        public DataTable GetAllOtherPlot(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            // sqlStr = "select * from tblother_plots where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and deed_year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            sqlStr = "select distinct a.* from tblother_plots a,index_of_property b where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "' and a.district_code = b.district_code and a.ro_code = b.ro_code and a.book = b.book and a.deed_year = b.deed_year and a.deed_no = b.deed_no and b.property_type = 'FL'";
            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,b.district_name,c.ro_name,a.Item_no,a.initial_name,a.First_name,a.Last_name,D.EC_NAME,a.Admit_code,a.Address,a.Address_district_code,a.Address_district_name,a.Address_ps_code,a.Address_ps_name,a.Father_mother,a.Rel_code,a.Relation,e.occupation_name,f.religion_name from index_of_name a,district b, ro_master c,party_CODE d,occupation e,religion f where a.District_Code = b.district_code  and a.ro_code = c.ro_code  and a.party_code = d.ec_code  and a.occupation_code = e.occupation_code and a.religion_code = f.religion_code and a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "' and a.deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
               
            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();
            return dsImage.Tables[0];
        }
        public DataTable GetAllPdfs(string Do_code, string RO_Code, string year, string deed_year, string deed_no)
        {
            string sqlStr = null;
            DataSet dsImage = new DataSet();
            DataSet ds = new DataSet();
            string exception = null;
            OdbcDataAdapter sqlAdap = null;
            string indexPageName = string.Empty;

            //sqlStr = "select a.District_Code,a.RO_Code,a.Book,a.Deed_year,a.Deed_no,a.Serial_No,a.Serial_Year,a.tran_maj_code,a.tran_min_code,a.Volume_No,a.Page_From,a.Page_To,a.Date_of_Completion,a.Date_of_Delivery,replace(replace(replace(a.Deed_Remarks,'\t',''),'\n',''),'\r','') as Deed_Remarks,a.Scan_doc_type,a.hold as Exception from deed_details a,deed_details_exception b where a.district_code = '" + Do_code + "' and a.Ro_code = '" + RO_Code + "' and a.book = '" + year + "' and a.deed_year = '" + deed_year + "'  and a.deed_no = '" + deed_no + "' and a.district_code = b.district_code and a.Ro_code = b.ro_code and a.book = b.book and a.deed_year =b.deed_year and a.deed_no = b.deed_no";
            sqlStr = "select pdf_path,pdf_name from tbl_img where district_code = '" + Do_code + "' and Ro_code = '" + RO_Code + "' and book = '" + year + "' and year = '" + deed_year + "' and deed_no = '" + deed_no + "'";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dsImage);

                
                if (dsImage.Tables[0].Rows.Count > 0)
                {
                    
                }
                else
                {
                    
                }
                
            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();


            }
            //DataRow dr = dsImage.Tables[0].Rows[0];
            //dsImage.Dispose();

            return dsImage.Tables[0];
        }

        public void DeedCSV()
        {
            DataTable Deed_dt = new DataTable();

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {
                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);

                        DataTable dt = GetAllDeedEX(District, Ro, Book, Year, Deed_No);

                        if (deedEx.Rows.Count < 1)
                        {

                            deedEx = dt.Clone();
                        }

                        foreach (DataRow dr in dt.Select())
                        {
                            deedEx.ImportRow(dr);
                        }
                        
                    }
                }

            
            
           
        }
        public void PersonCSV()
        {
            DataTable person_dt = new DataTable();

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {

                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);
                        DataTable dt1 = GetAllNameEX1(District, Ro, Book, Year, Deed_No);

                        if (NameEx.Rows.Count < 1)
                        {
                            NameEx = dt1.Clone();
                        }

                        foreach (DataRow dr1 in dt1.Select())
                        {
                            NameEx.ImportRow(dr1);
                        }

                        
                    }
                }
            
            

        }
        public void PropertyCSV()
        {
            DataTable property_dt = new DataTable();

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {

                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);
                        DataTable csvdt1 = GetcsvAllPropEX1(District, Ro, Book, Year, Deed_No);
                        if (CSVPropEx1.Rows.Count < 1)
                        {
                            CSVPropEx1 = csvdt1.Clone();
                        }

                        foreach (DataRow dr3 in csvdt1.Select())
                        {
                            CSVPropEx1.ImportRow(dr3);
                        }
                        
                    }
                }
            
            
                
        }
        public void PropertyOutWbCSV()
        {
            DataTable propertyOutWb_dt = new DataTable();
                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {

                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);

                        DataTable csvdt = GetAlloutsideWBDeedEX(District, Ro, Book, Year, Deed_No);
                        if (CSVPropEx.Rows.Count < 1)
                        {
                            CSVPropEx = csvdt.Clone();
                        }

                        foreach (DataRow dr3 in csvdt.Select())
                        {
                            CSVPropEx.ImportRow(dr3);
                        }
                    }
                }
            
           
        }
        public void OtherKhatian()
        {
            DataTable OtherKhatian_dt = new DataTable();
                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {
                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);
                        DataTable dtKhatian = GetAllOtherKhatian(District, Ro, Book, Year, Deed_No);
                        if (KhatianEx.Rows.Count < 1)
                        {
                            KhatianEx = dtKhatian.Clone();
                        }

                        foreach (DataRow dr in dtKhatian.Select())
                        {
                            KhatianEx.ImportRow(dr);
                        }
                    }
                }
           
            
        }
        public void OtherPlots()
        {
            DataTable OtherPlots_dt = new DataTable();
                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {
                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);
                        DataTable dtPlot = GetAllOtherPlot(District, Ro, Book, Year, Deed_No);
                        if (PlotEx.Rows.Count < 1)
                        {
                            PlotEx = dtPlot.Clone();
                        }

                        foreach (DataRow dr in dtPlot.Select())
                        {
                            PlotEx.ImportRow(dr);
                        }
                    }
                }
            
           
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            System.Data.DataTable csvData = new System.Data.DataTable();
            try
            {
                using (Microsoft.VisualBasic.FileIO.TextFieldParser csvReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        System.Data.DataColumn datecolumn = new System.Data.DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return csvData;
        }
                
        private void updateDeedToolStripMenuItem_Click(object sender, EventArgs e)      //downloadCSV
        {
            //if (checkDuplicate() == true)
            //{

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (!Directory.Exists(root_path))
                {
                    Directory.CreateDirectory(root_path);
                }


                //all datatable reset
                deedEx = new DataTable();
                NameEx = new DataTable();
                PropEx = new DataTable();
                CSVPropEx = new DataTable();
                CSVPropEx1 = new DataTable();
                PlotEx = new DataTable();
                KhatianEx = new DataTable();

                //gridview reset
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;
                dataGridView4.DataSource = null;
                dataGridView5.DataSource = null;
                dataGridView6.DataSource = null;


                //datatable rows add
                DeedCSV();
                PersonCSV();
                PropertyCSV();
                OtherPlots();
                OtherKhatian();
                PropertyOutWbCSV();



                //folder check and create
                //folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
                folder_name = "L" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                csvFolder = root_path + "\\" + folder_name + "\\csv";
                if (!Directory.Exists(root_path + "\\" + folder_name))
                {
                    Directory.CreateDirectory(root_path + "\\" + folder_name);
                    Directory.CreateDirectory(csvFolder);
                }
                else
                {
                    Directory.Delete(root_path + "\\" + folder_name, true);
                    Directory.CreateDirectory(root_path + "\\" + folder_name);
                    if (!Directory.Exists(csvFolder))
                    {
                        Directory.CreateDirectory(csvFolder);
                    }
                }

                //gridview data store
                dataGridView1.DataSource = deedEx;
                dataGridView2.DataSource = NameEx;
                dataGridView3.DataSource = CSVPropEx1;
                dataGridView5.DataSource = PlotEx;
                dataGridView6.DataSource = KhatianEx;
                dataGridView4.DataSource = CSVPropEx;

                //csv generate
                tabTextFile(dataGridView1, csvFolder + "\\" + folder_name + "_deed_details.csv");
                tabTextFile(dataGridView2, csvFolder + "\\" + folder_name + "_index_of_name.csv");
                tabTextFile(dataGridView3, csvFolder + "\\" + folder_name + "_index_of_property.csv");
                tabTextFile(dataGridView5, csvFolder + "\\" + folder_name + "_other_plots.csv");
                tabTextFile(dataGridView6, csvFolder + "\\" + folder_name + "_other_khatian.csv");
                tabTextFile(dataGridView4, csvFolder + "\\" + folder_name + "_index_of_property_out_wb.csv");


                MessageBox.Show(this, "All CSVs Donwloaded Successfully...", "Confirm !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    return;
            //}
             
             

             
        }


        public void tabTextFile(DataGridView dg, string filename)
        {

            DataSet ds = new DataSet();
            DataTable dtSource = null;
            DataTable dt = new DataTable();
            DataRow dr;
            if (dg.DataSource != null)
            {
                if (dg.DataSource.GetType() == typeof(DataSet))
                {
                    DataSet dsSource = (DataSet)dg.DataSource;
                    if (dsSource.Tables.Count > 0)
                    {
                        string strTables = string.Empty;
                        foreach (DataTable dt1 in dsSource.Tables)
                        {
                            strTables += TableToString(dt1);
                            strTables += "\r\n\r\n";
                        }
                        if (strTables != string.Empty)
                            SaveDataGridData(strTables, filename);
                    }
                }
                else
                {
                    if (dg.DataSource.GetType() == typeof(DataTable))
                        dtSource = (DataTable)dg.DataSource;
                    if (dtSource != null)

                        SaveDataGridData(TableToString(dtSource), filename);
                }
            }

        }
        private void SaveDataGridData(string strData, string strFileName)
        {
            FileStream fs;
            TextWriter tw = null;
            try
            {
                if (File.Exists(strFileName))
                {
                    fs = new FileStream(strFileName, FileMode.Open);
                }
                else
                {
                    fs = new FileStream(strFileName, FileMode.Create);
                }
                tw = new StreamWriter(fs);
                tw.Write(strData);
            }
            finally
            {
                if (tw != null)
                {
                    tw.Flush();
                    tw.Close();
                }
            }
        }
        private string TableToString(DataTable dt)
        {
            string strData = string.Empty;
            string sep = string.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    if (c.DataType != typeof(System.Guid) &&
                    c.DataType != typeof(System.Byte[]))
                    {
                        strData += sep + c.ColumnName;
                        sep = ",";
                    }
                }
                strData += "\r\n";
                foreach (DataRow r in dt.Rows)
                {
                    sep = string.Empty;
                    foreach (DataColumn c in dt.Columns)
                    {
                        if (c.DataType != typeof(System.Guid) &&
                        c.DataType != typeof(System.Byte[]))
                        {
                            if (!Convert.IsDBNull(r[c.ColumnName]))

                                strData += sep +
                                '"' + r[c.ColumnName].ToString().Replace("\n", " ").Replace(",", "-") + '"';

                            else

                                strData += sep + "";
                            sep = ",";

                        }
                    }
                    strData += "\r\n";

                }
            }
            else
            {
                //strData += "\r\n---> Table was empty!";
                foreach (DataColumn c in dt.Columns)
                {
                    if (c.DataType != typeof(System.Guid) &&
                    c.DataType != typeof(System.Byte[]))
                    {
                        strData += sep + c.ColumnName;
                        sep = ",";
                    }
                }
                strData += "\r\n";
            }
            return strData;
        }
        private void dgvbatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgvbatch.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvbatch_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgvbatch.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvbatch_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                checkedCount = 0;
                for (int i = 0; i < dgvbatch.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value) == true)
                    {
                        checkedCount++;
                    }
                }
                if (checkedCount > 0)
                {
                    cmsDeeds.Show(Cursor.Position);
                }
                //if (dgvbatch.Rows.FocusedItem.Bounds.Contains(e.Location) == true)
                //{
                //    cmsDeeds.Show(Cursor.Position);
                //}
            } 
        }

        private void dgvbatch_DoubleClick(object sender, EventArgs e)
        {
            if(dgvbatch.Rows.Count > 0)
            {
                if(dgvbatch.SelectedRows.Count > 0)
                {
                    string District = dgvbatch.SelectedRows[0].Cells[1].Value.ToString();
                    string Ro = dgvbatch.SelectedRows[0].Cells[2].Value.ToString();
                    string Book = dgvbatch.SelectedRows[0].Cells[3].Value.ToString();
                    string Year = dgvbatch.SelectedRows[0].Cells[4].Value.ToString();
                    string Deed_No = dgvbatch.SelectedRows[0].Cells[5].Value.ToString();
                    // GetDeed(Deed_No);

                    if (GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows.Count > 0)
                    {
                        Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        string pdf_path = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][0].ToString();
                        string pdf_name = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][1].ToString();

                        if (File.Exists(pdf_path))
                        {
                            if(pdf_path != null || pdf_path != "")
                            {
                                frmPDFViewer frm = new frmPDFViewer(sqlCon, pdf_path, pdf_name);
                                frm.ShowDialog(this);
                            }
                            else
                            {
                                MessageBox.Show(this, "Pdf not available...", "Not Found !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Pdf not available...", "Not Found !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(this,"Pdf not available...","Not Found !",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;

                    }
                }
            }
        }

        private void deleteDeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            DataTable Deed_dt = new DataTable();
            //if (checkDuplicate() == true)
            //{
                //folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
                folder_name = "L" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                csvFolder = root_path + "\\" + folder_name + "\\Images";
            
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (!Directory.Exists(root_path + "\\" + folder_name))
                {
                    Directory.CreateDirectory(root_path + "\\" + folder_name);
                    Directory.CreateDirectory(csvFolder);
                }
                else
                {
                    Directory.Delete(root_path + "\\" + folder_name, true);
                    Directory.CreateDirectory(root_path + "\\" + folder_name);
                    if (!Directory.Exists(csvFolder))
                    {
                        Directory.CreateDirectory(csvFolder);
                    }
                }

                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {
                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);

                        if(GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows.Count > 0)
                        {
                            Application.DoEvents();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            string pdf_path = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][0].ToString();
                            string pdf_name = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][1].ToString();

                            if (File.Exists(pdf_path))
                            {
                                if (!File.Exists(csvFolder + "\\" + pdf_name))
                                {
                                    File.Copy(pdf_path, csvFolder + "\\" + pdf_name, true);
                                }
                            }
                            else
                            {
                                //if (!File.Exists(root_path + "\\" + folder_name + "\\log.txt"))
                                //{
                                //    File.Create(root_path + "\\" + folder_name + "\\log.txt");
                                //}
                                sb.Append(DateTime.Now.ToString() + "  -----File Not Found-----  " + "Dis-" + District + " ,Ro-" + Ro + " ,Book- " + Book + " ,Year- " + Year + " ,Deed No - " + Deed_No);
                                sb.AppendLine();
                            }
                        }
                        else
                        {
                            //if (!File.Exists(root_path + "\\" + folder_name + "\\log.txt"))
                            //{
                            //    File.Create(root_path + "\\" + folder_name + "\\log.txt");
                            //}

                            sb.Append(DateTime.Now.ToString() + "  -----File Not Found-----  " + "Dis-"+District+" ,Ro-"+Ro+" ,Book- "+Book+" ,Year- "+Year+" ,Deed No - "+Deed_No);
                            sb.AppendLine();

                        }


                    }
                    
                }

                sb.AppendLine();
                sb.Append("-----------------------------------------------------------------------------------------------");
                sb.AppendLine();
                var DataToBeSave = sb.ToString();
                File.AppendAllText(root_path + "\\" + folder_name + "\\log.txt", DataToBeSave + Environment.NewLine);
                sb.Clear();

                MessageBox.Show(this, "All PDFs Donwloaded Successfully...", "Confirm !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //else
                //{

                //    return;
                //}
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

                Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //if (checkDuplicate() == true)
                //{
                //folder_name = "L" + district + ro + book_type + deed_year + DateTime.Now.ToString("ddMMyyyy");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                folder_name = "L" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                csvFolder = root_path + "\\" + folder_name + "\\Images";
                csvFolder1 = root_path + "\\" + folder_name + "\\csv";
                if (!Directory.Exists(root_path + "\\" + folder_name))
                {
                    Directory.CreateDirectory(root_path + "\\" + folder_name);
                    Directory.CreateDirectory(csvFolder);
                    Directory.CreateDirectory(csvFolder1);
                }
                else
                {
                    Directory.Delete(root_path + "\\" + folder_name, true);
                    Directory.CreateDirectory(root_path + "\\" + folder_name);

                    if (!Directory.Exists(csvFolder))
                    {
                        Directory.CreateDirectory(csvFolder);
                    }
                    if (!Directory.Exists(csvFolder1))
                    {
                        Directory.CreateDirectory(csvFolder1);
                    }
                }


                //all datatable reset
                deedEx = new DataTable();
                NameEx = new DataTable();
                PropEx = new DataTable();
                CSVPropEx = new DataTable();
                CSVPropEx1 = new DataTable();
                PlotEx = new DataTable();
                KhatianEx = new DataTable();

                //gridview reset
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;
                dataGridView4.DataSource = null;
                dataGridView5.DataSource = null;
                dataGridView6.DataSource = null;


                //datatable rows add
                DeedCSV();
                PersonCSV();
                PropertyCSV();
                OtherPlots();
                OtherKhatian();
                PropertyOutWbCSV();



                //folder check and create
                

                //gridview data store
                dataGridView1.DataSource = deedEx;
                dataGridView2.DataSource = NameEx;
                dataGridView3.DataSource = CSVPropEx1;
                dataGridView5.DataSource = PlotEx;
                dataGridView6.DataSource = KhatianEx;
                dataGridView4.DataSource = CSVPropEx;

                //csv generate
                tabTextFile(dataGridView1, csvFolder1 + "\\" + folder_name + "_deed_details.csv");
                tabTextFile(dataGridView2, csvFolder1 + "\\" + folder_name + "_index_of_name.csv");
                tabTextFile(dataGridView3, csvFolder1 + "\\" + folder_name + "_index_of_property.csv");
                tabTextFile(dataGridView5, csvFolder1 + "\\" + folder_name + "_other_plots.csv");
                tabTextFile(dataGridView6, csvFolder1 + "\\" + folder_name + "_other_khatian.csv");
                tabTextFile(dataGridView4, csvFolder1 + "\\" + folder_name + "_index_of_property_out_wb.csv");


                for (int k = 0; k < dgvbatch.Rows.Count; k++)
                {
                    Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (Convert.ToBoolean(dgvbatch.Rows[k].Cells[0].Value) == true)
                    {
                        string District = dgvbatch.Rows[k].Cells[1].Value.ToString();
                        string Ro = dgvbatch.Rows[k].Cells[2].Value.ToString();
                        string Book = dgvbatch.Rows[k].Cells[3].Value.ToString();
                        string Year = dgvbatch.Rows[k].Cells[4].Value.ToString();
                        string Deed_No = dgvbatch.Rows[k].Cells[5].Value.ToString();
                        // GetDeed(Deed_No);

                        if(GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows.Count > 0)
                        {
                            string pdf_path = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][0].ToString();
                            string pdf_name = GetAllPdfs(District, Ro, Book, Year, Deed_No).Rows[0][1].ToString();

                            if (File.Exists(pdf_path))
                            {
                                if (!File.Exists(csvFolder + "\\" + pdf_name))
                                {
                                    File.Copy(pdf_path, csvFolder + "\\" + pdf_name, true);
                                }
                            }
                            else
                            {
                                //if (!File.Exists(root_path + "\\" + folder_name + "\\log.txt"))
                                //{
                                //    File.Create(root_path + "\\" + folder_name + "\\log.txt");
                                //}
                                sb.Append(DateTime.Now.ToString() + "  -----File Not Found-----  " + "Dis-" + District + " ,Ro-" + Ro + " ,Book- " + Book + " ,Year- " + Year + " ,Deed No - " + Deed_No);
                                sb.AppendLine();
                            }
                        }
                        else
                        {
                            //if (!File.Exists(root_path + "\\" + folder_name + "\\log.txt"))
                            //{
                            //    File.Create(root_path + "\\" + folder_name + "\\log.txt");
                            //}
                            sb.Append(DateTime.Now.ToString() + "  -----File Not Found-----  " + "Dis-" + District + " ,Ro-" + Ro + " ,Book- " + Book + " ,Year- " + Year + " ,Deed No - " + Deed_No);
                            sb.AppendLine();
                        }

                    }

                   
                }


                sb.AppendLine();
                sb.Append("-----------------------------------------------------------------------------------------------");
                sb.AppendLine();
                var DataToBeSave = sb.ToString();
                File.AppendAllText(root_path + "\\" + folder_name + "\\log.txt", DataToBeSave + Environment.NewLine);
                sb.Clear();

                MessageBox.Show(this,"CSVs and PDFs are downloaded successfully","Confirm !",MessageBoxButtons.OK,MessageBoxIcon.Information);

            //}
            //else
            //{
            //    return;
            //}
        }
       
    }
}
