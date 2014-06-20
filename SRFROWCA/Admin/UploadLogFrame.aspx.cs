using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SRFROWCA.Common;
using System.IO;
using System.Web.Security;
using BusinessLogic;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

namespace SRFROWCA.Admin
{
    public partial class UploadLogFrame : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateLocationEmergencies();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            //try
            //{
                // Check if file is uploaded and is excel file
                if (!IsValidFile()) return;

            //    // Empty staging tables before data import.
            //    EmptyTable();
            //    // Fill staging table (TempData1) in db to import data using this table.
            //    FillStagingTableInDB();

            //    // Import data from another staging table (TempData).
            //    DataTable dt = ImportData();

            //    // If success or any error occoured in execution of procedure then show message to user.
            //    if (dt.Rows.Count > 0)
            //    {
            //        lblMessage.CssClass = "info-message";
            //        lblMessage.Visible = true;
            //        lblMessage.Text = dt.Rows[0][0].ToString() + "  ---  " + dt.Rows[0][1].ToString();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowMessage(ex.ToString());
            //}
        }

        // Populate Emergencies Drop down.
        private void PopulateLocationEmergencies()
        {
            //int locationId = (int)RC.SiteLanguage.English;
            //UI.FillEmergency(ddlEmergency, RC.GetAllEmergencies(locationId));            
        }

        // Get all emergencies.
        private object GetLocationEmergencies()
        {
            return DBContext.GetData("GetALLLocationEmergencies");
        }

        private bool IsValidFile()
        {
            if (fuExcel.HasFile)
            {
                string fileExt = Path.GetExtension(fuExcel.PostedFile.FileName);
                if (fileExt != ".xls" && fileExt != ".xlsx" && fileExt != ".xlsb")
                {
                    ShowMessage("Pleae use Excel files with 'xls' OR 'xlsx' extentions.");
                    return false;
                }
            }
            else
            {
                ShowMessage("Please select file to upload!");
                return false;
            }

            return true;
        }

        // First Empty staging tables.
        private void EmptyTable()
        {
            DBContext.Delete("DeleteStagingLogFrame", new object[] { DBNull.Value });
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

        // Upload file to server and return full path with name of file.
        private string UploadFile()
        {
            string path = fuExcel.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fuExcel.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuExcel.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += ((Guid)Membership.GetUser().ProviderUserKey).ToString() + DateTime.Now.ToString("MMM-dd-yy-hh-mm-ss") + fileExtension;
            string uploadDir = Server.MapPath(@"~/Admin/Documents/LogFrames");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string uploadFileLocation = uploadDir + "//" + fileName;
            fuExcel.SaveAs(uploadFileLocation);

            return uploadFileLocation;
        }

        // Get connectionstring. It is on the basis of what is the extention (xls or xlsx) of file given.
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

        // First Read data from excel sheet into data table.
        // Add UserId, Selected Emregency Id and Identity column.
        // Above added data is needed to update tables in db.
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

            UpdateDataTableWithIds(dt);

            return dt;
        }

        // Import all data from staging table to respective tables.
        private DataTable ImportData()
        {
            return DBContext.GetData("ImportClusterLogFrame");
        }

        // Create new datatable and appropriate columns.
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
            dt.Columns.Add("EmergencyId", typeof(int));
            dt.Columns.Add("Cluster_En", typeof(string));            
            dt.Columns.Add("Cluster_Fr", typeof(string));
            dt.Columns.Add("ClusterId", typeof(int));
            dt.Columns.Add("EmergencyClusterId", typeof(int));
            dt.Columns.Add("Objective_En", typeof(string));
            dt.Columns.Add("Objective_Fr", typeof(string));
            dt.Columns.Add("ObjectiveId", typeof(int));
            dt.Columns.Add("ClusterObjectiveId", typeof(int));
            dt.Columns.Add("Priority_En", typeof(string));
            dt.Columns.Add("Priority_Fr", typeof(string));
            dt.Columns.Add("PriorityId", typeof(int));
            dt.Columns.Add("ObjectivePriorityId", typeof(int));
            dt.Columns.Add("Activity_En", typeof(string));
            dt.Columns.Add("Activity_Fr", typeof(string));
            dt.Columns.Add("PriorityActivityId", typeof(int));
            dt.Columns.Add("Data_En", typeof(string));
            dt.Columns.Add("Data_Fr", typeof(string));
            dt.Columns.Add("ActivityDataId", typeof(int));
            dt.Columns.Add("Unit_En", typeof(string));
            dt.Columns.Add("Unit_Fr", typeof(string));
            dt.Columns.Add("UnitId", typeof(int));
            dt.Columns.Add("UserId", typeof(Guid));
            dt.Columns.Add("SecondaryCluster_En", typeof(string));            
            dt.Columns.Add("SecondaryCluster_Fr", typeof(string));            

            return dt;
        }

        // Add 'LocationEmergencyId' and 'UserId' in DataTable.
        private void UpdateDataTableWithIds(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                //dt.Rows[0]["EmergencyId"] = ddlEmergency.SelectedValue;
                //dt.Rows[0]["UserId"] = ((Guid)Membership.GetUser().ProviderUserKey);
            }
        }

        // Write data into db using SqlBulkCopy
        private void WriteDataToDB(DataTable dt)
        {
            // DB connection string.
            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = "StagingTableLogFrame";
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }

        private void ShowMessage(string messsage, string css = "error-message")
        {
            lblMessage.Visible = true;
            lblMessage.Text = messsage;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UploadActivities", this.User);
        }
    }
}