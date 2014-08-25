using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Controls
{
    public partial class ReportedIndicatorComments : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        public bool LoadComments(int reportId, int activityDataId)
        {
            bool isApproved = false;

            using (ORSEntities db = new ORSEntities())
            {
                DataTable dtResults = DBContext.GetData("GetIndicatorComments", new object[] { reportId, activityDataId });

                if (dtResults.Rows.Count > 0)
                    if (Convert.ToString(dtResults.Rows[0]["IsApproved"]).Equals("1"))
                        isApproved = true;

                rptIndComments.DataSource = dtResults;
                rptIndComments.DataBind();
            }

            hdnUpdate.Value = "-1";
            fcComments.Value = string.Empty;

            return isApproved;
        }

        public string GetComments()
        {
            string comments = fcComments.Value;
            fcComments.Value = "";

            return comments;
        }

        public bool CheckIfUpdate()
        {
            return !string.IsNullOrEmpty(hdnUpdate.Value);
        }

        public int GetIndicatorCommentDetailID()
        {
            int ID = Convert.ToInt32(hdnUpdate.Value);
            hdnUpdate.Value = "-1";

            return ID;
        }
    }
}