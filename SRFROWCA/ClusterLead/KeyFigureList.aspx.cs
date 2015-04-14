using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class KeyFigureList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                DisableDropDowns();
                LoadData();

                if (RC.IsRegionalClusterLead(this.User) || RC.IsOCHAStaff(this.User))
                {
                    btnNew.Visible = false;
                }
            }
        }

        // Disable Controls on the basis of user profile
        private void DisableDropDowns()
        {
            if (!(RC.IsAdmin(this.User) || RC.IsRegionalClusterLead(this.User)))
            {
                RC.EnableDisableControls(ddlCountry, false);               
            }           
        }

        internal override void BindGridData()
        {
            LoadCombos();
            DisableDropDowns();
            LoadData();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            PopulateCategories();

            ddlCategory.Items.Insert(0, new ListItem("--- Select Category ---", "0"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "0"));

            SetComboValues();
        }

        

        private void PopulateCategories()
        {
            ddlCategory.DataValueField = "KeyFigureCategoryID";
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataSource = DBContext.GetData("GetKeyFigureCategories");
            ddlCategory.DataBind();
             
        }



        private void LoadData()
        {
            gvKeyFigures.DataSource = SetDataSource();
            gvKeyFigures.DataBind();
        }

        private DataTable SetDataSource()
        {
            int countryId = 0;
            int categoryId = 0;

          countryId = Convert.ToInt32(ddlCountry.SelectedValue);
          categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

          return GetKeyFigures(categoryId, countryId);
        }

        private void SetComboValues()
        {
            if (!(RC.IsAdmin(this.User) || RC.IsRegionalClusterLead(this.User)))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }           
        }

        private DataTable GetKeyFigures(int categoryId, int countryId)
        {
            return DBContext.GetData("GetKeyFigures", new object[] { RC.SelectedSiteLanguageId, 0, countryId, categoryId });
        }       

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                if (RC.IsRegionalClusterLead(this.User) || RC.IsOCHAStaff(this.User))
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = SetDataSource();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvKeyFigures.DataSource = dt;
                gvKeyFigures.DataBind();
            }
        }

        protected void gvKeyFigure_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteFigure")
            {
                int Id = Convert.ToInt32(e.CommandArgument);               

                DeleteFigure(Id);
                LoadData();
            }

            // Edit .
            if (e.CommandName == "EditFigure")
            { 
                Response.Redirect("~/ClusterLead/AddEditKeyFigure.aspx?id=" + Utils.EncryptQueryString(e.CommandArgument.ToString()));               
            }
        }

        private void DeleteFigure(int id)
        {
            DBContext.Delete("DeleteKeyFigure", new object[] { id, DBNull.Value });
        }
        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/AddEditKeyFigure.aspx");
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();

            int countryId = 0;       
            int categoryId = 0;
           
            countryId = Convert.ToInt32(ddlCountry.SelectedValue);
            categoryId = Convert.ToInt32(ddlCategory.SelectedValue);

            DataTable dt = DBContext.GetData("GetKeyFiguresForExcel", new object[] { RC.SelectedSiteLanguageId, 0, countryId, categoryId });
            //RemoveColumnsFromDataTable(dt);

            //dt.DefaultView.Sort = "Country, CategoryName, KeyFigure, UnitName,PopulationInNeed,Men,Women,Girls,Boys,PopulationTargeted,TargetMen,TargetWomen,TargetGirls,TargetBoys, UpdatedDate";
            gvExport.DataSource = dt.DefaultView;
            gvExport.DataBind();

            string fileName = "KeyFigures";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);

        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("KeyFigureID");
                dt.Columns.Remove("EmergencyLocationID");
                dt.Columns.Remove("CategoryID");
                dt.Columns.Remove("Deleted");
                dt.Columns.Remove("CreatedByID");
                dt.Columns.Remove("UpdatedByID");
                dt.Columns.Remove("Unit");
                dt.Columns.Remove("KeyFigureDetailID");
                dt.Columns.Remove("LocationId");
                

            }
            catch { }
        }
    }
}