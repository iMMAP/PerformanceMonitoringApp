using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SRFROWCA.OPS
{
    public partial class GridTestOPS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataTable dt = DBContext.GetData("GetOPSActivities4", new object[] { 12,13, 1});
                rptIndicators.DataSource = dt;
                rptIndicators.DataBind();
            }

        }

        protected void rptIndicators_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                

                HiddenField hfLocTypeId = e.Item.FindControl("hfLocTypeId") as HiddenField;
                if (hfLocTypeId != null)
                {
                    if (hfLocTypeId.Value != "2")
                    {
                        HtmlTableRow tr = e.Item.FindControl("trIndTarget") as HtmlTableRow;
                        if (tr != null)
                        {
                            tr.Attributes.Add("class", "details0");
                        }

                        EmptyLable(e.Item, "lblObjective");
                        EmptyLable(e.Item, "lblActivity");
                        EmptyLable(e.Item, "lblIndicator");
                        EmptyLable(e.Item, "lblUnit");
                        EmptyLable(e.Item, "lblCalMethod");
                    }
                    else
                    {
                        EmptyLable(e.Item, "lblLocationName");
                    }
                }
            }
        }

        private void EmptyLable(RepeaterItem item, string elemName)
        {
            Label lbl = item.FindControl(elemName) as Label;
            if (lbl != null)
            {
                lbl.Text = "";
            }
        }
    }
}