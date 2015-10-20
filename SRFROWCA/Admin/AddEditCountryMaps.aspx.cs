using System;
using System.Web.UI;
using System.Data;
using BusinessLogic;
using System.IO;
using SRFROWCA.Common;
using System.Web.Security;

namespace SRFROWCA.Admin
{
    public partial class AddEditCountryMaps : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateLocations();
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    LoadCountryMap();
                }
            }
        }

        private void LoadCountryMap()
        {
            DataTable dt = DBContext.GetData("GetCountryMap", new object[] { Convert.ToInt32(Request.QueryString["id"]),true });
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCountry.SelectedValue = dt.Rows[0]["EmergencyLocationId"].ToString();
                txtMapTitleEn.Text = dt.Rows[0]["MapTitleEn"].ToString();
                txtMapTitleFr.Text = dt.Rows[0]["MapTitleFr"].ToString();
                ltrFileName.Text = dt.Rows[0]["MapUrl"].ToString();
                trFileName.Visible = true;
                chkIsPublic.Checked = Convert.ToBoolean(dt.Rows[0]["IsPublic"]);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int countryMapId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int.TryParse(Request.QueryString["id"].ToString(), out countryMapId);
            }

            trFileName.Visible = false;
            string fileName = ltrFileName.Text;
            string titleEn = txtMapTitleEn.Text.Trim();
            string titleFr = txtMapTitleFr.Text.Trim();
            titleFr = string.IsNullOrEmpty(titleFr) ? titleEn : titleFr;
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);

            if (countryMapId > 0)
            {
                if (fuMap.HasFile)
                {
                    fileName = UploadFile();
                }
                DBContext.Add("UpdateCountryMap", new Object[] {countryMapId, emgLocationId, titleEn, titleFr, 
                                                                 fileName, chkIsPublic.Checked, RC.GetCurrentUserId, DBNull.Value});
                Response.Redirect("CountryMapsListing.aspx");
            }
            else
            {
                fileName = UploadFile();
                DBContext.Add("InsertCountryMap", new Object[] {emgLocationId, titleEn, titleFr, 
                                                                 fileName, chkIsPublic.Checked, RC.GetCurrentUserId, DBNull.Value});
                Response.Redirect("CountryMapsListing.aspx");
            }
        }

        private string UploadFile()
        {
            string path = fuMap.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fuMap.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fuMap.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + fileExtension;
            string uploadDir = Server.MapPath(@"~/ORSMaps");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            string uploadFileLocation = uploadDir + "//" + fileName;
            fuMap.SaveAs(uploadFileLocation);
            return fileName;
        }

        private void PopulateLocations()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("CountryMapsListing.aspx");
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}