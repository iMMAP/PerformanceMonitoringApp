using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.IO;
using System.Drawing;

namespace SRFROWCA
{
    public static class ExportUtility
    {
        public static void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "Yes" : "No"));
                }
                else if (current is Button)
                {
                    control.Controls.Remove(current);
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

        

        private static StringWriter RenderGrid(GridView gv)
        {
            //  Create a table to contain the grid
            Table table = new Table();
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            //  include the gridline settings
            table.GridLines = gv.GridLines;

            //  add the header row to the table
            if (gv.HeaderRow != null)
            {
                ExportUtility.PrepareControlForExport(gv.HeaderRow);
                gv.HeaderRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#ddd");
                gv.HeaderRow.BorderColor = System.Drawing.ColorTranslator.FromHtml("#bbbbbb");
                foreach (TableCell cell in gv.HeaderRow.Cells)
                {
                    cell.BackColor = gv.HeaderStyle.BackColor;
                }
                
                table.Rows.Add(gv.HeaderRow);
            }          

            //  add each of the data rows to the table
            foreach (GridViewRow row in gv.Rows)
            {
                ExportUtility.PrepareControlForExport(row);
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#ebf0f4");
                    }
                    else
                    {
                        cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#fbfbfb");
                    }
                    //cell.BackColor = row.BackColor;
                    cell.CssClass = "textmode";
                }

                //row.CssClass = "istrow";
                table.Rows.Add(row);
            }

            //  add the footer row to the table
            if (gv.FooterRow != null)
            {
                ExportUtility.PrepareControlForExport(gv.FooterRow);
                table.Rows.Add(gv.FooterRow);
            }

            //  render the table into the htmlwriter
            table.RenderControl(htw);

            return sw;
        }

        internal static void ExportGridView(Control control, string fileName, string fileExtention, HttpResponse Response)
        {
            ExportGridView(control, fileName, fileExtention, Response, false);
        }

        internal static void ExportGridView(Control control, string fileName, string fileExtention, HttpResponse Response, bool disposePassedControl)
        {
            ExportUtility.PrepareControlForExport(control);
            string[] nameWithSpaces = fileName.Split(' ');
            fileName = string.Join("-", nameWithSpaces);
            fileName += DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + fileExtention;
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            GridView gv = control as GridView;

            Response.Write(RenderGrid(gv).ToString());
            if (disposePassedControl)
            {
                ///control.Dispose();
            }

            Response.End();
        }

        //public static void ExportWithBorder(string fileName, GridView gv)
        //{
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.AddHeader(
        //        "content-disposition", string.Format("attachment; filename={0}", fileName));
        //    HttpContext.Current.Response.ContentType = "application/ms-excel";

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        using (HtmlTextWriter htw = new HtmlTextWriter(sw))
        //        {
        //            //  Create a form to contain the grid
        //            Table table = new Table();

        //            //  add the header row to the table
        //            if (gv.HeaderRow != null)
        //            {
        //                PrepareControlForExport1(gv.HeaderRow);
        //                gv.HeaderRow.Style.Add("border", "solid 1px #c1d8f1");
        //                table.Rows.Add(gv.HeaderRow);
        //            }

        //            //  add each of the data rows to the table
        //            foreach (GridViewRow row in gv.Rows)
        //            {
        //                PrepareControlForExport1(row);
        //                row.Style.Add("border", "solid 1px #c1d8f1");
        //                table.Rows.Add(row);
        //            }

        //            //  add the footer row to the table
        //            if (gv.FooterRow != null)
        //            {
        //                PrepareControlForExport1(gv.FooterRow);
        //                gv.FooterRow.Style.Add("border", "solid 1px #c1d8f1");
        //                table.Rows.Add(gv.FooterRow);
        //            }

        //            //  render the table into the htmlwriter                    
        //            table.RenderControl(htw);

        //            //  render the htmlwriter into the response
        //            HttpContext.Current.Response.Write(sw.ToString());
        //            HttpContext.Current.Response.End();
        //        }
        //    }
        //}
    }
}
