using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SingleReporting;

/// <summary>
/// Summary description for SiteCookie
/// </summary>
public class SiteCookie
{
    private static string _domainCookie;
    public SiteCookie()
    {
       _domainCookie = UserInfo.UserDomain;
    }
    public static void Update(string name, string value, int numdays)
    {
        Update(name, value, numdays, 0);
    }
    public static void Update(string name, string value, int numdays, int minutes)
    {
        HttpCookie ck = new HttpCookie(name);
        ck.Value = value;
        DateTime expireDate = DateTime.Now;
        if (numdays > 0)
            expireDate = expireDate.AddDays(numdays);

        if (minutes > 0)
            expireDate = expireDate.AddMinutes(minutes);

        ck.Expires = expireDate;
        if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserInfo.UserDomain) || HttpContext.Current.Request.Url.Host.ToLower().Equals("www.powerbar.com"))
        {
            ck.Domain = UserInfo.UserDomain;
        }
        else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage." + UserInfo.UserDomain))
        {
            ck.Domain = "stage." + UserInfo.UserDomain;
        }
        else
        {
            ck.Domain = _domainCookie;
        }
        
        ck.Domain = _domainCookie;

        HttpContext.Current.Response.Cookies.Add(ck);
    }
    public static string Get(string name)
    {
        HttpRequest req = HttpContext.Current.Request;
        HttpCookie ck = req.Cookies[name];
        if (ck != null)
            return ck.Value;
        return "";
    }
    public static void Remove(string name)
    {
        Update(name, "", -1);
        HttpRequest req = HttpContext.Current.Request;
        req.Cookies.Remove(name);
    }
    public static void RemoveAll()
    {
        HttpRequest req = HttpContext.Current.Request;
        HttpCookieCollection cookies = req.Cookies;
        for (int i = 0; i < cookies.Count; i++)
        {
            req.Cookies.Remove(cookies[i].Name);
        }
    }
    public static string DomainCookie
    {
        get { return _domainCookie; }
        set { _domainCookie = value; }
    }
}
public class SiteCookieName
{

    public const string RandomUserCrypId = "WDRandomUserCrypId";

    public SiteCookieName()
    {

    }
}
