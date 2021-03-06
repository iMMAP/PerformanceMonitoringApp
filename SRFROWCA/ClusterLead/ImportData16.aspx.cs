﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ImportData16 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ddlYear_SelectedIndexChnaged(object sender, EventArgs e)
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            fuAchieved.Enabled = yearId > 0;
            btnImport.Enabled = yearId > 0;
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if file is uploaded and is excel file
                if (!IsValidFile()) return;

                // Upload selected file.
                string filePath = UploadFile();
                string excelConString = GetExcelConString(filePath);
                DataTable dt = GetDataFromSheet(filePath);

                //using (TransactionScope scope = new TransactionScope())
                {
                    if (dt.Rows.Count > 0)
                    {
                        FillStagingTableInDB(dt);
                        int returnValue = ImportData();
                        if (returnValue > 0)
                        {
                            DataTable dtWrongLocations = DBContext.GetData("GetTempInvalidLocations", new object[] { returnValue });
                            if (dtWrongLocations.Rows.Count > 0)
                            {
                                gvActivity.DataSource = dtWrongLocations;
                                gvActivity.DataBind();
                                spnWrongLocMessage.Visible = true;
                                ShowMessage("There are some issues with Data! For details look at the table below.", RC.NotificationType.Error, false);
                            }
                        }
                        else
                            ShowMessage("Data Imported Successfully!", RC.NotificationType.Success, false);
                    }

                    //scope.Complete();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.ToString();
                ShowMessage("Some Error Occoured During Import, Please contact with site Admin!", RC.NotificationType.Error, false);
            }
        }

        private bool IsValidFile()
        {
            if (fuAchieved.HasFile)
            {
                string fileExt = Path.GetExtension(fuAchieved.PostedFile.FileName);
                if (fileExt != ".xls" && fileExt != ".xlsx" && fileExt != ".xlsb" && fileExt != ".xlsm")
                {
                    ShowMessage("Pleae use Excel files with 'xls' OR 'xlsx' OR xlsm extentions.");
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

        // Upload file to server and return full path with name of file.
        private string UploadFile()
        {
            string path = fuAchieved.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fuAchieved.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuAchieved.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += DateTime.Now.ToString("MMM-dd-yy-hh-mm-ss") + fileExtension;
            string uploadDir = Server.MapPath(@"~/TEST");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string uploadFileLocation = uploadDir + "//" + fileName;
            fuAchieved.SaveAs(uploadFileLocation);

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
            else if (fileExtension == ".xlsm")
            {
                con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Macro;HDR=YES\"";
            }
            return con;
        }

        private DataTable GetDataFromSheet(string filePath)
        {
            //Get excel connection string
            string excelConString = GetExcelConString(filePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(excelConString))
            {
                dt = ReadDataInDataTable(excelConString, "RPT$");
                RemoveUnwantedColumns(dt);
            }

            return dt;
        }

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB(DataTable dt)
        {
            DBContext.Update("TruncateStagingTable2016", new object[] { DBNull.Value });
            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            WriteDataToDB(dt, conString);
        }

        // First Read data from excel sheet into data table.
        // Add UserId, Selected Emregency Id and Identity column.
        // Above added data is needed to update tables in db.
        private DataTable ReadDataInDataTable(string excelConString, string sheetName)
        {
            //Create Connection to Excel work book
            OleDbConnection excelCon = new OleDbConnection(excelConString);
            //Create OleDbCommand to fetch data from Excel
            OleDbCommand cmd = new OleDbCommand(string.Format(@"Select * from [{0}]", sheetName), excelCon);
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

        // Import all data from staging table to respective tables.
        private int ImportData()
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            return DBContext.Update("ImportUserCLDataFromStagingTable2016", new object[] { yearId, UserInfo.Organization, UserInfo.EmergencyCountry,
                                                                                           UserInfo.EmergencyCluster, RC.GetCurrentUserId, 
                                                                                           DBNull.Value });
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
            return dt;
        }

        // Write data into db using SqlBulkCopy
        private void WriteDataToDB(DataTable dt, string conString)
        {
            // DB connection string.

            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = "ImportFrameworkDataStaging_Temp";
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }

        private void RemoveUnwantedColumns(DataTable dt)
        {
            List<DataColumn> columns = new List<DataColumn>();
            foreach (DataColumn column in dt.Columns)
            {
                if (!(column.ColumnName == "Id" ||
                    column.ColumnName == "RPT ID" ||
                    column.ColumnName == "Month ID" ||
                    column.ColumnName == "project ID" ||
                    column.ColumnName == "Indicator ID" ||
                    column.ColumnName == "Location ID" ||
                    column.ColumnName == "Target Total" ||
                    column.ColumnName == "Target Male" ||
                    column.ColumnName == "Target Female" ||
                    column.ColumnName == "Achieved Total" ||
                    column.ColumnName == "Achieved Male" ||
                    column.ColumnName == "Achieved Female" ||
                    column.ColumnName == "ORG_ID" ||
                    column.ColumnName == "IP_ID"))
                {
                    columns.Add(column);
                }
            }

            foreach (DataColumn column in columns)
                dt.Columns.Remove(column);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData();
            ExportUtility.ExportGridView(dt, "ORS_CustomReport16", Response);
        }

        private DataTable GetData()
        {
            int? countryId = null;
            int? clusterId = null;
            int? orgId = null;
            if (RC.IsClusterLead(this.User))
            {
                countryId = UserInfo.Country;
                clusterId = UserInfo.Cluster;
            }
            else if (RC.IsCountryAdmin(this.User))
            {
                countryId = UserInfo.Country;
            }
            else if (RC.IsDataEntryUser(this.User))
            {
                countryId = UserInfo.Country;
                orgId = UserInfo.Organization;
            }

            return DBContext.GetData("GetReportedData2016_Report", new object[] { countryId, clusterId, orgId, RC.SelectedSiteLanguageId });
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success,
                                    bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}