
using SRFROWCA.Common;
using System;
namespace SRFROWCA
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            string postBackControl = Request.Form["__EventTarget"];

            if (!string.IsNullOrEmpty(postBackControl))
            {
                ROWCACommon.CultureSettings(postBackControl);
            }

            ROWCACommon.SetCulture();

            base.InitializeCulture();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        internal virtual void BindGridData(){}
    }
}