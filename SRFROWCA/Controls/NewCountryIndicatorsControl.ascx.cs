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
            string indEn = string.IsNullOrEmpty(txtInd1Eng.Text.Trim()) ? null : txtInd1Eng.Text.Trim();
            string indFr = string.IsNullOrEmpty(txtInd1Fr.Text.Trim()) ? null : txtInd1Fr.Text.Trim();
            int? target = !string.IsNullOrEmpty(txtTarget.Text.Trim()) ? Convert.ToInt32(txtTarget.Text.Trim()) : (int?)null;
            int unitId = RC.GetSelectedIntVal(ddlUnits);
            Guid userId = RC.GetCurrentUserId;

            if (countryId == 11)
            {
                DBContext.Add("InsertClusterIndicatorSahel", new object[] { indEn, indFr, target, unitId, countryId, clusterId, RC.GetCurrentUserId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertClusterIndicator", new object[] { indEn, indFr, target, unitId, countryId, clusterId, RC.GetCurrentUserId, DBNull.Value });
            }
        }

        private void LoadCombos()
        {
            UI.FillUnits(ddlUnits);
        }
    }
}