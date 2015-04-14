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
    public partial class AddKeyFigureControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //private void LoadKeyFigureIndicators(int subCatId)
        //{
        //    ddlKeyFigureIndicator.DataTextField = "KeyFigureIndicator";
        //    ddlKeyFigureIndicator.DataValueField = "KeyFigureIndicatorId";
        //    ddlKeyFigureIndicator.DataSource = DBContext.GetData("GetKeyFigureIndicators", new object[] { subCatId, RC.SelectedSiteLanguageId });
        //    ddlKeyFigureIndicator.DataBind();

        //    if (ddlKeyFigureIndicator.Items.Count > 0)
        //    {
        //        ddlKeyFigureIndicator.Items.Insert(0, new ListItem("Select Key Figure", "0"));
        //    }
        //}

        //private void LoadLocaitons(int emgLocId)
        //{
        //    ddlKeyFigureLocation.DataTextField = "LocationName";
        //    ddlKeyFigureLocation.DataValueField = "LocationId";
        //    ddlKeyFigureLocation.DataSource = DBContext.GetData("GetCountryAndAdmin1OnEmgLocation", new object[] { emgLocId });
        //    ddlKeyFigureLocation.DataBind();
        //}

        //internal void SaveKeyFigure(int reportId)
        //{
            ////int kfIndicatorId = RC.GetSelectedIntVal(ddlKeyFigureIndicator);
            ////int kfLocationId = RC.GetSelectedIntVal(ddlKeyFigureLocation);
            //int kfIndicatorId = 0;
            //int.TryParse(hfKeyFigureIndicatorId.Value, out kfIndicatorId);

            //int kfLocationId = 0;
            //int.TryParse(hfCountryId.Value, out kfLocationId);

            //int? needTotal = KeyFigureReported(txtNeedTotal);
            //int? needMen = KeyFigureReported(txtNeedMen);
            //int? needWomen = KeyFigureReported(txtNeedWomen);
            //int? targatedTotal = KeyFigureReported(txtTargetedTotal);
            //int? targatedMen = KeyFigureReported(txtTargetedMen);
            //int? targatedWomen = KeyFigureReported(txtTargetedWomen);
            //int? reachedTotal = KeyFigureReported(txtReachedTotal);
            //int? reachedMen = KeyFigureReported(txtReachedMen);
            //int? reachedWomen = KeyFigureReported(txtReachedWomen);

            //bool valueProvied = (needTotal != null || needMen != null || needWomen != null ||
            //                     targatedTotal != null || targatedMen != null || targatedWomen != null ||
            //                     reachedTotal != null || reachedMen != null || reachedWomen != null);
            //if (kfIndicatorId > 0 && kfLocationId > 0 && valueProvied)
            //{
            //    DBContext.Add("InsertKeyFigureReportDetails", new object[] {reportId, kfLocationId, kfIndicatorId,
            //                                                       needTotal, needMen, needWomen,
            //                                                        targatedTotal, targatedMen, targatedWomen,
            //                                                        reachedTotal, reachedMen, reachedWomen,
            //                                                        DBNull.Value});
            //}

        //}

        //private int? KeyFigureReported(TextBox txtValue)
        //{
        //    int? kfValue = null;
        //    if (!string.IsNullOrEmpty(txtValue.Text.Trim()))
        //    {
        //        kfValue = Convert.ToInt32(txtValue.Text.Trim());
        //    }

        //    return kfValue;
        //}

        //internal void AddControl(int emgLocationId, int subCatIndId)
        //{
        //    //LoadKeyFigureIndicators(subCatIndId);
        //    //LoadLocaitons(emgLocationId);
        //}

        //internal void PopulateData(DataRow row)
        //{
        //    ////ddlKeyFigureIndicator.SelectedValue = row["KeyFigureIndicatorId"].ToString();
        //    ////ddlKeyFigureLocation.SelectedValue = row["LocationId"].ToString();
        //    //lblKeyFigureIndicator.Text = row["KeyFigureIndicatorId"].ToString();
        //    //lblKeyFigureIndicator.Text = row["LocationId"].ToString();
        //    //txtNeedTotal.Text = row["NeedTotal"].ToString();
        //    //txtNeedMen.Text = row["NeedMen"].ToString();
        //    //txtNeedWomen.Text = row["NeedWomen"].ToString();
        //    //txtTargetedTotal.Text = row["TargetedTotal"].ToString();
        //    //txtTargetedMen.Text = row["TargetedMen"].ToString();
        //    //txtTargetedWomen.Text = row["TargetedWomen"].ToString();
        //    //txtReachedTotal.Text = row["ReachedTotal"].ToString();
        //    //txtReachedMen.Text = row["ReachedMen"].ToString();
        //    //txtReachedWomen.Text = row["ReachedWomen"].ToString();
        //}

        //internal void PopulateDropDowns(int subCatId, int emgLocationId)
        //{
        //    //LoadKeyFigureIndicators(subCatId);
        //    //LoadLocaitons(emgLocationId);
        //}
    }
}