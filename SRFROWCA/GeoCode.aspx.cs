using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;

namespace SRFROWCA
{
    public partial class GeoCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetData();
        }

      
        public string GetData()
        {
            DataTable dt = DBContext.GetData("GetLocationForGeo");

            string json = DataTableToJson.DataTableToJsonBySerializer(dt);
            return json;
        }
    }
}