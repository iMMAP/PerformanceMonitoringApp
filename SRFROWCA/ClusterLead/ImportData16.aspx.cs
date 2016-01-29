using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace SRFROWCA.ClusterLead
{
    public partial class ImportData16 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateMonths();
        }


        #region Upload Data

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result));
        }
        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if file is uploaded and is excel file
                if (!IsValidFile()) return;

                // Force user to select month. 
                if (!MonthSelected()) return;

                string filePath = UploadFile();
                string excelConString = GetExcelConString(filePath);

                DataTable dt = new DataTable();
                string[] sheets = GetExcelSheetNames(excelConString);
                if (!string.IsNullOrEmpty(excelConString))
                {
                    if (sheets.Length > 0)
                    {
                        dt = ReadDataInDataTable(excelConString, "RPT$");
                        RemoveUnwantedColumns(dt);
                    }
                }

                //using (TransactionScope scope = new TransactionScope())
                {
                    if (dt.Rows.Count > 0)
                    {
                        FillStagingTableInDB(dt);
                        //ImportData();
                        //TruncateTempTables();
                    }

                    //scope.Complete();
                    ShowMessage("Data Imported Successfully!", RC.NotificationType.Success, false);
                }
            }
            catch (Exception ex)
            {
                //TruncateTempTables();
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

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB(DataTable dt)
        {
            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            WriteDataToDB(dt, conString);
        }

        private String[] GetExcelSheetNames(string excelConString)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                //// Connection String. Change the excel file to the file you
                //// will search.
                //String connString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                //  "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                objConn = new OleDbConnection(excelConString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                // Loop through all of the sheets if you want too...
                //for (int j = 0; j < excelSheets.Length; j++)
                //{
                //    // Query each excel sheet.
                //}

                return excelSheets;
            }
            catch
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        //private void CreateStagingTable(string tableScript, string conString)
        //{
        //    SqlConnection con = new SqlConnection(conString);
        //    SqlCommand cmd = new SqlCommand(tableScript, con);
        //    try
        //    {
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

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
        private DataTable ImportData()
        {
            int yearId = 12;
            return DBContext.GetData("ImportUserCLDataFromStagingTable2015", new object[] { UserInfo.EmergencyCountry, yearId, RC.GetCurrentUserId, RC.IsClusterLead(User) });
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

        private bool MonthSelected()
        {
            if (ddlMonth.SelectedValue == "0")
            {
                ShowMessage("Please Select Month You Want To Report For", RC.NotificationType.Error, false);
                return false;
            }

            return true;
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
                    column.ColumnName == "Achieved Female"))
                {
                    columns.Add(column);
                }
            }

            foreach (DataColumn column in columns)
                dt.Columns.Remove(column);
        }

        #endregion

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
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