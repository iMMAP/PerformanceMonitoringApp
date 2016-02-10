using SRFROWCA.Common;
using SRFROWCA.Configurations;
using System;
using System.Linq;
using System.Web.UI;

namespace SRFROWCA.OrsProject
{
    public partial class ProjectTargets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddTargetControl();
        }

        private void AddTargetControl()
        {
            int projectId = 0;
            if (Request.QueryString["pid"] != null)
            {
                int.TryParse(Request.QueryString["pid"].ToString(), out projectId);
            }

            Project project = null;
            using(ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            }

            if (project == null)
                return;

            string key = project.EmergencyLocationId.ToString() + project.EmergencyClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.LocationTypes.Governorate)
                {
                    ctl = (ctlORSAdmin1Targets)LoadControl("~/OrsProject/ctlORSAdmin1Targets.ascx");
                    ((ctlORSAdmin1Targets)ctl).ID = "ORSTargetControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.District)
                {
                    ctl = (ctlORSAdmin2Targets)LoadControl("~/OrsProject/ctlORSAdmin2Targets.ascx");
                    ((ctlORSAdmin2Targets)ctl).ID = "ORSTargetControl";
                }

                if (ctl != null)
                    pnlTargets.Controls.Add(ctl);
            }
        }

        
        //private int EmgClusterId
        //{
        //    get
        //    {
        //        string clusterName = IsMSRefugee ? "multisectorforrefugees" : OPSClusterName;
        //        return RC.EmgClusterIdsSAH2015(RC.MapOPSWithORSClusterNames(clusterName));
        //    }
        //}
        //private int EmgLocationId
        //{
        //    get
        //    {
        //        return RC.EmgLocationIdsSAH2015(OPSCountryName);
        //    }
        //}

        //private bool IsMSRefugee
        //{
        //    get
        //    {
        //        string clusterName = "";
        //        if (Request.QueryString["clname2"] != null)
        //        {
        //            clusterName = Request.QueryString["clname2"].ToString();
        //        }

        //        return clusterName == "multisectorforrefugees";
        //    }
        //}
        //private string OPSClusterName
        //{
        //    get
        //    {
        //        string clusterName = "";
        //        if (Request.QueryString["clname"] != null)
        //        {
        //            clusterName = Request.QueryString["clname"].ToString();
        //        }
        //        return clusterName;
        //    }
        //}
        //private string OPSCountryName
        //{
        //    get
        //    {
        //        string countryName = "";
        //        if (Request.QueryString["cname"] != null)
        //        {
        //            string cName = Request.QueryString["cname"].ToString();
        //            if (cName == "burkinafaso" || cName == "Burkinafaso" || cName == "BURKINAFASO")
        //            {
        //                countryName = "Burkina Faso";
        //            }
        //            else if (cName == "region" || cName == "Region" || cName == "REGION" || cName == "sahelregion")
        //            {
        //                countryName = "Sahel Region";
        //            }
        //            else
        //            {
        //                countryName = cName;
        //            }
        //        }

        //        return countryName;
        //    }
        //}

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}