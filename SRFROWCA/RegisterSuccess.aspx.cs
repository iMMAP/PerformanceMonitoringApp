using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA
{
    public partial class RegisterSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["RegisterMessage"] != null)
                {
                    string message = Session["RegisterMessage"].ToString();
                    lblMessage.Text = message;
                    Session["RegisterMessage"] = null;

                }
            }
        }
    }
}