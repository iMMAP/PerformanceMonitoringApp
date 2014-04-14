using System;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class ChangePassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, "ChangePassword", User);
        }
    }
}
