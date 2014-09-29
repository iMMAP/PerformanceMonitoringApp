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
                PopulateMapTypes();
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
                txtMapTitle.Text = dt.Rows[0]["MapTitle"].ToString();
                ddlCountry.SelectedValue = dt.Rows[0]["LocationId"].ToString();
                ddlMapType.SelectedValue = dt.Rows[0]["MapTypeId"].ToString();
                ltrFileName.Text = dt.Rows[0]["MapUrl"].ToString();
                trFileName.Visible = true;
                chkActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                chkIsPublic.Checked = Convert.ToBoolean(dt.Rows[0]["IsPublic"]);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                trFileName.Visible = false;
                string fileName = fuMap.HasFile ? fuMap.PostedFile.FileName : ltrFileName.Text;

                object[] param = new object[] {0,Convert.ToInt32(Request.QueryString["id"]),txtMapTitle.Text,ddlCountry.SelectedValue,ddlMapType.SelectedValue,
                                           fileName,chkActive.Checked,chkIsPublic.Checked};
                DBContext.Add("UpdateCountryMap", param);
                if (fuMap.HasFile)
                {
                    UploadFile();
                }
                Response.Redirect("CountryMapsListing.aspx");
            }
            else
            {
                object[] param = new object[] {0,txtMapTitle.Text,ddlCountry.SelectedValue,ddlMapType.SelectedValue,
                                           fuMap.PostedFile.FileName,chkActive.Checked,chkIsPublic.Checked,
                                           (Guid)Membership.GetUser().ProviderUserKey,(int)RC.SiteLanguage.English};
                DBContext.Add("InsertCountryMap", param);
                UploadFile();
                ShowMessage("Map has been saved successfully!");
            }
            
            
            
        }

        private void UploadFile()
        {                                   
            string uploadDir = Server.MapPath(@"~/ORSMaps");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string uploadFileLocation = uploadDir + "//" + fuMap.PostedFile.FileName; 
            fuMap.SaveAs(uploadFileLocation);           
        }

        private void PopulateMapTypes()
        {
            ddlMapType.DataValueField = "MapTypeId";
            ddlMapType.DataTextField = "MapTypeTitle";
            ddlMapType.DataSource = GetMapTypes();
            ddlMapType.DataBind();
        }

        private DataTable GetMapTypes()
        {
            return DBContext.GetData("GetAllMapTypes");
        }

        private void PopulateLocations()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";
            ddlCountry.DataSource = GetLocations();
            ddlCountry.DataBind();
        }

        private DataTable GetLocations()
        {
            return DBContext.GetData("GetAllCountryMapLocations");
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
    }
}