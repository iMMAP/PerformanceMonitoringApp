using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCHA.Security.Library;
using System.Web;
using System.Web.UI.WebControls;

namespace SingleReporting
{
    public class BaseControl : System.Web.UI.UserControl
    {
        protected MemberInfo currentMemberInfo = null;
        public bool canView = false;
        public bool canAdd = false;
        public bool canUpdate = false;
        public bool canDelete = false;
        /// <summary>
        /// BaseControl Constructor
        /// </summary>
        public BaseControl()
        {

            currentMemberInfo = MemberInfo.GetMemberInfo(HttpContext.Current.User.Identity.Name);

        }

        private string _dateFilter = string.Empty;
        /// <summary>
        /// DateFilter Property
        /// </summary>
        public string DateFilter
        {
            get
            {
                if (_dateFilter != SiteCookie.Get("dateFilter") && !string.IsNullOrEmpty(_dateFilter))
                {
                    return _dateFilter;
                }
                else
                    return SiteCookie.Get("dateFilter");
                //return "TODAY";//if nothing is here then by default set it to TODAY
            }
            set
            {
                _dateFilter = value;
                SiteCookie.Update("dateFilter", value, 365);
            }
        }
        /// <summary>
        /// It shows the BreadCrumb functionality
        /// </summary>
        /// <param name="queryString">queryString on which the label should be displayed</param>
        /// <param name="lbl">The label which should be displayed</param>
        protected void BreadCrumb(string queryString, Label lbl)
        {
            int counter = 0;
            SiteMapNode node = SiteMap.CurrentNode;
            if (node != null)
                do
                {
                    HyperLink link = new HyperLink();
                    //if (!string.IsNullOrEmpty(node.Title))
                    //{

                    link.NavigateUrl = node.Url + "?" + queryString;
                    link.Text = node.Title;
                    Label label = new Label();
                    if (!string.IsNullOrEmpty(node.Title))
                    {
                        if (counter != 0)
                        {
                            label.Text = " >> ";
                            lbl.Controls.AddAt(0, label);
                        }
                        counter++;
                    }
                    lbl.Controls.AddAt(0, link);
                    node = node.ParentNode;


                    //}
                }
                while (node != null);
        }
        /// <summary>
        /// Get the Permission when module name is passed
        /// </summary>
        /// <param name="resourceType">Module Name</param>
        /// <param name="canView">Boolean Output param that states if user can view mentioned module or not</param>
        /// <param name="canAdd">Boolean Output param that states if user can Perform add in  mentioned module or not</param>
        /// <param name="canUpdate">Boolean Output param that states if user can Perform update in  mentioned module or not</param>
        /// <param name="canDelete">Boolean Output param that states if user can Perform delete in  mentioned module or not></param>
        public void GetPermission(string resourceType, ref bool canView, ref bool canAdd, ref bool canUpdate, ref bool canDelete)
        {

            MemberInfo user = MemberInfo.GetCurrentUserInfo();

            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("SecurityToken");
            string securityToken = string.Empty;
            if (cookie == null)
            {
                securityToken = RoleManagement.GetSecurityToken(user.RoleId);
                HttpCookie securityCookie = new HttpCookie("SecurityToken");
                securityCookie.Value = Server.UrlEncode(securityToken);
                HttpContext.Current.Response.Cookies.Add(securityCookie);
            }
            else
            {
                securityToken = Server.UrlDecode(cookie.Value);
                // securityToken = Encryption.Decrypt(securityToken);
            }

            Permission.GetPermission(resourceType, ref canView, ref canAdd, ref canUpdate, ref canDelete, securityToken);
        }
    }
}
