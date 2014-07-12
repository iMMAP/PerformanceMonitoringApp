using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Controls
{
    public partial class AddContryReportControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveCountryReport(int locationId, int isPublic)
        {
            string enTitle = txtEnReportTitle.Text.Trim();
            string enURL = txtEnReportURL.Text.Trim();
            string frTitle = txtFrReportTitle.Text.Trim();
            string frURL = txtFrReportURL.Text.Trim();
            int reportTypeId = Convert.ToInt32(txtEnReportTypeId.Text.Trim());
            Guid userId = RC.GetCurrentUserId;

            DBContext.Add("InsertCountryReport", new object[] {enTitle, enURL, frTitle, frURL, locationId, reportTypeId, userId, isPublic, DBNull.Value });
        }
    }
}