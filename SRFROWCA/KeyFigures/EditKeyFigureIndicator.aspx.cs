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
    public partial class EditKeyFigureIndicator : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                if (Request.QueryString["kf"] != null)
                {
                    LoadKeyFiguresToEdit();
                }
            }
        }

        internal override void BindGridData()
        {
            LoadCategories();
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

        private void LoadSubCategories(int categoryId)
        {
            ddlSubCategory.DataTextField = "KeyFigureSubCategory";
            ddlSubCategory.DataValueField = "KeyFigureSubCategoryId";
            ddlSubCategory.DataSource = DBContext.GetData("GetKeyFigureSubCategories", new object[] { categoryId, RC.SelectedSiteLanguageId });
            ddlSubCategory.DataBind();

            if (ddlSubCategory.Items.Count > 0)
            {
                ddlSubCategory.Items.Insert(0, new ListItem("Select Sub Category", "0"));
            }
        }

        private void LoadKeyFiguresToEdit()
        {
            if (Request.QueryString["kf"] != null)
            {
                int kfId = 0;
                int.TryParse(Request.QueryString["kf"].ToString(), out kfId);
                if (kfId > 0)
                {
                    DataTable dt = DBContext.GetData("GetKeyFigureOnId", new object[] { kfId });
                    if (dt.Rows.Count > 0)
                    {
                        KeyFigId = kfId;
                        int catId = Convert.ToInt32(dt.Rows[0]["KeyFigureCategoryId"].ToString());
                        ddlCategory.SelectedValue = dt.Rows[0]["KeyFigureCategoryId"].ToString();
                        LoadSubCategories(catId);
                        ddlSubCategory.SelectedValue = dt.Rows[0]["KeyFigureSubCategoryId"].ToString();
                        txtKeyFigEng.Text = dt.Rows[0]["CatEng"].ToString();
                        txtKeyFigFr.Text = dt.Rows[0]["CatFr"].ToString();
                    }
                }
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            if (categoryId > 0)
            {
                LoadSubCategories(categoryId);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string kfEng = txtKeyFigEng.Text.Trim();
            string kfFr = txtKeyFigFr.Text.Trim();
            kfEng = string.IsNullOrEmpty(kfEng) ? kfFr : kfEng;
            kfFr = string.IsNullOrEmpty(kfFr) ? kfEng : kfFr;

            if (!(string.IsNullOrEmpty(kfEng) && string.IsNullOrEmpty(kfFr)))
            {
                if (KeyFigId > 0)
                {
                    int subCatId = RC.GetSelectedIntVal(ddlSubCategory);
                    DBContext.Add("UpdateKeyFigureIndicator", new object[] { subCatId, KeyFigId, kfEng, kfFr, RC.GetCurrentUserId, DBNull.Value });
                }
            }
            KeyFigId = 0;
            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }

        private int KeyFigId
        {
            get
            {
                int id = 0;
                if (ViewState["KeyFigId"] != null)
                {
                    int.TryParse(ViewState["KeyFigId"].ToString(), out id);
                }

                return id;
            }
            set
            {
                ViewState["KeyFigId"] = value.ToString();
            }
        }
    }
}