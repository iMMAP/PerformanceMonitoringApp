using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ORSDocuments
{
    public partial class orsdocs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadDocuments();
        }

        private void LoadDocuments()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                gvDocuments.DataSource = GetDocuments();
                gvDocuments.DataBind();
            }
        }

        private object GetDocuments()
        {
            return DBContext.GetData("GetDocumentsOnCountry", new object[] { UserInfo.EmergencyCountry });
        }


        protected void DownloadFile(object sender, EventArgs e)
        {
            
            string filePath = (sender as LinkButton).CommandArgument;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string fileExt = Path.GetExtension(filePath);            
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName.Replace(' ', '_'));

            if (fileExt == ".pdf")
            {
                Response.ContentType = "application/pdf";
            }
            else if (fileExt == ".doc" || fileExt == ".docx")
            {
                Response.ContentType = "application/ms-excel";
            }
            else if (fileExt == ".xls" || fileExt == ".xlsx")
            {
                Response.ContentType = "application/ms-word";
            }

            Response.WriteFile(filePath);
            Response.End();
        }

    }
}