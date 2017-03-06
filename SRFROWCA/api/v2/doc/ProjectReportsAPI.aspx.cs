using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace SRFROWCA.api.v2.doc
{
    public partial class ProjectReportsAPI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            DataTable dt = DBContext.GetData("GetAllOrganizations");
            UI.FillOrganizations(ddlOrg, dt);
            if (ddlOrg.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlOrg.Items.Insert(0, item);
            }
        }
    }
}