using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class UploadSRP : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                menueExportWord.HRef = string.Format("../Reports/DownloadReport.aspx?type=10&emgCountryId={0}&emgClusterId={1}", ddlCountry.SelectedValue, ddlCluster.SelectedValue);

                if (ddlCountry.SelectedValue == "12")
                {
                    hlTemplate.NavigateUrl = "../Test/Burkina.xlsx";                    
                }

                if (ddlCountry.SelectedValue == "13")
                {
                    hlTemplate.NavigateUrl = "../Test/Cameroon.xlsx";
                }

                if (ddlCountry.SelectedValue == "14")
                {
                    hlTemplate.NavigateUrl = "../Test/Chad.xlsx";
                }

                if (ddlCountry.SelectedValue == "15")
                {
                    hlTemplate.NavigateUrl = "../Test/Gambia.xlsx";
                }

                if (ddlCountry.SelectedValue == "16")
                {
                    hlTemplate.NavigateUrl = "../Test/Mali.xlsx";
                }

                if (ddlCountry.SelectedValue == "17")
                {
                    hlTemplate.NavigateUrl = "../Test/Mauritania.xlsx";
                }

                if (ddlCountry.SelectedValue == "18")
                {
                    hlTemplate.NavigateUrl = "../Test/Niger.xlsx";
                }

                if (ddlCountry.SelectedValue == "19")
                {
                    hlTemplate.NavigateUrl = "../Test/Nigeria.xlsx";
                }

                if (ddlCountry.SelectedValue == "20")
                {
                    hlTemplate.NavigateUrl = "../Test/Senegal.xlsx";
                }
            }
        }

        private void PopulateDropDowns()
        {
            LoadCountires();
            LoadClusters();

            if (UserInfo.EmergencyCountry > 0)
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();

                if (!RC.IsRegionalClusterLead(this.User))
                {
                    ddlCountry.Enabled = false;
                    ddlCountry.BackColor = Color.LightGray;
                }
            }

            if (UserInfo.EmergencyCluster > 0)
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
            }
        }
        private void LoadCountires()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            if (ddlCountry.Items.Count > 0)
            {
                ListItem item = new ListItem("Select", "0");
                ddlCountry.Items.Insert(0, item);
            }
        }
        private void LoadClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (ddlCluster.Items.Count > 0)
            {
                ListItem item = new ListItem("Select", "0");
                ddlCluster.Items.Insert(0, item);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            menueExportWord.HRef = string.Format("../Reports/DownloadReport.aspx?type=10&emgCountryId={0}&emgClusterId={1}", ddlCountry.SelectedValue, ddlCluster.SelectedValue);
            if (ddlCountry.SelectedValue == "12")
            {
                hlTemplate.NavigateUrl = "../Test/Burkina.xlsx";
            }

            if (ddlCountry.SelectedValue == "13")
            {
                hlTemplate.NavigateUrl = "../Test/Cameroon.xlsx";
            }

            if (ddlCountry.SelectedValue == "14")
            {
                hlTemplate.NavigateUrl = "../Test/Chad.xlsx";
            }

            if (ddlCountry.SelectedValue == "15")
            {
                hlTemplate.NavigateUrl = "../Test/Gambia.xlsx";
            }

            if (ddlCountry.SelectedValue == "16")
            {
                hlTemplate.NavigateUrl = "../Test/Mali.xlsx";
            }

            if (ddlCountry.SelectedValue == "17")
            {
                hlTemplate.NavigateUrl = "../Test/Mauritania.xlsx";
            }

            if (ddlCountry.SelectedValue == "18")
            {
                hlTemplate.NavigateUrl = "../Test/Niger.xlsx";
            }

            if (ddlCountry.SelectedValue == "19")
            {
                hlTemplate.NavigateUrl = "../Test/Nigeria.xlsx";
            }

            if (ddlCountry.SelectedValue == "20")
            {
                hlTemplate.NavigateUrl = "../Test/Senegal.xlsx";
            }
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            menueExportWord.HRef = string.Format("../Reports/DownloadReport.aspx?type=10&emgCountryId={0}&emgClusterId={1}", ddlCountry.SelectedValue, ddlCluster.SelectedValue);
        }

        #region Upload

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string filePath = FU.UploadFile(fuSRP, Server.MapPath(@"~/TEST"));
            string excelConString = FU.GetExcelConString(filePath);
            string[] sheets = FU.GetExcelSheetNames(excelConString);

            DataTable dt = MakeDataTable();
            string tableScript = "";
            if (!string.IsNullOrEmpty(excelConString) && sheets.Length > 0)
            {
                dt = FU.ReadDataInDataTable(excelConString, "Framework$", dt);
                tableScript = CreateTableScript(dt);
            }

            //using (TransactionScope scope = new TransactionScope())
            {
                if (dt.Rows.Count > 0)
                {
                    string conString = ConfigurationManager.ConnectionStrings["live_dbName"].ConnectionString;
                    FU.CreateTableInDB(conString, tableScript);
                    FU.WriteDataInDBTable(conString, "ImportSRPTemp", dt);
                    UnpivotStagingTable(dt);
                    ImportData();
                    //TruncateTempTables();
                }

                //scope.Complete();
                ShowMessage("Framework Imported Successfully!", RC.NotificationType.Success, false);
            }
        }

        private bool IsValidFile()
        {
            if (fuSRP.HasFile)
            {
                string fileExt = Path.GetExtension(fuSRP.PostedFile.FileName);
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

        private string CreateTableScript(DataTable dt)
        {
            string query = @" IF OBJECT_ID('ImportSRPTemp') IS NOT NULL
							BEGIN
								DROP TABLE ImportSRPTemp
							END 
							CREATE TABLE [ImportSRPTemp](
								[Id] [int] IDENTITY(1,1) NOT NULL,
	                            [Objective] [nvarchar](1000) NULL,
	                            [Activity_En] [nvarchar](2000) NULL,
	                            [Activity_Fr] [nvarchar](2000) NULL,
	                            [Indicator_En] [nvarchar](2000) NULL,
	                            [Indicator_Fr] [nvarchar](2000) NULL,
	                            [Unit_En] [nvarchar](500) NULL,
	                            [Unit_Fr] [nvarchar](500) NULL ";

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

            query += @" CONSTRAINT [PK_ImportSRPTemp] PRIMARY KEY CLUSTERED 
						([id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
					) ON [PRIMARY] ";

            return query;
        }

        private bool LocationColumn(string name)
        {
            if (name == "Id" || name == "EmergencyLocationId" || name == "EmergencyClusterId"
                || name == "EmergencyObjectiveId" || name == "Objective" || name == "ObjectiveId"
                || name == "Activity_En" || name == "Activity_Fr" || name == "ActivityId"
                || name == "Indicator_En" || name == "Indicator_Fr" || name == "IndicatorId" 
                || name == "Unit_En" || name == "Unit_Fr" || name == "UnitId")
            {
                return false;
            }
            return true;
        }

        private void UnpivotStagingTable(DataTable dt)
        {
            string locations = GetLocationNames(dt);
            string locationsWithAliases = GetLocationNamesWithAliases(dt);
            string aliasesOfLocations = GetAlisesOfLocationNames(dt);

            DBContext.GetData("UnpivotImportSRPStaging", new object[] { locations, locationsWithAliases, aliasesOfLocations });
        }

        private DataTable ImportData()
        {
            int emgCountryId = Convert.ToInt32(ddlCountry.SelectedValue);
            int emgClusterId = Convert.ToInt32(ddlCluster.SelectedValue);
            int emergencyId = RC.EmergencySahel2015;
            return DBContext.GetData("ImportSRP", new object[] {emergencyId, emgCountryId, emgClusterId, RC.GetCurrentUserId});
        }

        private string GetLocationNames(DataTable dt)
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
                        locationNames += ",[" + column.ColumnName + "_" + j.ToString() + "]";
                    }
                    i += 1;
                }
            }

            return locationNames;
        }

        private string GetLocationNamesWithAliases(DataTable dt)
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
                        locationNames += "[" + column.ColumnName + "_" + j.ToString() + "] AS [t_" + column.ColumnName + j.ToString() + "]";
                    }
                    else
                    {
                        locationNames += ",[" + column.ColumnName + "_" + j.ToString() + "] AS [t_" + column.ColumnName + j.ToString() + "]";
                    }
                    i += 1;
                }
            }

            return locationNames;
        }

        private string GetAlisesOfLocationNames(DataTable dt)
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

        private DataTable MakeDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn idColumn = new DataColumn();
            idColumn.ColumnName = "Id";
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.AutoIncrement = true;
            idColumn.AutoIncrementSeed = 1;
            idColumn.AutoIncrementStep = 1;
            
            dt.Columns.Add(idColumn);
            //dt.Columns.Add("EmergencyId", typeof(int));
            //dt.Columns.Add("EmergencyLocationId", typeof(int));
            //dt.Columns.Add("EmergencyClusterId", typeof(int));
            //dt.Columns.Add("EmergencyObjectiveId", typeof(int));
            //dt.Columns.Add("ObjectiveId", typeof(int));
            //dt.Columns.Add("ActivityId", typeof(int));
            //dt.Columns.Add("IndicatorId", typeof(int));
            //dt.Columns.Add("UnitId", typeof(int));
            return dt;
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

        #endregion
    }
}