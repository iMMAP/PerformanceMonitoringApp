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
    public partial class KeyFigureIndicatorListing :  BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadKeyFigureFramework();
            }
        }

        private DataTable GetKeyFigures()
        {
            return DBContext.GetData("GetKeyFigureFrameWork", new object[] { RC.SelectedSiteLanguageId });
        }

        private void LoadKeyFigureFramework()
        {
            gvKFInd.DataSource = GetKeyFigures();
            gvKFInd.DataBind();
        }

        internal override void BindGridData()
        {
            LoadKeyFigureFramework();
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddKeyFigureCategory.aspx");
        }

        protected void btnAddSubCategory_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddKeyFigureSubCategory.aspx");
        }

        protected void btnAddKeyFigure_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddKeyFigureIndicator.aspx");
        }

        protected void gvKFInd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string subCatId = gvKFInd.DataKeys[e.Row.RowIndex].Values["SubCategoryId"].ToString();
                if(string.IsNullOrEmpty(subCatId))
                {
                    ImageButton btnImage = e.Row.FindControl("btnEditSubCatgory") as ImageButton;
                    if (btnImage != null)
                        btnImage.Visible = false;
                }
                string keyFigureId = gvKFInd.DataKeys[e.Row.RowIndex].Values["KeyFigureId"].ToString();
                if (string.IsNullOrEmpty(keyFigureId))
                {
                    ImageButton btnImage = e.Row.FindControl("btnEditKFInd") as ImageButton;
                    if (btnImage != null)
                        btnImage.Visible = false;
                }

                ImageButton deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this record?')");
                }
            }
        }

        protected void gvKFInd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditCategory")
            {
                int rowIndex = 0;
                int.TryParse(e.CommandArgument.ToString(), out rowIndex);
                string catId = gvKFInd.DataKeys[rowIndex].Values["CategoryId"].ToString();
                Response.Redirect(string.Format("EditKeyFigureCategory.aspx?cat=" + catId.ToString()));
            }

            if (e.CommandName == "EditSubCategory")
            {
                int rowIndex = 0;
                int.TryParse(e.CommandArgument.ToString(), out rowIndex);
                string catId = gvKFInd.DataKeys[rowIndex].Values["SubCategoryId"].ToString();
                Response.Redirect(string.Format("EditKeyFigureSubCategory.aspx?cat=" + catId.ToString()));
            }

            if (e.CommandName == "EditKfInd")
            {
                int rowIndex = 0;
                int.TryParse(e.CommandArgument.ToString(), out rowIndex);
                string catId = gvKFInd.DataKeys[rowIndex].Values["KeyFigureId"].ToString();
                Response.Redirect(string.Format("EditKeyFigureIndicator.aspx?kf=" + catId.ToString()));
            }

            if (e.CommandName == "DeleteFigure")
            {
                int rowIndex = 0;
                int.TryParse(e.CommandArgument.ToString(), out rowIndex);
                string kfId = gvKFInd.DataKeys[rowIndex].Values["KeyFigureId"].ToString();
                string subCatId= gvKFInd.DataKeys[rowIndex].Values["SubCategoryId"].ToString();
                string catId= gvKFInd.DataKeys[rowIndex].Values["CategoryId"].ToString();

                if (!string.IsNullOrEmpty(kfId))
                {
                    int returnVal = DBContext.Delete("DeleteKeyFigureIndicator", new object[] { kfId, DBNull.Value });
                    if (returnVal == 0)
                    {
                        ShowMessage("This Key Figure has data. It can not be deleted!", RC.NotificationType.Error, true, 1000);
                    }
                }
                else if (!string.IsNullOrEmpty(subCatId))
                {
                    DBContext.Delete("DeleteKeyFigureSubCategory", new object[] { subCatId, DBNull.Value });
                }
                else if (!string.IsNullOrEmpty(catId))
                {
                    DBContext.Delete("DeleteKeyFigureCategory", new object[] { catId, DBNull.Value });
                }

                LoadKeyFigureFramework();
            }
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = GetKeyFigures();

            string fileName = "KeyFiguresFramework";
            ExportUtility.ExportGridView(dt, fileName, Response);
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {

        }
    }
}