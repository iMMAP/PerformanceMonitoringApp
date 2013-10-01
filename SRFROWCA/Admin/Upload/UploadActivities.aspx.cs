﻿using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA.Admin.Upload
{
    public partial class UploadActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateLocationEmergencies();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Empty staging tables before data import.
                EmptyTable();
                // Fill staging table (TempData1) in db to import data using this table.
                FillStagingTableInDB();

                // Import data from another staging table (TempData).
                DataTable dt = ImportData();

                // If success or any error occoured in execution of procedure then show message to user.
                if (dt.Rows.Count > 0)
                {
                    lblMessage.CssClass = "info-message";
                    lblMessage.Visible = true;
                    lblMessage.Text = dt.Rows[0][0].ToString() + "  ---  " + dt.Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = ex.ToString();
            }
        }

        // Populate Emergencies Drop down.
        private void PopulateLocationEmergencies()
        {
            ddlEmergency.DataValueField = "LocationEmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";

            ddlEmergency.DataSource = GetLocationEmergencies();
            ddlEmergency.DataBind();

            ListItem item = new ListItem("Select Emergency", "0");
            ddlEmergency.Items.Insert(0, item);
        }

        // Get all emergencies.
        private object GetLocationEmergencies()
        {
            return DBContext.GetData("GetALLLocationEmergencies");
        }

        // First Empty staging tables.
        private void EmptyTable()
        {
            DBContext.Delete("DeleteTempData", new object[] { DBNull.Value });
        }

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB()
        {
            if (fuExcel.HasFile)
            {
                string filePath = UploadFile();
                string excelConString = GetExcelConString(filePath);
                if (string.IsNullOrEmpty(excelConString))
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Pleae use Excel files with 'xls' OR 'xlsx' extentions.";
                    return;
                }

                DataTable dt = ReadDataInDataTable(excelConString);
                WriteDataToDB(dt);
            }
        }

        // Import all data from staging table to respective tables.
        private DataTable ImportData()
        {
            return DBContext.GetData("ImportActivitiesFromTempData");
        }

        // Upload file to server and return full path with name of file.
        private string UploadFile()
        {
            string path = fuExcel.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fuExcel.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuExcel.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += ((Guid)Membership.GetUser().ProviderUserKey).ToString() + DateTime.Now.ToString("MMM-dd-yy-hh-mm-ss") + fileExtension;
            string uploadLocation = Server.MapPath(@"~/Documents/" + fileName);
            fuExcel.SaveAs(uploadLocation);

            return uploadLocation;
        }

        // Get connectionstring. It is on the basis of what is the extention (xls or xlsx) of file given.
        private string GetExcelConString(string filePath)
        {
            string con = "";
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
            OleDbCommand cmd = new OleDbCommand(@"Select * from [Formatted Data$]", excelCon);
            excelCon.Open();

            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = MakeDataTable();
            da.Fill(dt);
            da.Dispose();
            excelCon.Close();

            UpdateDataTableWithIds(dt);

            return dt;
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
            dt.Columns.Add("LocationEmergencyId", typeof(string));
            dt.Columns.Add("Cluster", typeof(string));
            dt.Columns.Add("EmergencyClusterId", typeof(string));
            dt.Columns.Add("SObjectives", typeof(string));
            dt.Columns.Add("Objectives", typeof(string));
            dt.Columns.Add("ClusterObjectiveId", typeof(string));
            dt.Columns.Add("Indicators", typeof(string));
            dt.Columns.Add("ObjectiveIndicatorId", typeof(string));
            dt.Columns.Add("Activities", typeof(string));
            dt.Columns.Add("IndicatorActivityId", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            dt.Columns.Add("ActivityDataId", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("UnitId", typeof(string));
            dt.Columns.Add("UserId", typeof(Guid));
            dt.Columns.Add("ActivityType", typeof(string));

            return dt;
        }

        // Add 'LocationEmergencyId' and 'UserId' in DataTable.
        private void UpdateDataTableWithIds(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["LocationEmergencyId"] = ddlEmergency.SelectedValue;
                dt.Rows[0]["UserId"] = ((Guid)Membership.GetUser().ProviderUserKey);
            }
        }

        // Write data into db using SqlBulkCopy
        private void WriteDataToDB(DataTable dt)
        {
            // DB connection string.
            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = "TempData1";
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }
    }
}