using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Configurations;
using SRFROWCA.Controls;
using System;
using System.Data;
using System.Web.UI;

namespace SRFROWCA.OPS
{
    public partial class OPSDataEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            AddOPSTargetControl();
        }

        private void AddOPSTargetControl()
        {
            //LoadIndLocations(indCtl, 0, false);
            //indCtl.ControlNumber = i + 1;
            //indCtl.ID = "indicatorControlId" + i.ToString();

            string key = EmgLocationId.ToString() + EmgClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.LocationTypes.Governorate)
                {
                    ctl = (ctlOPSAdmin1Targets)LoadControl("~/controls/ctlOPSAdmin1Targets.ascx");
                    ((ctlOPSAdmin1Targets)ctl).ID = "OPSTargetControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.District)
                {
                    ctl = (ctlOPSAdmin2Targets)LoadControl("~/controls/ctlOPSAdmin2Targets.ascx");
                    ((ctlOPSAdmin2Targets)ctl).ID = "OPSTargetControl";
                }

                if (ctl != null)
                    pnlTargets.Controls.Add(ctl);
            }
        }
        private int EmgClusterId
        {
            get
            {
                string clusterName = IsMSRefugee ? "multisectorforrefugees" : OPSClusterName;
                return RC.EmgClusterIdsSAH2015(RC.MapOPSWithORSClusterNames(clusterName));
            }
        }
        private int EmgLocationId
        {
            get
            {
                return RC.EmgLocationIdsSAH2015(OPSCountryName);
            }
        }
        private bool IsMSRefugee
        {
            get
            {
                string clusterName = "";
                if (Request.QueryString["clname2"] != null)
                {
                    clusterName = Request.QueryString["clname2"].ToString();
                }

                return clusterName == "multisectorforrefugees";
            }
        }
        private string OPSClusterName
        {
            get
            {
                string clusterName = "";
                if (Request.QueryString["clname"] != null)
                {
                    clusterName = Request.QueryString["clname"].ToString();
                }
                return clusterName;
            }
        }
        private string OPSCountryName
        {
            get
            {
                string countryName = "";
                if (Request.QueryString["cname"] != null)
                {
                    string cName = Request.QueryString["cname"].ToString();
                    if (cName == "burkinafaso" || cName == "Burkinafaso" || cName == "BURKINAFASO")
                    {
                        countryName = "Burkina Faso";
                    }
                    else if (cName == "region" || cName == "Region" || cName == "REGION" || cName == "sahelregion")
                    {
                        countryName = "Sahel Region";
                    }
                    else
                    {
                        countryName = cName;
                    }
                }

                return countryName;
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}