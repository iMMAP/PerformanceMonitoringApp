using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Controls
{
    public partial class ReportedIndicatorComments : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadComments(int reportId, int activityDataId)
        {
            using (ORSEntities db = new ORSEntities())
            {
                rptIndComments.DataSource = DBContext.GetData("GetIndicatorComments", new object[] {reportId, activityDataId });
                rptIndComments.DataBind();
            }
        }

        public string GetComments()
        {
            string comments = fcComments.Value;
            fcComments.Value = "";
            return comments;
        }

        //public int ActivityDataId
        //{
        //    get
        //    {
        //        int id = 0;
        //        if (ViewState["ActivityDataId"] != null)
        //        {
        //            int.TryParse(ViewState["ActivityDataId"].ToString(), out id);
        //        }

        //        return id;
        //    }
        //    set
        //    {
        //        ViewState["ActivityDataId"] = value;
        //    }
        //}

        //public int YearId
        //{
        //    get
        //    {
        //        int id = 0;
        //        if (ViewState["YearId"] != null)
        //        {
        //            int.TryParse(ViewState["YearId"].ToString(), out id);
        //        }

        //        return id;
        //    }
        //    set
        //    {
        //        ViewState["YearId"] = value;
        //    }
        //}
        //public int MonthId
        //{
        //    get
        //    {
        //        int id = 0;
        //        if (ViewState["MonthId"] != null)
        //        {
        //            int.TryParse(ViewState["MonthId"].ToString(), out id);
        //        }

        //        return id;
        //    }
        //    set
        //    {
        //        ViewState["MonthId"] = value;
        //    }
        //}
        //public int ProjectId
        //{
        //    get
        //    {
        //        int id = 0;
        //        if (ViewState["ProjectId"] != null)
        //        {
        //            int.TryParse(ViewState["ProjectId"].ToString(), out id);
        //        }

        //        return id;
        //    }
        //    set
        //    {
        //        ViewState["ProjectId"] = value;
        //    }
        //}
        //public int EmgLocationId
        //{
        //    get
        //    {
        //        int id = 0;
        //        if (ViewState["EmgLocationId"] != null)
        //        {
        //            int.TryParse(ViewState["EmgLocationId"].ToString(), out id);
        //        }

        //        return id;
        //    }
        //    set
        //    {
        //        ViewState["EmgLocationId"] = value;
        //    }
        //}

    }
}