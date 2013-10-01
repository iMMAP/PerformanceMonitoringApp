using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;
using System.Globalization;
using SingleReporting;
using System.Collections.Generic;
using SingleReporting.Security;
using OCHA.Security.Library;

/// <summary>
/// Asad Aziz
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public SiteInfo CurrentSiteInfo;
    public UserInfo CurrentUserInfo;
    public List<Role> CurrentRolesInfo;
    protected int pageId = 1;
    protected int pageSize = 20;
    protected int totalRows;
    protected string rawUrl = string.Empty;
    public string _PageTitle = string.Empty;
    protected MemberInfo currentMemberInfo = null;
    public bool canView = true;
    public bool canAdd = false;
    public bool canUpdate = false;
    public bool canDelete = false;
    /// <summary>
    /// Constructor for BasePage
    /// </summary>
    public BasePage()
    {

        currentMemberInfo = MemberInfo.GetMemberInfo(HttpContext.Current.User.Identity.Name);

    }
    private NumberFormatInfo _NumberFormat = null;
    public NumberFormatInfo NumberFormat
    {
        get
        {
            _NumberFormat = new System.Globalization.NumberFormatInfo();
            _NumberFormat.CurrencyDecimalDigits = 0;
            _NumberFormat.CurrencySymbol = "$";
            return _NumberFormat;
        }
    }

    protected virtual void Page_PreInit(Object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IsMobile())
            {
                string url = this.Request.Url.AbsoluteUri.Substring(0, this.Request.Url.AbsoluteUri.LastIndexOf("/"));
                string MobileUrl = this.Request.Url.AbsoluteUri.Replace(url, ConfigurationManager.AppSettings["MobileUrl"].ToString());
                Response.Redirect(MobileUrl);
            }


        }
    }
    /// <summary>
    /// It shows BreadCrumb functionality
    /// </summary>
    /// <param name="queryString">queryString on which the label should be displayed</param>
    /// <param name="lbl">The label which should be displayed</param>
    protected void BreadCrumb(string queryString,Label lbl) 
    {
        SiteMapNode node = SiteMap.CurrentNode;
        do
        {

            HyperLink link = new HyperLink();
            link.NavigateUrl = node.Url+"?"+queryString;
            link.Text = node.Title;
            lbl.Controls.AddAt(0, link);
            Label label = new Label();
            label.Text = " >> ";
            lbl.Controls.AddAt(0, label);
            node = node.ParentNode;
        }
        while (node != null);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IsMobile()
    {
        string strUserAgent = Request.UserAgent.ToString().ToLower();
        if (strUserAgent != null)
        {
            if (Request.Browser.IsMobileDevice == true || strUserAgent.Contains("iphone") ||
                            strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                            strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") ||
                            strUserAgent.Contains("palm"))
            {
                return true;
                //if (this.Request.Url.AbsolutePath.ToLower().Contains("login.aspx"))
                //{
                //    if (clsCommon.GetQueryStringValue("ReturnUrl").ToLower().Contains("updatebiddetail.aspx"))
                //        return true;
                //    else
                //        return false;
                //}
                //else if (this.Request.Url.AbsolutePath.ToLower().Contains("/updatebiddetail.aspx"))
                //    return true;
                //else
                //    return false;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    protected string PageName
    {
        get { return this.Context.Request.Url.ToString(); }
    }
    //public BasePage()
    //{
    //    CurrentRolesInfo = Role.GetRoleByUser(User.Identity.Name);
    //    CurrentUserInfo = UserInfo.GetUserByUserName(User.Identity.Name);
    //}

    protected override bool OnBubbleEvent(object source, EventArgs args)
    {
        return base.OnBubbleEvent(source, args);
    }

    public string ReturnUrl
    {
        get
        {
            string redirectURL = string.Empty;
            if (Request["ReturnUrl"] != null)
            {
                redirectURL = Request["ReturnUrl"].ToString();
            }
            return redirectURL;
        }
    }
    /// <summary>
    /// Checks if user Logged In or not
    /// </summary>
    /// <returns></returns>
    public bool IsUserLoggedIn()
    {
        bool isLoggedIn = false;
        string userID = this.GetCurrentUserID();
        if (userID != null && userID.Length > 0)
        {
            HttpCookie cookies = this.getApplicationCookies(false);
            if (cookies != null)
            {
                string activityFlag = cookies.Values.Get("activityFlag");
                isLoggedIn = (activityFlag != null && activityFlag.Length > 0);
            }
        }
        return isLoggedIn;
    }
    /// <summary>
    /// This gives Current User's userID
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserID()
    {
        string userID = null;
        HttpCookie cookies = this.getApplicationCookies(true);
        if (cookies != null)
        {
            userID = cookies.Values.Get("UserID");
        }
        return userID;
    }

    private HttpCookie getApplicationCookies(bool permanent)
    {
        if (permanent)
        {
            return Request.Cookies.Get("Wedgewood.Net.Permanent");
        }
        else
        {
            return Request.Cookies.Get("Wedgewood.Net.Tmp");
        }
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
            //securityToken = Encryption.Decrypt(securityToken);
        }

        Permission.GetPermission(resourceType, ref canView, ref canAdd, ref canUpdate, ref canDelete, securityToken);
    }
}

