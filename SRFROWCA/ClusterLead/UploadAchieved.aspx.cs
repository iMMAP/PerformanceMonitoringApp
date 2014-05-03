using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class UploadAchieved : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrganizations();
                if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
                {
                    PopulateClusters();
                }
                else
                {
                    ddlClusters.Visible = false;
                    liClusters.Visible = false;
                }
            }
        }

        private void PopulateClusters()
        {
            int emgId = 1;
            UI.FillEmergnecyClusters(ddlClusters, emgId);
        }

        private void LoadOrganizations()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            ddlOrganizations.DataTextField = "OrganizationName";
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataSource = DBContext.GetData("GetProjectsOrganizations", new object[] { emgLocationId, clusterId });
            ddlOrganizations.DataBind();
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrganizations();
        }

        #region Download Template

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (!IsIndicatorSelected())
            {
                ShowMessage("Please at least select one CheckBox i.e. Country, Regional or All to get the list of indicators!", RC.NotificationType.Error, true, 2000);
                return;
            }
            GridView gv = new GridView();
            gv.DataSource = GetIndicators();
            gv.DataBind();
            ExportGridView(gv, "ORSDataTemplate", ".xls", true);
        }

        private bool IsIndicatorSelected()
        {
            bool countryInd = chkCountryIndicators.Checked;
            bool regionalInd = chkRegionalInidcators.Checked;
            bool allInd = chkAllIndicators.Checked;

            return countryInd || regionalInd || allInd;
                
        }

        private DataTable GetIndicators()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            bool countryInd = chkCountryIndicators.Checked;
            bool regionalInd = chkRegionalInidcators.Checked;
            bool allInd = chkAllIndicators.Checked;
            string orgIds = RC.GetSelectedValues(ddlOrganizations);
            return DBContext.GetData("GetIndicatorsForDataEntryTemplate", new object[]{emgLocationId, clusterId,
                                                                                        orgIds, countryInd, regionalInd, allInd,
                                                                                        RC.SelectedSiteLanguageId});
        }

        #endregion

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
                DataTable dt = ImportData();

                // If success or any error occoured in execution of procedure then show message to user.
                if (dt.Rows.Count > 0)
                {
                    string message = "Data Imported Successfully! Records Updated " + dt.Rows[0][0].ToString() + "  ---  New Records " + dt.Rows[0][1].ToString();
                    ShowMessage(message, RC.NotificationType.Success, false);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.ToString();
                string message = "Some Error Occoured During Import, Please contact with site Admin!";
                ShowMessage(message, RC.NotificationType.Error, false);
            }
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

        //// First Empty staging tables.
        //private void EmptyTable()
        //{
        //    DBContext.Delete("DeleteStagingLogFrame", new object[] { DBNull.Value });
        //}

        // Read Data From Excel Sheet and Save into DB
        private void FillStagingTableInDB()
        {
            string filePath = UploadFile();
            string excelConString = GetExcelConString(filePath);

            if (!string.IsNullOrEmpty(excelConString))
            {
                DataTable dt = ReadDataInDataTable(excelConString);
                dt.Columns.Remove("Project Title");
                string tableScript = CreateTableScript(dt);
                string tableScript2 = CreateTableScript2(dt);
                string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
                CreateStagingTable(tableScript, conString);
                CreateStagingTable(tableScript2, conString);
                WriteDataToDB(dt, conString);
                string locationColumnNames = GetLocationColumnNames(dt);
                string locationColumnNamesWithAliases = GetLocationColumnNamesWithAliases(dt);
                string locationColumnNamesAlias = GetLocationColumnNamesAlias(dt);
                UnpivotStagingTable(locationColumnNames, locationColumnNamesWithAliases, locationColumnNamesAlias);
            }
        }

        private void UnpivotStagingTable(string locationColumnNames, string locationColumnNamesWithAliases, string locationColumnNamesAlias)
        {
            DBContext.GetData("UnpivotImportAchievedStagingTable", new object[] { locationColumnNames, locationColumnNamesWithAliases, locationColumnNamesAlias });
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
        private DataTable ReadDataInDataTable(string excelConString)
        {
            //Create Connection to Excel work book
            OleDbConnection excelCon = new OleDbConnection(excelConString);
            //Create OleDbCommand to fetch data from Excel
            OleDbCommand cmd = new OleDbCommand(@"Select * from [ORSDataTemplate$]", excelCon);
            excelCon.Open();

            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;

            DataTable dt = MakeDataTable();
            //DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            excelCon.Close();

            //UpdateDataTableWithIds(dt);

            return dt;
        }

        // Import all data from staging table to respective tables.
        private DataTable ImportData()
        {
            int yearId = 10;
            return DBContext.GetData("ImportCLDataFromStagingTable", new object[] {UserInfo.EmergencyCountry, UserInfo.EmergencyCluster,
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
            //dt.Columns.Add("EmergencyId", typeof(int));
            //dt.Columns.Add("ClusterId", typeof(int));
            //dt.Columns.Add("EmergencyClusterId", typeof(int));
            //dt.Columns.Add("ObjectiveId", typeof(int));
            //dt.Columns.Add("ClusterObjectiveId", typeof(int));
            //dt.Columns.Add("PriorityId", typeof(int));
            //dt.Columns.Add("ObjectivePriorityId", typeof(int));
            //dt.Columns.Add("Activity", typeof(string));
            //dt.Columns.Add("PriorityActivityId", typeof(int));
            //dt.Columns.Add("Data", typeof(string));
            //dt.Columns.Add("ActivityDataId", typeof(int));

            return dt;
        }

        // Add 'LocationEmergencyId' and 'UserId' in DataTable.
        private void UpdateDataTableWithIds(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                //dt.Rows[0]["EmergencyId"] = ddlEmergency.SelectedValue;
                //dt.Rows[0]["UserId"] = RC.GetCurrentUserId;
            }
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
	                            [ProjectCode] [nvarchar](20) NULL,
	                            [Objective] [nvarchar](100) NULL,
	                            [Priority] [nvarchar](100) NULL,
	                            [Activity] [nvarchar](1000) NULL,
	                            [Indicator Id] [int] NULL,
	                            [Indicator] [nvarchar](1000) NULL,
	                            [Accumulative] [int] NULL,
	                            [Mid Year Target] [int] NULL,
	                            [Full Year Target] [int] NULL";

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
	                            [ProjectCode] [nvarchar](20) NULL,
	                            [Objective] [nvarchar](100) NULL,
	                            [Priority] [nvarchar](100) NULL,
	                            [Activity] [nvarchar](1000) NULL,
	                            [Indicator Id] [int] NULL,
	                            [Indicator] [nvarchar](1000) NULL,
	                            [Accumulative] [int] NULL,
	                            [Mid Year Target] [int] NULL,
	                            [Full Year Target] [int] NULL";
            int i = 1;
            foreach (DataColumn column in dt.Columns)
            {
                if (LocationColumn(column.ColumnName))
                {
                    query += " ,[" + column.ColumnName + "_" + i.ToString() + "] int NULL";
                    i += 1;
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
                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += column.ColumnName + "_" + i.ToString();
                    }
                    else
                    {
                        locationNames += "," + column.ColumnName + "_" + i.ToString();
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
                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += column.ColumnName + "_" + i.ToString() + " AS t_" + column.ColumnName + i.ToString();
                    }
                    else
                    {
                        locationNames += "," + column.ColumnName + "_" + i.ToString() + " AS t_" + column.ColumnName + i.ToString();
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
                    if (string.IsNullOrEmpty(locationNames))
                    {
                        locationNames += "t_" + column.ColumnName + i.ToString();
                    }
                    else
                    {
                        locationNames += "," + "t_" + column.ColumnName + i.ToString();
                    }

                    i += 1;
                }
            }

            return locationNames;
        }

        private bool LocationColumn(string name)
        {
            if (name == "Id" || name == "Month" || name == "ProjectCode" || name == "Objective" || name == "Priority" || name == "Activity" ||
                name == "Indicator Id" || name == "Indicator" || name == "Accumulative" || name == "Mid Year Target" || name == "Full Year Target")
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