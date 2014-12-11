
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
        }

        internal virtual void BindGridData(){}
    }
}