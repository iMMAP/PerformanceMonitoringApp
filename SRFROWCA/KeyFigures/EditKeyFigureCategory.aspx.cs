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
    public partial class EditKeyFigureCategory : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["cat"] != null)
                {
                    int catId = 0;
                    int.TryParse(Request.QueryString["cat"].ToString(), out catId);
                    if (catId > 0)
                    {
                        LoadCategories(catId);
                    }
                }
            }
        }

        internal override void BindGridData()
        {
            
        }

        private void LoadCategories(int catId)
        {
            CategoryId = catId;
            DataTable dt = DBContext.GetData("GetKeyFigureCategoryOnId", new object[] { catId });
            if (dt.Rows.Count > 0)
            {
                txtKeyFigEng.Text = dt.Rows[0]["CatEng"].ToString();
                txtKeyFigFr.Text = dt.Rows[0]["CatFr"].ToString();                
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CategoryId > 0)
            {
                string kfEng = txtKeyFigEng.Text.Trim();
                string kfFr = txtKeyFigFr.Text.Trim();

                kfEng = string.IsNullOrEmpty(kfEng) ? kfFr : kfEng;
                kfFr = string.IsNullOrEmpty(kfFr) ? kfEng : kfFr;

                if ((!(string.IsNullOrEmpty(kfEng) && string.IsNullOrEmpty(kfFr))) && CategoryId > 0)
                {
                    DBContext.Add("UpdateKeyFigureCategory", new object[] { CategoryId, kfEng, kfFr, RC.GetCurrentUserId, DBNull.Value });
                }
            }

            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }

        public int CategoryId
        {
            get
            {
                int id = 0;
                if (ViewState["KFCategoryId"] != null)
                {
                    int.TryParse(ViewState["KFCategoryId"].ToString(), out id);
                }

                return id;
            }
            set
            {
                ViewState["KFCategoryId"] = value.ToString();
            }
        }
    }
}