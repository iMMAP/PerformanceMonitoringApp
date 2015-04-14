using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace SRFROWCA.ClusterLead
{
    public partial class AddEditKeyFigure : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("MM-dd-yyyy");
                txtFromDate.Attributes.Add("readonly", "readonly");
                LoadCombos();
                DisableDropDowns();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {                    
                    PopulateForm();
                    txtFromDate.Enabled = false;
                    ddlCategory.Enabled = false;
                    ddlCountry.Enabled = false;
                    ddlCategory.BackColor = Color.LightGray;
                    ddlCountry.BackColor = Color.LightGray;
                }
               
            }


        }

        private void PopulateForm()
        {
            DataTable dt = DBContext.GetData("GetKeyFiguresByID", new object[] { RC.SelectedSiteLanguageId, Utils.DecryptQueryString(Request.QueryString["id"])});
            if (dt != null && dt.Rows.Count > 0)
            {
                txtFromDate.Text = Convert.ToDateTime(dt.Rows[0]["CreatedDate"].ToString()).ToString("dd-MM-yyyy");
                ddlCategory.SelectedValue = dt.Rows[0]["CategoryID"].ToString();
                ddlCountry.SelectedValue = dt.Rows[0]["EmergencyLocationID"].ToString();
                txtInd1Eng.Text = dt.Rows[0]["FigureEn"].ToString();
                txtInd1Fr.Text = dt.Rows[0]["FigureFr"].ToString();
                ddlUnit.SelectedValue = dt.Rows[0]["Unit"].ToString();
                txtNeed.Text = dt.Rows[0]["PopulationInNeed"].ToString();
                txtPopulationTarget.Text = dt.Rows[0]["PopulationTargeted"].ToString();
                txtNeedMen.Text = dt.Rows[0]["Men"].ToString();
                txtNeedWomen.Text = dt.Rows[0]["Women"].ToString();
                txtNeedGirls.Text = dt.Rows[0]["Girls"].ToString();
                txtNeedBoys.Text = dt.Rows[0]["Boys"].ToString();
                txtTargetMen.Text = dt.Rows[0]["TargetMen"].ToString();
                txtTargetWomen.Text = dt.Rows[0]["TargetWomen"].ToString();
                txtTargetGirls.Text = dt.Rows[0]["TargetGirls"].ToString();
                txtTargetBoys.Text = dt.Rows[0]["TargetBoys"].ToString();
                ddlSource.SelectedValue = dt.Rows[0]["Source"].ToString();
                if (ddlSource.SelectedValue == "Other")
                {
                    txtOther.Text = dt.Rows[0]["Other"].ToString();
                }
            }
        }

      

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);          
            UI.FillUnits(ddlUnit);           
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "0"));
          
            ddlUnit.Items.Insert(0, new ListItem("--- Select Unit ---", "0"));
            PopulateCategories();
            ddlCategory.Items.Insert(0, new ListItem("--- Select Category ---", "0"));
            SetComboValues();
        }

        private void PopulateCategories()
        {
            ddlCategory.DataValueField = "KeyFigureCategoryID";
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataSource = DBContext.GetData("GetKeyFigureCategories");
            ddlCategory.DataBind();

        }

        internal override void BindGridData()
        {
            //PopulateClusters();
            //PopulateObjective();

        }

        private void SetComboValues()
        {
            if (!RC.IsAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }           
        }
       
        // Disable Controls on the basis of user profile
        private void DisableDropDowns()
        {
            if (!RC.IsAdmin(this.User))
            {                
                RC.EnableDisableControls(ddlCountry, false);
            }           
        }

        private void SaveNewKeyFigure()
        {           
            DBContext.Add("InsertKeyFigure", new object[] { 
                Convert.ToInt32(ddlCountry.SelectedValue),
                 Convert.ToInt32(ddlCategory.SelectedValue),
                 string.IsNullOrEmpty(txtNeed.Text) ? (int?)null : Convert.ToInt32(txtNeed.Text.Trim()),
                 string.IsNullOrEmpty(txtPopulationTarget.Text) ? (int?)null : Convert.ToInt32(txtPopulationTarget.Text.Trim()),
                 ddlSource.SelectedValue == "0" ? null : ddlSource.SelectedValue,
                 string.IsNullOrEmpty(txtOther.Text) ? null : txtOther.Text.Trim(),
                 string.IsNullOrEmpty(txtNeedMen.Text) ? (int?)null : Convert.ToInt32(txtNeedMen.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedWomen.Text) ? (int?)null : Convert.ToInt32(txtNeedWomen.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedGirls.Text) ? (int?)null : Convert.ToInt32(txtNeedGirls.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedBoys.Text) ? (int?)null : Convert.ToInt32(txtNeedBoys.Text.Trim()),
                 Convert.ToDateTime(txtFromDate.Text),
                 RC.GetCurrentUserId,
                 RC.GetCurrentUserId,
                 Convert.ToInt32(ddlUnit.SelectedValue),
                 string.IsNullOrEmpty(txtTargetMen.Text) ? (int?)null : Convert.ToInt32(txtTargetMen.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetWomen.Text) ? (int?)null : Convert.ToInt32(txtTargetWomen.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetGirls.Text) ? (int?)null : Convert.ToInt32(txtTargetGirls.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetBoys.Text) ? (int?)null : Convert.ToInt32(txtTargetBoys.Text.Trim()),
                 string.IsNullOrEmpty(txtInd1Eng.Text) ? null : txtInd1Eng.Text.Trim(),
                 string.IsNullOrEmpty(txtInd1Fr.Text) ? null : txtInd1Fr.Text.Trim(),
               DBNull.Value
            
            });
        }

        private void UpdateKeyFigure(int id)
        {

            DBContext.Update("UpdateKeyFigure", new object[] {  
                id,
                 string.IsNullOrEmpty(txtNeed.Text) ? (int?)null : Convert.ToInt32(txtNeed.Text.Trim()),
                 string.IsNullOrEmpty(txtPopulationTarget.Text) ? (int?)null : Convert.ToInt32(txtPopulationTarget.Text.Trim()),
                 ddlSource.SelectedValue == "0" ? null : ddlSource.SelectedValue,
                 string.IsNullOrEmpty(txtOther.Text) ? null : txtOther.Text.Trim(),
                 string.IsNullOrEmpty(txtNeedMen.Text) ? (int?)null : Convert.ToInt32(txtNeedMen.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedWomen.Text) ? (int?)null : Convert.ToInt32(txtNeedWomen.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedGirls.Text) ? (int?)null : Convert.ToInt32(txtNeedGirls.Text.Trim()),
                 string.IsNullOrEmpty(txtNeedBoys.Text) ? (int?)null : Convert.ToInt32(txtNeedBoys.Text.Trim()),                
                 RC.GetCurrentUserId,
                 Convert.ToInt32(ddlUnit.SelectedValue),
                 string.IsNullOrEmpty(txtTargetMen.Text) ? (int?)null : Convert.ToInt32(txtTargetMen.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetWomen.Text) ? (int?)null : Convert.ToInt32(txtTargetWomen.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetGirls.Text) ? (int?)null : Convert.ToInt32(txtTargetGirls.Text.Trim()),
                 string.IsNullOrEmpty(txtTargetBoys.Text) ? (int?)null : Convert.ToInt32(txtTargetBoys.Text.Trim()),
                 string.IsNullOrEmpty(txtInd1Eng.Text) ? null : txtInd1Eng.Text.Trim(),
                 string.IsNullOrEmpty(txtInd1Fr.Text) ? null : txtInd1Fr.Text.Trim(),
               DBNull.Value
            
            });
        }

        private void LoadDataByDateCategoryAndCountry()
        {

           DataTable dt =  DBContext.GetData("GetKeyFiguresByCountryCategoryDate", new object[] {
                 RC.SelectedSiteLanguageId,
                 Convert.ToInt32(ddlCategory.SelectedValue),
                Convert.ToInt32(ddlCountry.SelectedValue),
                 Convert.ToDateTime(txtFromDate.Text)
             });

           if (dt != null && dt.Rows.Count > 0)
           {
               ViewState["KeyFigureID"] = dt.Rows[0]["KeyFigureID"].ToString();
               txtInd1Eng.Text = dt.Rows[0]["FigureEn"].ToString();
               txtInd1Fr.Text = dt.Rows[0]["FigureFr"].ToString();
               ddlUnit.SelectedValue = dt.Rows[0]["Unit"].ToString();
               txtNeed.Text = dt.Rows[0]["PopulationInNeed"].ToString();
               txtPopulationTarget.Text = dt.Rows[0]["PopulationTargeted"].ToString();
               txtNeedMen.Text = dt.Rows[0]["Men"].ToString();
               txtNeedWomen.Text = dt.Rows[0]["Women"].ToString();
               txtNeedGirls.Text = dt.Rows[0]["Girls"].ToString();
               txtNeedBoys.Text = dt.Rows[0]["Boys"].ToString();
               txtTargetMen.Text = dt.Rows[0]["TargetMen"].ToString();
               txtTargetWomen.Text = dt.Rows[0]["TargetWomen"].ToString();
               txtTargetGirls.Text = dt.Rows[0]["TargetGirls"].ToString();
               txtTargetBoys.Text = dt.Rows[0]["TargetBoys"].ToString();
               ddlSource.SelectedValue = dt.Rows[0]["Source"].ToString();
               if (ddlSource.SelectedValue == "Other")
               {
                   txtOther.Text = dt.Rows[0]["Other"].ToString();                  
               }
               else if (ddlSource.SelectedValue != "0")
               {
                   txtOther.Attributes.Add("disabled", "disabled");
               }
           }else{
               ViewState["KeyFigureID"] = "";
               txtInd1Eng.Text ="";
               txtInd1Fr.Text = "";
               ddlUnit.SelectedValue = "0";
               txtNeed.Text = "";
               txtPopulationTarget.Text = "";
               txtNeedMen.Text = "";
               txtNeedWomen.Text = "";
               txtNeedGirls.Text = "";
               txtNeedBoys.Text = "";
               txtTargetMen.Text = "";
               txtTargetWomen.Text = "";
               txtTargetGirls.Text = "";
               txtTargetBoys.Text = "";
               ddlSource.SelectedValue = "0";
               txtOther.Text = "";
               txtOther.Attributes.Add("disabled", "disabled");
           }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                UpdateKeyFigure(Convert.ToInt32(Utils.DecryptQueryString(Request.QueryString["id"])));
                //RC.SendEmail(UserInfo.Country, "SAHEL ORS: Key figure updated!", "Key figure updated for Contry " + ddlCountry.SelectedItem.Text + "and Category " + ddlCategory.SelectedItem.Text);
            }
            else if (!string.IsNullOrEmpty(ViewState["KeyFigureID"].ToString()))
            {
                UpdateKeyFigure(Convert.ToInt32(ViewState["KeyFigureID"].ToString()));
                //RC.SendEmail(UserInfo.Country, "SAHEL ORS: Key figure updated!", "Key figure updated for Contry " + ddlCountry.SelectedItem.Text + "and Category " + ddlCategory.SelectedItem.Text);
            }
            else
            {
                SaveNewKeyFigure();
                //RC.SendEmail(UserInfo.Country, "SAHEL ORS: New Key figure Added!", "Key figure added for Contry " + ddlCountry.SelectedItem.Text + "and Category " + ddlCategory.SelectedItem.Text);
            }

            Response.Redirect("~/ClusterLead/keyfigurelist.aspx");
        }

        private void SendKeyFigureEmail(string subject, string body)
        {
            //RC.SendEmail(UserInfo.Country, subject, body);
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/keyfigurelist.aspx");

        }
        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadDataByDateCategoryAndCountry();
        }

      
               

            
    }
}