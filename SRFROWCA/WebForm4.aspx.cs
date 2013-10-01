using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            GridView1.DataSource = DBContext.GetData("GetTargetAndAchievedByMonthAndLocationPivot_t");
            GridView1.DataBind();
        }
    }
}