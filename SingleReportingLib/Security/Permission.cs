using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using SingleReporting.Utilities;


namespace OCHA.Security.Library
{
    public class Permission
    {
        //This function will update base page class properties boolView,boolUpdate,boolDelete,boolAdd
        #region GetPermission(..)
        public static void GetPermission(string resourceType, ref bool canView, ref bool canAdd, ref bool canUpdate, ref bool canDelete, string accessToken)
        {

            AccessToken token = new AccessToken();

            DataTable dt = Module.GetPermissionBits(resourceType, getRoleFromCookie());

            if (dt != null)
                if (dt.Rows.Count > 3)
                {
                    accessToken = clsCommon.ParseString(dt.Rows[0]["st"]);
                    // View Permission
                    int viewResourceId = Convert.ToInt32(dt.Rows[0]["permissionBit"]);
                    canView = token.CheckRights(viewResourceId, accessToken);

                    //HttpContext.Current.Response.Write("token  " + viewResourceId + " cokie  " + accessToken);

                    // Add Permission
                    int addResourceId = Convert.ToInt32(dt.Rows[1]["permissionBit"]);
                    canAdd = token.CheckRights(addResourceId, accessToken);

                    // Updated Permission
                    int updateResourceId = Convert.ToInt32(dt.Rows[2]["permissionBit"]);
                    canUpdate = token.CheckRights(updateResourceId, accessToken);

                    // Delete Permission
                    int deleteResourceId = Convert.ToInt32(dt.Rows[3]["permissionBit"]);
                    canDelete = token.CheckRights(deleteResourceId, accessToken);
                }
        }
        #endregion


        //This function take a cookie and return RoleID
        #region getRoleFromCookie()
        private static int getRoleFromCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("RoleId");
            int roleID = 0;
            if (cookie == null)
            { }
            else
            {
                roleID = clsCommon.ParseInt(Encryption.Decrypt(cookie.Value));
                //securityToken = Encryption.Decrypt(securityToken);
            }

            return roleID;

        }
        #endregion


        //Overload GetPermission(..)

        public static void GetPermission(string resourceType, ref bool canView, ref bool canAdd, ref bool canUpdate, ref bool canDelete)
        {

          
           
            DataTable dt = Module.GetPermissionBits(resourceType, getRoleFromCookie(),1);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    
                    // View Permission
                    bool viewResourceId = Convert.ToBoolean(dt.Rows[0]["bitCanView"].ToString());
                    canView = viewResourceId;  

                    //HttpContext.Current.Response.Write("token  " + viewResourceId + " cokie  " + accessToken);

                    // Add Permission
                    bool addResourceId = Convert.ToBoolean(dt.Rows[0]["bitCanAdd"].ToString());
                    canAdd = addResourceId;

                    // Updated Permission
                    bool updateResourceId = Convert.ToBoolean(dt.Rows[0]["bitCanUpdate"].ToString());
                    canUpdate = updateResourceId;

                    // Delete Permission
                    bool deleteResourceId = Convert.ToBoolean(dt.Rows[0]["bitCanDelete"].ToString());
                    canDelete = deleteResourceId;
                }
        }

    }
}

