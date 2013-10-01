using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Net;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using System.Net;
using System.IO;
using SingleReporting.Utilities;
using SingleReporting;



//Author:        Asad Aziz
//Date Created : 6/25/2008
//Last Modified: 7/5/2008

public enum PhotoType
{
    Profile,
    Welcome,
    Gallery,
}

public enum LookUps
{
    Orgnizations = 1,
    Sectors = 2,
    ProjectStatus = 3,
    Appeals = 4,
    Currencies = 5,
    ProjectTypes = 6,
    units = 7,
    DataType = 8,
    NotMembershipMember,
    NotMembershipService,
    NotMembershipRole,
    MemberRoles,
    Role,
    NotMemberRole,
    RoleMember,
    Membsership,
    NotMembership,
    PermissionBits,
    Subsector,
    OrgnizationsTypes,
    ProjectFrequencies,
    SectorFrequncy,
    LocationLevels,
    EmergencyTypes
}

public enum ListType
{
    Member_Type = 1,
    Contact_Type,
    Address_Type,
    Phone_Type,
    WebContact_Type,
    Current_Involvement,
    Compete_Level,
    Geographic_Level,
    Events_Frequency,
    Train_Member,
    Train_Frequency,
    Training_Period,
    Training_Method,
    People_Type,
    Participated_Events,
    Professions,
    Number_of_Years,
    Asset_Type,
    Majority_TeamMembers_Citizens,
    Average_Age_Teammembers,
    Team_Stuff,
    TeamContact_Type,
    Role_In_Team,
    Involvement_Areas,
    Folks,
    TE_Status
}
public enum RepFrequencyUnits
{
    Years = 1,
    Months = 2,
    Weeks = 3,
    Days = 4,

}
//===================================================================================================
/// <summary>
/// Lookup routines
/// Author: Asad Aziz
/// Date: 6/19/2008
/// </summary>
public class Utils
{
    private const string SECRET_SALT = "A5ghMk980CFg42Ws";

    protected static string[] restrictedUsernameSubstrings = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedUsernameSubstrings"]) ? null : ConfigurationManager.AppSettings["RestrictedUsernameSubstrings"].Split(','));
    protected static string[] restrictedUsernames = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedUsernames"]) ? null : ConfigurationManager.AppSettings["RestrictedUsernames"].Split(','));
    protected static string[] restrictedTags = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedTags"]) ? null : ConfigurationManager.AppSettings["RestrictedTags"].Split(','));
    protected static string[] restrictedWords = (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RestrictedWords"]) ? null : ConfigurationManager.AppSettings["RestrictedWords"].Split(','));
    protected static string[] restrictedHitBoxCharacters = new string[] { "‘", "\"", "&", "|", "#", "$", "%", "^", "*", ":", "!", @"\", "<", ">", "~", ";" };

    private static NumberFormatInfo _NumberFormat = null;
    public static NumberFormatInfo NumberFormat
    {
        get
        {
            _NumberFormat = new System.Globalization.NumberFormatInfo();
            _NumberFormat.CurrencyDecimalDigits = 0;
            _NumberFormat.CurrencySymbol = "$";
            return _NumberFormat;
        }
    }
    public static void BindDropDownListToEnum(DropDownList ddl, Type enumType)
    {
        BindDropDownListToEnum(ddl, enumType, "");
    }
    public static void BindDropDownListToEnum(DropDownList ddl, Type enumType, object selectedEnumValue)
    {
        if (selectedEnumValue != null)
            BindDropDownListToEnum(ddl, enumType, Enum.GetName(enumType, selectedEnumValue));
    }
    public static void BindDropDownListToEnum(DropDownList ddl, Type enumType, string selectedValue)
    {
        ddl.Items.Clear();
        ddl.DataSource = Enum.GetNames(enumType);
        ddl.DataBind();
        if (selectedValue.Length > 0)
            ddl.SelectedValue = selectedValue;
    }
    public static void BindDropDownListToEnumDesc(DropDownList ddl, Type enumType, object selectedEnumValue)
    {
        ddl.Items.Clear();
        System.Reflection.FieldInfo[] enumFields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

        try
        {
            for (int i = 0; i < enumFields.Length; i++)
            {
                string value = Convert.ToString((Enum)enumFields[i].GetValue(enumType));
                string name = enumFields[i].Name;

                // Use description
                System.ComponentModel.DescriptionAttribute[] ea =
                    (System.ComponentModel.DescriptionAttribute[])enumFields[i].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (ea.Length > 0)
                    name = ea[0].Description;

                ddl.Items.Add(new ListItem(name, value));
            }

            if (selectedEnumValue != null)
                ddl.SelectedValue = Enum.GetName(enumType, selectedEnumValue);
        }
        catch (Exception ex)
        {
            ddl.Items.Clear();
        }
    }
    public static bool CheckTopLevelDomaiReferrer(HttpContext context, string checkvalue)
    {
        try
        {
            string url = context.Request.UrlReferrer.Host.ToString().ToLower();
            return url.EndsWith(checkvalue.ToLower());
        }
        catch
        {
            return false;
        }
    }
    public static string CheckUsernameRestrictions(string username)
    {
        string checkName = username.ToLower();

        foreach (string s in restrictedUsernames)
        {
            if (checkName == s)
                return (@"""" + s + @""" is a restricted username.&nbsp;&nbsp;Please use a different username.<br>");
        }
        foreach (string s in restrictedUsernameSubstrings)
        {
            if (checkName.EndsWith(s))
                return (@"Username cannot end with """ + s + @""".&nbsp;&nbsp;Please use a different username.<br>");
        }

        // Check bad words
        StringFilter filter = new StringFilter();
        string temp;
        filter.WhiteFilter(checkName, true, out temp);
        if (temp.IndexOf("*") >= 0)
            return "Invalid username.  Please use a different username.";

        return "";
    }

    public static string replaceCommans(string value)
    {
        return value.Replace("'", "''");
    }

    public static DataTable GetAllCountries()
    {
        SqlParameter[] prams = null;
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                return db.GetDataSet("up_getAllCountries", null).Tables[0];
            }
        }
        catch (Exception e)
        {

            //   new SqlLog().InsertSqlLog(0, "Utils.GetCountries", e);


        }
        return null;
    }
    public static string GetStateAbvByID(int stateid)
    {
        SqlDataReader reader = null;
        string abr = string.Empty;
        try
        {
            SqlParameter[] parameter = new SqlParameter[1];
            using (DbManager db = DbManager.GetDbManager())
            {

                parameter[0] = db.MakeInParam("@pintStateId", SqlDbType.Int, 0, stateid);
                db.RunProc("up_getStateId", parameter, out reader);

                if (reader.Read())
                {
                    abr = reader["chAbbreviation"].ToString();
                    return abr;
                }

            }
        }
        catch (Exception ex)
        {
            //     new SqlLog().InsertSqlLog(0, "Utils.GetStateAbvByID", ex);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
        return abr;


    }

    public static void GetLookUpData<T>(ref T t, LookUps lookup)
    {
        GetLookUpData(ref t, lookup, null);
    }
    public static void GetLookUpDataByType<T>(ref T t, LookUps lookup, string sListTypeEnum)
    {
        SqlParameter[] param = new SqlParameter[1];
        DbManager db = DbManager.GetDbManager();
        // param[0] = DbManager.MakeSqlParam("@pListTypeName", SqlDbType.VarChar, 100, ParameterDirection.Input, sListTypeEnum);
        param[0] = db.MakeInParam("@pListTypeName", SqlDbType.VarChar, 100, sListTypeEnum);
        GetLookUpData(ref t, lookup, param);
    }

    public static void GetLookUpData<T>(ref T t, LookUps lookup, SqlParameter[] prams)
    {
        GetLookUpData<T>(ref t, lookup, prams, 0);
    }

    public static void GetLookUpData<T>(ref T t, LookUps lookup, int firstlevelId)
    {
        GetLookUpData<T>(ref t, lookup, null, firstlevelId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="lookup"></param>
    public static void GetLookUpData<T>(ref T t, LookUps lookup, SqlParameter[] prams, int firstlevelId)
    {
        SqlDataReader reader = null;
        DropDownList dropDownList = null;
        RadioButtonList radioList = null;
        CheckBoxList chkList = null;
        ListBox lstList = null;

        if (t is DropDownList)
        {
            dropDownList = t as DropDownList;
            dropDownList.Items.Clear();
        }
        else if (t is RadioButtonList)
            radioList = t as RadioButtonList;

        else if (t is CheckBoxList)
            chkList = t as CheckBoxList;

        else if (t is ListBox)
            lstList = t as ListBox;

        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                switch (lookup)
                {
                    case LookUps.Sectors:
                        prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@username", SqlDbType.VarChar, 100, HttpContext.Current.User.Identity.Name);
                       // reader = db.GetDataReader("Usp_User_GetAllSectors");  
                        reader = db.GetDataReader("up_getAllSectorsNames", prams);
                        break;
                    case LookUps.SectorFrequncy:
                        prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@sectorId", SqlDbType.Int, 0, firstlevelId);
                        reader = db.GetDataReader("up_getAllFrequncybySector", prams);
                        break;

                    case LookUps.ProjectStatus:
                        reader = db.GetDataReader("up_getAllProjectStatus");
                        break;
                    case LookUps.Appeals:
                        reader = db.GetDataReader("up_getAllAppealNames");
                        break;
                    case LookUps.Orgnizations:
                        reader = db.GetDataReader("up_getAllOrganizationNames");
                        break;
                    case LookUps.OrgnizationsTypes:
                        reader = db.GetDataReader("up_getAllOrganizationTypes");
                        break;
                    case LookUps.Currencies:
                        reader = db.GetDataReader("up_getAllCurrencies");
                        break;
                    case LookUps.ProjectTypes:
                        reader = db.GetDataReader("up_getAllProjectTypes");
                        break;
                    case LookUps.LocationLevels:
                        reader = db.GetDataReader("up_getAllLocationLevels");
                        break;
                    case LookUps.EmergencyTypes:
                        reader = db.GetDataReader("up_getAllEmergencyTypes");
                        break;
                    case LookUps.ProjectFrequencies:
                        reader = db.GetDataReader("up_getAllFrequencies");
                        break;
                    case LookUps.units:
                        reader = db.GetDataReader("up_getAllActiveUnits");
                        break;
                    case LookUps.DataType:
                        reader = db.GetDataReader("up_getAllDataType");
                        break;
                    case LookUps.PermissionBits:
                        reader = db.GetDataReader("up_getFreePermissionBits");
                        break;
                    case LookUps.Role:
                        reader = db.GetDataReader("up_getAllRoles");
                        break;
                    case LookUps.NotMemberRole:
                        prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@intmemberid", SqlDbType.Int, 0, firstlevelId);
                        reader = db.GetDataReader("up_member_notroles_getbymemberid", prams);
                        break;
                    case LookUps.MemberRoles:
                        prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@intmemberid", SqlDbType.Int, 0, firstlevelId);
                        reader = db.GetDataReader("up_memberrole_getbymemberid", prams);
                        break;
                    case LookUps.Subsector:
                        prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@intSectorid", SqlDbType.Int, 0, firstlevelId);
                        reader = db.GetDataReader("up_getSubsectorBySectorId", prams);
                       // reader = db.GetDataReader("Usp_User_GetSubSectorBySectorID", prams);
                        break;
                    //case LookUps.OccupancyType:
                    //    reader = db.GetDataReader("up_getOccupancyType");
                    //    break;
                    //case LookUps.County:
                    //    reader = db.GetDataReader("up_getCounty");
                    //    break;
                    //case LookUps.ValueType:
                    //    reader = db.GetDataReader("up_getValueType");
                    //    break;
                    //case LookUps.CurrentStatusTypes:
                    //    reader = db.GetDataReader("up_getCurrentStatusTypes");
                    //    break;
                    //case LookUps.EntityTypes:
                    //    reader = db.GetDataReader("up_getEntityTypes");
                    //    break;
                    //case LookUps.Entities:
                    //    reader = db.GetDataReader("up_getEntities", prams);
                    //    break;
                }

                if (t is DropDownList)
                {
                    ListItem i = new ListItem(("--Select--"), "0");
                    dropDownList.Items.Insert(0, i);
                }

                while (reader.Read())
                {
                    ListItem item = new ListItem();
                    item.Value = reader[0].ToString();
                    item.Text = reader[1].ToString().Replace("<sup>", string.Empty);
                    item.Text = item.Text.Replace("</sup>", string.Empty);
                    if (t is DropDownList)
                    {
                        dropDownList.Items.Add(item);
                    }
                    else if (t is RadioButtonList)
                    {
                        radioList.Items.Add(item);
                    }
                    else if (t is ListBox)
                    {
                        lstList.Items.Add(item);
                    }
                    else if (t is CheckBoxList)
                    {
                        chkList.Items.Add(item);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (null != reader) { reader.Close(); }
        }
    }


    public static string GetMediaAgeString(DateTime MediaDate)
    {
        string returnStr = "";
        TimeSpan dateDiff = DateTime.Now.Subtract(MediaDate);
        int mediaAgeMinutes = Convert.ToInt32(dateDiff.TotalMinutes);

        if (mediaAgeMinutes < 60)
        {
            if (mediaAgeMinutes <= 1)
                returnStr = "1 minute ago";
            else
                returnStr = mediaAgeMinutes.ToString() + " minutes ago";
        }
        else if (mediaAgeMinutes < 1440)
        {
            int hoursAgo = Convert.ToInt32(Math.Floor(Convert.ToDecimal(mediaAgeMinutes) / 60));
            if (hoursAgo == 1)
                returnStr = "1 hour ago";
            else
                returnStr = hoursAgo.ToString() + " hours ago";
        }
        else
        {
            int daysAgo = Convert.ToInt32(Math.Floor(Convert.ToDecimal(mediaAgeMinutes) / 1440));
            if (daysAgo == 1)
                returnStr = "1 day ago";
            else if (daysAgo <= 7)
                returnStr = daysAgo.ToString() + " days ago";
            else
                returnStr = MediaDate.ToShortDateString();
        }

        return returnStr;
    }

    public static string GetEnumDesc(Enum e)
    {
        System.Reflection.FieldInfo EnumInfo = e.GetType().GetField(e.ToString());
        System.ComponentModel.DescriptionAttribute[] EnumAttributes =
            (System.ComponentModel.DescriptionAttribute[])
            EnumInfo.GetCustomAttributes
                (typeof(System.ComponentModel.DescriptionAttribute), false);
        if (EnumAttributes.Length > 0)
        {
            return EnumAttributes[0].Description;
        }
        return e.ToString();
    }
    public static bool IsValidURL(string url)
    {
        string regExPattern = @"^^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
        return Regex.IsMatch(url, regExPattern);
    }
    public static bool IsValidZipCode(string country, string zip)
    {
        // USA, Canda and UK required to enter postal code
        if (country == "US" || country == "CA" || country == "UK")
        {
            if (string.IsNullOrEmpty(zip))
                return false;
            if (zip.Length >= 5 && zip.Length <= 10)
                return true;
            else
                return false;
        }
        else
            return true;
    }

    public static string CleanStringForHitBox(string strInput)
    {
        if (!string.IsNullOrEmpty(strInput))
            for (int x = 0; x < restrictedHitBoxCharacters.Length; x++)
            {
                if (strInput.Contains(restrictedHitBoxCharacters[x]))
                    strInput = strInput.Replace(restrictedHitBoxCharacters[x], "");
            }
        return strInput;
    }

    public static string RemoveWhiteSpaceForHitBox(string strInput)
    {
        if (!string.IsNullOrEmpty(strInput))
            strInput = strInput.Replace(" ", "+");
        return strInput;
    }

    public static string ConvertZipToState(string zip)
    {
        string[] temp = zip.Split('-');
        return ConvertZipToState(Convert.ToInt32(temp[0]));
    }

    public static string ConvertZipToState(int zip)
    {
        string state = "";
        if ((zip >= 600 && zip <= 799) || (zip >= 900 && zip <= 999)) // Puerto Rico (00600-00799 and 900--00999 ranges)
            state = "PR";
        else if (zip >= 800 && zip <= 899) // US Virgin Islands (00800-00899 range)            
            state = "VI";
        else if (zip >= 1000 && zip <= 2799) // Massachusetts (01000-02799 range)
            state = "MA";
        else if (zip >= 2800 && zip <= 2999) // Rhode Island (02800-02999 range)
            state = "RI";
        else if (zip >= 3000 && zip <= 3899) // New Hampshire (03000-03899 range)
            state = "NH";
        else if (zip >= 3900 && zip <= 4999) // Maine (03900-04999 range)
            state = "ME";
        else if (zip >= 5000 && zip <= 5999) // Vermont (05000-05999 range)
            state = "VT";
        else if ((zip >= 6000 && zip <= 6999) && zip != 6390) // Connecticut (06000-06999 range excluding 6390)
            state = "CT";
        else if (zip >= 70000 && zip <= 8999) // New Jersey (07000-08999 range)
            state = "NJ";
        else if ((zip >= 10000 && zip <= 14999) || zip == 6390 || zip == 501 || zip == 544) // New York (10000-14999 range and 6390, 501, 544)
            state = "NY";
        else if (zip >= 15000 && zip <= 19699) // Pennsylvania (15000-19699 range)
            state = "PA";
        else if (zip >= 19700 && zip <= 19999) // Delaware (19700-19999 range)
            state = "DE";
        else if ((zip >= 20000 && zip <= 20099) || (zip >= 20200 && zip <= 20599) || (zip >= 56900 && zip <= 56999)) // District of Columbia (20000-20099, 20200-20599, and 56900-56999 ranges)
            state = "DC";
        else if (zip >= 20600 && zip <= 21999) // Maryland (20600-21999 range)            
            state = "MD";
        else if ((zip >= 20100 && zip <= 20199) || (zip >= 22000 && zip <= 24699)) // Virginia (20100-20199 and 22000-24699 ranges, also some taken from 20000-20099 DC range)
            state = "VA";
        else if (zip >= 24700 && zip <= 26999) // West Virginia (24700-26999 range)
            state = "WV";
        else if (zip >= 27000 && zip <= 28999) // North Carolina (27000-28999 range)
            state = "NC";
        else if (zip >= 29000 && zip <= 29999) // South Carolina (29000-29999 range)            
            state = "SC";
        else if ((zip >= 30000 && zip <= 31999) || (zip >= 39800 && zip <= 39999)) // Georgia (30000-31999, 39901[Atlanta] range)
            state = "GA";
        else if (zip >= 32000 && zip <= 34999) // Florida (32000-34999 range)
            state = "FL";
        else if (zip >= 35000 && zip <= 36999) // Alabama (35000-36999 range)
            state = "AL";
        else if (zip >= 37000 && zip <= 38599) // Tennessee (37000-38599 range)
            state = "TN";
        else if (zip >= 38600 && zip <= 39799) // Mississippi (38600-39999 range)
            state = "MS";
        else if (zip >= 40000 && zip <= 42799) // Kentucky (40000-42799 range)
            state = "KY";
        else if (zip >= 43000 && zip <= 45999) // Ohio (43000-45999 range)
            state = "OH";
        else if (zip >= 46000 && zip <= 47999) // Indiana (46000-47999 range)
            state = "IN";
        else if (zip >= 48000 && zip <= 49999) // Michigan (48000-49999 range)
            state = "MI";
        else if (zip >= 50000 && zip <= 52999) // Iowa (50000-52999 range)
            state = "IA";
        else if (zip >= 53000 && zip <= 54999) // Wisconsin (53000-54999 range)
            state = "WI";
        else if (zip >= 55000 && zip <= 56799) // Minnesota (55000-56799 range)
            state = "MN";
        else if (zip >= 57000 && zip <= 57999) // South Dakota (57000-57999 range)
            state = "SD";
        else if (zip >= 58000 && zip <= 58999) // North Dakota (58000-58999 range)
            state = "ND";
        else if (zip >= 59000 && zip <= 59999) // Montana (59000-59999 range)
            state = "MT";
        else if (zip >= 60000 && zip <= 62999) // Illinois (60000-62999 range)
            state = "IL";
        else if (zip >= 63000 && zip <= 65999) // Missouri (63000-65999 range)
            state = "MO";
        else if (zip >= 66000 && zip <= 67999) // Kansas (66000-67999 range)
            state = "KS";
        else if (zip >= 68000 && zip <= 69999) // Nebraska (68000-69999 range)
            state = "NE";
        else if (zip >= 70000 && zip <= 71599) // Louisiana (70000-71599 range)
            state = "LA";
        else if (zip >= 71600 && zip <= 72999) // Arkansas (71600-72999 range)
            state = "AR";
        else if (zip >= 73000 && zip <= 74999) // Oklahoma (73000-74999 range)
            state = "OK";
        else if ((zip >= 75000 && zip <= 79999) || (zip >= 88500 && zip <= 88599)) // Texas (75000-79999 and 88500-88599 ranges)
            state = "TX";
        else if (zip >= 80000 && zip <= 81999) // Colorado (80000-81999 range)
            state = "CO";
        else if (zip >= 82000 && zip <= 83199) // Wyoming (82000-83199 range)
            state = "WY";
        else if (zip >= 83200 && zip <= 83999) // Idaho (83200-83999 range)
            state = "ID";
        else if (zip >= 84000 && zip <= 84999) // Utah (84000-84999 range)
            state = "UT";
        else if (zip >= 85000 && zip <= 86999) // Arizona (85000-86999 range)
            state = "AZ";
        else if (zip >= 87000 && zip <= 88499) // New Mexico (87000-88499 range)
            state = "NM";
        else if (zip >= 88900 && zip <= 89999) // Nevada (88900-89999 range)
            state = "NV";
        else if (zip >= 90000 && zip <= 96199) // California (90000-96199 range)
            state = "CA";
        else if (zip >= 96700 && zip <= 96899) // Hawaii (96700-96899 range)  
            state = "HI";
        else if (zip >= 97000 && zip <= 97999) // Oregon (97000-97999 range)
            state = "OR";
        else if (zip >= 98000 && zip <= 99499) // Washington (98000-99499 range)
            state = "WA";
        else if (zip >= 99500 && zip <= 99999) // Alaska (99500-99999 range)
            state = "AK";
        return state;
    }

    public static string GetPagingString(int curPage, int pageSize, int rowCount, string url)
    {
        return GetPagingString(curPage, pageSize, rowCount, url, false);
    }

    public static DataRow GetCurrentRow(Repeater sender, RepeaterItemEventArgs e)
    {
        return ((DataTable)(sender.DataSource)).Rows[e.Item.ItemIndex];
    }

    public static string Ellipses(string input, int cutoff)
    {
        if (cutoff < 1 || input.Length <= cutoff)
            return input;
        else
            return input.Substring(0, cutoff) + "...";
    }

    public static string SetPagingQueryString(string pageName)
    {
        string url = pageName.Substring(pageName.LastIndexOf("/") + 1);
        if (url.IndexOf("?") > 0)
        {
            url = url.Substring(0, url.IndexOf("?"));
        }
        return url + "?i=";
    }

    //#region Get Paging String with extra Prams 

    //public static string GetPagingStringwithExtraprams(int curPage, int pageSize, int rowCount, string url,string prams)
    //{
    //    return GetPagingStringwithExtraprams(curPage, pageSize, rowCount, url, false, prams);
    //}
    //     public static string GetPagingStringwithExtraprams(int curPage, int pageSize, int rowCount, string url, bool useAjax,string prams)
    //{
    //    StringBuilder sb = new StringBuilder(200);
    //    int startPage, endPage;

    //    // Previous
    //    if (curPage > 1)
    //    {
    //        //if(useAjax)
    //        //    LoadPage(sb, curPage, curPage - 1, "<img src=images/previousDark.gif border=0 align=absmiddle>", url, useAjax);
    //        //else
    //        //    LoadPage(sb, curPage, curPage - 1, "<img src=images/previousDark.gif border=0 align=absmiddle>", url);
    //        if (useAjax)
    //            LoadPageWithQuerString(sb, curPage, curPage - 1, "<<", url, useAjax,prams);
    //        else
    //            LoadPageWithQuerString(sb, curPage, curPage - 1, "<<", url,prams);
    //    }

    //    // Calculate page range
    //    if (curPage <= 6)
    //        startPage = 1;
    //    else
    //        startPage = curPage - 5;
    //    endPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(rowCount) / Convert.ToDecimal(pageSize)));

    //    // Load pages
    //    if (endPage > 1)
    //    {
    //        while (startPage <= endPage)
    //        {
    //            if (useAjax)
    //                LoadPageWithQuerString(sb, curPage, startPage, "", url.ToString(), useAjax,prams);
    //            else
    //                LoadPageWithQuerString(sb, curPage, startPage, "", url,prams);
    //            startPage++;
    //        }
    //    }

    //    // Next
    //    if (endPage > curPage)
    //    {
    //        if (useAjax)
    //            LoadPageWithQuerString(sb, curPage, curPage + 1, ">>", url, useAjax, prams);
    //        else
    //            LoadPageWithQuerString(sb, curPage, curPage + 1, ">>", url, prams);

    //        //LoadPage(sb, curPage, curPage + 1, "Next", url);
    //        //if(useAjax)
    //        //  LoadPage(sb, curPage, curPage + 1, "<img src=images/nextDark.gif border=0 align=absmiddle>", url, useAjax);
    //        //else
    //        //  LoadPage(sb, curPage, curPage + 1, "<img src=images/nextDark.gif border=0 align=absmiddle>", url);
    //    }

    //    return sb.ToString();
    //}

    //     private static void LoadPageWithQuerString(StringBuilder sb, int curPage, int PageNum, string Caption, string url,string prams)
    //     {
    //         LoadPageWithQuerString(sb, curPage, PageNum, Caption, url, false, prams);
    //     }
    //     private static void LoadPageWithQuerString(StringBuilder sb, int curPage, int PageNum, string Caption, string url, bool useAjax, string prams)
    //     {
    //         sb.Append("&nbsp;&nbsp;");
    //         if (PageNum == curPage)
    //         {
    //             sb.Append("<b>");
    //             sb.Append(PageNum.ToString());
    //             sb.Append("</b>");
    //         }
    //         else
    //         {
    //             if (!useAjax)
    //             {
    //                 sb.Append(@"<a href=""");
    //                 sb.Append(url);
    //                 if (url.IndexOf("?") > 0)
    //                     sb.Append("&p=");
    //                 else
    //                     sb.Append("?p=");
    //                 sb.Append(PageNum.ToString()+prams);
    //                 sb.Append(@""">");
    //                 if (Caption.Length > 0)
    //                     sb.Append(Caption);
    //                 else
    //                     sb.Append(PageNum.ToString());

    //                 sb.Append("</a>");
    //             }
    //             else
    //             {
    //                 sb.Append("<a href='#' onclick='gotoNextPage(" + PageNum.ToString() + ",\"" + url + "\");'>");
    //                 if (Caption.Length > 0)
    //                     sb.Append(Caption);
    //                 else
    //                     sb.Append(PageNum.ToString());
    //                 sb.Append("</a>");
    //             }
    //         }
    //     }



    //#endregion

    public static string GetPagingString(int curPage, int pageSize, int rowCount, string url, bool useAjax)
    {
        StringBuilder sb = new StringBuilder(200);
        int startPage, endPage;

        // Previous
        if (curPage > 1)
        {
            //if(useAjax)
            //    LoadPage(sb, curPage, curPage - 1, "<img src=images/previousDark.gif border=0 align=absmiddle>", url, useAjax);
            //else
            //    LoadPage(sb, curPage, curPage - 1, "<img src=images/previousDark.gif border=0 align=absmiddle>", url);
            if (useAjax)
                LoadPage(sb, curPage, curPage - 1, "<<", url, useAjax);
            else
                LoadPage(sb, curPage, curPage - 1, "<<", url);
        }

        // Calculate page range
        if (curPage <= 6)
            startPage = 1;
        else
            startPage = curPage - 5;
        endPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(rowCount) / Convert.ToDecimal(pageSize)));

        // Load pages
        if (endPage > 1)
        {
            while (startPage <= endPage)
            {
                if (useAjax)
                    LoadPage(sb, curPage, startPage, "", url.ToString(), useAjax);
                else
                    LoadPage(sb, curPage, startPage, "", url);
                startPage++;
            }
        }

        // Next
        if (endPage > curPage)
        {
            if (useAjax)
                LoadPage(sb, curPage, curPage + 1, ">>", url, useAjax);
            else
                LoadPage(sb, curPage, curPage + 1, ">>", url);

            //LoadPage(sb, curPage, curPage + 1, "Next", url);
            //if(useAjax)
            //  LoadPage(sb, curPage, curPage + 1, "<img src=images/nextDark.gif border=0 align=absmiddle>", url, useAjax);
            //else
            //  LoadPage(sb, curPage, curPage + 1, "<img src=images/nextDark.gif border=0 align=absmiddle>", url);
        }

        return sb.ToString();
    }

    private static void LoadPage(StringBuilder sb, int curPage, int PageNum, string Caption, string url)
    {
        LoadPage(sb, curPage, PageNum, Caption, url, false);
    }
    private static void LoadPage(StringBuilder sb, int curPage, int PageNum, string Caption, string url, bool useAjax)
    {
        sb.Append("&nbsp;&nbsp;");
        if (PageNum == curPage)
        {
            sb.Append("<b>");
            sb.Append(PageNum.ToString());
            sb.Append("</b>");
        }
        else
        {
            if (!useAjax)
            {
                sb.Append(@"<a href=""");
                sb.Append(url);
                if (url.IndexOf("?") > 0)
                    sb.Append("&p=");
                else
                    sb.Append("?p=");
                sb.Append(PageNum.ToString());
                sb.Append(@""">");
                if (Caption.Length > 0)
                    sb.Append(Caption);
                else
                    sb.Append(PageNum.ToString());

                sb.Append("</a>");
            }
            else
            {
                sb.Append("<a href='#' onclick='gotoNextPage(" + PageNum.ToString() + ",\"" + url + "\");'>");
                if (Caption.Length > 0)
                    sb.Append(Caption);
                else
                    sb.Append(PageNum.ToString());
                sb.Append("</a>");
            }
        }
    }

    public static void ShowResults(System.Web.UI.MasterPage page, string message, bool blnError)
    {
        StringBuilder msg = new StringBuilder();

        if (blnError)
        {
            msg.Append("<table width=792 border=1 cellspacing=1 cellpadding=1 bgcolor=#CCCCCC align=left><tr bgcolor=\"#EEEEEE\"><td><table width=100% border=0 cellspacing=0 cellpadding=0 align=left><tr><td width=50 align=center valign=top></td><td valign=middle><b><font color=#FF0000>");
            msg.Append(message);
            msg.Append("</font></b></td></tr></table></td></tr></table>");
        }
        else
        {
            msg.Append("<table width=792 border=0 cellspacing=1 cellpadding=10 bgcolor=#92B0DD align=left><tr bgcolor=\"#E2EAF8\"><td><table width=100% border=0 cellspacing=0 cellpadding=0 align=center><tr><td width=50 align=center valign=top></td><td valign=middle><b>");
            msg.Append(message);
            msg.Append("</b></td></tr></table></td></tr></table>");
        }

        ((Panel)page.FindControl("pnlResults")).Visible = true;
        if (blnError)
            ((Literal)page.FindControl("sResults")).Text = msg.ToString();
        else
            ((Literal)page.FindControl("sResults")).Text = msg.ToString();


    }

    public static string GetMD5Hash(string filename)
    {
        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        // Create a new Stringbuilder to collect the bytes and create a string.
        StringBuilder sBuilder = new StringBuilder();
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        using (FileStream fs = new FileStream(filename, FileMode.Open))
        {
            try
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hasher.ComputeHash(fs);

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
        }
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public static int validateAdminLogin(string login, string pwd)
    {
        int userId = 0;
        using (DbManager db = DbManager.GetDbManager())
        {
            SqlParameter[] prams = new SqlParameter[2];
            prams[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 75, login);
            prams[1] = db.MakeInParam("@vchPwd", SqlDbType.VarChar, 75, pwd);

            IDataReader reader = db.GetDataReader("up_adminLog", prams);
            if (reader.Read())
                userId = reader["intuserId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intuserId"]);
            if (reader != null)
                reader.Close();
        }
        return userId;
    }

    public static string GetMediaPlayer(string videoFile)
    {
        StringBuilder sb = new StringBuilder(4096);
        sb.Append("<object ID=WMPlay width=461 height=380 classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\" codebase=\"http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701\"");
        sb.Append("<PARAM name=URL value=\"" + videoFile + "\"");
        sb.Append("<PARAM name=AllowChangeDisplaySize value=True>");
        sb.Append("<PARAM NAME=ShowControls VALUE=1>");
        sb.Append("<PARAM NAME=ShowDisplay VALUE=1>");
        sb.Append("<PARAM NAME=ShowStatusBar VALUE=1>");
        sb.Append("<PARAM NAME=AutoStart VALUE=TRUE>");
        sb.Append("<embed name=WMplay width=461 height=380 type=\"application/x-mplayer2\" pluginspage=\"http://www.microsoft.com/Windows/Downloads/Contents/Products/MediaPlayer/\"");
        sb.Append("src=\"" + videoFile + "\" AutoStart=True></embed>");
        sb.Append("</object>");
        return sb.ToString();
    }

    public static string GetPlayerCode(string path)
    {
        StringBuilder sb = new StringBuilder(4096);
        sb.Append("<OBJECT  classid=\"clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95\" VIEWASTEXT height=\"380\" width=\"460\">");
        sb.Append("<PARAM NAME=\"AutoStart\" VALUE=\"true\">");
        sb.Append("<PARAM NAME=\"Filename\" VALUE=\"" + path + "\">");
        sb.Append("<PARAM NAME=\"ShowStatusBar\" VALUE=\"true\">");
        sb.Append("<PARAM NAME=\"EnableContextMenu\" VALUE=\"false\">");
        sb.Append("<PARAM NAME=\"Loop\" VALUE=\"false\">");
        sb.Append("<EMBED SRC=\"" + path + "\" WIDTH=\"460\" HEIGHT=\"380\" AUTOPLAY=\"true\" CONTROLLER=\"true\" ");
        sb.Append("	KIOSKMODE=\"true\" PLUGINSPAGE=\"http://www.apple.com/quicktime/download/\"> </EMBED>");
        sb.Append("</OBJECT>");
        return sb.ToString();
    }

    public static bool IsValidFileExtension(string extension)
    {
        string[] allowed_extensions = ConfigurationManager.AppSettings["PhotoExtensions"].Split(new char[] { ',' });
        extension = extension.ToLower().Trim();
        for (int i = 0; i < allowed_extensions.Length; i++)
        {
            if (allowed_extensions[i].Equals(extension)) return true;
        }
        return false;
    }

    public static bool IsCrypId(string test)
    {
        if (string.IsNullOrEmpty(test))
            return false;
        else if (test.Length < 32)
            return false;
        else
            return IsAlphaNumeric(test);
    }

    public static bool IsNumeric(char cToTest)
    {
        bool bNumeric = false;
        Regex regexNumeric = new Regex("[^0-9]");
        bNumeric = regexNumeric.IsMatch(cToTest.ToString());
        return !bNumeric;
    }
    public static bool IsNumeric(string sToTest)
    {
        Regex regexNumeric = new Regex(@"^\d+$");
        Match m = regexNumeric.Match(sToTest);
        return m.Success;
    }

    public static bool IsAlpha(char cToTest)
    {
        bool bAlpha = false;
        Regex regexAlpha = new Regex("[^a-zA-Z]");
        bAlpha = regexAlpha.IsMatch(cToTest.ToString());
        return !bAlpha;
    }

    public static bool IsAlphaNumeric(char cToTest)
    {
        bool bAlpha = false;
        Regex regexAlpha = new Regex("[^a-zA-Z0-9]");
        bAlpha = regexAlpha.IsMatch(cToTest.ToString());
        return !bAlpha;
    }

    public static bool IsAlphaNumeric(string ToTest)
    {
        bool bAlpha = false;
        Regex regexAlpha = new Regex("[^a-zA-Z0-9]");
        bAlpha = regexAlpha.IsMatch(ToTest);
        return !bAlpha;
    }


    public static string filterembedCode(string input)
    {
        string chkEmbedCode = "(?i)<(object|embed)[=\"\'/0-9a-zA-Z#;:&-.?<>_ ]*(object|embed)>";
        bool isMatch = Regex.IsMatch(input, chkEmbedCode);

        //if (isMatch)
        //    return ResetEmbedWidthHeight(input);
        //else
        //{
        Regex regex = new Regex(chkEmbedCode, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(input);
        foreach (Match m in matches)
        {
            input = m.Value.ToString();


        }
        return ResetEmbedWidthHeight(input);
        //}
    }

    public static string filterembedCode(string input, int width, int height)
    {
        string chkEmbedCode = "(?i)<(object|embed)[=\"\'/0-9a-zA-Z#;:&-.?<>_ ]*(object|embed)>";
        bool isMatch = Regex.IsMatch(input, chkEmbedCode);

        //if (isMatch)
        //    return ResetEmbedWidthHeight(input, width, height);
        //else
        //{
        Regex regex = new Regex(chkEmbedCode, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(input);
        foreach (Match m in matches)
        {
            input = m.Value.ToString();
        }
        return ResetEmbedWidthHeight(input, width, height);
        //}
    }

    public static bool validateembedCode(string input, int width, int height)
    {
        string chkEmbedCode = "(?i)<(object|embed)[=\"\'/0-9a-zA-Z#;:&-.?<>_ ]*(object|embed)>";
        bool isMatch = Regex.IsMatch(input, chkEmbedCode);

        if (isMatch)
            return true;
        else
        {
            return false;

        }
    }

    public static string ResetEmbedWidthHeight(string input)
    {
        return ResetEmbedWidthHeight(input, SiteInfo.Embed_Width, SiteInfo.Embed_Height);
    }
    public static string ResetEmbedWidthHeight(string input, int width, int height)
    {


        string exp = "(width|height)=[\"|\']?[0-9]+[\"|\']?[%|px]*[\"|\']?";
        Regex regex = new Regex(exp, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(input);
        foreach (Match m in matches)
        {
            string s = string.Empty;
            if (m.Value.ToLower().IndexOf("height") > -1)
            {
                s = m.Value.Substring(m.Value.IndexOf("=") + 1);
                input = input.Replace(s, "\"" + height.ToString() + "\"");
            }
            else if (m.Value.ToLower().IndexOf("width") > -1)
            {
                s = m.Value.Substring(m.Value.IndexOf("=") + 1);
                input = input.Replace(s, "\"" + width.ToString() + "\"");
            }
        }


        return input;

    }




    public static bool IsSafari(string browser)
    {
        string checkBrowser = browser.ToLower();
        if (checkBrowser.IndexOf("safari") >= 0)
            return true;
        else
            return false;
    }
    public static string GetFileNameURL(string filename)
    {
        return GetFileNameURL(filename, false);
    }
    public static string GetFileNameURL(string filename, bool urlEncode)
    {
        // Allows word characters [A-Za-z0-9_], dash
        string regExPattern = @"^[A-Za-z0-9]";
        string RPLCHR = "-";
        int maxFilename = 30;
        filename = filename.Trim();
        if (filename.Length > maxFilename)
            filename = filename.Substring(0, maxFilename);

        StringBuilder filenameURL = new StringBuilder();
        char[] chars = filename.ToCharArray();
        string prevChr = "";
        foreach (char c in chars)
        {
            string chr = c.ToString();
            if ((chr == RPLCHR && prevChr != RPLCHR) || Regex.IsMatch(chr, regExPattern))
            {
                filenameURL.Append(c);
            }
            else
            {
                if (prevChr != RPLCHR)
                    filenameURL.Append(RPLCHR);
                chr = RPLCHR;
            }
            prevChr = chr;
        }

        if (urlEncode)
            return HttpUtility.UrlEncode(filenameURL.ToString().ToLower().Trim());
        else
            return filenameURL.ToString().ToLower().Trim();
    }
    public static string SafeHTML(string strText)
    {
        strText = strText.Replace("<script", "&lt;script");
        return strText;
    }

    public static StringBuilder CleanString(string strBuffer)
    {
        bool bTag = false;
        string strToken = "";
        StringBuilder sbClean = new StringBuilder();

        foreach (char cBuffer in strBuffer)
        {
            // Check whether a token is completed
            if (!IsAlphaNumeric(cBuffer))
            {

                // Check whether the token is clean
                sbClean.Append(GetClean(strToken, bTag));
                sbClean.Append(cBuffer);
                strToken = "";

                switch (cBuffer)
                {
                    case '<':
                        bTag = true;
                        break;
                    case '>':
                        bTag = false;
                        break;
                }
            }
            else
            {

                strToken += cBuffer;
            }
        }

        return sbClean;
    }
    public static StringBuilder GetClean(string strText, bool bTag)
    {
        StringBuilder sbClean = new StringBuilder();

        if (bTag)
        {
            bool bInvalidTag = false;
            foreach (string strTag in restrictedTags)
            {
                if (0 == strTag.CompareTo(strText.ToLower()))
                {
                    bInvalidTag = true;
                    break;
                }
            }
            bTag = bInvalidTag;

            if (bTag)
            {
                sbClean.Append("!-- RESTRICTED --");
            }
        }


        if (!bTag)
        {
            bool bClean = true;
            foreach (string strCurr in restrictedWords)
            {
                if (0 == strCurr.CompareTo(strText.ToLower()))
                {
                    bClean = false;
                    break;
                }
            }
            if (!bClean)
            {
                foreach (char cIndex in strText)
                {
                    sbClean.Append('*');
                }
            }
            else
            {
                sbClean.Append(strText);
            }
        }

        return sbClean;
    }
    /// <summary>
    /// Get the Permission for the current user
    /// </summary>
    /// <param name="resType">Pass resource Type Enumeration</param>
    /// <param name="header"> Master control Header :- Master.FindControl("Header") </param>
    /// <param name="is_delete_allowed">User has Permission to Delete or Not</param>
    /// <param name="is_add_allowed">User has Permission to add or Not</param>
    /// <param name="is_view_allowed">User has Permission to view or Not</param>
    /// <param name="is_update_allowed">User has Permission to update or Not</param>
    public static int GetAdminCurrentUserID()
    {
        int userID = 0;
        HttpCookie cookies = HttpContext.Current.Request.Cookies.Get("PBEP");
        if (cookies != null)
        {
            string[] ckvalues = Regex.Split(Encryption.Decrypt(cookies.Values[0]), "_!_");
            if (ckvalues.Length > 0)
                userID = Convert.ToInt32(ckvalues[0]);
        }
        return userID;
    }

    public static string ApplyLanguageFilter(string input)
    {
        LanguageFilter filter = new LanguageFilter();
        return filter.BadLanguageFilter(filter.RestrictedUserSubstringFilter(filter.RestrictedTagsFilter(input)));
    }
    public static string ApplyUserNameFilter(string input)
    {
        LanguageFilter filter = new LanguageFilter();
        return filter.BadLanguageFilter(filter.RestrictedUserNameFilter(filter.RestrictedUserSubstringFilter(filter.RestrictedTagsFilter(input))));
    }
    public static string ApplyTagFilter(string input)
    {
        LanguageFilter filter = new LanguageFilter();
        return filter.HTMLTagFilter(input);
    }
    public static bool CheckBoolValue(object objvalue)
    {
        string blStringvalue = Convert.ToString(objvalue);
        bool blReturnValue = false;
        if (!string.IsNullOrEmpty(blStringvalue))
        {
            bool.TryParse(blStringvalue, out blReturnValue);
        }
        return blReturnValue;

    }

    public static string TruncateContent(string s, int lenToget)
    {
        if (lenToget >= s.Length || lenToget < 0)
        {
            return s;
        }
        while (s.Substring(0, lenToget).LastIndexOf(" ") != lenToget - 1)
        {
            lenToget++;
            if (s.Length == lenToget)
            {
                break;
            }
        }
        return s.Substring(0, lenToget).Trim();
    }

    public static string TruncateWords(string text)
    {
        return TruncateWords(text, text.Length);
    }

    public static string TruncateWords(string text, int maxLen)
    {
        int i = 0, len = 0, lastI = 0;
        while (i < text.Length && (len = NextWord(text, len, ref i)) <= maxLen)
            lastI = i;
        if (lastI < 1) lastI = Math.Min(maxLen, text.Length);
        return text.Substring(0, lastI).Trim();
    }
    private static int NextWord(string text, int len, ref int pos)
    {
        int i = pos;
        while (i < text.Length && Char.IsWhiteSpace(text, i))
        {
            i++;
            len++;
        }
        if (!ParseLink(text, ref i, ref len))
        {
            while (i < text.Length && !IsLink(text, i) && !Char.IsWhiteSpace(text, i))
            {
                i++;
                len++;
            }
        }
        pos = i;
        return len;
    }
    private static bool IsLink(string text, int i)
    {

        if (i + 1 < text.Length)
        {
            return text[i] == '<' && i + 1 < text.Length && Char.ToLower(text[i + 1]) == 'a';
        }
        else
        {
            return false;
        }
    }
    private static bool ParseLink(string text, ref int i, ref int len)
    {
        if (IsLink(text, i))
        {
            int startText = text.IndexOf('>', i + 1) + 1;
            int end = text.ToLower().IndexOf("</a>", i + 1);
            if (startText >= 0 && end >= 0)
            {
                string innerText = text.Substring(startText, end - startText).Trim();
                len += innerText.Length;
                i = end +
                "</a>".Length;
                return true;
            }
        }
        return false;
    }

    public static string SetHeader(int imageId, string group, string name, string location, bool myPerformance, bool myProfile)
    {
        group = group.Replace(" ", "");
        string t = string.Empty;

        if (myPerformance)
            name = "My Performance";
        else if (myProfile)
            name = "My Profile";

        switch (group.ToLower())
        {
            case "ironman":
                if (myProfile)
                    t = "<h3 class='ironman' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='ironman' style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
            case "dietitians":
            case "dietitian":
            case "dietician":
            case "dieticians":
                if (myProfile)
                    t = "<h3 class='dietician' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='dietician'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
            case "teamintraining":
                if (myProfile)
                    t = "<h3 class='teamintraining' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='teamintraining'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
            //case "teamelite":
            //    if (myProfile)
            //        t = "<h3 class='teamelite' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
            //    else
            //        t = "<h3 class='teamelite'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
            //    break;
            case "racedirector":
                if (myProfile)
                    t = "<h3 class='racedirector' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='racedirector'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
            case "fitness":
                if (myProfile)
                    t = "<h3 class='fitness' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='fitness'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
            case "coach":
            case "coaches":
                if (myProfile)
                    t = "<h3 class='coach' style='color:white;'>" + name + "<img src='/createImage.aspx?i=" + imageId.ToString() + "&m=false'/><br/><span style='font-size:20px;'>" + DateTime.Now.ToString("dd-MMMM-yyyy") + "</span></h3>";
                else
                    t = "<h3 class='coach'  style='color:white;'>" + name + "<br/><span style='font-size:20px;'>" + location + "</span></h3>";
                break;
        }
        return t;
    }
    //TODO: Asad Duplicate code, should come from database but no time to do that now
    public static string GetDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
    }

    public static string fixNullString(Object objValue)
    {
        if (objValue != null && objValue.ToString().Equals("Null")) return "";
        if (System.Convert.ToString(objValue).Trim().Equals("")) return "";
        return System.Convert.ToString(objValue);
    }

    public static string updateWidthHeight(string input, string width, string Height)
    {
        string chkEmbedCode = "^<(object|embed)[.]*(object|embed)>$";
        bool isMatch = Regex.IsMatch(input, chkEmbedCode);

        string patternWidth = "(width)=[\"|\'|( )* ]?[0-9]+[\"|\'|( )*]?";
        string patternHeight = "(height)=[\"|\'|( )* ]?[0-9]+[\"|\'|( )*]?";

        MatchCollection mcWidth = Regex.Matches(input, patternWidth, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        MatchCollection mcHeight = Regex.Matches(input, patternHeight, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        foreach (Match match in mcWidth)
        {
            input = input.Replace(match.Value.ToString(), "width=\"" + width + "\"");

        }

        foreach (Match match in mcHeight)
        {
            input = input.Replace(match.Value.ToString(), "height=\"" + Height + "\"");

        }

        return input;
    }

    public static string CleanHTML(string input)
    {
        return Regex.Replace(input, @"</?(?i:script|div|frameset|frame|iframe|meta|link|style|body|BODY)(.|\n)*?>", "");//todo
    }
    public static string CleanBlogHTML(string input)
    {
        return Regex.Replace(input, @"</?(?i:script|div|frameset|frame|iframe|meta|link|style|body|BODY|P|p)(.|\n)*?>", "");
    }
    public static string CleanVideoHTML(string input)
    {
        return Regex.Replace(input, @"</?(?i:script|SCRIPT|DIV|div|frameset|frame|iframe|meta|link|style|body|BODY|P|p|SPAN|span)(.|\n)*?>", "");
    }

    public static string RemoveHTML(string body)
    {
        return Regex.Replace(body, @"<(.|\n)*?>", "");
    }
    private Bitmap CropImage(Bitmap orignalImage, Point topleft, Point bottomRight)
    {
        Bitmap croppedImage = new Bitmap((bottomRight.Y - topleft.Y), (bottomRight.X - topleft.X));
        Graphics g = Graphics.FromImage(croppedImage);
        g.DrawImage(orignalImage, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height), topleft.X, topleft.Y,
                                                                croppedImage.Width, croppedImage.Height, GraphicsUnit.Pixel);

        g.Dispose();
        return croppedImage;
    }
    public static ArrayList ParseLinks(string HTML)
    {

        System.Text.RegularExpressions.Regex objRegEx = default(System.Text.RegularExpressions.Regex);
        System.Text.RegularExpressions.Match objMatch = default(System.Text.RegularExpressions.Match);
        System.Collections.ArrayList arrLinks = new System.Collections.ArrayList();
        objRegEx = new System.Text.RegularExpressions.Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
        objMatch = objRegEx.Match(HTML);
        while (objMatch.Success)
        {
            string strMatch = null;
            strMatch = objMatch.Groups[1].ToString();
            arrLinks.Add(strMatch);
            objMatch = objMatch.NextMatch();
        }
        return arrLinks;
    }
    #region GetHashTableFromEnum
    public static Hashtable GetEnumForBind(Type enumeration)
    {
        string[] names = Enum.GetNames(enumeration);
        Array values = Enum.GetValues(enumeration);
        Hashtable ht = new Hashtable();


        for (int i = 0; i < names.Length; i++)
        {
            ht.Add(Convert.ToInt32(values.GetValue(i)).ToString(), names[i]);
        }
        return ht;
    }

    #endregion
    public static void SetRememberMeCookie(string userName, string pwd, bool RemeberMe)
    {
        try
        {
            if (RemeberMe)
            {
                HttpCookie cookie = new HttpCookie("RememberMe");
                HttpContext.Current.Response.Cookies.Remove("RememberMe");
                HttpContext.Current.Response.Cookies.Add(cookie);
                cookie.Values.Add("userName", userName);
                cookie.Values.Add("password", Encryption.Encrypt(pwd));
                if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserInfo.UserDomain) || HttpContext.Current.Request.Url.Host.ToLower().Equals("www.powerbar.com"))
                {
                    cookie.Domain = UserInfo.UserDomain;
                }
                else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage.engage." + UserInfo.UserDomain))
                    cookie.Domain = "stage.engage." + UserInfo.UserDomain;
                else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage." + UserInfo.UserDomain))
                {
                    cookie.Domain = "stage." + UserInfo.UserDomain;
                }
                else
                    cookie.Domain = SiteCookie.DomainCookie;

                //cookie.Domain = SiteCookie.DomainCookie;
                DateTime dtExpiry = DateTime.Now.AddDays(30);
                HttpContext.Current.Response.Cookies["RememberMe"].Expires = dtExpiry;
            }
            else
            {
                // if the client has a cookie, expire it
                if (HttpContext.Current.Request.Cookies["RememberMe"] != null)
                {
                    if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserInfo.UserDomain) || HttpContext.Current.Request.Url.Host.ToLower().Equals("www.powerbar.com"))
                        HttpContext.Current.Response.Cookies["domain"].Domain = UserInfo.UserDomain;
                    else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage.engage." + UserInfo.UserDomain))
                        HttpContext.Current.Response.Cookies["domain"].Domain = "stage.engage." + UserInfo.UserDomain;
                    else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage." + UserInfo.UserDomain))
                        HttpContext.Current.Response.Cookies["domain"].Domain = "stage." + UserInfo.UserDomain;
                    else
                        HttpContext.Current.Response.Cookies["domain"].Domain = SiteCookie.DomainCookie;

                    HttpContext.Current.Response.Cookies["RememberMe"].Expires = DateTime.Now.AddYears(-30);
                }
            }
        }

        catch (Exception ex)
        {
            //     new SqlLog().InsertSqlLog(0, "Utils Unable To set Cookie", ex);
            if (HttpContext.Current.Request.Cookies["RememberMe"] != null)
            {
                if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserInfo.UserDomain) || HttpContext.Current.Request.Url.Host.ToLower().Equals("www.powerbar.com"))
                    HttpContext.Current.Response.Cookies["domain"].Domain = UserInfo.UserDomain;
                else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage.engage." + UserInfo.UserDomain))
                    HttpContext.Current.Response.Cookies["domain"].Domain = "stage.engage." + UserInfo.UserDomain;
                else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage." + UserInfo.UserDomain))
                    HttpContext.Current.Response.Cookies["domain"].Domain = "stage." + UserInfo.UserDomain;
                else
                    HttpContext.Current.Response.Cookies["domain"].Domain = SiteCookie.DomainCookie;

                HttpContext.Current.Response.Cookies["RememberMe"].Expires = DateTime.Now.AddYears(-30);
            }
        }
    }


    public static string getDefaultValue(string valueColName, string countyId)
    {
        List<SqlParameter> prams = new List<SqlParameter>();
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                prams.Add(db.MakeInParam("@intCountyId", SqlDbType.Int, 0, Convert.ToInt32(countyId)));
                IDataReader dr = db.GetDataReader("up_County_getDefaultValue", prams.ToArray());
                while (dr.Read())
                {
                    return dr[valueColName].ToString();
                }
            }
            return string.Empty;
        }
        catch
        {
            throw;
        }
    }

    public static bool SetDefaultValue(double commission, int daysHeld, int countyId)
    {
        List<SqlParameter> prams = new List<SqlParameter>();
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                prams.Add(db.MakeInParam("@mnyCommission", SqlDbType.Money, 0, Convert.ToInt32(commission)));
                prams.Add(db.MakeInParam("@intDaysHeld", SqlDbType.Int, 0, Convert.ToInt32(daysHeld)));
                prams.Add(db.MakeInParam("@intCountyId", SqlDbType.Int, 0, Convert.ToInt32(countyId)));
                int exec = db.RunProc("up_County_InsertDefaultValues", prams.ToArray());
                if (exec >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        catch
        {
            throw;
        }
    }
    public static DataTable GetDateDifference(DateTime startDate, DateTime endDate, string freq)
    {

        DataTable dt = null;
        SqlParameter[] prams;
        try
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[3];
                prams[0] = db.MakeInParam("@StartDate", SqlDbType.DateTime, 0, startDate);
                prams[1] = db.MakeInParam("@EndDate", SqlDbType.DateTime, 0, endDate);
                prams[2] = db.MakeInParam("@Freq", SqlDbType.VarChar, 2, freq);
                dt = db.GetDataSet("up_getDateDifference", prams).Tables[0];
            }
        }
        catch (Exception ex)
        {
            //      new SqlLog().InsertSqlLog(0, "Utills DataTable GetDateDifference(DateTime startDate, DateTime endDate, string freq) ", ex);

            return null;
        }
        return dt;

    }

}
