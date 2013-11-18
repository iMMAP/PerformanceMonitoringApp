using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;

namespace SRFROWCA.OPS
{
    public partial class OPSActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void btnAdd_Click(object sender, EventArgs e)
        { }

        protected void btnDelete_Click(object sender, EventArgs e)
        { }

        protected void gvClusters_RowDataBound(object sender, GridViewRowEventArgs e)
        { }

        protected void gvActivities_RowCommand(object sender, GridViewCommandEventArgs e)
        { }
    }
}