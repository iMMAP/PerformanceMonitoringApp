using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class EditKeyFigureSubCategory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                if (Request.QueryString["cat"] != null)
                {
                    LoadKeyFigSubCatToEdit();
                }
            }
        }

        internal override void BindGridData()
        {
            LoadCategories();
        }

        private void LoadKeyFigSubCatToEdit()
        {
            if (Request.QueryString["cat"] != null)
            {
                int subCatId = 0;
                int.TryParse(Request.QueryString["cat"].ToString(), out subCatId);
                if (subCatId > 0)
                {
                    SubCategoryId = subCatId;
                    DataTable dt = DBContext.GetData("GetKeyFigureSubCategoryOnId", new object[] { subCatId });
                    if (dt.Rows.Count > 0)
                    {
                        ddlCategory.SelectedValue = dt.Rows[0]["KeyFigureCategoryId"].ToString();
                        txtKeyFigEng.Text = dt.Rows[0]["CatEng"].ToString();
                        txtKeyFigFr.Text = dt.Rows[0]["CatFr"].ToString();
                        if (dt.Rows[0]["IsPopulation"].ToString() == "True")
                        {
                            cbIsPopulation.Checked = true;
                        }
                    }
                }
            }
        }

        private void LoadCategories()
        {
            ddlCategory.DataTextField = "KeyFigureCategory";
            ddlCategory.DataValueField = "KeyFigureCategoryId";
            ddlCategory.DataSource = DBContext.GetData("GetKeyFigureCategories", new object[] { RC.SelectedSiteLanguageId });
            ddlCategory.DataBind();

            if (ddlCategory.Items.Count > 0)
            {
                ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            string kfEng = txtKeyFigEng.Text.Trim();
            string kfFr = txtKeyFigFr.Text.Trim();
            kfEng = string.IsNullOrEmpty(kfEng) ? kfFr : kfEng;
            kfFr = string.IsNullOrEmpty(kfFr) ? kfEng : kfFr;

            if (!(string.IsNullOrEmpty(kfEng) && string.IsNullOrEmpty(kfFr)))
            {
                if (SubCategoryId > 0)
                {
                    DBContext.Add("UpdateKeyFigureSubCategory", new object[] { categoryId, SubCategoryId, kfEng, kfFr, cbIsPopulation.Checked, RC.GetCurrentUserId, DBNull.Value });
                }
            }
            SubCategoryId = 0;
            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }

        private int SubCategoryId
        {
            get
            {
                int id = 0;
                if (ViewState["KFSubCategoryId"] != null)
                {
                    int.TryParse(ViewState["KFSubCategoryId"].ToString(), out id);
                }

                return id;
            }
            set
            {
                ViewState["KFSubCategoryId"] = value.ToString();
            }
        }
    }
}