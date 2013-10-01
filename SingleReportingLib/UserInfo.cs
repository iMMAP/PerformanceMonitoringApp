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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Net.Mail;
using SingleReporting.Utilities;
using OCHA.Security.Library;

/// <summary>
/// Summary description for UserInfo
/// </summary>
/// 

namespace SingleReporting
{

    /// <summary>
    /// Get Details for UserInfo
    /// </summary>
    public class UserInfo
    {

        private string _userName = "";
        private string _password = "";

        private int _userId;

        private string _websiteUrl = "";
        private string _firstName = "";
        private string _middleName = "";
        private string _lastName = "";
        private DateTime _datecreated = new DateTime();
        private bool _Active = false;


        public UserInfo()
        {
        }

        public UserInfo(int userId)
        {
            LoadUser(userId);
        }
        public UserInfo(string userName)
        {
            LoadUser(userName);
        }

        //public UserInfo(string userName, string userPassword)
        //{
        //    LoadUser(userName, userPassword);
        //}
        /// <summary>
        /// This Load User by User Id
        /// </summary>
        /// <param name="userId">The user Id by which user should be loaded</param>
        private void LoadUser(int userId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 0, userId);
                    reader = db.GetDataReader("up_getUserById", prams);
                    if (reader.Read())
                        LoadUser(reader);
                }
            }
            catch (Exception ex)
            {
                //       new SqlLog().InsertSqlLog(userId, "UserInfo.loadUserUserId", ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// This Load user by User name
        /// </summary>
        /// <param name="userName">The username whose data needs to be loaded</param>
        private void LoadUser(string userName)
        {
            //IDataReader reader = null;

            //try
            //{
            //    using (DbManager db = DbManager.GetDbManager())
            //    {
            //        SqlParameter[] prams = new SqlParameter[1];
            //        prams[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 25, userName);
            //        reader = db.GetDataReader("up_getUserByUserName", prams);
            //        if (reader.Read())
            //        {
            //            //string s = reader[0].ToString();
            //           // IDataReader bs = reader;
            //            LoadUser(reader);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //  //  new SqlLog().InsertSqlLog(0, "UserInfo.LoadUserCrypId", ex);
            //    throw;
            //}
            //finally
            //{
            //    if (reader != null)
            //        reader.Close();
            //}

            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, userName);
                    reader = db.GetDataReader("up_getUserByUserName", prams);
                    if (reader.Read())
                        LoadUser(reader);
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(1, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// This Get DataTable of Passwords by Login Name and Email Address
        /// </summary>
        /// <param name="Login">Login name whose record needs to be fetched</param>
        /// <param name="Email">EmailAddress whose record needs to be fetched</param>
        /// <returns></returns>
        public static DataTable getPasswordByLoginNameAndEmail(string Login, string Email)
        {
            DataSet ds = null;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@pLoginName", SqlDbType.VarChar, 100, Login);
                    prams[1] = db.MakeInParam("@pEmailAddress", SqlDbType.VarChar, 100, Email);
                    ds = db.GetDataSet("up_getPasswordByLoginNameAndEmail", prams);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];

                        }
                    }


                }


            }
            catch (Exception ex)
            {
#if debug
                throw ex;
#endif
            }
            return null;

        }
        public void GetUser(int UserId, int SiteId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 4, UserId);
                    prams[1] = db.MakeInParam("@intSiteId", SqlDbType.Int, 4, SiteId);
                    reader = db.GetDataReader("up_GetUserInfobyUserId", prams);
                    if (reader.Read())
                        LoadUser(reader);//fill properties of user object.
                }
            }
            catch (Exception ex)
            {
                //    new SqlLog().InsertSqlLog(0, "UserInfo.LoadUserCrypId", ex);

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// This Load User if Username and Password matches
        /// </summary>
        /// <param name="userName">Username of user whose recored to be fetched</param>
        /// <param name="userPassword">Password of user whose record to be fetched</param>
        private void LoadUser(string userName, string userPassword)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 75, userName);
                    prams[1] = db.MakeInParam("@vchPassword", SqlDbType.VarChar, 100, userPassword);
                    reader = db.GetDataReader("up_validateLogin", prams);
                    if (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["intuserId"]);
                        LoadUser(userId);
                    }

                }
            }
            catch (Exception ex)
            {
                //    new SqlLog().InsertSqlLog(0, "UserInfo.LoadUserLoginPwd", ex);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// This get UserInfo class object of User by Username
        /// </summary>
        /// <param name="username">Username whose data needs to be fetched</param>
        /// <returns></returns>
        public static UserInfo GetUserByUserName(string username)
        {
            //up_getUserByDisplayName
            IDataReader reader = null;
            UserInfo userInfo = new UserInfo();
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, username);
                    reader = db.GetDataReader("up_getUserByUserName", prams);
                    if (reader.Read())
                    {
                        userInfo.LoadUser(reader);
                    }
                }
                catch (Exception ex)
                {
                    //new SqlLog().InsertSqlLog(0, "UserInfo.LoadUserByDisplayName", ex);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return userInfo;
        }

        /// <summary>
        /// This get UserInfo class of User object by Display name
        /// </summary>
        /// <param name="displayName">Display Name whose data needs to be fetched</param>
        /// <returns></returns>
        public static UserInfo GetUserByDisplayName(string displayName)
        {
            //up_getUserByDisplayName
            IDataReader reader = null;
            UserInfo userInfo = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchDisplayName", SqlDbType.VarChar, 75, displayName);
                    reader = db.GetDataReader("up_getUserByDisplayName", prams);
                    if (reader.Read())
                    {
                        userInfo = new UserInfo(Convert.ToInt32(reader["intUserId"]));
                    }
                }
                catch (Exception ex)
                {
                    //   new SqlLog().InsertSqlLog(0, "UserInfo.LoadUserByDisplayName", ex);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return userInfo;
        }



        /// <summary>
        /// Get Paged DataTable of All Users
        /// </summary>
        /// <param name="pageId">The page Number</param>
        /// <param name="pageSize">No of records on a Page</param>
        /// <param name="totalRows">Total number of rows Returned</param>
        /// <param name="sortExpression">The colunm Name by which sorting is done</param>
        /// <param name="sortDirection">The Direction of sorting (Ascending/Desecnding)</param>
        /// <returns>Paged DataTable of Users</returns>
        public static DataTable GetUsers(int pageId, int pageSize, out int totalRows, string sortExpression, string sortDirection)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[5];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    prams[2] = db.MakeInParam("@vchSortExpression", SqlDbType.VarChar, 50, sortExpression);
                    prams[3] = db.MakeInParam("@vchSortDirection", SqlDbType.VarChar, 20, sortDirection);
                    prams[4] = db.MakeReturnParam(SqlDbType.Int, 0);
                    DataSet ds = db.GetDataSet("up_getAllUsersPaged", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    totalRows = 0;
                    //     new SqlLog().InsertSqlLog(0, "UserInfo.GetUsers", ex);
                }
            }
            totalRows = prams[4].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[4].Value);
            List<Security.Role> currentrole = Security.Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);

            //foreach (Security.Role item in currentrole)
            //{
            table = null;// Permission.FilterByRole(currentrole, table);
            //}
            return table;
        }
        public static DataTable GetUsers(int pageId, int pageSize, out int totalRows, string strSearch)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[4];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    prams[2] = db.MakeInParam("@vchSearch", SqlDbType.NVarChar, 75, strSearch);
                    prams[3] = db.MakeReturnParam(SqlDbType.Int, 0);
                    table = db.GetDataSet("up_getAllUsers", prams).Tables[0];
                }
                catch (Exception ex)
                {
                    totalRows = 0;
                    //   new SqlLog().InsertSqlLog(0, "UserInfo.GetUsers", ex);
                }
            }
            totalRows = prams[3].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[3].Value);
            return table;
        }

        /// <summary>
        /// This checks for the supplied Display name exists or not
        /// </summary>
        /// <param name="displayName">The Display Name which needs to be searched</param>
        /// <returns>True when exists / False when not Exists</returns>
        public static bool CheckDisplayNamesExists(string displayName)
        {
            IDataReader reader = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchDisplayName", SqlDbType.VarChar, 75, displayName);
                    reader = db.GetDataReader("up_checkDisplayName", prams);
                    if (reader.Read())
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    //    new SqlLog().InsertSqlLog(0, "UserInfo.CheckDisplayNameExists", ex);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// This Get Paged DataTable of Users by Category id, State id, Country id, page id and page size
        /// </summary>
        /// <param name="categoryId">The Category id of user whose date needs to be fectched</param>
        /// <param name="stateId">The State id of user whose date needs to be fectched</param>
        /// <param name="countryId">The Country id of user whose date needs to be fectched</param>
        /// <param name="pageId">Page Number</param>
        /// <param name="pageSize">Number of record on Page</param>
        /// <param name="totalRows">Total Rows Returned</param>
        /// <returns></returns>
        public static DataTable GetUsers(int categoryId, int stateId, int countryId, int pageId, int pageSize, out int totalRows)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[6];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    if (categoryId <= 0)
                        prams[2] = db.MakeInParam("@intCategoryId", SqlDbType.Int, 0, System.Data.SqlTypes.SqlInt32.Null);
                    else
                        prams[2] = db.MakeInParam("@intCategoryId", SqlDbType.Int, 0, categoryId);

                    if (stateId <= 0)
                        prams[3] = db.MakeInParam("@intStateId", SqlDbType.Int, 0, System.Data.SqlTypes.SqlInt32.Null);
                    else
                        prams[3] = db.MakeInParam("@intStateId", SqlDbType.Int, 0, stateId);

                    if (countryId <= 0)
                        prams[4] = db.MakeInParam("@intCountryId", SqlDbType.Int, 0, System.Data.SqlTypes.SqlInt32.Null);
                    else
                        prams[4] = db.MakeInParam("@intCountryId", SqlDbType.Int, 0, countryId);


                    prams[5] = db.MakeReturnParam(SqlDbType.Int, 0);
                    table = db.GetDataSet("up_getInfluencerUsers", prams).Tables[0];
                }
                catch (Exception ex)
                {
                    totalRows = 0;
                    //    new SqlLog().InsertSqlLog(0, "UserInfo.GetUsers", ex);
                }
            }
            totalRows = prams[5].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[5].Value);
            return table;
        }
        private void LoadUser(IDataReader reader)
        {

            _userName = reader["vchEmailAddress"].ToString();
            //_password = reader["vchPassword"].ToString();
            _websiteUrl = reader["vchWebSiteUrl"].ToString();
            //_userId = Convert.ToInt32(reader["intUserId"].ToString());
            _firstName = reader["vchFirstName"].ToString();
            _lastName = reader["vchLastName"].ToString();
            _middleName = reader["vchMiddleName"].ToString();
            _datecreated = Convert.ToDateTime(reader["dtmDateCreated"]);
            _Active = Convert.ToBoolean(reader["bitActive"]);
            //_displayName = reader["vchDisplayName"].ToString();
            //_crypId = reader["vchCrypId"].ToString();
            //_cr3Id = reader["vchCR3ID"].ToString();
            _userId = reader["intUserId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intUserId"]);
            //_path = reader["vchPath"].ToString();
            //_languageId = reader["intLanguageId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intLanguageId"]);

            /*
             user properties
             */


            //_stateId = reader["intStateId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intStateId"]);
            //_state = reader["vchState"].ToString();
            //_countryId = reader["intCountryId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intCountryId"]);
            //_country = reader["vchCountry"].ToString();
            //_influencerCategory = reader["Category"].ToString();
            //_influencerCategoryId = reader["intInfluencerCategoryId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intInfluencerCategoryId"]);
            //_influencerSubCategoryId = reader["intInfluencerSubCategoryId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intInfluencerSubCategoryId"]);
            //_influencerSubCategory = reader["SubCategory"].ToString();
            //_jobTitle = reader["vchJobTitle"].ToString();
            //_sportId = reader["intSportId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intSportId"]);
            //_primarySport = reader["vchSport"].ToString();
            //_sport2Id = reader["intSport2Id"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intSport2Id"]);
            //_sport2 = reader["sport2"].ToString();
            //_sport3Id = reader["intSport3Id"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intSport3Id"]);
            //_sport3 = reader["sport3"].ToString();
            //_active = reader["bitActive"] == System.DBNull.Value ? false : Convert.ToBoolean(reader["bitActive"]);
            //_dateCreated = reader["dtmDateCreated"] as DateTime?;
            //_private = reader["bitPrivate"] == System.DBNull.Value ? false : Convert.ToBoolean(reader["bitPrivate"]);
            //_profileUrl = reader["vchProfileUrl"].ToString();
            //_aboutMe = reader["vchAboutMe"].ToString();
            //_profilePhoto = reader["vchProfilePhoto"].ToString();
            //_profileThumbnail = reader["vchProfileThumbnail"].ToString();
            //_welcomePhoto = reader["vchWelcomePhoto"].ToString();
            //_welcomePhotoThumbnail = reader["vchWelcomePhotoThumbnail"].ToString();
            //_welcomeTitle = reader["vchWelcomeTitle"].ToString();
            //_welcomeMessage = reader["vchWelcomeMessage"].ToString();
            //_ageRangeId = reader["tntAgeRangeId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intAgeRangeId"]);
            //_OKtoSendNewsLetter = reader["bitOKtoSendNewsLetter"] == System.DBNull.Value ? false : Convert.ToBoolean(reader["bitOKtoSendNewsLetter"]);
            //_profileImageId = reader["intImageId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intImageId"]);
            //_influencerUser = reader["bitIsInfluencerUser"] == DBNull.Value ? false : Convert.ToBoolean(reader["bitIsInfluencerUser"]);
            //        try
            //        {
            //            _yearOfBirth = reader["intYOB"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intYOB"]);
            //        }
            //        catch (Exception)
            //        {
            //        }


            //        try
            //        {
            //            _isTeamElite = reader["bitIsTeamElite"] == DBNull.Value ? false : Convert.ToBoolean(reader["bitIsTeamElite"]);
            //        }
            //        catch (Exception)
            //        { }

            //        try
            //        {
            //            _workOutTo = reader["vchWorkoutto"].ToString();
            //            _unusualregimens = reader["vchUnusualRegimens"].ToString();
            //            _leastFavInTraining = reader["vchLeastFavInTraining"].ToString();
            //            _favInTraining = reader["vchFavInTraining"].ToString();
            //            _biggestAthleticAccomplishment = reader["vchBiggestAthleticAccomplishment"].ToString();
            //            _nobodyKnows = reader["vchNobodyKnows"].ToString();
            //            _Superstitions = reader["vchSuperstitions"].ToString();
            //            _biggestChallenge = reader["vchBiggestChallenge"].ToString();
            //            _inspirations = reader["vchInspirations"].ToString();
            //            _favouriteProductId = reader["intFavouriteProductId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intFavouriteProductId"]);
            //        }
            //        catch (Exception)
            //        {
            //#if DEBUG
            //            throw;
            //#endif

            //        }


            //        _aboutMe = reader["vchAboutMe"].ToString();
            //        _interests = reader["vchInterest"].ToString();
            //        _website1 = reader["vchWebsite1"].ToString();
            //        _website2 = reader["vchWebsite2"].ToString();
            //        _website3 = reader["vchWebsite3"].ToString();
            //        _siteId = reader["intSiteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intSiteId"]);
        }

        public static byte[] GetUserProfilePhoto(int photoId, bool IsProfilePhoto)
        {
            byte[] buffer = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@intImageId", SqlDbType.Int, 0, photoId);
                prams[1] = db.MakeInParam("@bIsSmallImage", SqlDbType.Bit, 0, IsProfilePhoto);

                IDataReader reader = null;
                try
                {
                    reader = db.GetDataReader("up_getUserImage", prams);
                    if (reader.Read())
                        buffer = (byte[])reader[0];
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
            return buffer;
        }


        /// <summary>
        ///     todo Change this function
        /// </summary>
        /// <param name="info"></param>
        /// <param name="rememberMe"></param>
        public static void GetAuthenticationTicket(UserInfo info, bool rememberMe) //toto
        {
            StringBuilder sb = new StringBuilder(200);
            // sb.Append(info.CrypId);
            sb.Append("_!_");
            sb.Append(info.UserId.ToString());
            sb.Append("_!_");
            sb.Append(info.UserName);
            sb.Append("_!_");
            //sb.Append(info.CR3Id);
            HttpCookie ck;
            FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, sb.ToString(), DateTime.Now, DateTime.Now.AddDays(5), rememberMe, "");
            string cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (rememberMe)
                ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            ck.Domain = SiteCookie.DomainCookie;//HttpContext.Current.Request.Url.Host;

            if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserDomain))
                ck.Domain = UserInfo.UserDomain;
            else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage.engage." + UserInfo.UserDomain))
                ck.Domain = "stage.engage." + UserInfo.UserDomain;
            else if (HttpContext.Current.Request.Url.Host.ToLower().Equals("stage." + UserDomain))
                ck.Domain = "stage." + UserDomain;
            else
                ck.Domain = SiteCookie.DomainCookie;

            HttpContext.Current.Response.Cookies.Add(ck);
            SiteCookie.Update(SiteCookieName.RandomUserCrypId, System.Guid.NewGuid().ToString(), 30);
        }
        public static bool IsUserLoggedIn()
        {
            return HttpContext.Current.User.Identity.Name != null && HttpContext.Current.User.Identity.Name.Length > 0;
        }

        /// <summary>
        /// This Get User From Cookie by cookie value
        /// </summary>
        /// <param name="cookieValue"></param>
        private void GetUserFromCookie(string cookieValue)
        {
            try
            {
                string[] ckvalues = Regex.Split(cookieValue, "_!_");
                if (ckvalues.Length > 0)
                    //_crypId = ckvalues[0];
                    if (ckvalues.Length > 1)
                        _userId = Convert.ToInt32(ckvalues[1]);
                if (ckvalues.Length > 2)
                    _userName = ckvalues[2];
                // if (ckvalues.Length > 3)
                // _cr3Id = ckvalues[3];
            }
            catch
            {
                // cookie is corrupted, 
                // TODO: reload user info 
            }
        }

        /// <summary>
        /// This Get object of Class UserInfo loaded with currenct User Information
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo()
        {
            UserInfo ui = new UserInfo();

            if (HttpContext.Current.User.Identity.Name != null && HttpContext.Current.User.Identity.Name.Length > 0)
                ui.GetUserFromCookie((string)HttpContext.Current.User.Identity.Name);

            return ui;
        }

        /// <summary>
        /// This Disable Users by user id
        /// </summary>
        /// <param name="userId">The id of User which needs to be disabled</param>
        public static void DisableUsers(int userId)
        {
            SqlParameter[] prams = new SqlParameter[1];
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 0, userId);
                    int exe = db.RunProc("up_Disable_user", prams);
                }
                catch (Exception ex)
                {
                    //   new SqlLog().InsertSqlLog(0, "UserInfo.DisableUsers", ex);
                }
            }

        }

        /// <summary>
        /// This Enalbe users by user id
        /// </summary>
        /// <param name="userId">The id of User which needs to be enabled</param>
        public static void EnableUsers(int userId)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[1];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 0, userId);

                    db.RunProc("up_Enable_user", prams);
                }
                catch (Exception ex)
                {
                    //    new SqlLog().InsertSqlLog(0, "UserInfo.EnableUsers", ex);
                }
            }

        }
        public static bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$");
        }

        /// <summary>
        /// This Checks for the supplied email exists or not
        /// </summary>
        /// <param name="email">The email which needs to be searched</param>
        /// <returns>True when exist / Fasle when not exists</returns>
        public static bool CheckEmailExists(string email)
        {
            bool emailExists = false;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@vchEmailAdress", SqlDbType.VarChar, 75, email);
                    prams[1] = db.MakeReturnParam(SqlDbType.Int, 0);
                    int exe = db.RunProc("up_checkEmailExists", prams);
                    int count = Convert.ToInt32(prams[1].Value.ToString());
                    if (count > 0)
                        emailExists = true;
                }
            }
            catch (Exception ex)
            {
                //       new SqlLog().InsertSqlLog(0, "UserInfo.CheckEmailExists", ex);
            }
            return emailExists;
        }

        /// <summary>
        /// This insert new user with object of class UserInfo
        /// </summary>
        /// <param name="user">The object of class UserInfo which needs to be Inserted</param>
        /// <returns></returns>
        public static int InsertUser(UserInfo user)
        {

            SqlParameter retVal = null;
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {

                    prams.Add(db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, user.UserName));
                    prams.Add(db.MakeInParam("@vchPassword", SqlDbType.VarChar, 100, Encryption.Encrypt(user.Password)));
                    prams.Add(db.MakeInParam("@vchFirstName", SqlDbType.VarChar, 50, user.FirstName));
                    prams.Add(db.MakeInParam("@vchMiddleName", SqlDbType.VarChar, 50, user.MiddleName));
                    prams.Add(db.MakeInParam("@vchLastName", SqlDbType.VarChar, 50, user.LastName));
                    prams.Add(db.MakeInParam("@vchWebsiteUrl", SqlDbType.VarChar, 500, user.WebsiteUrl));
                    prams.Add(db.MakeInParam("@dtmDateCreated", SqlDbType.DateTime, 0, user.DateCreated));
                    prams.Add(db.MakeInParam("@bitActive", SqlDbType.Bit, 0, user.Active));


                    prams.Add(retVal = db.MakeReturnParam(SqlDbType.Int, 0));
                    int exec = db.RunProc("up_insertUser", prams.ToArray());
                }
                user._userId = retVal.Value == DBNull.Value ? 0 : Convert.ToInt32(retVal.Value);
            }
            catch (Exception ex)
            {
                //           new SqlLog().InsertSqlLog(0, "UserInfo.AddUser", ex);
            }
            return user._userId;
        }


        /// <summary>
        /// This update User Profile Message with the object of class UserInfo
        /// </summary>
        /// <param name="user">Object of class Userinfo with which User Profile Message should be Updated</param>
        /// <returns></returns>
        public static bool UpdateProfileMessage(UserInfo user)
        {
            try
            {
                UserInfo userInfo = new UserInfo(user.UserId);
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[8];
                    prams[0] = db.MakeInParam("@intUserId", SqlDbType.Int, 0, user.UserId);
                    //if (!string.IsNullOrEmpty(user.ProfilePhoto))
                    //    prams[1] = db.MakeInParam("@vchProfilePhoto", SqlDbType.VarChar, 120, user.ProfilePhoto);
                    //else
                    //    prams[1] = db.MakeInParam("@vchProfilePhoto", SqlDbType.VarChar, 120, userInfo.ProfilePhoto);

                    //prams[2] = db.MakeInParam("@vchAboutMe", SqlDbType.VarChar, 500, user.AboutMe);
                    //prams[3] = db.MakeInParam("@bitPrivate", SqlDbType.Bit, 0, user.IsPrivate);


                    int exec = db.RunProc("up_updateProfileMessage", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //    new SqlLog().InsertSqlLog(user.UserId, "UserInfo.UpdateProfileMessage", ex);
                return false;
            }
        }

        /// <summary>
        /// This update User Profile with the object of class UserInfo
        /// </summary>
        /// <param name="user">Object of class Userinfo with which User Profile should be Updated</param>
        /// <returns></returns>
        public static bool UpdateProfile(UserInfo user)
        {
            try
            {
                //UserInfo userInfo = new UserInfo(user.UserId);

                List<SqlParameter> prams = new List<SqlParameter>();
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        ///prams.Add(db.MakeInParam("@intUserId", SqlDbType.Int, 0, user.UserId));
                        /*
                         * @vchUsername varchar(155),
     @vchFirstName varchar(50),
     @vchMiddleName varchar(50),
     @vchLastName varchar(50),
     @vchPassword varchar(50),
     @vchWebsiteUrl varchar(500)
                         * 
                         * */
                        prams.Add(db.MakeInParam("@vchUsername", SqlDbType.VarChar, 155, user.UserName));
                        prams.Add(db.MakeInParam("@vchFirstName", SqlDbType.VarChar, 50, user.FirstName));
                        prams.Add(db.MakeInParam("@vchMiddleName", SqlDbType.VarChar, 50, user.MiddleName));
                        prams.Add(db.MakeInParam("@vchLastName", SqlDbType.VarChar, 50, user.LastName));
                        prams.Add(db.MakeInParam("@vchPassword", SqlDbType.VarChar, 100, SingleReporting.Utilities.Encryption.Encrypt(user.Password)));
                        prams.Add(db.MakeInParam("@vchWebsiteUrl", SqlDbType.VarChar, 500, user.WebsiteUrl));
                        //prams.Add(db.MakeInParam("@vchAboutMe", SqlDbType.NVarChar, 1000, user.AboutMe));
                        int exec = db.RunProc("up_UpdateUser", prams.ToArray());
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    //   new SqlLog().InsertSqlLog(user.UserId, "UserInfo.UpdateProfile", ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // new SqlLog().InsertSqlLog(user.UserId, "UserInfo.UpdateProfileMessage", ex);
                return false;
            }
        }
   

   
        /// <summary>
        /// This Varify User by Username and Password
        /// </summary>
        /// <param name="strUsername">The Username of user which needs to be varified</param>
        /// <param name="strPassword">The password of user which needs to be varified</param>
        /// <returns>True when exist / Fasle when does'nt exist</returns>
        public static bool AuthenticateUser(string strUsername, string strPassword)
        {
            bool verified = false;
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] prams = new SqlParameter[3];

                try
                {
                    prams[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, strUsername);

                    prams[1] = db.MakeInParam("@vchPassword", SqlDbType.VarChar, 100, strPassword);
                    prams[2] = db.MakeReturnParam(SqlDbType.Bit, 0);

                    db.RunProc("up_VerifyUser", prams);
                    verified = Convert.ToBoolean(prams[2].Value);
                }
                catch (Exception ex)
                {

                    //       new SqlLog().InsertSqlLog(0, "UserInfo.GetUsers", ex);

                }
            }
            return verified;

        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string WebsiteUrl
        {
            get { return _websiteUrl; }
            set { _websiteUrl = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }



        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public DateTime DateCreated
        {
            get { return _datecreated; }
            set { _datecreated = value; }
        }
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }

        }



        public static string UserDomain
        {
            get { return ConfigurationManager.AppSettings["SiteDomain"]; }

        }

    }
}