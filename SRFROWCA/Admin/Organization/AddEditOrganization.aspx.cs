using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin.Organization
{
    public partial class AddEditOrganization : System.Web.UI.Page
    {
        private BusinessLogic.Organization objOrganization = new BusinessLogic.Organization();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSave.UniqueID;
            if (IsPostBack) return;
            BindData();

        }
        private void BindData()
        {
            LoadOrganizationTypes();
            LoadCountries();
            if (!string.IsNullOrEmpty(Request.QueryString["oid"]))
            {
                PopulateForm();
            }
            
        }          

        private void LoadOrganizationTypes()
        {
            ddlType.DataSource = DBContext.GetData("GetOrganizationTypes");
            ddlType.DataTextField = "OrganizationType";
            ddlType.DataValueField = "OrganizationTypeId";
            ddlType.DataBind();

        }

        private void LoadCountries()
        {
            int locTypeId = 2;
            object[] parameters = new object[] { locTypeId };
            ddlCountry.DataSource = DBContext.GetData("GetLocationOnType", parameters);
            ddlCountry.DataTextField = "LocationName";
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataBind();

        }
        private void PopulateForm()
        {
            BusinessLogic.Organization obj = new BusinessLogic.Organization();
            obj = objOrganization.GetOrganizationByID(Convert.ToInt32(Utils.DecryptQueryString(Request.QueryString["oid"])));
            if (obj != null)
            {
                txtOrgName.Text = obj.Name;
                txtOrgAcronym.Text = obj.Acronym;
                ddlType.SelectedValue = obj.TypeId.ToString();
                ddlCountry.SelectedValue = obj.CountryId.ToString();
                txtPhone.Text = obj.Phone;
                txtAddress.Text = obj.Address;
                chkStatus.Checked = obj.Status;
            }
        }
        private void SaveOrganization()
        {
            try
            {              
                int retVal = 0;                
                objOrganization.Name = txtOrgName.Text;
                objOrganization.Acronym = txtOrgAcronym.Text;
                objOrganization.TypeId = Convert.ToInt32(ddlType.SelectedValue);
                objOrganization.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
                objOrganization.Status = chkStatus.Checked;
                objOrganization.Phone = txtPhone.Text;
                objOrganization.Address = txtAddress.Text;
                objOrganization.UserId = (Guid)Membership.GetUser().ProviderUserKey;
                if (string.IsNullOrEmpty(Request.QueryString["oid"]))
                {
                    retVal = objOrganization.Add();
                } 
                else
                {
                    objOrganization.Id = Convert.ToInt32(Utils.DecryptQueryString(Request.QueryString["oid"]));
                    retVal = objOrganization.Update();
                }
                if (retVal > 0)
                {
                    Response.Redirect(string.Format("{0}/Admin/Organization/OrganizationList.aspx", Master.BaseURL));
                }
            }
            catch (Exception ex)
            {
                string message = "";
                if (ex.Message.Contains("Cannot insert duplicate key row in object"))
                {
                    message = "Organization with same name already exists!";
                }
                else
                {
                    message = "Some error occoured!";
                }
                
                RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, RC.NotificationType.Error, false);
               
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrganization();
        }

       
    }
}