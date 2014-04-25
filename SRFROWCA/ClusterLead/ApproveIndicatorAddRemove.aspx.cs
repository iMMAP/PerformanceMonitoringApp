using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ApproveIndicatorAddRemove : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProjects();
                LoadIndicators();
            }
        }

        protected void gvAdded_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e);
                ObjPrToolTip.PrioritiesIconToolTip(e);
                ObjPrToolTip.RegionalIndicatorIcon(e, 11);
                ObjPrToolTip.CountryIndicatorIcon(e, 12);
            }
        }

        protected void gvDeleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e);
                ObjPrToolTip.PrioritiesIconToolTip(e);
                ObjPrToolTip.RegionalIndicatorIcon(e, 11);
                ObjPrToolTip.CountryIndicatorIcon(e, 12);
            }
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadIndicators();
        }

        private void LoadIndicators()
        {
            LoadAddedIndicators();
            LoadDeletedIndicators();
        }

        private void LoadAddedIndicators()
        {
            int? projectId = Convert.ToInt32(rblProjects.SelectedValue) > 0 ? Convert.ToInt32(rblProjects.SelectedValue) : (int?)null;
            gvAdded.DataSource = DBContext.GetData("GetAddedProjectIndicatorsToApprove", new object[] { UserInfo.EmergencyCountry,
                                                                                                        UserInfo.EmergencyCluster,
                                                                                                        projectId,
                                                                                                        RC.SelectedSiteLanguageId });
            gvAdded.DataBind();
        }

        private void LoadDeletedIndicators()
        {
            int? projectId = Convert.ToInt32(rblProjects.SelectedValue) > 0 ? Convert.ToInt32(rblProjects.SelectedValue) : (int?)null;
            gvDeleted.DataSource = DBContext.GetData("GetDeletedProjectIndicatorsToApprove", new object[] { UserInfo.EmergencyCountry,
                                                                                                            UserInfo.EmergencyCluster,
                                                                                                            projectId,
                                                                                                            RC.SelectedSiteLanguageId });
            gvDeleted.DataBind();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvAdded.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projIndId = Convert.ToInt32(gvAdded.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());
                    CheckBox cb = gvAdded.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            ApproveAddedIndicator(projIndId);
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvDeleted.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projIndId = Convert.ToInt32(gvDeleted.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());
                    CheckBox cb = gvDeleted.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            ApproveDeletedIndicator(projIndId);
                        }
                    }
                }
            }

            LoadProjects();
            LoadIndicators();
            //AddNotification("CLApproveComments", UserInfo.EmergencyCountry, UserInfo.EmergencyCluster)
        }

        private void ApproveAddedIndicator(int projIndId)
        {
            DBContext.GetData("ApproveAddedProjectIndicator", new object[] { projIndId });
        }

        private void ApproveDeletedIndicator(int projIndId)
        {
            DBContext.GetData("ApproveDeletedProjectIndicator", new object[] { projIndId });
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvAdded.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projIndId = Convert.ToInt32(gvAdded.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());
                    CheckBox cb = gvAdded.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            RejectAddedIndicator(projIndId);
                        }
                    }
                }
            }

            foreach (GridViewRow row in gvDeleted.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int projIndId = Convert.ToInt32(gvDeleted.DataKeys[row.RowIndex].Values["ProjectIndicatorId"].ToString());
                    CheckBox cb = gvDeleted.Rows[row.RowIndex].FindControl("chkApproved") as CheckBox;
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {
                            RejectDeletedIndicator(projIndId);
                        }
                    }
                }
            }
            LoadProjects();
            LoadIndicators();
        }

        private void RejectAddedIndicator(int projIndId)
        {
            DBContext.GetData("RejectAddedProjectIndicator", new object[] { projIndId });
        }

        private void RejectDeletedIndicator(int projIndId)
        {
            DBContext.GetData("RejectDeletedProjectIndicator", new object[] { projIndId });
        }

        private void LoadProjects()
        {
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectCode";
            rblProjects.DataSource = DBContext.GetData("GetProjectsToApproveIndicatorsAddRemoved", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster });
            rblProjects.DataBind();

            if (rblProjects.Items.Count > 0)
            {
                rblProjects.SelectedIndex = 0;
            }
        }
    }
}