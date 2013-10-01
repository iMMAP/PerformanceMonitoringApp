using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Reports
{
    public partial class ReportsDefault : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Control control = new Control();
            //MenuItemCollection reportItems = ReportMenu.Items;
            //foreach (MenuItem menuItem in reportItems)
            //{
            //    if (menuItem.Text == "T&A By Locations")
            //    {
            //        control = (AllTargetsAndAchieved)Page.LoadControl("AllTargetsAndAchieved.ascx");
            //    }

            //    if (menuItem.Text == "T&A By Duration")
            //    {
            //        control = (AllTargetsAndAchieved)Page.LoadControl("~/Pages/AllTargetsAndAchieved.ascx");
            //    }
            //}

            //pnlReport.Controls.Add(control);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ReportMenue_OnClick(object sender, MenuEventArgs e)
        {
            Control control = new Control();
            MenuItemCollection reportItems = ReportMenu.Items;

            if (e.Item.Text == "T&A By Locations")
            {
                control = (AllTargetsAndAchieved)Page.LoadControl("AllTargetsAndAchieved.ascx");
            }

            if (e.Item.Text == "T&A By Duration")
            {
                control = (AllTargetsAndAchieved)Page.LoadControl("~/Pages/AllTargetsAndAchieved.ascx");
            }

            pnlReport.Controls.Add(control);
        }
    }
}