using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class AddKeyFigureCategory : BasePage
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
                AddIndicatorControl(0);
                IndControlId += 1;
            }
        }

        internal override void BindGridData()
        {
            
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
                    AddKeyFigureCategoryCtrl ctlCategory = ctl as AddKeyFigureCategoryCtrl;
                    if (ctlCategory != null)
                    {
                        ctlCategory.Save();
                    }
                }
            }

            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }


        private void AddIndicatorControl(int i)
        {
            AddKeyFigureCategoryCtrl ctlCategory = (AddKeyFigureCategoryCtrl)LoadControl("~/KeyFigures/AddKeyFigureCategoryCtrl.ascx");
            ctlCategory.ID = "ctlCategoryId" + i.ToString();
            pnlKeyFigCategory.Controls.Add(ctlCategory);
        }

        public int IndControlId
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