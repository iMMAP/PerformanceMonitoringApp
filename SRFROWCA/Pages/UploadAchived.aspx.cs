using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class UploadAchived : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateAdmin1(UserInfo.Country);
                PopulateProjects();
                PopulateMonths();
            }
        }

        #region Download Template

        private void PopulateProjects()
        {
            DataTable dt = GetUserProjects();
            ddlProjects.DataValueField = "ProjectId";
            ddlProjects.DataTextField = "ProjectCode";
            ddlProjects.DataSource = dt;
            ddlProjects.DataBind();
        }

        private DataTable GetUserProjects()
        {
            bool? isOPSProject = true;
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject });
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (UserInfo.Organization > 0)
            {
                GridView gv = new GridView();
                gv.DataSource = GetIndicators();
                gv.DataBind();
                ExportGridView(gv, "ORSDataTemplate", ".xls", true);
            }
            else
            {
                ShowMessage("Some Error Occured. Please contact admin of the site!", RC.NotificationType.Error, false);
            }
        }

        private DataTable GetIndicators()
        {
            int? emgLocationId = UserInfo.EmergencyCountry;

            bool countryInd = false;
            bool regionalInd = false;
            bool allInd = false;
            string projectIds = null;
            int? orgId = null;            

            projectIds = RC.GetSelectedValues(ddlProjects);
            if (projectIds == null)
            {
                orgId = UserInfo.Organization;
            }
            int yearId = 10;

            string locationIds = RC.GetSelectedValues(cblLocations);
            locationIds = string.IsNullOrEmpty(locationIds) ? null : locationIds;

            return DBContext.GetData("GetIndicatorsForUserDataEntryTemplate", new object[]{ emgLocationId, orgId, projectIds, countryInd,
                                                                                            regionalInd, allInd, RC.SelectedSiteLanguageId, yearId, locationIds});
        }

        #endregion

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            ListItem item = new ListItem("Select Month", "0");
            ddlMonth.Items.Insert(0, item);
            ddlMonth.SelectedIndex = 0;

            //var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            //result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            //ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result));
        }
        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);
            cblLocations.DataValueField = "LocationId";
            cblLocations.DataTextField = "LocationName";
            cblLocations.DataSource = dt;
            cblLocations.DataBind();

            foreach (ListItem item in cblLocations.Items)
            {
                item.Selected = true;
            }
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            string procedure = "GetSecondLevelChildLocations";

            if (parentLocationId == 567)
            {
                procedure = "GetSecondLevelChildLocationsAndCountry";
            }

            DataTable dt = DBContext.GetData(procedure, new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMonth.SelectedValue == "0")
            {
                fuAchieved.Enabled = false;
            }
            else
            {
                fuAchieved.Enabled = true;
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if file is uploaded and is excel file
                if (!IsValidFile()) return;

                if (!MonthSelected()) return;


                // Fill staging table (TempData1) in db to import data using this table.
                string filePath = UploadFile();
                string excelConString = GetExcelConString(filePath);

                DataTable dt = new DataTable();
                string tableScript = "";
                string tableScript2 = "";
                if (!string.IsNullOrEmpty(excelConString))
                {
                    string[] sheets = GetExcelSheetNames(excelConString);
                    if (sheets.Length > 0)
                    {
                        dt = ReadDataInDataTable(excelConString, sheets[0]);

                        RemoveUnwantedColumns(dt);
                        tableScript = CreateTableScript(dt);
                        tableScript2 = CreateTableScript2(dt);
                    }
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    if (dt.Rows.Count > 0)
                    {
                        FillStagingTableInDB(tableScript, tableScript2, dt);
                        ImportData();                        
                        TruncateTempTables();
                    }

                    scope.Complete();
                    ShowMessage("Data Imported Successfully!", RC.NotificationType.Success, false);
                }
            }
            catch (Exception ex)
            {
                TruncateTempTables();
                lblMessage.Text = ex.ToString();
                ShowMessage("Some Error Occoured During Import, Please contact with site Admin!", RC.NotificationType.Error, false);
            }
        }

        private void TruncateTempTables()
        {
            DBContext.Update("TruncateImportUserTempTables", new object[] { DBNull.Value });
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

        // Get all emergencies.
        private object GetLocationEmergencies()
        {
            return DBContext.GetData("GetALLLocationEmergencies");
        }

        private bool IsValidFile()
        {
            if (fuAchieved.HasFile)
            {
                string fileExt = Path.GetExtension(fuAchieved.PostedFile.FileName);
                if (fileExt != ".xls" && fileExt != ".xlsx" && fileExt != ".xlsb")
                {
                    ShowMessage("Pleae use Excel files with 'xls' OR 'xlsx' extentions.", RC.NotificationType.Error, false);
                    return false;
                }
            }
            else
            {
                ShowMessage("Please select file to upload!", RC.NotificationType.Error, true, 1000);
                return false;
            }

            return true;
        }

        //// First Empty staging tables.
        //private void EmptyTable()
        //{
        //    DBContext.Delete("DeleteStagingLogFrame", new object[] { DBNull.Value });
        //}

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB(string tableScript, string tableScript2, DataTable dt)
        {

            string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
            CreateStagingTable(tableScript, conString);
            CreateStagingTable(tableScript2, conString);
            WriteDataToDB(dt, conString);
            string locationColumnNames = GetLocationColumnNames(dt);
            string locationColumnNamesWithAliases = GetLocationColumnNamesWithAliases(dt);
            string locationColumnNamesAlias = GetLocationColumnNamesAlias(dt);
            UnpivotStagingTable(locationColumnNames, locationColumnNamesWithAliases, locationColumnNamesAlias);


        }

        private void RemoveUnwantedColumns(DataTable dt)
        {
            if (dt.Columns.Contains("Project Title"))
                dt.Columns.Remove("Project Title");

            if (dt.Columns.Contains("Organization"))
                dt.Columns.Remove("Organization");
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
                for (int j = 0; j < excelSheets.Length; j++)
                {
                    // Query each excel sheet.
                }

                return excelSheets;
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

        private void UnpivotStagingTable(string locationColumnNames, string locationColumnNamesWithAliases, string locationColumnNamesAlias)
        {
            DBContext.GetData("UnpivotUserImportAchievedStagingTable", new object[] { locationColumnNames, locationColumnNamesWithAliases, locationColumnNamesAlias });
        }

        // Add 'LocationEmergencyId' and 'UserId' in DataTable.
        private void UpdateDataTableWithIds(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["Month"] = ddlMonth.SelectedValue;
            }
        }

        private void CreateStagingTable(string tableScript, string conString)
        {
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(tableScript, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
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

            UpdateDataTableWithIds(dt);

            return dt;
        }

        // Import all data from staging table to respective tables.
        private DataTable ImportData()
        {
            int yearId = 10;
            return DBContext.GetData("ImportUserCLDataFromStagingTable", new object[] {UserInfo.EmergencyCountry, UserInfo.EmergencyCluster,
                                                                                    yearId, RC.GetCurrentUserId, RC.IsClusterLead(User)});
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
            dt.Columns.Add("Month", typeof(string));
            return dt;
        }

        private string CreateTableScript(DataTable dt)
        {
            string query = @" IF OBJECT_ID('ImportCLDataStaging_Temp') IS NOT NULL
                            BEGIN
                                DROP TABLE ImportCLDataStaging_Temp
	                        END 
                            CREATE TABLE [ImportCLDataStaging_Temp](
	                            [Id] [int] IDENTITY(1,1) NOT NULL,
	                            [Month] [nvarchar](20) NULL,
	                            [ProjectCode] [nvarchar](50) NULL,
	                            [Objective] [nvarchar](500) NULL,
	                            [Priority] [nvarchar](500) NULL,
	                            [Activity] [nvarchar](2000) NULL,
	                            [Indicator Id] [int] NULL,
	                            [Indicator] [nvarchar](2000) NULL,
	                            [Accumulative] [nvarchar](10) NULL,
                                [Unit] [nvarchar](150) NULL ";

            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    query += " ,[" + column.ColumnName + "] int NULL";
                }
            }

            query += @" CONSTRAINT [PK_ImportCLDataStaging_Temp] PRIMARY KEY CLUSTERED 
                        ([id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                    ) ON [PRIMARY] ";

            return query;
        }

        private string CreateTableScript2(DataTable dt)
        {
            string query = @" IF OBJECT_ID('ImportCLDataStaging_Temp2') IS NOT NULL
                            BEGIN
                                DROP TABLE ImportCLDataStaging_Temp2
	                        END 
                            CREATE TABLE [ImportCLDataStaging_Temp2](
	                            [Id] [int] NOT NULL,
	                            [Month] [nvarchar](20) NULL,
	                            [ProjectCode] [nvarchar](50) NULL,
	                            [Objective] [nvarchar](500) NULL,
	                            [Priority] [nvarchar](500) NULL,
	                            [Activity] [nvarchar](2000) NULL,
	                            [Indicator Id] [int] NULL,
	                            [Indicator] [nvarchar](2000) NULL,
	                            [Accumulative] [nvarchar](10) NULL,
                                [Unit] [nvarchar](150) NULL ";
            int i = 0;
            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    i += 1;
                    string j = "";
                    if (i < 10)
                    {
                        j = i.ToString("00");
                    }
                    else
                    {
                        j = i.ToString();
                    }

                    query += " ,[" + column.ColumnName + "_" + j.ToString() + "] int NULL";
                }
            }

            query += " ) ";

            return query;
        }

        private string GetLocationColumnNames(DataTable dt)
        {
            string locationNames = "";
            int i = 1;
            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    string j = "";
                    if (i < 10)
                    {
                        j = i.ToString("00");
                    }
                    else
                    {
                        j = i.ToString();
                    }

                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += "[" + column.ColumnName + "_" + j.ToString() + "]";
                    }
                    else
                    {
                        locationNames += "," + "[" + column.ColumnName + "_" + j.ToString() + "]";
                    }

                    i += 1;
                }
            }

            return locationNames;
        }

        private string GetLocationColumnNamesWithAliases(DataTable dt)
        {
            string locationNames = "";
            int i = 1;
            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    string j = "";
                    if (i < 10)
                    {
                        j = i.ToString("00");
                    }
                    else
                    {
                        j = i.ToString();
                    }

                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += "[" + column.ColumnName + "_" + j.ToString() + "]" + " AS [t_" + column.ColumnName + j.ToString() + "]";
                    }
                    else
                    {
                        locationNames += "," + "[" + column.ColumnName + "_" + j.ToString() + "]" + " AS [t_" + column.ColumnName + j.ToString() + "]";
                    }
                    i += 1;
                }
            }

            return locationNames;
        }

        private string GetLocationColumnNamesAlias(DataTable dt)
        {
            string locationNames = "";
            int i = 1;
            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    string j = "";

                    if (i < 10)
                    {
                        j = i.ToString("00");
                    }
                    else
                    {
                        j = i.ToString();
                    }

                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += "[t_" + column.ColumnName + j.ToString() + "]";
                    }
                    else
                    {
                        locationNames += "," + "[t_" + column.ColumnName + j.ToString() + "]";
                    }

                    i += 1;
                }
            }

            return locationNames;
        }

        private bool LocationColumn(string name)
        {
            if (name == "Id" || name == "Month" || name == "ProjectCode" || name == "Objective" || name == "Priority" || name == "Activity" ||
                name == "Indicator Id" || name == "Indicator" || name == "Accumulative" || name == "Unit")
            {
                return false;
            }
            return true;
        }

        private void testc(Type type)
        {
            throw new NotImplementedException();
        }

        // Write data into db using SqlBulkCopy
        private void WriteDataToDB(DataTable dt, string conString)
        {
            // DB connection string.
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = "ImportCLDataStaging_Temp";
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }

        private void ExportGridView(Control control, string fileName, string fileExtention, bool disposePassedControl)
        {
            string[] nameWithSpaces = fileName.Split(' ');
            fileName = string.Join("-", nameWithSpaces);
            fileName += fileExtention;
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            GridView gv = control as GridView;
            Response.Write(RenderGrid(gv).ToString());
            if (disposePassedControl)
            {
                control.Dispose();
            }

            Response.End();
        }

        private static StringWriter RenderGrid(GridView gv)
        {
            //  Create a table to contain the grid
            Table table = new Table();
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //  include the gridline settings
            table.GridLines = gv.GridLines;

            //  add the header row to the table
            if (gv.HeaderRow != null)
            {
                ExportUtility.PrepareControlForExport(gv.HeaderRow);
                gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#ddd");
                gv.HeaderRow.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bbbbbb");
                foreach (TableCell cell in gv.HeaderRow.Cells)
                {
                    cell.BackColor = gv.HeaderStyle.BackColor;
                }

                table.Rows.Add(gv.HeaderRow);
            }

            string strGroup = "";
            int t = 0;
            string color = "";
            //  add each of the data rows to the table
            foreach (GridViewRow row in gv.Rows)
            {
                if (!(row.Cells[1].Text == strGroup))
                {

                    strGroup = row.Cells[1].Text;
                    t += 1;
                }
                if (t % 2 == 0)
                {

                    color = "#ebf0f4";
                }
                else
                {
                    color = "#fbfbfb";
                }

                ExportUtility.PrepareControlForExport(row);
                foreach (TableCell cell in row.Cells)
                {

                    cell.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                    cell.CssClass = "textmode";
                }

                //row.CssClass = "istrow";
                table.Rows.Add(row);
            }

            //  add the footer row to the table
            if (gv.FooterRow != null)
            {
                ExportUtility.PrepareControlForExport(gv.FooterRow);
                table.Rows.Add(gv.FooterRow);
            }

            //  render the table into the htmlwriter
            table.RenderControl(htw);

            return sw;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UploadActivities", this.User);
        }
    }
}