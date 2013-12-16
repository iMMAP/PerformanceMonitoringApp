using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace SRFROWCA.OPS
{
    public partial class UploadOPSProjects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if file is uploaded and is excel file
                if (!IsValidFile()) return;

                // Empty staging tables before data import.
                //EmptyTable();
                // Fill staging table (TempData1) in db to import data using this table.
                FillStagingTableInDB();

                // Import data from another staging table (TempData).
                //DataTable dt = ImportData();

                // If success or any error occoured in execution of procedure then show message to user.
                //if (dt.Rows.Count > 0)
                {
                    //lblMessage.CssClass = "info-message";
                    //lblMessage.Visible = true;
                    //lblMessage.Text = dt.Rows[0][0].ToString() + "  ---  " + dt.Rows[0][1].ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        private bool IsValidFile()
        {
            if (fuExcel.HasFile)
            {
                string fileExt = Path.GetExtension(fuExcel.PostedFile.FileName);
                if (fileExt != ".xls" && fileExt != ".xlsx")
                {
                    lblmessage.Text = "Pleae use Excel files with 'xls' OR 'xlsx' extentions.";
                    return false;
                }
            }
            else
            {
                lblmessage.Text = "Please select file to upload!";
                return false;
            }

            return true;
        }

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB()
        {
            string filePath = UploadFile();
            string excelConString = GetExcelConString(filePath);

            if (!string.IsNullOrEmpty(excelConString))
            {
                DataTable dt = ReadDataInDataTable(excelConString);                
                WriteDataToDB(dt);
            }
        }

        private string UploadFile()
        {
            string path = fuExcel.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fuExcel.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuExcel.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += DateTime.Now.ToString("MMM-dd-yy-hh-mm-ss") + fileExtension;
            string uploadDir = Server.MapPath(@"~/OPS/OPSProjects");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string uploadFileLocation = uploadDir + "//" + fileName;
            fuExcel.SaveAs(uploadFileLocation);

            return uploadFileLocation;
        }

        private string GetExcelConString(string filePath)
        {
            string con = null;
            string fileExtension = Path.GetExtension(filePath);
            if (fileExtension == ".xls")
            {
                con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else if (fileExtension == ".xlsx")
            {
                con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }
            return con;
        }

        private DataTable ReadDataInDataTable(string excelConString)
        {
            //Create Connection to Excel work book
            OleDbConnection excelCon = new OleDbConnection(excelConString);
            //Create OleDbCommand to fetch data from Excel
            OleDbCommand cmd = new OleDbCommand(@"Select * from [Sheet1$]", excelCon);
            excelCon.Open();

            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = MakeDataTable();
            //DataTable dt = new DataTable();            
            da.Fill(dt);
            da.Dispose();
            excelCon.Close();

            return dt;
        }

        private DataTable MakeDataTable()
        {
            DataColumn idColumn = new DataColumn();
            idColumn.ColumnName = "Id";
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            idColumn.AutoIncrementStep = 1;
            

            DataTable dt = new DataTable();
            dt.Columns.Add(idColumn);
            dt.Columns.Add("Project_ID", typeof(int));
            dt.Columns.Add("Project_Title", typeof(string));
            dt.Columns.Add("Project_Code", typeof(string));
            dt.Columns.Add("Organization_Name", typeof(string));
            dt.Columns.Add("Organisation_Id", typeof(int));
            dt.Columns.Add("Original Request", typeof(decimal));
            dt.Columns.Add("Original A", typeof(string));
            dt.Columns.Add("Original A amt", typeof(decimal));
            dt.Columns.Add("Original B", typeof(string));
            dt.Columns.Add("Original B amt", typeof(decimal));
            dt.Columns.Add("Original C", typeof(string));
            dt.Columns.Add("Original C amt", typeof(decimal));
            dt.Columns.Add("Original D", typeof(string));
            dt.Columns.Add("Original D amt", typeof(decimal));
            dt.Columns.Add("Original E", typeof(string));
            dt.Columns.Add("Original E amt", typeof(decimal));
            dt.Columns.Add("CurrentRequest", typeof(decimal));
            dt.Columns.Add("Current A", typeof(string));
            dt.Columns.Add("Current A amt", typeof(decimal));
            dt.Columns.Add("Current B", typeof(string));
            dt.Columns.Add("Current B amt", typeof(decimal));
            dt.Columns.Add("Current C", typeof(string));
            dt.Columns.Add("Current C amt", typeof(decimal));
            dt.Columns.Add("Current D", typeof(string));
            dt.Columns.Add("Current D amt", typeof(decimal));
            dt.Columns.Add("Current E", typeof(string));
            dt.Columns.Add("Current E amt", typeof(decimal));
            dt.Columns.Add("RunningRequest", typeof(decimal));
            dt.Columns.Add("Running A", typeof(string));
            dt.Columns.Add("Running A amt", typeof(decimal));
            dt.Columns.Add("Running B", typeof(string));
            dt.Columns.Add("Running B amt", typeof(decimal));
            dt.Columns.Add("Running C", typeof(string));
            dt.Columns.Add("Running C amt", typeof(decimal));
            dt.Columns.Add("Running D", typeof(string));
            dt.Columns.Add("Running D amt", typeof(decimal));
            dt.Columns.Add("Running E", typeof(string));
            dt.Columns.Add("Running E amt", typeof(decimal));
            dt.Columns.Add("Cluster", typeof(string));
            dt.Columns.Add("Status_LongDs", typeof(string));
            dt.Columns.Add("Province_Name", typeof(string));
            dt.Columns.Add("Objective_Text", typeof(string));
            dt.Columns.Add("Beneficiary Total Number", typeof(int));
            dt.Columns.Add("Beneficiaries Children", typeof(int));
            dt.Columns.Add("Beneficiaries Women", typeof(int));
            dt.Columns.Add("Beneficiaries Others", typeof(int));
            dt.Columns.Add("Beneficiaries Description", typeof(string));
            dt.Columns.Add("Beneficiaries Total Description", typeof(string));
            dt.Columns.Add("Project Start Date", typeof(DateTime));
            dt.Columns.Add("Project End Date", typeof(DateTime));
            dt.Columns.Add("Project Implementing partner", typeof(string));
            dt.Columns.Add("Needs", typeof(string));
            dt.Columns.Add("Activities", typeof(string));
            dt.Columns.Add("Outcomes", typeof(string));
            dt.Columns.Add("Priority", typeof(string));
            dt.Columns.Add("Project Contact Name", typeof(string));
            dt.Columns.Add("Project ContactEmail", typeof(string));
            dt.Columns.Add("Project ContactPhone", typeof(string));
            dt.Columns.Add("Agency Project Code", typeof(string));
            dt.Columns.Add("Related URL", typeof(string));
            dt.Columns.Add("WorkFlow Comments", typeof(string));
            dt.Columns.Add("ProjectCreated On", typeof(DateTime));
            dt.Columns.Add("ProjectType", typeof(string));
            dt.Columns.Add("GenderMarkerCode", typeof(string));
            dt.Columns.Add("Last Updated On", typeof(DateTime));
            dt.Columns.Add("Last Updated By", typeof(string));
            dt.Columns.Add("Subset of Appeal", typeof(string));

            return dt;
        }

        // Write data into db using SqlBulkCopy
        private void WriteDataToDB(DataTable dt)
        {
            // DB connection string.
            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = "OPSProjectDetails_StagingTable";
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }
    }
}