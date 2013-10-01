using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace SRFROWCA
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pdfpath = Server.MapPath("img2");

            string imagepath = @"E:\img\";
            //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                doc.Open();                
                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "temp1.jpg");
                doc.Add(gif);
            }

            catch (Exception ex)
            {
                //Log error;
            }
            finally
            {
                doc.Close();
            }
        }
    }
}