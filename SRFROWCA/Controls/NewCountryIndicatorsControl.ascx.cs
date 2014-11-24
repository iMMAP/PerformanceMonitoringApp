using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Controls
{
    public partial class NewCountryIndicatorsControl : System.Web.UI.UserControl
    {
        public int ControlNumber { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();

            LoadCombos();
        }

        //public void SaveIndicators(int objectiveId, int countryId, int clusterId)
        public void SaveIndicators(int countryId, int clusterId)
        {
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();

            if (string.IsNullOrEmpty(indEn) && string.IsNullOrEmpty(indFr))
            {

            }
            string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";
            string cultureTarget = txtTarget.Text.Trim();

            if (RC.SelectedSiteLanguageId.Equals(2))
                cultureTarget = cultureTarget.Replace(".", ",");

            string target = decimal.Round(Convert.ToDecimal(cultureTarget, new CultureInfo(siteCulture)), 0).ToString();
            string unitId = ddlUnits.SelectedValue;
            
            Guid userId = RC.GetCurrentUserId;

            DBContext.Add("uspInsertIndicator", new object[] { indEn, indFr, target, unitId, countryId, clusterId, RC.GetCurrentUserId, null , null});
        }

        private void LoadCombos() 
        {
            UI.FillUnits(ddlUnits);
        }
    }
}