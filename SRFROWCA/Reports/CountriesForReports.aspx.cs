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
    public partial class CountriesForReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCountries();
        }

        protected void gvCountries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Data.DataRowView drv = e.Row.DataItem as System.Data.DataRowView;
                e.Row.Attributes.Add("onclick", String.Format("window.location='CountryReports.aspx?cid={0}&cname={1}'", drv["LocationId"], drv["LocationName"]));
            }

        }

        protected void gvCountries_RowCommand(object sender, GridViewCommandEventArgs e)
        { }

        private void LoadCountries()
        {
            DataTable dt = RC.GetLocations(this.User, 2);
            //DataTable dt = GetLocaitons();
            gvCountries.DataSource = dt;
            gvCountries.DataBind();
        }

        //private DataTable GetLocaitons()
        //{
        //    return DBContext.GetData("GetCountryForCountryReports", new object[] { RC.SelectedSiteLanguageId });
        //}
    }
}