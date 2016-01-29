using SRFROWCA.Common;
using System;
using System.Web;


namespace SRFROWCA
{
    public partial class _Default : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo.UserProfileInfo(RC.SelectedEmergencyId);
            }

            if (!IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (RC.IsClusterLead(this.User))
                        Response.Redirect("~/Landing/ClusterCord.aspx");
                    else if (RC.IsDataEntryUser(this.User))
                        Response.Redirect("~/Landing/ImplementingPartners.aspx");
                    else if (RC.IsCountryAdmin(this.User))
                        Response.Redirect("~/Landing/OCHALanding.aspx");
                    else
                        Response.Redirect("Dashboard.aspx");
                }
            }
        }

        internal override void BindGridData()
        {
            base.BindGridData();
        }

    }
}