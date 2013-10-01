using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLogic;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace SRFROWCA
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        //private DataTable GetReportData()
        //{
        //    return DBContext.GetData("GetMonthlyTargetAndAchieved");
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string format = "{'' {0} ''}";
            string value = "Cluster: 1037 EDUIndicator: % d' écoles d'accueil et d'écoles du Nord recevant des kits enseignantsActivity: Distribution de kits enseignants>Data: Nombre d'écoles au Nord (source Ministère)";
            string abc = string.Format("\" {0} \"", value);

        }
    }
}