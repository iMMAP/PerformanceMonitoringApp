using System;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class ChangePasswordSuccess : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
