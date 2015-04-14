using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class AddKeyFigureSubCategory : BasePage
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);
            if (control == "btnAddIndicatorControl")
            {
                IndControlId += 1;
            }

            if (control == "btnRemoveIndicatorControl")
            {
                IndControlId -= 1;
            }

            if (IndControlId <= 1)
            {
                btnRemoveIndicatorControl.Visible = false;
            }
            else
            {
                btnRemoveIndicatorControl.Visible = true;
            }

            for (int i = 0; i < IndControlId; i++)
            {
                AddIndicatorControl(i);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                AddIndicatorControl(0);
                IndControlId += 1;
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

        internal override void BindGridData()
        {
            LoadCategories();
        }

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in pnlKeyFigCategory.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("ctlCategoryId"))
                {
                    AddKeyFigureSubCategoryCtrl ctlCategory = ctl as AddKeyFigureSubCategoryCtrl;
                    if (ctlCategory != null)
                    {
                        int catId = RC.GetSelectedIntVal(ddlCategory);
                        if (catId > 0)
                        {
                            ctlCategory.Save(catId);
                        }
                    }
                }
            }

            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }


        private void AddIndicatorControl(int i)
        {
            AddKeyFigureSubCategoryCtrl ctlCategory = (AddKeyFigureSubCategoryCtrl)LoadControl("~/KeyFigures/AddKeyFigureSubCategoryCtrl.ascx");
            ctlCategory.ID = "ctlCategoryId" + i.ToString();
            pnlKeyFigCategory.Controls.Add(ctlCategory);
        }

        private int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["IndicatorControlId"] != null)
                {
                    int.TryParse(ViewState["IndicatorControlId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["IndicatorControlId"] = value.ToString();
            }
        }
    }
}