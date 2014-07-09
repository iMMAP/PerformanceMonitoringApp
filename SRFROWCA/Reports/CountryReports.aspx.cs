using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Reports
{
    public partial class CountryReports : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Request.QueryString["cname"] != null)
            {
                lblCountryName.Text = Request.QueryString["cname"].ToString() + " 2014";
            }
            
            LoadReportTypes();
        }

        internal override void BindGridData()
        {
            LoadReportTypes();
        }

        private void LoadReportTypes()
        {
            rptReportTypes.DataSource = GetReportTypes();
            rptReportTypes.DataBind();
        }

        private DataTable GetReportTypes()
        {
            DataTable dt = new DataTable();
            if (Request.QueryString["cid"] != null)
            {
                int countryId = 0;
                int.TryParse(Request.QueryString["cid"].ToString(), out countryId);
                if (countryId > 0)
                {
                    dt = DBContext.GetData("GetPublicCountryReportTypes", new object[] { countryId, RC.SelectedSiteLanguageId });
                }
            }

            return dt;
        }

        private DataTable GetReports(int countryId, int reportTypeId)
        {
            return DBContext.GetData("GetPublicCountryReports", new object[] { countryId, reportTypeId, RC.SelectedSiteLanguageId });
        }

        protected void rptReportTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hfReportTypeId = e.Item.FindControl("hfReportTypeTitle") as HiddenField;
            int reportId = 0;
            int.TryParse(hfReportTypeId.Value, out reportId);

            //Repeater rptReports = e.Item.FindControl("rptReports") as Repeater;
            GridView gvReports = e.Item.FindControl("gvReports") as GridView;

            if (Request.QueryString["cid"] != null)
            {
                int countryId = 0;
                int.TryParse(Request.QueryString["cid"].ToString(), out countryId);
                if (countryId > 0)
                {
                    //if (rptReports != null && reportId > 0)
                    //{
                    //    rptReports.DataSource = GetReports(countryId, reportId);
                    //    rptReports.DataBind();
                    if (gvReports != null && reportId > 0)
                    {
                        gvReports.DataSource = GetReports(countryId, reportId);
                        gvReports.DataBind();
                    }
                }
            }
        }

        public static Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
                return root;

            return root.Controls.Cast<Control>()
               .Select(c => FindControlRecursive(c, id))
               .FirstOrDefault(c => c != null);
        }

        //protected void rptReports_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "DeleteReport")
        //    {
        //        int countryReportId = 0;
        //        int.TryParse(e.CommandArgument.ToString(), out countryReportId);
        //        if (countryReportId > 0)
        //        {
        //            DBContext.Delete("DeleteCountryReport", new object[] { countryReportId, DBNull.Value });
        //            LoadReportTypes();
        //        }
        //    }
        //}

        protected void gvReports_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated || !this.User.IsInRole("Admin"))
            {
                Button btnDelete = e.Row.FindControl("btnDelete") as Button;
                if (btnDelete != null)
                {
                    btnDelete.Visible = false;
                }
            }
        }

        protected void gvReports_RowCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReport")
            {
                int countryReportId = 0;
                int.TryParse(e.CommandArgument.ToString(), out countryReportId);
                if (countryReportId > 0)
                {
                    DBContext.Delete("DeleteCountryReport", new object[] { countryReportId, DBNull.Value });
                    LoadReportTypes();
                }
            }
        }
    }
}