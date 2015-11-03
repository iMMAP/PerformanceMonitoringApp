using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace SRFROWCA.Admin
{
    public partial class UnitsListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateUnits();
        }

        private void SaveGenderUnitsInFile()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", ""));
            XElement root = new XElement("units");

            foreach (GridViewRow row in gvUnits.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int unitId = Convert.ToInt32(gvUnits.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    DropDownList ddlIsGender = row.FindControl("ddlIsGender") as DropDownList;
                    if (ddlIsGender != null)
                    {
                        int genderStatus = RC.GetSelectedIntVal(ddlIsGender);
                        if (genderStatus == 1)
                        {
                            XElement unit = new XElement("unit", unitId.ToString());
                            root.Add(unit);
                        }
                    }
                }
            }
            
            doc.Add(root);

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\Units.xml";
            doc.Save(PATH);
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
            SaveGenderUnitsInFile();
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

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}