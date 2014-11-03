using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class AddCountryIndicator : BasePage
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);

            if (control == "btnAddIndicatorControl")
                IndControlId += 1;

            if (control == "btnRemoveIndicatorControl")
                IndControlId -= 1;

            if (IndControlId <= 1)
                btnRemoveIndicatorControl.Visible = false;

            else
                btnRemoveIndicatorControl.Visible = true;

            for (int i = 0; i < IndControlId; i++)
                AddIndicatorControl(i);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();

                PopulateObjective();

                AddIndicatorControl(0);
                IndControlId = 1;
            }
        }

        internal override void BindGridData()
        {
            PopulateObjective();
        }

        private void PopulateObjective()
        {
            LoadCombos();

            //ListItem item = new ListItem("--- Select Objective ---", "0");
            //ddlObjective.Items.Insert(0, item);
        }

        private void LoadCombos()
        {
            ddlObjective.DataValueField = "ObjectiveID";
            ddlObjective.DataTextField = "Objective";

            ddlObjective.DataSource = DBContext.GetData("GetObjectives", new object[] { RC.SelectedSiteLanguageId, null });
            ddlObjective.DataBind();

            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
        }

        public int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["CountryIndicatorControlId"] != null)
                {
                    int.TryParse(ViewState["CountryIndicatorControlId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["CountryIndicatorControlId"] = value.ToString();
            }
        }

        private void AddIndicatorControl(int i)
        {
            NewCountryIndicatorsControl newIndSet = (NewCountryIndicatorsControl)LoadControl("~/controls/NewCountryIndicatorsControl.ascx");
            newIndSet.ControlNumber = i + 1;
            newIndSet.ID = "countryIndicatorControlId" + i.ToString();
            pnlIndicaotrs.Controls.Add(newIndSet);
        }

        protected void btnAddIndicatorControl_ServerClick(object sender, EventArgs e)
        { }

        protected void btnSave_ServerClick(object sender, EventArgs e)
        {
            SaveData();
            Response.Redirect("~/Admin/CountryIndicators.aspx");
        }

        private void SaveData()
        {
            int objectiveID = RC.GetSelectedIntVal(ddlObjective);

            if (objectiveID > 0)
            {
                foreach (Control ctl in pnlIndicaotrs.Controls)
                {
                    if (ctl != null
                        && ctl.ID != null
                        && ctl.ID.Contains("countryIndicatorControlId"))
                    {
                        NewCountryIndicatorsControl indControl = ctl as NewCountryIndicatorsControl;

                        if (indControl != null)
                        {
                            TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                            TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");

                            TextBox txtTarget = (TextBox)indControl.FindControl("txtTarget");
                            DropDownList ddlUnits = (DropDownList)indControl.FindControl("ddlUnits");

                            indControl.SaveIndicators(objectiveID, Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlCluster.SelectedValue));
                        }
                    }
                }
            }
        }
    }
}