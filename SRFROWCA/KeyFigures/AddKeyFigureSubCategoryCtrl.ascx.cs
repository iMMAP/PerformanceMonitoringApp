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
    public partial class AddKeyFigureSubCategoryCtrl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal void Save(int catId)
        {
            string kfEng = txtKeyFigEng.Text.Trim();
            string kfFr = txtKeyFigFr.Text.Trim();

            kfEng = string.IsNullOrEmpty(kfEng) ? kfFr : kfEng;
            kfFr = string.IsNullOrEmpty(kfFr) ? kfEng : kfFr;

            if (!(string.IsNullOrEmpty(kfEng) && string.IsNullOrEmpty(kfFr)))
            {
                DBContext.Add("InsertKeyFigureSubCategory", new object[] { catId, kfEng, kfFr, cbIsPopulation.Checked, RC.GetCurrentUserId, DBNull.Value });
            }
        }
    }
}