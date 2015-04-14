using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Novacode;
using System.Drawing;

namespace SRFROWCA
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;

            //string fileName = @"e:\work\winword\DocXExample.docx";

            //// Create a document in memory:
            //var document = DocX.Create(fileName);

            //// Get the first Table in this document.
            //Novacode.Table table = document.InsertTable(5, 2);
            ////table.Design = TableDesign.MediumGrid1;

            ////// Insert a new column to this right of this table.
            ////table.InsertColumn(0);
            ////table.InsertColumn(1);

            ////// Set the new coloumns text to "Row no."
            ////table.Rows[0].Cells[table.ColumnCount - 1].Paragraphs[0].InsertText("Row no.", false);

            //table.Rows[0].MergeCells(0, 1);
            //Cell cellClusterOutPutInd = table.Rows[0].Cells[0];
            //cellClusterOutPutInd.FillColor = Color.LightGray;
            //cellClusterOutPutInd.VerticalAlignment = VerticalAlignment.Center;
            //Formatting f = new Formatting();
            //f.FontColor = Color.Blue;
            //f.Size = 10;
            //f.FontFamily = new FontFamily("Arial");
            //Paragraph p = cellClusterOutPutInd.Paragraphs[0];
            
            //p.InsertText("Cluster Outputput Indicator", false, f);
            

            //// Loop through each row in the table.
            //for (int i = 1; i < table.Rows.Count; i++)
            //{
                


            //    //// The cell in this row that belongs to the new coloumn.
            //    //Cell cell = row.Cells[0];
            //    //cell.FillColor = Color.LightGray;

            //    //// The Paragraph that this cell houses.
            //    //Paragraph p = cell.Paragraphs[0];

            //    //// Insert this rows index.
            //    //p.InsertText("New indicator", false);

            //    //Cell cell1 = row.Cells[1];

            //    //// The Paragraph that this cell houses.
            //    //Paragraph p1 = cell1.Paragraphs[0];

            //    //// Insert this rows index.
            //    //p1.InsertText("0", false);
            //}

            //document.Save();


            //// Open in Word:
            //Process.Start("WINWORD.EXE", fileName);

        }
    }
}