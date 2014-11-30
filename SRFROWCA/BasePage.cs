
using SRFROWCA.Common;
using System;
using System.Web;
using System.Web.UI.WebControls;
namespace SRFROWCA
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            string postBackControl = Request.Form["__EventTarget"];

            if (!string.IsNullOrEmpty(postBackControl))
            {
                RC.CultureSettings(postBackControl);
            }

            RC.SetCulture();

            base.InitializeCulture();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
            //int emergencyId = 1;

            //var pageHandler = HttpContext.Current.CurrentHandler;
            //if (pageHandler is System.Web.UI.Page)
            //{
            //    DropDownList ddlEmergency = ((System.Web.UI.Page)pageHandler).Master.FindControl("ddlEmgergency") as DropDownList;
            //    if (ddlEmergency != null)
            //    {
            //        string s = ddlEmergency.SelectedValue;
            //    }
            //}


            //UserInfo.UserProfileInfo(emergencyId);
        }

        internal virtual void BindGridData(){}
    }
}