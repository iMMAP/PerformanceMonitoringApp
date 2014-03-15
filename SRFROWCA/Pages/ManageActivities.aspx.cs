using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using BusinessLogic;
using SRFROWCA.Common;


namespace SRFROWCA.Pages
{
    public partial class ManageActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
                PopulateProjects();
                PopulateObjectives();
                PopulatePriorities();

                if (rblProjects.Items.Count > 0)
                {
                    rblProjects.SelectedIndex = 0;
                    PopulateLogFrame();
                }
            }
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
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
            bool? isOPSProject = null;
            DataTable dt = DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject });
            Session["testprojectdata"] = dt;
            return dt;
        }

        private void SaveOPSIndicator()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            int? orgId = null;
            Guid userId = RC.GetCurrentUserId;
            DBContext.Delete("DeleteIndicatorFromProject", new object[] { projectId, 0, DBNull.Value, DBNull.Value });
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());

                    CheckBox cbIsSRP = gvIndicators.Rows[row.RowIndex].FindControl("cbIsSRP") as CheckBox;
                    if (cbIsSRP == null) return;
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

        private void PopulateLogFrame()
        {
            DataTable dtIndicators = new DataTable();

            int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            int projectClusterId = 0;
            using (ORSEntities db = new ORSEntities())
            {
                projectClusterId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyClusterId).SingleOrDefault();
            }

            if (projectClusterId > 0)
            {
                dtIndicators = DBContext.GetData("GetProjectIndicators", new object[] { projectClusterId, UserInfo.EmergencyCountry, projectId, 1 });
            }

            gvIndicators.DataSource = dtIndicators;
            gvIndicators.DataBind();
        }

        private DataTable GetProjectCluster(int projectId)
        {
            return DBContext.GetData("GetProjectCluster", new object[] { projectId });
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            PopulateLogFrame();
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

        protected void cbIsAdded_CheckedChanged(object sender, EventArgs e)
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            if (projectId == 0) return;

            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox isAdded = gvIndicators.Rows[index].FindControl("cbIsAdded") as CheckBox;
            if (isAdded == null) return;

            int indicatorId = 0;
            int.TryParse(gvIndicators.DataKeys[index].Values["ActivityDataId"].ToString(), out indicatorId);

            if (indicatorId > 0)
            {
                CheckBox cbIsSRP = gvIndicators.Rows[index].FindControl("cbIsSRP") as CheckBox;
                if (cbIsSRP == null) return;

                Guid userId = RC.GetCurrentUserId;
                int yearId = 10;
                DBContext.Add("InsertProjectIndicator", new object[] { projectId, indicatorId, UserInfo.EmergencyCountry, yearId, isAdded.Checked, userId, DBNull.Value });
            }
        }
    }
}