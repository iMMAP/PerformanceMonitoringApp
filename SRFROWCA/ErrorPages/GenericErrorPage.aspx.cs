using System;
using SRFROWCA.Common;

namespace SRFROWCA.ErrorPages
{
    public partial class GenericErrorPage : System.Web.UI.Page
    {
        protected Exception ex = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get the last error from the server
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                // Create a safe message
                string safeMsg = "A problem has occurred in the web site. ";

                // Show Inner Exception fields for local access
                if (ex.InnerException != null)
                {
                    innerTrace.Text = ex.InnerException.StackTrace;
                    InnerErrorPanel.Visible = Request.IsLocal;
                    innerMessage.Text = ex.InnerException.Message;
                }
                // Show Trace for local access
                if (Request.IsLocal)
                    exTrace.Visible = true;
                else
                    ex = new Exception(safeMsg, ex);

                // Fill the page fields
                exMessage.Text = ex.Message;
                exTrace.Text = ex.StackTrace;

                ExceptionUtility.NotifySystemOps(ex, "OfficeListing.aspx", this.User);

                // Clear the error from the server
                Server.ClearError();
            }
        }
    }
}