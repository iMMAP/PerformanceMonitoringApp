using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class ManageActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjects();
                PopulateObjectives();
                PopulatePriorities();

                if (rblProjects.Items.Count > 0)
                {
                    rblProjects.SelectedIndex = 0;
                    PopulateLogFrame();
                }
            }

            //PopulateTragetsGrid();
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
        }

        private void PopulateLocations()
        {
            cblAdmin2.DataValueField = "LocationId";
            cblAdmin2.DataTextField = "LocationName";
            DataTable dt = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { 2 });
            cblAdmin2.DataSource = dt;
            cblAdmin2.DataBind();

            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";

            cblAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { 2 });
            cblAdmin1.DataBind();
        }

        private void PopulateProjects()
        {
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectCode";

            rblProjects.DataSource = GetUserProjects();
            rblProjects.DataBind();
        }

        private DataTable GetUserProjects()
        {
            DataTable dt = DBContext.GetData("GetOPSAndORSUserProjects", new object[] { UserInfo.GetCountry, UserInfo.GetOrganization });
            Session["testprojectdata"] = dt;
            return dt;
        }

        protected void wzrdReport_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 1)
            {
                PopulateLocations();
                SaveOPSIndicator();
            }
            else if (e.NextStepIndex == 2)
            {
                PopulateTragetsGrid();
            }
        }

        private void PopulateTragetsGrid()
        {
            DataTable dt = GetIndicatorsAndLocationsForTarget();
            AddDynamicColumnsInGrid(dt);
            gvTargts.DataSource = dt;
            gvTargts.DataBind();
            Session["dtProjectIndicators"] = dt;
        }

        private void SaveOPSIndicator()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            int? orgId = null;
            Guid userId = RC.GetCurrentUserId;
            DBContext.Delete("DeleteIndicatorFromProject", new object[] {projectId, 0, DBNull.Value, DBNull.Value });
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());

                    CheckBox cbIsSRP = gvIndicators.Rows[row.RowIndex].FindControl("cbIsSRP") as CheckBox;
                    if (cbIsSRP == null)return;
                    if (!cbIsSRP.Checked)
                    {
                        CheckBox cbIsAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                        if (cbIsAdded == null) return;

                        int isOPS = 1;
                        int isActive = Convert.ToInt32(cbIsAdded.Checked);

                        DBContext.Update("UpdateOPSProjectIndicatorStatus", new object[] { projectId, indicatorId, isOPS, isActive, orgId, userId, DBNull.Value });
                    }
                    else
                    {
                        CheckBox cbIsAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                        if (cbIsAdded == null) return;

                        if (cbIsAdded.Checked)
                        {
                            int isActive = Convert.ToInt32(cbIsAdded.Checked);
                            int projSelectedIndId = DBContext.Add("InsertProjectIndicator2",
                                                                    new object[] { projectId, indicatorId, 0, isActive, orgId, userId, DBNull.Value });
                        }
                    }
                }
            }
        }

        protected void wzrdReport_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 0)
            {
                PopulateProjects();
                PopulateLocations();
            }
        }

        private DataTable GetIndicatorsAndLocationsForTarget()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);

            int clusterId = 0;
            DataTable dt = Session["testprojectdata"] as DataTable;

            DataTable dtIndicators = new DataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ProjectId"].ToString() == projectId.ToString())
                    {
                        clusterId = Convert.ToInt32(dr["ClusterId"]);
                    }
                }

                string selectedAdmin1 = GetSelectedItems(cblAdmin1);
                string selectedAdmin2 = GetSelectedItems(cblAdmin2);
                string locationSelected = selectedAdmin1 + "," + selectedAdmin2;

                string notSelectedAdmin1 = GetNotSelectedItems(cblAdmin1);
                string notSelectedAdmin2 = GetNotSelectedItems(cblAdmin2);
                string locationNotSelectedIds = ""; // notSelectedAdmin1 + "," + notSelectedAdmin2;
                string actIds = GetSelectedIndicators();
                int lngId = 1;
                dtIndicators = DBContext.GetData("GetProjectIndicatorsForTargets", new object[] { 1, 
                                                                                                    clusterId,  locationSelected, 
                                                                                                    locationNotSelectedIds,
                                                                                                    actIds, lngId});
            }
            return dtIndicators;
        }

        private void AddDynamicColumnsInGrid(DataTable dt)
        {
            foreach (DataColumn column in dt.Columns)
            {
                TemplateField customField = new TemplateField();
                // Create the dynamic templates and assign them to 
                // the appropriate template property.

                string columnName = column.ColumnName;
                if (!(columnName == "ReportId" || columnName == "ClusterName" || columnName == "Objective" ||
                    columnName == "HumanitarianPriority" || columnName == "SecondaryCluster" || columnName == "ActivityName" ||
                    columnName == "DataName" || columnName == "IsActive" || columnName == "ActivityDataId" ||
                    columnName == "ProjectTitle" || columnName == "ProjectId" ||
                    columnName == "ObjAndPrId" || columnName == "ObjectiveId" || columnName == "HumanitarianPriorityId" ||
                    columnName == "objAndPrAndPId" || columnName == "objAndPId" || columnName == "PrAndPId"))
                {
                    customField.ItemTemplate = new GridViewTemplateIndTarget(DataControlRowType.DataRow, column.ColumnName, "1");
                    customField.HeaderTemplate = new GridViewTemplateIndTarget(DataControlRowType.Header, column.ColumnName, "1");
                    gvTargts.Columns.Add(customField);
                }
            }
        }

        private string GetSelectedIndicators()
        {
            string actIds = "";
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    CheckBox cbIsSRP = gvIndicators.Rows[row.RowIndex].FindControl("cbIsSRP") as CheckBox;
                    if (cbIsSRP != null)
                    {
                        if (cbIsSRP.Checked)
                        {
                            CheckBox cb = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                            if (cb != null)
                            {
                                if (cb.Checked)
                                {
                                    if (actIds != "")
                                    {
                                        actIds += "," + indicatorId.ToString();
                                    }
                                    else
                                    {
                                        actIds += indicatorId.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return actIds;
        }

        protected void wzrdReport_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            //Save();
            //Response.Redirect("~/Pages/AddActivities.aspx");
        }

        private void PopulateLogFrame()
        {
            int projectId = Convert.ToInt32(rblProjects.SelectedValue);

            int clusterId = 0;
            DataTable dt = Session["testprojectdata"] as DataTable;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ProjectId"].ToString() == projectId.ToString())
                {
                    clusterId = Convert.ToInt32(dr["ClusterId"]);
                }
            }

            int emgId = 1;
            int locationId = UserInfo.GetCountry;

            gvIndicators.DataSource = DBContext.GetData("GetOPSandORSProjectIndicators", new object[] { emgId, 
                                                                                                        clusterId, locationId, 
                                                                                                                   projectId, 1 });
            gvIndicators.DataBind();
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            PopulateLogFrame();
        }

        private void Save()
        {
            SaveReportDetails();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgObj = e.Row.FindControl("imgObjective") as Image;
                if (imgObj != null)
                {
                    string txt = e.Row.Cells[0].Text;
                    if (txt.Contains("1"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so1.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                    }
                    else if (txt.Contains("2"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so2.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                    }
                    else if (txt.Contains("3"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so3.png";
                        imgObj.ToolTip = " STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                    }
                }
                Image imghp = e.Row.FindControl("imgPriority") as Image;
                if (imghp != null)
                {
                    string txtHP = e.Row.Cells[1].Text;
                    if (txtHP == "1")
                    {
                        imghp.ImageUrl = "~/images/icon/hp1.png";
                        imghp.ToolTip = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                    }
                    else if (txtHP == "2")
                    {
                        imghp.ImageUrl = "~/images/icon/hp2.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                    }
                    else if (txtHP == "3")
                    {
                        imghp.ImageUrl = "~/images/icon/hp3.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                    }
                    else if (txtHP == "4")
                    {
                        imghp.ImageUrl = "~/images/icon/hp4.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Food insecurity";
                    }
                    else if (txtHP == "5")
                    {
                        imghp.ImageUrl = "~/images/icon/hp5.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Malnutrition";
                    }

                }
            }
        }

        protected void gvTargts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgObj = e.Row.FindControl("imgObjective") as Image;
                if (imgObj != null)
                {
                    string txt = e.Row.Cells[0].Text;
                    if (txt.Contains("1"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so1.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                    }
                    else if (txt.Contains("2"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so2.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                    }
                    else if (txt.Contains("3"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so3.png";
                        imgObj.ToolTip = " STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                    }
                }
                Image imghp = e.Row.FindControl("imgPriority") as Image;
                if (imghp != null)
                {
                    string txtHP = e.Row.Cells[1].Text;
                    if (txtHP == "1")
                    {
                        imghp.ImageUrl = "~/images/icon/hp1.png";
                        imghp.ToolTip = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                    }
                    else if (txtHP == "2")
                    {
                        imghp.ImageUrl = "~/images/icon/hp2.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                    }
                    else if (txtHP == "3")
                    {
                        imghp.ImageUrl = "~/images/icon/hp3.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                    }
                    else if (txtHP == "4")
                    {
                        imghp.ImageUrl = "~/images/icon/hp4.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Food insecurity";
                    }
                    else if (txtHP == "5")
                    {
                        imghp.ImageUrl = "~/images/icon/hp5.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Malnutrition";
                    }

                }
            }
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

        protected void cbIsAdded_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SaveReportDetails()
        {
            int activityDataId = 0;
            int projectId = RC.GetSelectedIntVal(rblProjects);
            foreach (GridViewRow row in gvTargts.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow) return;
                activityDataId = Convert.ToInt32(gvTargts.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());

                int colummCounts = gvTargts.Columns.Count;
                DataTable dtIndicators = (DataTable)Session["dtProjectIndicators"];
                List<KeyValuePair<int, decimal?>> dataSave = new List<KeyValuePair<int, decimal?>>();
                int i = 0;
                foreach (DataColumn dc in dtIndicators.Columns)
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

                    if (locationId > 0 && value != null)
                    {
                        Guid userId = RC.GetCurrentUserId;
                        int? orgId = null;
                        int projSelectedIndId = DBContext.Add("InsertProjectIndicator", 
                                                                new object[] { projectId, activityDataId, 0, 1, orgId, userId, DBNull.Value});
                        int newReportDetailId = DBContext.Add("InsertAnnualTargetOfIndicatorOnLocation",
                                                                new object[] { activityDataId, locationId, 
                                                                                            value, 1, DBNull.Value });
                    }
                }

            }
        }

    }

    public class GridViewTemplateIndTarget : ITemplate
    {
        private DataControlRowType templateType;
        private string columnName;
        private string locationId;

        public GridViewTemplateIndTarget(DataControlRowType type, string colname, string locId)
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
                    break;
                case DataControlRowType.DataRow:
                    TextBox firstName = new TextBox();
                    firstName.CssClass = "numeric1";
                    firstName.Width = 55;
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
}