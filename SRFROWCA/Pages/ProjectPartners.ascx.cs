using BusinessLogic;
using Saplin.Controls;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SRFROWCA.Pages
{
    public partial class ProjectPartners : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateObjectives();
                PopulateLocations();
                PopulateActivities();
            }

            string controlName = GetPostBackControlId(this.Page);

            if (controlName == "rblProjects")
            {
                LocationRemovedPartner = 0;
                RemoveSelectedLocations(cblAdmin1);
            }
        }

        internal void PopulateActivities()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            string locationIds = GetSelectedLocations();
            string locIdsNotIncluded = GetNotSelectedLocations();
            Guid userId = RC.GetCurrentUserId;

            DataTable dt = DBContext.GetData("GetProjectImplementingPartners", new object[] { locationIds, 11, locIdsNotIncluded, 
                                                                                                RC.SelectedSiteLanguageId, projectId});
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
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
            if (LocationRemovedPartner == 1)
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


        private string GetSelectedLocations()
        {
            return GetSelectedItems(cblAdmin1);
        }

        private string GetNotSelectedLocations()
        {
            return GetNotSelectedItems(cblAdmin1);
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);

                DropDownCheckBoxes ddlOrgs = e.Row.FindControl("ddlOrgs") as DropDownCheckBoxes;
                if (ddlOrgs != null)
                {
                    PopulateOrganizations(ddlOrgs);
                    int activityId = Convert.ToInt32(gvActivities.DataKeys[e.Row.RowIndex].Values["ActivityId"].ToString());
                    int locationId = Convert.ToInt32(gvActivities.DataKeys[e.Row.RowIndex].Values["LocId"].ToString());
                    SelectProjectPartnerOrgs(ddlOrgs, activityId, locationId);
                }
            }
        }

        private void SelectProjectPartnerOrgs(DropDownCheckBoxes ddlOrgs, int activityId, int locationId)
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            DataTable dt = DBContext.GetData("GetProjectPartners", new object[] { projectId, activityId, locationId, 11 });
            foreach (DataRow dr in dt.Rows)
            {
                foreach (ListItem item in ddlOrgs.Items)
                {
                    if (item.Value == dr["OrganizationId"].ToString())
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void PopulateOrganizations(DropDownCheckBoxes ddlOrgs)
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            ddlOrgs.DataValueField = "OrganizationId";
            ddlOrgs.DataTextField = "OrganizationAcronym";
            ddlOrgs.DataSource = DBContext.GetData("GetProjectPartnerOrganizations", new object[] {projectId });
            ddlOrgs.DataBind();

        }

        private DataTable GetOrganizations(int? orgId, string projIDs)
        {
            return DBContext.GetData("GetAllOrganizations");
        }

        public void PopulateLocations()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            int projectEmgLocationId = 0;
            using (ORSEntities db = new ORSEntities())
            {
                projectEmgLocationId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyLocationId).SingleOrDefault();
            }

            if (projectEmgLocationId > 0)
            {
                PopulateAdmin1(projectEmgLocationId);
            }
        }

        private void PopulateAdmin1(int parentLocationId)
        {
            DataTable dt = GetAdmin1Locations(parentLocationId);
            cblAdmin1.DataValueField = "LocationId";
            cblAdmin1.DataTextField = "LocationName";
            cblAdmin1.DataSource = dt;
            cblAdmin1.DataBind();
        }

        private DataTable GetAdmin1Locations(int emgLocationId)
        {
            DataTable dt = DBContext.GetData("GetAdmin1OfEmergnecyLocation", new object[] { emgLocationId, (int)RC.LocationCategory.Government });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private DataTable GetUserProjects()
        {
            return RC.GetOrgProjectsOnLocation(null);
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true, RC.SelectedEmergencyId);
        }



        private void PopulateToolTips()
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        private void ProjectsToolTip(ListControl ctl, DataTable dt)
        {
            foreach (ListItem item in ctl.Items)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (item.Text == row["ProjectCode"].ToString())
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                }
            }
        }

        private void RemoveSelectedLocations(ListControl control)
        {
            foreach (ListItem item in control.Items)
            {
                item.Selected = false;
            }
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddLocationsInSelectedList();
        }

        private void AddLocationsInSelectedList()
        {
            PopulateLocations();
        }

        protected void btnLocation_Click(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterStartupScript(GetType(), "key", "launchModal();", true);
            LocationRemovedPartner = 1;
            List<int> locationIds = GetLocationIdsFromGrid();
            SelectLocationsOfGrid(locationIds);
        }

        private void SelectLocationsOfGrid(List<int> locationIds)
        {
            foreach (ListItem item in cblAdmin1.Items)
            {
                item.Selected = locationIds.Contains(Convert.ToInt32(item.Value));
            }
        }

        private List<int> GetLocationIdsFromGrid()
        {
            List<int> locationIds = new List<int>();
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int locationId = 0;
                    locationId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["LocId"].ToString());

                    if (locationId > 0)
                        locationIds.Add((locationId));
                }
            }

            return locationIds.Distinct().ToList();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SavePartners();
        }

        private void SavePartners()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int projectId = ((ManageProject)this.Page).ProjectId;
                DBContext.Delete("DeleteProjectPartners", new object[] { projectId, DBNull.Value });
                SaveProjectPartners(projectId);
                scope.Complete();
            }
        }

        private void SaveProjectPartners(int projectId)
        {
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownCheckBoxes ddlOrgs = row.FindControl("ddlOrgs") as DropDownCheckBoxes;
                    if (ddlOrgs != null)
                    {
                        if (IsOrgSelected(ddlOrgs))
                        {
                            int activityId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["ActivityId"].ToString());
                            int projPartnerActId = SaveProjectPartnerActivity(projectId, activityId);

                            int locationId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["LocId"].ToString());
                            int projPartnerActLocId = SaveProjectPartnerLocation(projPartnerActId, locationId);

                            SaveProjectpartnerLocationOrgs(projPartnerActLocId, ddlOrgs);
                        }
                    }
                }
            }
        }

        private void SaveProjectpartnerLocationOrgs(int projPartnerActLocId, DropDownCheckBoxes ddlOrgs)
        {
            foreach (ListItem item in ddlOrgs.Items)
            {
                if (item.Selected)
                {
                    int orgId = 0;
                    int.TryParse(item.Value, out orgId);
                    if (orgId > 0)
                    {
                        DBContext.Add("InsertProjectPartnerOrganizations", new object[] { projPartnerActLocId, orgId, DBNull.Value });
                    }
                }
            }
        }

        private int SaveProjectPartnerLocation(int projPartnerActId, int locationId)
        {
            return DBContext.Add("InsertProjectPartnerLocations", new object[] { projPartnerActId, locationId, DBNull.Value });
        }

        private int SaveProjectPartnerActivity(int projectId, int activityId)
        {
            return DBContext.Add("InsertProjectPartnerActivities", new object[] { projectId, activityId, 11, RC.GetCurrentUserId, DBNull.Value });
        }

        private bool IsOrgSelected(DropDownCheckBoxes ddlOrgs)
        {
            bool isSelected = false;
            foreach (ListItem item in ddlOrgs.Items)
            {
                if (item.Selected)
                {
                    isSelected = true;
                    break;
                }
            }

            return isSelected;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PopulateActivities();
        }

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

        private int LocationRemovedPartner
        {
            get
            {
                int locationRemoved = 0;
                if (ViewState["LocationRemovedPartner"] != null)
                {
                    int.TryParse(ViewState["LocationRemovedPartner"].ToString(), out locationRemoved);
                }

                return locationRemoved;
            }
            set
            {
                ViewState["LocationRemovedPartner"] = value;
            }
        }

    }
}