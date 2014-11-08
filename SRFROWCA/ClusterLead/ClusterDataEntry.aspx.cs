using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterDataEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClusterIndicators();
                PopulateYears();
                PopulateMonths();

                SetDates();
            }
        }

        private void SetDates()
        {
            int month = DateTime.Now.Month-1;
            int year = 11;// DateTime.Now.Year;

            if (month.Equals(12))
                month = 1;
            else if (month.Equals(11))
                month = 12;

            ddlMonth.SelectedValue = month.ToString();
            ddlYear.SelectedValue = year.ToString();
        }

        private void LoadClusterIndicators()
        {
            gvIndicators.DataSource = GetClusterIndicatros(null, null, null);
            gvIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string indicator)
        {
            return DBContext.GetData("uspGetClusterIndicators", new object[] { clusterId, countryId, indicator, RC.SelectedSiteLanguageId });
        }

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

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);

            int monthNumber = MonthNumber.GetMonthNumber(result);
            monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;
            
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }
    }
}