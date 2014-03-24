using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace SRFROWCA.Pages
{
    public partial class AddActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string languageChange = "";
            if (Session["SiteChanged"] != null)
            {
                languageChange = Session["SiteChanged"].ToString();
            }

            if (!string.IsNullOrEmpty(languageChange))
            {
                PopulateMonths();
                Session["SiteChanged"] = null;
            }

            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
                PopulateLocations();
                PopulateYears();
                PopulateMonths();

                PopulateProjects();
                if (rblProjects.Items.Count > 0)
                {
                    rblProjects.SelectedIndex = 0;
                }
            }

            PopulateObjectives();
            PopulatePriorities();

            this.Form.DefaultButton = this.btnSave.UniqueID;

            string controlName = GetPostBackControlId(this);

            if (controlName == "ddlMonth" || controlName == "ddlYear" || controlName == "rblProjects")
            {
                LocationRemoved = 0;
                RemoveSelectedLocations(cblAdmin1);
                RemoveSelectedLocations(cblLocations);
            }

            //if (controlName != "rblProjects")
            {
                DataTable dtActivities = GetActivities();
                AddDynamicColumnsInGrid(dtActivities);
                Session["dtActivities"] = dtActivities;
                GetReportId(dtActivities);
                gvActivities.DataSource = dtActivities;
                gvActivities.DataBind();
            }
        }
        private DataTable GetUserProjects()
        {
            bool? isOPSProject = null;
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization,  isOPSProject});
        }

        private void PopulateProjects()
        {
            DataTable dt = GetUserProjects();

            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectCode";            
            rblProjects.DataSource = dt;
            rblProjects.DataBind();

            cblExportProjects.DataValueField = "ProjectId";
            cblExportProjects.DataTextField = "ProjectCode";
            cblExportProjects.DataSource = dt;
            cblExportProjects.DataBind();

            ProjectsToolTip(rblProjects, dt);
            ProjectsToolTip(cblExportProjects, dt);

        }

        private void ProjectsToolTip(ListControl ctl, DataTable dt)
        {
            foreach (ListItem item in ctl.Items)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (item.Text == row["ProjectCode"].ToString())
                    {
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                    }
                }
            }
        }

        #region Events.

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataEntryColumnsAlternateColors(e);
                ObjectiveIconToolTip(e);
                PrioritiesIconToolTip(e);


                Image imgRind = e.Row.FindControl("imgRind") as Image;
                if (imgRind != null)
                {
                    //if (e.Row.RowIndex == 2 || e.Row.RowIndex == 1)
                    //{
                    //    imgRind.ImageUrl = "~/images/rind.png";
                    //    imgRind.ToolTip = "Regional Indicator";
                    //}
                    //else
                    //{
                    //    imgRind.Visible = false;
                    //}
                    imgRind.Visible = false;
                }

                Image imgCind = e.Row.FindControl("imgCind") as Image;
                if (imgCind != null)
                {
                    //if (e.Row.RowIndex == 2 || e.Row.RowIndex == 3)
                    //{
                    //    imgCind.ImageUrl = "~/images/cind.png";
                    //    imgCind.ToolTip = "Country Specific Indicator";
                    //}
                    //else
                    //{
                    //    imgCind.Visible = false;
                    //}

                    imgCind.Visible = false;
                }
            }
        }

        private void PrioritiesIconToolTip(GridViewRowEventArgs e)
        {
            Image imghp = e.Row.FindControl("imgPriority") as Image;
            if (imghp != null)
            {
                string txtHP = e.Row.Cells[1].Text;
                if (txtHP == "1")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp1.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux catastrophes naturelles (inondations, etc.)" :
                        "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                }
                else if (txtHP == "2")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp2.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux conflits (PDIs, refugies, protection, etc.)" :
                        "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                }
                else if (txtHP == "3")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp3.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues aux épidémies (cholera, paludisme, etc.)" :
                        "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                }
                else if (txtHP == "4")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp4.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues à l’insécurité alimentaire" :
                        "Addressing the humanitarian impact of Food insecurity";
                }
                else if (txtHP == "5")
                {
                    imghp.ImageUrl = "~/assets/orsimages/icon/hp5.png";
                    imghp.ToolTip = RC.SelectedSiteLanguageId == 2 ?
                        "Répondre aux conséquences humanitaires dues à la malnutrition" :
                        "Addressing the humanitarian impact of Malnutrition";
                }
            }
        }

        private void ObjectiveIconToolTip(GridViewRowEventArgs e)
        {
            Image imgObj = e.Row.FindControl("imgObjective") as Image;
            if (imgObj != null)
            {
                string txt = e.Row.Cells[0].Text;

                if (txt.Contains("1"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so1.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°1 : Recueillir les données sur les risques et les vulnérabilités, les analyser et intégrer les résultats dans la programmation humanitaire et de développement." :
                        "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                }
                else if (txt.Contains("2"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so2.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°2 : Soutenir les populations vulnérables à mieux faire face aux chocs en répondant aux signaux d’alerte de manière anticipée, réduisant la durée du relèvement post-crise et renforçant les capacités des acteurs nationaux." :
                        "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                }
                else if (txt.Contains("3"))
                {
                    imgObj.ImageUrl = "~/assets/orsimages/icon/so3.png";
                    imgObj.ToolTip = RC.SelectedSiteLanguageId == 2 ? "OBJECTIF STRATÉGIQUE N°3 : Fournir aux personnes en situation d’urgence une assistance coordonnée et intégrée, nécessaire à leur survie." :
                        "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                }
            }
        }

        private void DataEntryColumnsAlternateColors(GridViewRowEventArgs e)
        {
            int j = 0;
            for (int i = 11; i < e.Row.Cells.Count; i++)
            {
                TableCell Cell = e.Row.Cells[i];

                // Three columns, 'Annual Targets', 'ACCUM', and 'Monthly Achieved'
                // will have alternate color on each location
                if (j <= 2)
                {
                    j++;
                    string color = RC.ConfigSettings("AlternateColumnColor");
                    Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                }
                else if ((j > 2))
                {
                    j++;
                    string color = RC.ConfigSettings("ColumnColor");
                    Cell.BackColor = System.Drawing.ColorTranslator.FromHtml(color);

                    if (j == 6) j = 0;
                }
            }
        }

        private void DataEntryColumnsAlternateColors()
        {
            throw new NotImplementedException();
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridData();
            AddLocationsInSelectedList();
        }

        private string BreakString(string fullString)
        {
            const Int32 MAX_WIDTH = 60;
            int offset = 0;
            string text = Regex.Replace(fullString, @"\s{2,}", " ");
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();

            while (offset < text.Length)
            {
                int index = text.LastIndexOf(" ",
                                 Math.Min(text.Length, offset + MAX_WIDTH));
                string line = text.Substring(offset,
                    (index - offset <= 0 ? text.Length : index) - offset);
                offset += line.Length + 1;
                lines.Add(line);
                sb.Append(line);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            BindGridData();
            AddLocationsInSelectedList();
        }

        protected void btnLocation_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            LocationRemoved = 1;
            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<int> dataSave = new List<int>();
                    int i = 0;
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;
                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(locationId);
                            if (i == 1)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = Convert.ToInt32(item.ToString());
                                        j++;
                                    }
                                    else
                                    {
                                        j = 0;
                                    }
                                }
                            }
                            else
                            {
                                i = 1;
                            }
                        }
                    }

                    List<ListItem> itemsToDelete = new List<ListItem>();

                    foreach (ListItem item in cblLocations.Items)
                    {
                        if (dataSave.Contains(Convert.ToInt32(item.Value)))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }

                    foreach (ListItem item in cblAdmin1.Items)
                    {
                        if (dataSave.Contains(Convert.ToInt32(item.Value)))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveProjectData();
        }

        private void SaveProjectData()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (ReportId > 0)
                {
                    DeleteReportAndItsChild();
                }

                if (IsDataExistsToSave())
                {
                    SaveReport();
                }

                scope.Complete();
                ShowMessage("Your Data Saved Successfuly!");
            }
        }

        #endregion

        #region Methods.

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
            ObjectivesToolTip();
        }

        private void ObjectivesToolTip()
        {
            if (RC.SelectedSiteLanguageId == 2)
            {
                cblObjectives.Items[0].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°1 : Recueillir les données sur les risques et les vulnérabilités, les analyser et intégrer les résultats dans la programmation humanitaire et de développement.";
                cblObjectives.Items[1].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°2 : Soutenir les populations vulnérables à mieux faire face aux chocs en répondant aux signaux d’alerte de manière anticipée, réduisant la durée du relèvement post-crise et renforçant les capacités des acteurs nationaux.";
                cblObjectives.Items[2].Attributes["title"] = "OBJECTIF STRATÉGIQUE N°3 : Fournir aux personnes en situation d’urgence une assistance coordonnée et intégrée, nécessaire à leur survie.";
            }
            else
            {
                cblObjectives.Items[0].Attributes["title"] = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and development programming.";
                cblObjectives.Items[1].Attributes["title"] = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                cblObjectives.Items[2].Attributes["title"] = "STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
            }

        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
            PrioritiesToolTip();
        }

        private void PrioritiesToolTip()
        {
            if (RC.SelectedSiteLanguageId == 2)
            {
                cblPriorities.Items[0].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux catastrophes naturelles (inondations, etc.)";
                cblPriorities.Items[1].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux conflits (PDIs, refugies, protection, etc.)";
                cblPriorities.Items[2].Attributes["title"] = "Répondre aux conséquences humanitaires dues aux épidémies (cholera, paludisme, etc.)";
                cblPriorities.Items[3].Attributes["title"] = "Répondre aux conséquences humanitaires dues à l’insécurité alimentaire";
                cblPriorities.Items[4].Attributes["title"] = "Répondre aux conséquences humanitaires dues à la malnutrition";
            }
            else
            {
                cblPriorities.Items[0].Attributes["title"] = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                cblPriorities.Items[1].Attributes["title"] = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                cblPriorities.Items[2].Attributes["title"] = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                cblPriorities.Items[3].Attributes["title"] = "Addressing the humanitarian impact of Food insecurity";
                cblPriorities.Items[4].Attributes["title"] = "Addressing the humanitarian impact of Malnutrition";
            }
        }

        private DataTable GetLocationEmergencies(int locationId)
        {
            DataTable dt = DBContext.GetData("GetEmergencyOnLocation", new object[] { locationId, RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Months Drop Down
        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            if (i > -1)
            {
                ddlMonth.SelectedIndex = i;
            }
            else
            {
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result.ToString()));
            }

            cblMonths.DataValueField = "MonthId";
            cblMonths.DataTextField = "MonthName";
            cblMonths.DataSource = GetMonth();
            cblMonths.DataBind();
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }
        // Populate Years Drop Down
        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            var result = DateTime.Parse(DateTime.Now.ToShortDateString()).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));
        }

        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // In this method we will get the postback control.
        public string GetPostBackControlId(Page page)
        {
            // If page is requested first time then return.
            if (!page.IsPostBack)
                return "";

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                string controlId;
                Control foundControl;

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            DataTable dt = DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        internal override void BindGridData()
        {
            DataTable dt = GetActivities();
            Session["dtActivities"] = dt;
            GetReportId(dt);
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        private string GetSelectedLocations()
        {
            string admin1 = GetSelectedItems(cblAdmin1);
            string admin2 = GetSelectedItems(cblLocations);

            if (!string.IsNullOrEmpty(admin1) && string.IsNullOrEmpty(admin2))
            {
                return admin1;
            }
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin2;
            }
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin1 + ", " + admin2;
            }

            return "";
        }

        private string GetNotSelectedLocations()
        {
            string admin1 = GetNotSelectedItems(cblAdmin1);
            string admin2 = GetNotSelectedItems(cblLocations);

            if (!string.IsNullOrEmpty(admin1) && string.IsNullOrEmpty(admin2))
            {
                return admin1;
            }
            else if (string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin2;
            }
            else if (!string.IsNullOrEmpty(admin1) && !string.IsNullOrEmpty(admin2))
            {
                return admin1 + ", " + admin2;
            }

            return "";
        }

        private string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

        private string GetNotSelectedItems(object sender)
        {
            string itemIds = "";
            if (LocationRemoved == 1)
            {
                foreach (ListItem item in (sender as ListControl).Items)
                {
                    if (!item.Selected)
                    {
                        if (itemIds != "")
                        {
                            itemIds += "," + item.Value;
                        }
                        else
                        {
                            itemIds += item.Value;
                        }
                    }
                }
            }

            return itemIds;
        }

        private DataTable GetActivities()
        {
            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            int projectId = RC.GetSelectedIntVal(rblProjects);

            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();
            Guid userId = RC.GetCurrentUserId;

            DataTable dt = DBContext.GetData("GetIPData", new object[] { UserInfo.EmergencyCountry, locationIds, yearId, monthId,
                                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                                                                        UserInfo.Organization, projectId});
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddLocationsInSelectedList()
        {
            PopulateLocations();
        }

        private void PopulateLocations()
        {
            int countryId = UserInfo.Country;
            PopulateAdmin1(countryId);
            PopulateAdmin2(countryId);
        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);
            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";
            cblAdmin1.DataSource = dt;
            cblAdmin1.DataBind();
            lblLocAdmin1.Text = UserInfo.CountryName + " Admin 1 Locations";
        }

        private void PopulateAdmin2(int parentLocationId)
        {
            DataTable dt = GetAdmin2Locations(parentLocationId);
            cblLocations.DataValueField = "LocationId";
            cblLocations.DataTextField = "LocationName";
            cblLocations.DataSource = dt;
            cblLocations.DataBind();
            lblLocAdmin2.Text = UserInfo.CountryName + " Admin 2 Locations";
        }

        private DataTable GetReportLocations()
        {
            DataTable dt = DBContext.GetData("GetReportLocations", new object[] { ReportId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void AddDynamicColumnsInGrid(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                TemplateField customField = new TemplateField();
                // Create the dynamic templates and assign them to 
                // the appropriate template property.

                string columnName = column.ColumnName;
                if (!(columnName == "ReportId" || columnName == "ProjectCode" || columnName == "Objective" ||
                    columnName == "HumanitarianPriority" || columnName == "ActivityName" || columnName == "DataName" ||
                    columnName == "ActivityDataId" || columnName == "ProjectTitle" || columnName == "ProjectId" ||
                    columnName == "ObjAndPrId" || columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" ||
                    columnName == "objAndPrAndPId" || columnName == "objAndPId" || columnName == "PrAndPId" ||
                    columnName == "ProjectIndicatorId"))
                {
                    if (columnName.Contains("_2-ACCUM"))
                    {
                        customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "CheckBox", column.ColumnName, "1");
                        customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "CheckBox", column.ColumnName, "1");
                        gvActivities.Columns.Add(customField);
                    }
                    else
                    {
                        if (columnName.Contains("_1-"))
                        {
                            customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName, "1", "Annual");
                            customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName, "1", "Annual");
                            gvActivities.Columns.Add(customField);
                        }
                        else
                        {
                            customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, "TextBox", column.ColumnName, "1");
                            customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, "TextBox", column.ColumnName, "1");
                            gvActivities.Columns.Add(customField);
                        }
                    }
                }
            }
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetSecondLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetAdmin2Locations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)RC.LocationTypes.Governorate;
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        public static List<ListItem> GetSortedList(ListBox sourceListBox, ListBox destinationListBox, ListItem selectedItem)
        {
            List<ListItem> sortedList = new List<ListItem>();

            // Add all items from source listbox to sortedList List.
            if (sourceListBox != null)
            {
                foreach (ListItem item in sourceListBox.Items)
                {
                    sortedList.Add(item);
                }
            }

            // Add all items from destination listbox to sortedList List.
            // We need this to sort items which are already in listbox.
            if (destinationListBox != null)
            {
                foreach (ListItem item in destinationListBox.Items)
                {
                    sortedList.Add(item);
                }
            }

            // If items is passed from calling method then add it in sortedList.
            // selectedItem will have data when only one item is being add/remove
            if (selectedItem != null)
            {
                sortedList.Add(selectedItem);
            }

            // Sort items in listbox.
            sortedList = sortedList.OrderBy(li => li.Text).ToList();

            return sortedList;
        }

        private void DeleteReportAndItsChild()
        {
            //DeleteReportDetail();
            DeleteReport();
        }

        private void DeleteReport()
        {
            DBContext.Delete("DeleteReport", new object[] { ReportId, DBNull.Value });
        }

        private void DeleteReportDetail()
        {
            DBContext.Delete("DeleteReportDetail", new object[] { ReportId, DBNull.Value });
        }

        private void SaveReport()
        {
            SaveReportMainInfo();
            SaveReportLocations();
            SaveReportDetails();
        }

        private void SaveReportLocations()
        {
            int k = 0;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (k > 0) break;
                k++;
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DataTable dtActivities = (DataTable)Session["dtActivities"];

                    List<int> dataSave = new List<int>();
                    int i = 0;
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;
                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(locationId);
                            if (i == 2)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = Convert.ToInt32(item.ToString());
                                        j++;
                                    }
                                    else
                                    {
                                        j = 0;
                                    }
                                }

                                if (locationId > 0)
                                {
                                    DBContext.Add("InsertReportLocations", new object[] { ReportId, locationId, DBNull.Value });
                                }
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                    }
                }
            }
        }

        private void SaveReportMainInfo()
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projId = RC.GetSelectedIntVal(rblProjects);
            Guid loginUserId = RC.GetCurrentUserId;
            ReportId = DBContext.Add("InsertReport", new object[] { yearId, monthId, projId, UserInfo.EmergencyCountry,
                                                                    UserInfo.Organization, loginUserId, DBNull.Value });
        }

        private bool IsDataExistsToSave()
        {
            bool returnValue = false;
            //foreach (GridViewRow row in gvActivities.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        DataTable dtActivities = (DataTable)Session["dtActivities"];

            //        List<int> dataSave = new List<int>();
            //        foreach (DataColumn dc in dtActivities.Columns)
            //        {
            //            string colName = dc.ColumnName;
            //            int locationId = 0;
            //            HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
            //            if (hf != null)
            //            {
            //                locationId = Convert.ToInt32(hf.Value);
            //                if (locationId > 0)
            //                {
            //                    returnValue = true;
            //                    break;
            //                }
            //            }
            //        }

            //        if (returnValue) break;
            //    }
            //}
            returnValue = true;
            return returnValue;
        }

        protected void CaptureDataFromGrid()
        {
            string activityDataId = "";
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow) return;

                activityDataId = gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString();
                int colummCounts = gvActivities.Columns.Count;

                DataTable dtActivities = (DataTable)Session["dtActivities"];
                if (dtActivities == null) return;

                DataTable dtClone;
                if (Session["dtClone"] != null)
                {
                    dtClone = (DataTable)Session["dtClone"];
                }
                else
                {
                    dtClone = dtActivities.Copy();
                    foreach (DataRow dr in dtClone.Rows)
                    {
                        foreach (DataColumn dc in dtClone.Columns)
                        {
                            if (dc.DataType == typeof(string))
                            {
                                dr[dc] = "";
                            }
                        }
                    }
                }

                foreach (DataColumn dc in dtClone.Columns)
                {
                    string colName = dc.ColumnName;
                    TextBox t = row.FindControl(colName) as TextBox;
                    if (t != null)
                    {
                        dtClone.Rows[row.RowIndex][colName] = t.Text;
                        dtClone.Rows[row.RowIndex]["ActivityDataId"] = activityDataId;
                    }
                }

                Session["dtClone"] = dtClone;
            }
        }

        private void SaveReportDetails()
        {
            int activityDataId = 0;
            int projIndicatorId = 0;
            int yearId = RC.GetSelectedIntVal(ddlYear);
            Random rnd = new Random();
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projectId = RC.GetSelectedIntVal(rblProjects);
                    activityDataId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    projIndicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());

                    int colummCounts = gvActivities.Columns.Count;
                    DataTable dtActivities = (DataTable)Session["dtActivities"];
                    List<KeyValuePair<int, decimal?>> dataSave = new List<KeyValuePair<int, decimal?>>();
                    int i = 0;
                    bool isAccum = false;
                    foreach (DataColumn dc in dtActivities.Columns)
                    {
                        string colName = dc.ColumnName;
                        int locationId = 0;
                        HiddenField hf = row.FindControl("hf" + colName) as HiddenField;
                        if (hf != null)
                        {
                            locationId = Convert.ToInt32(hf.Value);
                        }

                        decimal? value = null;
                        TextBox t = row.FindControl(colName) as TextBox;
                        if (t != null)
                        {
                            if (!string.IsNullOrEmpty(t.Text))
                            {
                                value = Convert.ToDecimal(t.Text, System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }

                        CheckBox cbAccum = row.FindControl(colName) as CheckBox;
                        if (cbAccum != null)
                        {
                            isAccum = cbAccum.Checked;
                        }

                        if (locationId > 0)
                        {
                            dataSave.Add(new KeyValuePair<int, decimal?>(locationId, value));
                            if (i == 2)
                            {
                                i = 0;
                                int locationIdToSaveT = 0;
                                decimal? valToSaveT = null;
                                decimal? valToSaveA = null;
                                int j = 0;
                                foreach (var item in dataSave)
                                {
                                    if (j == 0)
                                    {
                                        locationIdToSaveT = item.Key;
                                        valToSaveT = item.Value;
                                        j++;
                                    }
                                    else if (j == 1)
                                    {
                                        j++;
                                    }
                                    else
                                    {
                                        valToSaveA = item.Value;
                                        j = 0;
                                    }
                                }

                                dataSave.Clear();
                                Guid userId = RC.GetCurrentUserId;

                                if (!(valToSaveT == null))
                                {
                                    DBContext.Add("InsertUpdateIndicatorLocationAnnualTarget", new Object[] {UserInfo.EmergencyCountry,
                                                    UserInfo.Organization, locationIdToSaveT, projectId,
                                                    activityDataId, valToSaveT, yearId, projIndicatorId, userId, DBNull.Value});
                                }
                                valToSaveA = rnd.Next(1, 15);
                                if (!(valToSaveA == null))
                                {
                                    int newReportDetailId = DBContext.Add("InsertReportDetails",
                                                                            new object[] { ReportId, activityDataId, locationIdToSaveT, 
                                                                                            valToSaveA, isAccum, 1, userId, DBNull.Value });
                                    isAccum = false;
                                }
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                    }
                }
            }
        }

        private void GetReportId(DataTable dt)
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int projectId = RC.GetSelectedIntVal(rblProjects);

            using (ORSEntities db = new ORSEntities())
            {
                Report r = db.Reports.Where(x => x.ProjectId == projectId
                                            && x.YearId == yearId
                                            && x.MonthId == monthId
                                            && x.EmergencyLocationId == UserInfo.EmergencyCountry
                                            && x.OrganizationId == UserInfo.Organization).SingleOrDefault();
                ReportId = r != null ? r.ReportId : 0;
            }           
        }

        private void RemoveSelectedLocations(ListControl control)
        {
            foreach (ListItem item in control.Items)
            {
                item.Selected = false;
            }
        }

        protected void btnExcel_Export(object sender, EventArgs e)
        {
            SaveProjectData();
            UnCheckAllItemsOfListControl(cblMonths);
            UnCheckAllItemsOfListControl(cblExportProjects);
            SelectMonthAndProject();
            ExportDocumentType = 2;
            mpeExport.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (ExportDocumentType == 2)
            {
                dt = GetProjectsData(true);
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                string fileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_" + ddlMonth.SelectedItem.Text + "_Report";
                ExportUtility.ExportGridView(gv, fileName, ".xls", Response, true);
            }
            else
            {
                dt = GetProjectsData(false);
                GeneratePDF(dt);
            }
        }

        private DataTable GetProjectsData(bool isPivot)
        {
            int yearId = RC.GetSelectedIntVal(ddlYear);
            string monthIds = GetSelectedItems(cblMonths);
            string projectIds = GetSelectedItems(cblExportProjects);
            Guid userId = RC.GetCurrentUserId;

            string procedureName = "GetProjectsReportDataOfMultipleProjectsAndMonths";
            if (!isPivot)
            {
                procedureName = "GetProjectsDataByLocations";
            }

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(monthIds) && !string.IsNullOrEmpty(projectIds))
            {
                dt = DBContext.GetData(procedureName, new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, yearId, monthIds,
                                                                        projectIds, RC.SelectedSiteLanguageId, userId});

                //dt = DBContext.GetData(procedureName, new object[] { UserInfo.EmergencyCountry, locationIds, yearId, monthIds,
                //                                                        locIdsNotIncluded, RC.SelectedSiteLanguageId, userId,
                //                                                        UserInfo.Country, UserInfo.Organization, projectIds});
            }

            return dt;
        }

        protected void btnExportToExcelClose_Click(object sender, EventArgs e)
        {
            mpeExport.Hide();
        }

        private void SelectMonthAndProject()
        {
            cblMonths.SelectedValue = ddlMonth.SelectedValue;
            cblExportProjects.SelectedValue = rblProjects.SelectedValue;
        }

        private void UnCheckAllItemsOfListControl(ListControl control)
        {
            foreach (ListItem item in control.Items)
            {
                item.Selected = false;
            }
        }

        protected void btnPDF_Export(object sender, EventArgs e)
        {
            SaveProjectData();
            UnCheckAllItemsOfListControl(cblMonths);
            UnCheckAllItemsOfListControl(cblExportProjects);
            SelectMonthAndProject();
            ExportDocumentType = 1;
            mpeExport.Show();
        }

        private void GeneratePDF(DataTable dt)
        {
            using (iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream))
                    {
                        document.Open();

                        WriteDataEntryPDF.GenerateDocument(document, dt);

                        document.Close();

                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}.pdf", UserInfo.CountryName));
                        Response.BinaryWrite(outputStream.ToArray());
                    }
                }
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", RC.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "AddActivites", this.User);
        }

        #endregion

        #region Properties & Enums

        public int ReportId
        {
            get
            {
                int ReportId = 0;
                if (ViewState["ReportId"] != null)
                {
                    int.TryParse(ViewState["ReportId"].ToString(), out ReportId);
                }

                return ReportId;
            }
            set
            {
                ViewState["ReportId"] = value.ToString();
            }
        }

        public int LocationRemoved
        {
            get
            {
                int locationRemoved = 0;
                if (ViewState["LocationRemoved"] != null)
                {
                    int.TryParse(ViewState["LocationRemoved"].ToString(), out locationRemoved);
                }

                return locationRemoved;
            }
            set
            {
                ViewState["LocationRemoved"] = value.ToString();
            }
        }

        public int ExportDocumentType
        {
            get
            {
                int docType = 0;
                if (ViewState["ExportDocumentType"] != null)
                {
                    int.TryParse(ViewState["ExportDocumentType"].ToString(), out docType);
                }

                return docType;
            }
            set
            {
                ViewState["ExportDocumentType"] = value.ToString();
            }
        }

        #endregion

    }

    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType _templateType;
        private string _columnName;
        private string _locationId;
        private string _controlType;
        private string _txtBoxType;

        public GridViewTemplate(DataControlRowType type, string controlType, string colname, string locId, string txtBoxType = "Achieved")
        {
            _templateType = type;
            _controlType = controlType;
            _columnName = colname;
            _locationId = locId;
            _txtBoxType = txtBoxType;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            if (_templateType == DataControlRowType.Header)
            {
                string[] words = _columnName.Split('^');
                Label lc = new Label();
                lc.Width = 50;
                lc.Text = "<b>" + words[1] + "</b>";
                container.Controls.Add(lc);
            }
            else if (_templateType == DataControlRowType.DataRow)
            {
                if (_controlType == "TextBox")
                {
                    TextBox txtAchieved = new TextBox();
                    txtAchieved.CssClass = "numeric1";
                    txtAchieved.Width = 50;
                    txtAchieved.DataBinding += new EventHandler(this.txtAchieved_DataBinding);
                    container.Controls.Add(txtAchieved);
                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);
                    if (_txtBoxType == "Annual")
                    {
                        string color = RC.ConfigSettings("AnnualTargetTextBoxColor");
                        txtAchieved.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                    }
                }
                else if (_controlType == "CheckBox")
                {
                    CheckBox cbLocAccum = new CheckBox();
                    cbLocAccum.DataBinding += new EventHandler(this.cbAccum_DataBinding);
                    container.Controls.Add(cbLocAccum);
                    HiddenField hf = new HiddenField();
                    string[] words1 = _columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + _columnName;
                    container.Controls.Add(hf);
                }
            }
        }

        private void txtAchieved_DataBinding(Object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.ID = _columnName;
            txt.MaxLength = 12;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            txt.Text = DataBinder.Eval(row.DataItem, _columnName).ToString();
        }

        private void cbAccum_DataBinding(Object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cb.ID = _columnName;
            GridViewRow row = (GridViewRow)cb.NamingContainer;
            //cb.Text = DataBinder.Eval(row.DataItem, _columnName).ToString();
            bool isChecked = false;
            if ((DataBinder.Eval(row.DataItem, _columnName)).ToString() == "1")
                isChecked = true;
            cb.Checked = isChecked;
        }
    }
}