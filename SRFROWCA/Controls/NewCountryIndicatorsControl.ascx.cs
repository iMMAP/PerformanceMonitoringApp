using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Controls
{
    public partial class NewCountryIndicatorsControl : System.Web.UI.UserControl
    {
        public int ControlNumber { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();

            LoadCombos();
        }

        //public void SaveIndicators(int objectiveId, int countryId, int clusterId)
        public void SaveIndicators(int countryId, int clusterId)
        {
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            string target = decimal.Round(Convert.ToDecimal(txtTarget.Text.Trim()),0).ToString();
            string unitId = ddlUnits.SelectedValue;
            
            Guid userId = RC.GetCurrentUserId;

            //DBContext.Add("uspInsertIndicator", new object[] {objectiveId, indEn, indFr, target, unitId, countryId, clusterId, RC.GetCurrentUserId, null });
            DBContext.Add("uspInsertIndicator", new object[] { indEn, indFr, target, unitId, countryId, clusterId, RC.GetCurrentUserId, null , null});
        }

        private void LoadCombos() 
        {
            UI.FillUnits(ddlUnits);
        }
    }
}