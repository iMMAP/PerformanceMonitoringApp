using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.GV
{
    public partial class GVScroll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gv.DataSource = DBContext.GetData("getp");
                gv.DataBind();
            }
        }

        protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow gvHeaderRowCopy2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                this.gv.Controls[0].Controls.AddAt(0, gvHeaderRowCopy1);
                this.gv.Controls[0].Controls.AddAt(1, gvHeaderRowCopy2);

                TableCell tcFirst = e.Row.Cells[0];
                tcFirst.RowSpan = 3;
                gvHeaderRowCopy1.Cells.AddAt(0, tcFirst);

                TableCell tcTop = new TableCell();
                tcTop.Text = "Product Full Information";
                tcTop.ColumnSpan = 9;

                gvHeaderRowCopy1.Cells.AddAt(1, tcTop);

                TableCell tcBasic = new TableCell();
                tcBasic.Text = "Basic";
                tcBasic.ColumnSpan = 4;

                gvHeaderRowCopy2.Cells.AddAt(0, tcBasic);

                TableCell tcAddition = new TableCell();
                tcAddition.Text = "Addition";
                tcAddition.ColumnSpan = 5;

                gvHeaderRowCopy2.Cells.AddAt(1, tcAddition);
            }
        }
    }
}