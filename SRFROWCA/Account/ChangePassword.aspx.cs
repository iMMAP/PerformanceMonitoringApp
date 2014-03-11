using System;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class ChangePassword : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "ChangePassword", this.User);
        }
    }
}
