using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.LeadPages
{
    public partial class FundingListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFunding();
            }
        }

        private void LoadFunding()
        {
            using (ORSEntities db = new ORSEntities())
            {
                gvFunding.DataSource = db.FTSFundings;
                gvFunding.DataBind();
            }
        }
    }
}