using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class UnitsListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateUnits();
        }

        private void PopulateUnits()
        {
            gvUnits.DataSource = GetUnits();
            gvUnits.DataBind();
        }

        private object GetUnits()
        {
            string unit = string.IsNullOrEmpty(txtUnits.Text.Trim()) ? null : txtUnits.Text.Trim();
            int? isGender = null; 
            if (cbIsGender.Checked)
            {
                isGender = 1;
            }
            return DBContext.GetData("GetAllUnitsEngFr", new object[] { unit, isGender });
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateUnits();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveIsGender();
        }

        private void SaveIsGender()
        {
            foreach (GridViewRow row in gvUnits.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int unitId = Convert.ToInt32(gvUnits.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    DropDownList ddlIsGender = row.FindControl("ddlIsGender") as DropDownList;
                    if (ddlIsGender != null)
                    {
                        int genderStatus = RC.GetSelectedIntVal(ddlIsGender);
                        DBContext.Update("UpdateUnitsGender", new object[] { unitId, genderStatus, DBNull.Value });
                    }
                }
            }
        }

        protected void btnAddUnit_Click(object sender, EventArgs e)
        { }
    }
}