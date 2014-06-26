using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.LeadPages
{
    public partial class FundingListing : BasePage
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
            try
            {
                using (ORSEntities db = new ORSEntities())
                {
                    gvFunding.DataSource = db.FTSFundings;
                    gvFunding.DataBind();
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }
    }
}