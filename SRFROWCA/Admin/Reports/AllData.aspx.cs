using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Drawing;
using System.IO;

namespace SRFROWCA.Admin.Reports
{
    public partial class AllData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.Identity.IsAuthenticated || !this.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (IsPostBack) return;
            LoadData();
        }

        private void LoadData()
        {
            gvReport.DataSource = DBContext.GetData("GetAllTasksDataReportPivot");
            gvReport.DataBind();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int j = 0; j < e.Row.Cells.Count; j++)
                {
                    TableCell cell = e.Row.Cells[j];
                    cell.Wrap = false;

                    if (j > 8)
                    {
                        cell.HorizontalAlign = HorizontalAlign.Right;
                    }
                }
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportUtility.PrepareGridViewForExport(gvReport);
            ExportGridView();
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        private void ExportGridView()
        {
            string attachment = "attachment; filename=3wopreport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            gvReport.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}