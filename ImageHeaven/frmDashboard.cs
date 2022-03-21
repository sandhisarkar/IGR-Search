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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using Microsoft;
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;



namespace ImageHeaven
{
    public partial class frmDashboard : Form
    {

        OdbcConnection sqlCon;
        public int checkedCount = 0;


        public frmDashboard()
        {
            InitializeComponent();
        }

        public frmDashboard(OdbcConnection prmCon)
        {
            InitializeComponent();

            sqlCon = prmCon;
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

        public AutoCompleteStringCollection GetSuggestions1(string tblName1, string tblName2, string fldName)
        {
            AutoCompleteStringCollection x = new AutoCompleteStringCollection();
            string sql = "Select distinct a." + fldName + " from " + tblName1 + " a, " + tblName2 + " b where a.district_code = b.district_code ";
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

        private void frmDashboard_Load(object sender, EventArgs e)
        {

            dgvbatch.DataSource = null;

            this.deTextBox1.AutoCompleteCustomSource = GetSuggestions1("district", "deed_details", "district_name");
            this.deTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            this.deTextBox2.AutoCompleteCustomSource = GetSuggestions2("ro_master", "deed_details", "ro_name");
            this.deTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;


            this.deTextBox3.AutoCompleteCustomSource = GetSuggestions("deed_details", "book");
            this.deTextBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.deTextBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;


            //init();
        }


        public System.Data.DataTable getDeedCount(string dis, string ro, string book, string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            OdbcDataAdapter sqlAdap = null;
            string sqlStr = "select Count(*) from deed_details where district_code = '" + dis + "' and Ro_code = '" + ro + "' and book = '" + book + "' and deed_year = '" + year + "' ";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dt);


                if (dt.Rows.Count > 0)
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

            return dt;
        }

        public int getImageCount(string dis, string ro, string book, string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            OdbcDataAdapter sqlAdap = null;

            int imgCou = 0;

            string sqlStr = "select pdf_path from tbl_img where district_code = '" + dis + "' and Ro_code = '" + ro + "' and book = '" + book + "' and year = '" + year + "' ";
            try
            {
                sqlAdap = new OdbcDataAdapter(sqlStr, sqlCon);
                sqlAdap.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string filePath = dt.Rows[i][0].ToString();
                        if (File.Exists(filePath))
                        {
                            PdfReader pdfReader = new PdfReader(filePath);
                            int numberOfPages = pdfReader.NumberOfPages;
                            imgCou = imgCou + numberOfPages;
                        }
                       
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                sqlAdap.Dispose();
            }

            return imgCou;
        }

        public void init()
        {
            string search_str = "select distinct district_code as 'District Code',ro_code 'RO Code',book as 'Book Type',deed_year as 'Deed Year' from deed_details ";
            //if (deTextBox1.Text.Trim() == "" && deTextBox2.Text.Trim() == "" && deTextBox3.Text.Trim() == "")
            //{
            //    //MessageBox.Show(this, "No search field is entered");
            //    dgvbatch.DataSource = null;
            //    dgvbatch.Columns[0].Visible = false;
            //    return;
            //}
            //else
            //{
            //    if (!(deTextBox1.Text.Trim() == ""))
            //    {
            //        search_str = search_str + "district_code = (select dis_code from district_master where dis_name = '" + deTextBox1.Text + "')  and ";

            //    }
            //    if (!(deTextBox2.Text.Trim() == ""))
            //    {

            //        search_str = search_str + " Ro_code = (select ro_code from ro_master where ro_name = '" + deTextBox2.Text + "') and ";

            //    }
            //    if (!(deTextBox3.Text.Trim() == ""))
            //    {
            //        search_str = search_str + " book = '" + deTextBox3.Text + "' and ";

            //    }
            //}
            //search_str = search_str.Substring(0, search_str.Length - 4);
            OdbcCommand cmd1 = new OdbcCommand(search_str, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            System.Data.DataTable dt = new System.Data.DataTable();
            sqlAdap.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                //MessageBox.Show(this, "No data found", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvbatch.DataSource = null;
                dgvbatch.ColumnHeadersVisible = false;
            }
            else
            {
                //dt... deed and image count
                dt.Columns.Add("Deed Count");
                dt.Columns.Add("Image Count");


                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string docode = dt.Rows[i][0].ToString();
                    string ro = dt.Rows[i][1].ToString();
                    string book = dt.Rows[i][2].ToString();
                    string year = dt.Rows[i][3].ToString();
                    
                    dt.Rows[i]["Deed Count"] = getDeedCount(docode, ro, book, year).Rows[0][0].ToString();
                    dt.Rows[i]["Image Count"] = Convert.ToString(getImageCount(docode, ro, book, year).ToString());

                }

                dgvbatch.DataSource = dt;
                dgvbatch.ColumnHeadersVisible = true;
                dgvbatch.Columns[0].Visible = true;
                dgvbatch.Columns[0].Width = 25;
            }
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            string search_str = "select distinct district_code as 'District Code',ro_code 'RO Code',book as 'Book Type',deed_year 'Deed Year' from deed_details where ";
            if (deTextBox1.Text.Trim() == "" && deTextBox2.Text.Trim() == "" && deTextBox3.Text.Trim() == "")
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
                    search_str = search_str + "district_code = (select district_code from district where district_name = '" + deTextBox1.Text + "')  and ";

                }
                if (!(deTextBox2.Text.Trim() == ""))
                {

                    search_str = search_str + " Ro_code = (select ro_code from ro_master where ro_name = '" + deTextBox2.Text + "') and ";

                }
                if (!(deTextBox3.Text.Trim() == ""))
                {
                    search_str = search_str + " book = '" + deTextBox3.Text + "' and ";

                }
            }
            search_str = search_str.Substring(0, search_str.Length - 4);
            OdbcCommand cmd1 = new OdbcCommand(search_str, sqlCon);
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(cmd1);
            System.Data.DataTable dt = new System.Data.DataTable();
            sqlAdap.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                //MessageBox.Show(this, "No data found", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvbatch.DataSource = null;
                dgvbatch.ColumnHeadersVisible = false;
            }
            else
            {

                //dt... deed and image count
                dt.Columns.Add("Deed Count");
                dt.Columns.Add("Image Count");

                System.Windows.Forms.Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    System.Windows.Forms.Application.DoEvents();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    string docode = dt.Rows[i][0].ToString();
                    string ro = dt.Rows[i][1].ToString();
                    string book = dt.Rows[i][2].ToString();
                    string year = dt.Rows[i][3].ToString();

                    dt.Rows[i]["Deed Count"] = getDeedCount(docode, ro, book, year).Rows[0][0].ToString();
                    dt.Rows[i]["Image Count"] = Convert.ToString(getImageCount(docode, ro, book, year).ToString());

                }

                dgvbatch.DataSource = dt;
                dgvbatch.ColumnHeadersVisible = true;
                dgvbatch.Columns[0].Visible = true;
                dgvbatch.Columns[0].Width = 25;
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

        private void dgvbatch_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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

        private void dgvbatch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dgvbatch.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void updateDeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("District Code");
            dt.Columns.Add("RO Code");
            dt.Columns.Add("Book Type");
            dt.Columns.Add("Deed Year");
            dt.Columns.Add("Deed Count");
            dt.Columns.Add("Image Count");

            int check = 0;

            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {

                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value) == true)
                {
                    string docode = dgvbatch.Rows[i].Cells[1].Value.ToString();
                    string ro = dgvbatch.Rows[i].Cells[2].Value.ToString();
                    string book = dgvbatch.Rows[i].Cells[3].Value.ToString();
                    string year = dgvbatch.Rows[i].Cells[4].Value.ToString();
                    string deedCou = dgvbatch.Rows[i].Cells[5].Value.ToString();
                    string imgCou = dgvbatch.Rows[i].Cells[6].Value.ToString();

                    dt.Rows.Add(docode, ro, book, year, deedCou, imgCou);

                    check++;
                }

            }

            if (check > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                dataGridView1.DataSource = null;
            }

            if(dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                //app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Deed Report";

                worksheet.Cells[1, 3] = "Deed Details Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "District Name : " + deTextBox1.Text.Trim();
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Registry Office Name : " + deTextBox2.Text.Trim();
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[5, 1] = "Book : " + deTextBox3.Text.Trim();
                Range range53 = worksheet.get_Range("A5");
                range53.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A5");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("A7", "F7");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("A7", "F7");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[7, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 8, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 8, j + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "Deed_Details_Report" + ".xls";
                string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                sfdUAT.Filter = "Xls files (*.xls)|*.xls";
                sfdUAT.FilterIndex = 2;
                sfdUAT.RestoreDirectory = true;
                sfdUAT.FileName = namexls;
                sfdUAT.ShowDialog();

                workbook.SaveAs(sfdUAT.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("District Code");
            dt.Columns.Add("RO Code");
            dt.Columns.Add("Book Type");
            dt.Columns.Add("Deed Year");
            dt.Columns.Add("Deed Count");
            dt.Columns.Add("Image Count");

            int check = 0;

            for (int i = 0; i < dgvbatch.Rows.Count; i++)
            {

                if (Convert.ToBoolean(dgvbatch.Rows[i].Cells[0].Value) == true)
                {
                    string docode = dgvbatch.Rows[i].Cells[1].Value.ToString();
                    string ro = dgvbatch.Rows[i].Cells[2].Value.ToString();
                    string book = dgvbatch.Rows[i].Cells[3].Value.ToString();
                    string year = dgvbatch.Rows[i].Cells[4].Value.ToString();
                    string deedCou = dgvbatch.Rows[i].Cells[5].Value.ToString();
                    string imgCou = dgvbatch.Rows[i].Cells[6].Value.ToString();

                    dt.Rows.Add(docode, ro, book, year, deedCou, imgCou);

                    check++;
                }

            }

            if (check > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                dataGridView1.DataSource = null;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                //app.Visible = false;

                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];


                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

                worksheet.Name = "Deed Report";

                worksheet.Cells[1, 3] = "Deed Details Report";
                Range range44 = worksheet.get_Range("C1");
                range44.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.YellowGreen);

                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();


                worksheet.Cells[3, 1] = "District Name : " + deTextBox1.Text.Trim();
                Range range43 = worksheet.get_Range("A3");
                range43.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[4, 1] = "Registry Office Name : " + deTextBox2.Text.Trim();
                Range range33 = worksheet.get_Range("A4");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                worksheet.Cells[5, 1] = "Book : " + deTextBox3.Text.Trim();
                Range range53 = worksheet.get_Range("A5");
                range33.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                Range range = worksheet.get_Range("A3", "A5");
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);


                Range range1 = worksheet.get_Range("A6", "K6");
                range1.Borders.Color = ColorTranslator.ToOle(Color.Black);

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {


                    Range range2 = worksheet.get_Range("A7", "F7");
                    range2.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range2.EntireRow.AutoFit();
                    range2.EntireColumn.AutoFit();
                    worksheet.Cells[6, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        Range range3 = worksheet.Cells;
                        //range3.Borders.Color = ColorTranslator.ToOle(Color.Black);
                        range3.EntireRow.AutoFit();
                        range3.EntireColumn.AutoFit();
                        worksheet.Cells[i + 7, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        worksheet.Cells[i + 7, j + 1].Borders.Color = ColorTranslator.ToOle(Color.Black);

                    }

                }

                string namexls = "Deed_Details_Report" + ".xls";
                string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                sfdUAT.Filter = "Xls files (*.xls)|*.xls";
                sfdUAT.FilterIndex = 2;
                sfdUAT.RestoreDirectory = true;
                sfdUAT.FileName = namexls;
                sfdUAT.ShowDialog();

                workbook.SaveAs(sfdUAT.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
