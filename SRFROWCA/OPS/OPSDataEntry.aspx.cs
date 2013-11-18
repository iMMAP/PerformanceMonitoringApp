using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.OPS
{
    public partial class OPSDataEntry : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSave.UniqueID;

            if (!IsPostBack)
            {
                PopulateDropDowns();
                PopulateLocations(LocationId);
                LoadClusters();
            }

            string controlName = GetPostBackControlId(this);
            if (controlName == "ddlMonth" || controlName == "ddlOffice")
            {
                LocationRemoved = 0;
                lstSelectedLocations.Items.Clear();
            }

            //DataTable dtActivities = GetActivities();
            //AddDynamicColumnsInGrid(dtActivities);
            //GetReport(dtActivities);
        }

        private void AddDynamicColumnsInGrid(DataTable dt, GridView gv)
        {
            foreach (DataColumn column in dt.Columns)
            {
                TemplateField customField = new TemplateField();
                // Create the dynamic templates and assign them to 
                // the appropriate template property.

                string columnName = column.ColumnName;
                if (!(columnName == "ReportId" || columnName == "ClusterName" || columnName == "IndicatorName" || columnName == "ActivityDataId" || columnName == "ActivityName" || columnName == "IsActive"))
                {
                    customField.ItemTemplate = new GridViewTemplate(DataControlRowType.DataRow, column.ColumnName, "1");
                    customField.HeaderTemplate = new GridViewTemplate(DataControlRowType.Header, column.ColumnName, "1");
                    gv.Columns.Add(customField);
                }
            }
        }

        // Populate Clusters Grid.
        private void LoadClusters()
        {
            PopulateClusters();            
        }

        private void PopulateClusters()
        {
            DataTable dt = new DataTable();
            int emgLocationId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocationId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            if (emgLocationId > 0 && officeId > 0)
            {
                dt = GetEmergencyClusters(1);
            }

            gvClusters.DataSource = dt;
            gvClusters.DataBind();
        }

        private DataTable GetEmergencyClusters(int emgLocationId)
        {
            return DBContext.GetData("GetEmergencyClustersOfData", new object[] { emgLocationId });
        }

        #region Drop Downs Events & Methods.

        #region Drop Down Events
        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            //BindGridData();
            //AddLocationsInSelectedList();
        }
        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            //BindGridData();
            //AddLocationsInSelectedList();
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            //BindGridData();
            //AddLocationsInSelectedList();
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationRemoved = 0;
            //AddLocationsInSelectedList();
            //BindGridData();
        }
        #endregion

        #region Drop Downs Methods.

        private void PopulateDropDowns()
        {
            // Get details of user from aspnet_Users_Custom tbale
            DataTable dt = ROWCACommon.GetUserDetails();
            if (dt.Rows.Count > 0)
            {
                lblCountry.Text = dt.Rows[0]["LocationName"].ToString();
                lblOrganization.Text = dt.Rows[0]["OrganizationName"].ToString();

                // Set Header of Location List Box.
                lblLocationLevelOfCountry.Text = "Admin2 Locations of " + lblCountry.Text;

                LocationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                int organizationId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());

                PopulateLocationEmergencies(LocationId);
                PopulateOffices(LocationId, organizationId);

            }

            // Populate Year Drop Down.
            var result = DateTime.Parse(DateTime.Now.ToShortDateString(), new CultureInfo("en-US")).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));

            // Populate Months Drop Down.
            var result1 = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result1.ToString()));
        }

        // Populate Emergency Drop Down.
        private void PopulateLocationEmergencies(int locationId)
        {
            ddlEmergency.DataValueField = "LocationEmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";

            ddlEmergency.DataSource = GetLocationEmergencies(locationId);
            ddlEmergency.DataBind();

            if (ddlEmergency.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddlEmergency.Items.Insert(0, item);
            }
        }
        private DataTable GetLocationEmergencies(int locationId)
        {
            DataTable dt = DBContext.GetData("GetLocationEmergencies", new object[] { locationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Office Drop Down.
        private void PopulateOffices(int locationId, int organizationId)
        {
            ddlOffice.DataValueField = "OfficeId";
            ddlOffice.DataTextField = "OfficeName";

            ddlOffice.DataSource = GetOffices(locationId, organizationId);
            ddlOffice.DataBind();

            if (ddlOffice.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Your Office", "0");
                ddlOffice.Items.Insert(0, item);
            }
        }
        private DataTable GetOffices(int locationId, int organizationId)
        {
            DataTable dt = DBContext.GetData("GetOrganizationOffices", new object[] { locationId, organizationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Months Drop Down
        private void PopulateMonths()
        {
            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            ListItem item = new ListItem("Select Month", "0");
            ddlMonth.Items.Insert(0, item);

            var result = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(result.ToString()));
        }
        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate Years Drop Down
        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            ListItem item = new ListItem("Select Year", "0");
            ddlYear.Items.Insert(0, item);

            var result = DateTime.Parse(DateTime.Now.ToShortDateString(), new CultureInfo("en-US")).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));
        }
        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        #endregion

        #endregion

        #region Locations Popup Events & Methods.
        private void PopulateLocations(int parentLocationId)
        {
            lstLocations.DataValueField = "LocationId";
            lstLocations.DataTextField = "LocationName";

            lstLocations.DataSource = GetChildLocations(parentLocationId);
            lstLocations.DataBind();

            if (lstLocations.Items.Count > 0)
            {
                lstLocations.SelectedIndex = 0;
            }
        }

        private object GetChildLocations(int parentLocationId)
        {
            DataTable dt = DBContext.GetData("GetThirdLevelChildLocations", new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        #endregion

        #region Class Utility Methods
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
        #endregion

        #region Grid Events & Methods

        #region gvClusters Events & Methods

        protected void gvClusters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvStrObjectives = e.Row.FindControl("gvStrObjectives") as GridView;
            if (gvStrObjectives != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int clusterId = 0;
                int.TryParse(dr["EmergencyClusterId"].ToString(), out clusterId);

                // Get all activities and bind grid.
                gvStrObjectives.DataSource = GetStrObjectives(clusterId);
                gvStrObjectives.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetStrObjectives(int clusterId)
        {
            return DBContext.GetData("GetStrategicObjectives", new object[] { clusterId });
        }
        #endregion

        #region gvStrObjectives Events & Methods
        protected void gvStrObjectives_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvSpcObjectives = e.Row.FindControl("gvSpcObjectives") as GridView;
            if (gvSpcObjectives != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int strObjId = 0;
                int.TryParse(dr["StrategicObjectiveId"].ToString(), out strObjId);

                // Get all activities and bind grid.
                gvSpcObjectives.DataSource = GetSpecificObjectives(strObjId);
                gvSpcObjectives.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetSpecificObjectives(int strObjId)
        {
            return DBContext.GetData("GetAllSpecifObjectivesOfAStrObjective", new object[] { strObjId });
        }
        #endregion

        #region gvSpecificObjectives Events & Methods

        protected void gvSpcObjectives_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvIndicators = e.Row.FindControl("gvIndicators") as GridView;
            if (gvIndicators != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int spcObjId = 0;
                int.TryParse(dr["ClusterObjectiveId"].ToString(), out spcObjId);

                // Get all activities and bind grid.
                gvIndicators.DataSource = GetIndicators(spcObjId);
                gvIndicators.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetIndicators(int spcObjId)
        {
            return DBContext.GetData("GetObjectiveIndicators", new object[] { spcObjId });
        }

        #endregion

        #region gvIndicators Events & Methods
        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvActivities = e.Row.FindControl("gvActivities") as GridView;
            if (gvActivities != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int indId = 0;
                int.TryParse(dr["ObjectiveIndicatorId"].ToString(), out indId);

                // Get all activities and bind grid.
                gvActivities.DataSource = GetActivities(indId);
                gvActivities.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetActivities(int indId)
        {
            return DBContext.GetData("GetIndicatorActivities", new object[] { indId });
        }
        #endregion

        #region gvActivites Events & Methods
        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvData = e.Row.FindControl("gvData") as GridView;
            if (gvData != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int activityId = 0;
                int.TryParse(dr["IndicatorActivityId"].ToString(), out activityId);

                // Get all activities and bind grid.
                gvData.DataSource = GetData(activityId);
                gvData.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetData(int activityId)
        {
            return DBContext.GetData("GetActivityData", new object[] { activityId });
        }
        #endregion

        #region gvData Events & Methods.
        protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvLocations = e.Row.FindControl("gvLocations") as GridView;
            if (gvLocations != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int dataId = 0;
                int.TryParse(dr["ActivityDataId"].ToString(), out dataId);

                // Get all activities and bind grid.
                DataTable dt = GetDataLocations(dataId);
                gvLocations.DataSource = dt;
                gvLocations.DataBind();

                // Attch row command event with grid.
                //gvStrObjective.RowCommand += gvStrObjective_RowCommand;
            }
        }
        private DataTable GetDataLocations(int dataId)
        {
            int locEmergencyId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out locEmergencyId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            int yearId = 0;
            int.TryParse(ddlYear.SelectedValue, out yearId);

            int monthId = 0;
            int.TryParse(ddlMonth.SelectedValue, out monthId);

            string locationIds = GetSelectedLocationIds();
            string locIdsNotIncluded = GetNotSelectedLocationIds();

            Guid userId = ROWCACommon.GetCurrentUserId();
            DataTable dt = DBContext.GetData("GetIPDataLocations", new object[] { locEmergencyId, locationIds, officeId,
                                                                                  yearId, monthId, locIdsNotIncluded,
                                                                                  userId, dataId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private string GetSelectedLocationIds()
        {
            string locationIds = "";

            for (int i = 0; i < lstSelectedLocations.Items.Count; i++)
            {
                if (locationIds != "")
                {
                    locationIds += "," + lstSelectedLocations.Items[i].Value;
                }
                else
                {
                    locationIds += lstSelectedLocations.Items[i].Value;
                }
            }

            return locationIds;
        }

        private string GetNotSelectedLocationIds()
        {
            string locIdsNotIncluded = "";
            if (LocationRemoved == 1)
            {
                for (int i = 0; i < lstLocations.Items.Count; i++)
                {
                    if (locIdsNotIncluded != "")
                    {
                        locIdsNotIncluded += "," + lstLocations.Items[i].Value;
                    }
                    else
                    {
                        locIdsNotIncluded += lstLocations.Items[i].Value;
                    }
                }
            }

            return locIdsNotIncluded;
        }
        #endregion        

        #endregion

        #region Button Events
        protected void btnSave_Click(object sender, EventArgs e)
        { }

        protected void btnLocation_Click(object sender, EventArgs e)
        { }

        protected void btnAdd_Click(object sender, EventArgs e)
        { }

        protected void btnRemove_Click(object sender, EventArgs e)
        { }
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
        public int LocationId
        {
            get
            {
                int locationId = 0;
                if (ViewState["LocationId"] != null)
                {
                    int.TryParse(ViewState["LocationId"].ToString(), out locationId);
                }

                return locationId;
            }
            set
            {
                ViewState["LocationId"] = value.ToString();
            }
        }
        public int Count1
        {
            get
            {
                int count1 = 0;
                if (ViewState["Count1"] != null)
                {
                    int.TryParse(ViewState["Count1"].ToString(), out count1);
                }

                return count1;
            }
            set
            {
                ViewState["Count1"] = value.ToString();
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

        #endregion
    }

    #region Template Class

    public class GridViewTemplate : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private string locationId;

        public GridViewTemplate(DataControlRowType type, string colname, string locId)
        {
            templateType = type;
            columnName = colname;
            locationId = locId;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            // Create the content for the different row types.
            switch (templateType)
            {
                case DataControlRowType.Header:
                    string[] words = columnName.Split('^');
                    Label lc = new Label();
                    lc.Width = 50;
                    lc.Text = "<b>" + words[1] + "</b>";
                    container.Controls.Add(lc);
                    //CheckBox cb = new CheckBox();
                    //if (columnName.Contains('T'))
                    //{
                    //    cb.ID = columnName;
                    //    container.Controls.Add(cb);
                    //}
                    break;
                case DataControlRowType.DataRow:
                    TextBox firstName = new TextBox();
                    firstName.CssClass = "numeric1";
                    firstName.Width = 50;
                    firstName.DataBinding += new EventHandler(this.FirstName_DataBinding);
                    container.Controls.Add(firstName);
                    HiddenField hf = new HiddenField();
                    string[] words1 = columnName.Split('^');
                    hf.Value = words1[0];
                    hf.ID = "hf" + columnName;
                    container.Controls.Add(hf);
                    break;

                default:
                    // Insert code to handle unexpected values.
                    break;
            }
        }

        private void FirstName_DataBinding(Object sender, EventArgs e)
        {
            TextBox l = (TextBox)sender;
            l.ID = columnName;
            l.MaxLength = 12;
            GridViewRow row = (GridViewRow)l.NamingContainer;
            l.Text = DataBinder.Eval(row.DataItem, columnName).ToString();
        }
    }
    #endregion
}