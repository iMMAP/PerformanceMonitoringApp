﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;

namespace SRFROWCA.ErrorPages
{
    public partial class DefaultRedirectErrorPage : System.Web.UI.Page
    {
        protected HttpException ex = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Log the exception and notify system operators
            ex = new HttpException("defaultRedirect");
            ExceptionUtility.LogException(ex, "Caught in DefaultRedirectErrorPage");
            ExceptionUtility.NotifySystemOps(ex);
        }
    }
}