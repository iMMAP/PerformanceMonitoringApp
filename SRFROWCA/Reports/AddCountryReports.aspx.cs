using System;
using System.Web.UI;
using SRFROWCA.Common;
using SRFROWCA.Controls;

namespace SRFROWCA.Reports
{
    public partial class AddCountryReports : System.Web.UI.Page
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
            if (IsPostBack) return;
            AddIndicatorControl(0);
            IndControlId = 1;
        }

        private void AddIndicatorControl(int i)
        {
            AddContryReportControl newIndSet = (AddContryReportControl)LoadControl("~/controls/AddContryReportControl.ascx");
            newIndSet.ID = "indicatorControlId" + i.ToString();
            pnlAdditionalIndicaotrs.Controls.Add(newIndSet);
        }

        public int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["IndicatorControlId2"] != null)
                {
                    int.TryParse(ViewState["IndicatorControlId2"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["IndicatorControlId2"] = value.ToString();
            }
        }

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int locationId = Convert.ToInt32(txtEnLocationId.Text.Trim());
            int isPublic = Convert.ToInt32(txtPublic.Text.Trim());
            foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                {
                    AddContryReportControl indControl = ctl as AddContryReportControl;

                    if (indControl != null)
                    {
                        indControl.SaveCountryReport(locationId, isPublic);
                    }
                }
            }
        }
    }
}