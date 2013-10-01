using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SiteInfo
/// </summary>
public class SiteInfo
{
    static public string VIDEO_PATH;
    static public string IMAGE_PATH;
    static public string DOMAIN;
    static public string MEDIA_DOMAIN;
    static public int Welcome_Thumbnail_Height;
    static public int Welcome_Thumbnail_Width;
    static public int Profile_Thumbnail_Height;
    static public int Profile_Thumbnail_Width;
    static public int Profile_Small_Thumbnail_Width;
    static public int Profile_Small_Thumbnail_Height;    
    static public int Embed_Height;
    static public int Embed_Width;
    public static int ProfileAdminMaxImageSize;
    public static int ProfileAdminMaxImageHeight;
    public static int ProfileAdminImageHeight;
    public static int ProfileAdminImageWidth;
    public static int ProfileAdminPageSize;
    public static int ProfileAdminCMSListPageSize;
    public static int ProfileAdminThumbnailsPerRow;
    public static double GalleryMaxImageSize;

    public static int GalleryMaxImageWidth;
    public static int GalleryMaxImageHeight;
    public static int Gallery_Thumbnail_Height;
    public static int Gallery_Thumbnail_Width;
    public static string HomepageToutsPath;
    public static string TeamEliteResourcesPath;
    public  static string  SiteImagesUploadPath;
    public static string WallpapersToutPath;
    private int _siteId;
    private string _siteLogo;
    private string _siteURL;
   
    /// <summary>
    /// Load all data of SiteInfo
    /// </summary>
    public static void Load()
    {     
        LoadSettings();

        Welcome_Thumbnail_Width         = Convert.ToInt32(ConfigurationManager.AppSettings["WelcomeThumbnailWidth"]);
        Welcome_Thumbnail_Height        = Convert.ToInt32(ConfigurationManager.AppSettings["WelcomeThumbnailHeight"]);
        Profile_Thumbnail_Width         = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileThumbnailWidth"]);
        Profile_Thumbnail_Height        = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileThumbnailHeight"]);
        Profile_Small_Thumbnail_Width   = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileSmallThumbnailWidth"]);
        Profile_Small_Thumbnail_Height  = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileSmallThumbnailHeight"]);
        Embed_Height                    = Convert.ToInt32(ConfigurationManager.AppSettings["EmbedHeight"]);
        Embed_Width                     = Convert.ToInt32(ConfigurationManager.AppSettings["EmbedWidth"]);
        ProfileAdminMaxImageSize        = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminMaxImageSize"]);
        ProfileAdminMaxImageHeight      = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminMaxImageHeight"]);
        ProfileAdminImageHeight         = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminImageHeight"]);
        ProfileAdminImageWidth          = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminImageWidth"]);
        ProfileAdminPageSize            = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminPageSize"]);
        ProfileAdminCMSListPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminCMSListPageSize"]);
        ProfileAdminThumbnailsPerRow    = Convert.ToInt32(ConfigurationManager.AppSettings["ProfileAdminThumbnailsPerRow"]);
        GalleryMaxImageSize = Convert.ToDouble(ConfigurationManager.AppSettings["GalleryMaxImageSize"]);

        GalleryMaxImageWidth          = Convert.ToInt32(ConfigurationManager.AppSettings["GalleryImageWidth"]);
        GalleryMaxImageHeight          = Convert.ToInt32(ConfigurationManager.AppSettings["GalleryImageHeight"]);
        Gallery_Thumbnail_Height          = Convert.ToInt32(ConfigurationManager.AppSettings["GalleryThumbnailHeight"]);
        Gallery_Thumbnail_Width          = Convert.ToInt32(ConfigurationManager.AppSettings["GalleryThumbnailWidth"]);
        SiteImagesUploadPath = ConfigurationManager.AppSettings["SiteImagesUploadPath"];
        HomepageToutsPath = ConfigurationManager.AppSettings["HomepageToutsPath"].ToString();
        WallpapersToutPath = ConfigurationManager.AppSettings["WallpapersToutPath"].ToString();
        TeamEliteResourcesPath = ConfigurationManager.AppSettings["TeamEliteResourcesPath"].ToString();
    }
    /// <summary>
    /// Load All Settings
    /// </summary>
    static void LoadSettings()
    {
        SqlDataReader reader = null;
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@intSiteId", SqlDbType.Int, 0, ConfigurationManager.AppSettings["SiteId"]);
                reader = db.GetDataReader("up_getSiteSettings", prams);
                while (reader.Read())
                {
                    VIDEO_PATH = reader["vchVideoPath"].ToString();
                    IMAGE_PATH = reader["vchImagePath"].ToString();
                    DOMAIN = reader["vchDomain"].ToString();                                        
                    MEDIA_DOMAIN = reader["vchMediaDomain"].ToString();                                                               
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (reader != null) reader.Close();
        }
    }
    /// <summary>
    /// Get DataSet of All Sites
    /// </summary>
    /// <returns>DataSet of All Sites</returns>
    public static DataSet GetSites()
    {
        DataSet ds = null;
        using (DbManager dbm = DbManager.GetDbManager())
        {
            try
            {
                dbm.ProcName = "up_getAllSites";
                ds = dbm.GetDataSet("up_getAllSites");
            }
            catch (Exception el) { }
        }
        return ds;
    }
    /// <summary>
    /// Get Site id of Current Site
    /// </summary>
    /// <returns>Site Id</returns>
    public static int GetCurrentSiteID()
    {        
        return Convert.ToInt32(ConfigurationManager.AppSettings["SiteId"]);
    }
    public static string GetHttpHost()
    {
        return HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
    }
}

