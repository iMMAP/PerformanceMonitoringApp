using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using System.IO;

namespace SRFROWCA
{
    public static class ExportUtility
    {
        //public static void PrepareGridViewForExport1(Control gv)
        //{
        //    LinkButton lb = new LinkButton();
        //    Literal l = new Literal();
        //    string name = String.Empty;
        //    for (int i = 0; i < gv.Controls.Count; i++)
        //    {
        //        if (gv.Controls[i].GetType() == typeof(LinkButton))
        //        {
        //            l.Text = (gv.Controls[i] as LinkButton).Text;
        //            gv.Controls.Remove(gv.Controls[i]);
        //            gv.Controls.AddAt(i, l);
        //        }
        //        else if (gv.Controls[i].GetType() == typeof(DropDownList))
        //        {
        //            l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
        //            gv.Controls.Remove(gv.Controls[i]);
        //            gv.Controls.AddAt(i, l);
        //        }
        //        else if (gv.Controls[i].GetType() == typeof(CheckBox))
        //        {
        //            l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
        //            gv.Controls.Remove(gv.Controls[i]);
        //            gv.Controls.AddAt(i, l);
        //        }
        //        else if (gv.Controls[i].GetType() == typeof(Button))
        //        {
        //            //l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
        //            gv.Controls.Remove(gv.Controls[i]);
        //            //gv.Controls.AddAt(i, l);
        //        }

        //        if (gv.Controls[i].HasControls())
        //        {
        //            PrepareGridViewForExport(gv.Controls[i]);
        //        }
        //    }
        //}

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
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
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
                table.Rows.Add(gv.HeaderRow);
            }

            //  add each of the data rows to the table
            foreach (GridViewRow row in gv.Rows)
            {
                ExportUtility.PrepareControlForExport(row);
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
            ExportUtility.PrepareControlForExport(control);
            fileName += DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + fileExtention;
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            GridView gv = control as GridView;
            Response.Write(RenderGrid(gv).ToString());
            Response.End();
        }
    }
}
