using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClosedXML.Excel;
using Microsoft;
using Microsoft.Office;
using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data.Odbc;

namespace ImageHeaven
{
    public partial class frmRequest : Form
    {
        OdbcConnection sqlCon;
        public frmRequest()
        {
            InitializeComponent();
        }
        public frmRequest(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void deButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";

            DialogResult dlg = openFileDialog.ShowDialog();

            if (dlg == DialogResult.OK)
            {
                deTextBox1.Text = openFileDialog.FileName;
                deButton1.Enabled = false;
                dataGridView1.DataSource = null;
                deLabel3.Text = string.Empty;
                deLabel2.Text = string.Empty;

                System.Data.DataTable dt = new System.Data.DataTable();

                System.Windows.Forms.Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (deTextBox1.Text != null)
                {
                    try
                    {
                        //Create Object for Microsoft.Office.Interop.Excel that will be use to read excel file

                        Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                        Microsoft.Office.Interop.Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(deTextBox1.Text);
                        int count = excelWorkbook.Sheets.Count;
                        bool check = false;
                        //for (int k = 1; k <= count; k++)
                        //{

                        Microsoft.Office.Interop.Excel._Worksheet excelWorksheet = excelWorkbook.Sheets[1];

                        Microsoft.Office.Interop.Excel.Range excelRange = excelWorksheet.UsedRange;

                        int rowCount = excelRange.Rows.Count; //get row count of excel data

                        int colCount = excelRange.Columns.Count; // get column count of excel data
                        bool retVal = false;
                        //Get the first Column of excel file which is the Column Name
                        //  dt.Columns.Add("Serial No");
                        if(colCount == 5)
                        {
                            if(excelRange.Cells[1,1].Value2.ToString() == "Sl no" && excelRange.Cells[1, 2].Value2.ToString() == "District" && excelRange.Cells[1, 3].Value2.ToString() == "Office Name" && excelRange.Cells[1, 4].Value2.ToString() == "Deed Year" && excelRange.Cells[1, 5].Value2.ToString() == "Deed No")
                            {
                                retVal = true;
                            }
                            else
                            { retVal = false; }
                        }
                        else
                        {
                            retVal = false;
                        }
                        if(retVal == true)
                        {
                            for (int i = 1; i <= rowCount; i++)
                            {
                                for (int j = 2; j <= colCount; j++)
                                {
                                    dt.Columns.Add(excelRange.Cells[i, j].Value2.ToString());
                                }
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please provide proper excel file...");
                            deButton1.Enabled = true;
                            return;
                        }

                        System.Windows.Forms.Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        //Get Row Data of Excel
                        int rowCounter; //This variable is used for row index number
                        for (int i = 2; i <= rowCount; i++) //Loop for available row of excel data
                        {
                            System.Windows.Forms.Application.DoEvents();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            DataRow row = dt.NewRow(); //assign new row to DataTable
                            rowCounter = 0;
                            for (int j = 2; j <= colCount; j++) //Loop for available column of excel data
                            {
                                //check if cell is empty
                                if (excelRange.Cells[i, j] != null && excelRange.Cells[i, j].Value2 != null)
                                {
                                    if (j == colCount)
                                    {
                                        string str = Convert.ToString(excelRange.Cells[i, j].Value2);
                                        row[rowCounter] = str.PadLeft(5, '0');
                                    }
                                    else
                                        row[rowCounter] = excelRange.Cells[i, j].Value2.ToString();
                                }
                                else
                                {
                                    row[i] = "";
                                }
                                rowCounter++;
                            }
                            dt.Rows.Add(row); //add row to DataTable
                        }

                        dataGridView1.DataSource = dt;
                        //  }//assign DataTable as Datasource for DataGridview
                        if(dataGridView1.Rows.Count > 0)
                        {
                            deButton2.Enabled = true;
                        }

                        deLabel2.Text = "Total Records Found : " +dataGridView1.Rows.Count;

                        //close and clean excel process
                        System.Windows.Forms.Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                       
                        excelWorkbook.Close();
                        
                        excelApp.Quit();
                      
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    deTextBox1.Text = string.Empty;
                    return;
                }
            }
        }

        private void frmRequest_Load(object sender, EventArgs e)
        {
            deTextBox1.Text = string.Empty;
            deButton1.Enabled = true;
            deLabel2.Text = string.Empty;
            deLabel3.Text = string.Empty;
            dataGridView1.DataSource = null;
            deButton2.Enabled = false;
        }

        private void deButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public System.Data.DataTable getMaxReq()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string max_req = "Select MAX(req_id) from tbl_request";
            OdbcDataAdapter sqlAdap = new OdbcDataAdapter(max_req, sqlCon);
            sqlAdap.Fill(dt);
            return dt;
        }

        private void deButton2_Click(object sender, EventArgs e)
        {
            //import button

            try
            {
                System.Windows.Forms.Application.DoEvents();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                deButton2.Enabled = false;
                string req = getMaxReq().Rows[0][0].ToString();

                int req_id = 0;
                if(req == null || req == string.Empty)
                {

                    req_id = 1;
                }
                else
                {
                    req_id = Convert.ToInt32(getMaxReq().Rows[0][0].ToString())+ 1;
                }
                //if (getMaxReq().Rows[0][0].ToString() == string.Empty)
                //    req_id = 1;
                //else
                //    req_id += 1;
                if (dataGridView1.Rows.Count > 0)
                {
                    int item = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        string insert_str = "insert into tbl_request(req_id, district_name, office_name, deed_year, deed_no) values('"+req_id+"', '"+dataGridView1.Rows[i].Cells[0].Value.ToString()+ "', '" + dataGridView1.Rows[i].Cells[1].Value.ToString() + "', '" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "', '" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "')";
                        OdbcCommand cmd1 = new OdbcCommand(insert_str, sqlCon);
                        OdbcDataReader myreader = cmd1.ExecuteReader();
                        myreader.Close();
                        item++;
                    }

                    deLabel3.Text = "Inserted Records : " + item;
                    MessageBox.Show(this, "Records submitted successfully for request id "+req_id,"",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    deButton2.Enabled = true;
                    deButton1.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }

        }
    }
}
