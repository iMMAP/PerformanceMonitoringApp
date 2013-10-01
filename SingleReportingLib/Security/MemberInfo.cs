using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using SingleReporting.Utilities;
using System.Threading;


namespace OCHA.Security.Library
{
    public class MemberInfo
    {

        #region member
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _fullName;
        private DateTime _dateCreated;
        private int _createdById;
        private int _memberId;
        private bool _isActive = true;
        private string _OrganizationName = string.Empty;
        private string _emailId = string.Empty;
        private int _roleId;
        private string _login;
        private string _password;
        private string _roleName;

        // 8,11 April 2011
        private string _ReqTelephoneNo;
        private string _ReqCellNo;

        private string _SupFirstName;
        private string _SupLastName;
        private string _SupTelephoneNo;
        private string _SupCellNo;
        private string _SupEmail;

        private string _RemarksByManager;
        private bool _IsApprovedByManager;
        private string _ReasonforAccount;
        private bool _IsTrained;

        private int _ReqProvience;
        private int _ReqCity;

       #region new properties  8,11 april 2011


        public int ReqProvience
        {
            get { return _ReqProvience; }
            set { _ReqProvience = value; }
        }


        public int ReqCity
        {
            get { return _ReqCity; }
            set { _ReqCity = value; }
        }


        public string RemarksByManager
        {
            get { return _RemarksByManager; }
            set { _RemarksByManager = value; }
        }

        public string ReasonforAccount
        {
            get { return _ReasonforAccount; }
            set { _ReasonforAccount = value; }
        }

        public bool IsApprovedByManager
        {
            get { return _IsApprovedByManager; }
            set { _IsApprovedByManager = value; }
        }


        public bool IsTrained
        {
            get { return _IsTrained; }
            set { _IsTrained = value; }
        }
        
        
        public string ReqTelephoneNo
        {
            get { return _ReqTelephoneNo; }
            set { _ReqTelephoneNo = value; }
        }

        public string ReqCellNo
        {
            get { return _ReqCellNo; }
            set { _ReqCellNo = value; }
        }

        public string SupFirstName
        {
            get { return _SupFirstName; }
            set { _SupFirstName = value; }
        }

        public string SupLastName
        {
            get { return _SupLastName; }
            set { _SupLastName = value; }
        }

        public string SupTelephoneNo
        {
            get { return _SupTelephoneNo; }
            set { _SupTelephoneNo = value; }
        }

        public string SupCellNo
        {
            get { return _SupCellNo; }
            set { _SupCellNo = value; }
        }

        public string SupEmail
        {
            get { return _SupEmail; }
            set { _SupEmail = value; }
        }
       

#endregion

       

        public static string UserDomain
        {
            get { return String.Empty; }
        }


        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public int MemberId
        {
            get { return _memberId; }
            set { _memberId = value; }
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
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string LoginId
        {
            get { return _login; }
            set { _login = value; }
        }
        public int RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }
        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }
        public string EmailId
        {
            get { return _emailId; }
            set { _emailId = value; }
        }

        public int CreatedById
        {
            get { return _createdById; }
            set { _createdById = value; }
        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public bool ISActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }

        #endregion

        public MemberInfo() { }

        public MemberInfo(int memberId)
        {
            Load(memberId);
        }

        private void Load(int memberId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, memberId);
                    reader = db.GetDataReader("up_Member_getById", prams);
                    if (reader.Read())
                        Load(reader);
                }
            }
            catch (Exception e)
            {
                //  new SqlLog().InsertSqlLog(0, "Member.Load", e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void Load(IDataReader reader)
        {
            /* [intMemberId]
          ,[vchEmail]
          ,[vchFirstName]
          ,[vchMiddleName]
          ,[vchLastName]
          ,[intCreatedBy]
          ,[bitActive]
          ,[dtmDateCreated]*/
            _memberId = reader["intMemberId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intMemberId"]);
            _emailId = reader["vchEmail"].ToString();
            _firstName = reader["vchFirstName"].ToString();
            _middleName = reader["vchMiddleName"].ToString();
            _lastName = reader["vchLastName"].ToString();
            _createdById = reader["intCreatedBy"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["intCreatedBy"]);
            _isActive = reader["bitActive"] == System.DBNull.Value ? false : Convert.ToBoolean(reader["bitActive"]);
            _dateCreated = Convert.ToDateTime(reader["dtmDateCreated"]);
            _OrganizationName = clsCommon.ParseString(reader["Organization_Name"]);
            _roleName = reader["vchRoleName"].ToString();
            _login = reader["vchLogin"].ToString();


        }




        #region InsertMember(MemberInfo m) [Updated on 5 April 2011]





        public static int InsertMember(MemberInfo member)
        {
            SqlParameter[] prams = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[21];
                    prams[0] = db.MakeInParam("@pvchFirstName", SqlDbType.VarChar, 200, member.FirstName);
                    prams[1] = db.MakeInParam("@pvchMiddleName", SqlDbType.VarChar, 200, member.MiddleName);
                    prams[2] = db.MakeInParam("@pvchLastName", SqlDbType.VarChar, 200, member.LastName);
                    prams[3] = db.MakeInParam("@pintCreatedBy", SqlDbType.Int, 0, member.CreatedById);
                    prams[4] = db.MakeInParam("@pvchEmail", SqlDbType.VarChar, 200, member.EmailId);
                    prams[5] = db.MakeInParam("@pintMemberId", SqlDbType.Int, 0, member.MemberId);
                    prams[6] = db.MakeReturnParam(SqlDbType.Int, 0);
                    prams[7] = db.MakeInParam("@pbitActive", SqlDbType.Bit, 0, member.ISActive);
                    prams[8] = db.MakeInParam("@pintRoleId", SqlDbType.Int, 5, member.RoleId);
                    prams[9] = db.MakeInParam("@ReqTelephoneNo", SqlDbType.VarChar, 150, member.ReqTelephoneNo);
                    prams[10] = db.MakeInParam("@ReqCellNo", SqlDbType.VarChar, 150, member.ReqCellNo);
                    prams[11] = db.MakeInParam("@SupFirstName", SqlDbType.VarChar, 150, member.SupFirstName);
                    prams[12] = db.MakeInParam("@SupLastName", SqlDbType.VarChar, 150, member.SupLastName);
                    prams[13] = db.MakeInParam("@SupTelephoneNo", SqlDbType.VarChar, 150, member.SupTelephoneNo);
                    prams[14] = db.MakeInParam("@SupCellNo", SqlDbType.VarChar, 150, member.SupCellNo);
                    prams[15] = db.MakeInParam("@SupEmail", SqlDbType.VarChar, 150, member.SupEmail);

                    prams[16] = db.MakeInParam("@IsApprovedByManager", SqlDbType.Bit, 0, member.IsApprovedByManager);
                    prams[17] = db.MakeInParam("@ReasonForAccount", SqlDbType.VarChar,2000, member.ReasonforAccount);
                    prams[18] = db.MakeInParam("@Istrained", SqlDbType.Bit, 150, member.IsTrained);

                    prams[19] = db.MakeInParam("@ReqProvience", SqlDbType.Int, 5, member.ReqProvience);
                    prams[20] = db.MakeInParam("@ReqCity", SqlDbType.Int, 5, member.ReqCity);


                    int exec = db.RunProc("UP_member_InsertUpdate", prams);
                }
            }
            catch (Exception)
            {
                // new SqlLog().InsertSqlLog(0, "MemberInfo.InsertMember", ex);
            }
            return prams[6].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[6].Value);
        }









        #endregion






        public static DataTable getUserMemberOrganiztionByMemberId(int memberId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, memberId);

                    return db.GetDataSet("up_getMemberOrgnizationDetail", prams).Tables[0];

                }
            }
            catch (Exception ex)
            {
                //     new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

            }
            return null;

        }

        public static int InsertMemberOrganization(int memberId, int organizationId, string designation, string dept, string phone, string comment)
        {
            return InsertMemberOrganization(memberId, organizationId, designation, dept, phone, comment, string.Empty, 0);
        }





        #region Insert member to Orginization new methods() 8 April



        public static int InsertMemmberIntoOrginization(int Member_ID, int Org_ID)
        {
            return InsertMemberOrganization(Member_ID, Org_ID);
        }



        public static int InsertMemberOrganization(int memberId, int organizationId)
        {
            SqlParameter[] prams = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[4];

                    prams[0] = db.MakeInParam("@pMemberId", SqlDbType.Int,0, memberId);
                    prams[1] = db.MakeInParam("@pOrganizationId", SqlDbType.Int, 0, organizationId);
                    prams[2] = db.MakeInParam("@pMemberOrganizationId", SqlDbType.Int, 0, 0);
                    prams[3] = db.MakeReturnParam(SqlDbType.Int, 0);

                    int exec = db.RunProc("Usp_User_Member_Organization_InsertUpdate", prams);
                }
            }
            catch (Exception)
            {
                // new SqlLog().InsertSqlLog(0, "MemberInfo.InsertMember", ex);
            }
            return prams[3].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[3].Value);
        }

        #endregion










        public static int InsertMemberOrganization(int memberId, int organizationId, string designation, string dept, string phone, string comment, string tempOrg, int RoleID)
        {
            SqlParameter[] prams = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[10];
                    prams[0] = db.MakeInParam("@pComment", SqlDbType.VarChar, 100, comment);
                    prams[1] = db.MakeInParam("@pPhone", SqlDbType.VarChar, 100, phone);
                    prams[2] = db.MakeInParam("@pDesignation", SqlDbType.VarChar, 100, designation);
                    prams[3] = db.MakeInParam("@pDepartment", SqlDbType.VarChar, 150, dept);
                    prams[4] = db.MakeInParam("@pMemberId", SqlDbType.Int, 0, memberId);
                    prams[5] = db.MakeInParam("@pMemberOrganizationId", SqlDbType.Int, 0, 0);
                    prams[6] = db.MakeInParam("@pOrganizationId", SqlDbType.Int, 0, organizationId);
                    prams[7] = db.MakeInParam("@pTempOrg", SqlDbType.VarChar, 150, tempOrg);
                    prams[8] = db.MakeInParam("@pTempRole", SqlDbType.Int, 0, RoleID);
                    prams[9] = db.MakeReturnParam(SqlDbType.Int, 0);
                    int exec = db.RunProc("UP_Member_Organization_InsertUpdate", prams);
                }
            }
            catch (Exception)
            {
                // new SqlLog().InsertSqlLog(0, "MemberInfo.InsertMember", ex);
            }
            return prams[9].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[9].Value);
        }

        public static bool UpdateMember(MemberInfo member)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[7];

                    //prams[0] = db.MakeInParam("@intMemberTypeId", SqlDbType.Int, 0, member.MemberTypeId);
                    //prams[1] = db.MakeInParam("@vchFirstName", SqlDbType.VarChar, 100, member.FirstName);
                    //prams[2] = db.MakeInParam("@vchMiddleName", SqlDbType.VarChar, 100, member.MiddleName);
                    //prams[3] = db.MakeInParam("@vchLastName", SqlDbType.VarChar, 100, member.LastName);
                    //prams[4] = db.MakeInParam("@bitActive", SqlDbType.Bit, 0, member.IsActive);
                    //prams[5] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, member.MemberId);
                    //prams[6] = db.MakeInParam("@vchEmail", SqlDbType.VarChar, 0, member.EmailId);
                    int exec = db.RunProc("up_member_update", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //     new SqlLog().InsertSqlLog(0, "memberinfo.UpdateMember", ex);
                return false;
            }

        }


        public static bool CheckLock(string usrname, string pasword)
        {
            bool chkStat = false;
            try {
            
            using (DbManager db = DbManager.GetDbManager())
            {
            
              var prms = new SqlParameter[2];
              prms[0] = db.MakeInParam("@username",  SqlDbType.VarChar, 150, usrname);
              prms[1] = db.MakeInParam("@password", SqlDbType.VarChar, 150, pasword);

              DataSet ds = db.GetDataSet("Usp_User_CheckUserStatus", prms);
              int numofrows = ds.Tables[0].Rows.Count;

              if (numofrows > 0)
              {

                  chkStat = true;
              }


            }
            
            
            }
            catch (Exception exx)
            {
            }

            return chkStat;
        }

        public static bool CheckUserNameAvalibality(string LoginName)
        {
            bool isAvalible = false;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prms = new SqlParameter[1];
                    prms[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 200, LoginName);

                    DataTable tbl = db.GetDataSet("Usp_User_CheckAvalibalityOfUser", prms).Tables[0];
                    if (tbl.Rows.Count > 0)
                    {
                        isAvalible = false;
                    }
                    else
                    {
                        isAvalible = true;
                    }

                }

            }
            catch
            {

            }
            return isAvalible;

        }
       

        //Cookie is set here Role is placed in Cookie

        public static MemberInfo AuthenticateMember(string login, string password)
        {
            MemberInfo member = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 75, login);
                    prams[1] = db.MakeInParam("@vchPassword", SqlDbType.VarChar, 175, password);

                    using (IDataReader reader = db.GetDataReader("up_authenticateMember", prams))
                    {
                        if (reader.Read())
                        {
                            member = new MemberInfo();
                            member.MemberId = reader["intMemberId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intMemberId"]);
                            member.FirstName = reader["vchFirstName"].ToString();
                            member.MiddleName = reader["vchMiddleName"].ToString();
                            member.LastName = reader["vchLastName"].ToString();
                            member.FullName = member.FirstName + " " + member.MiddleName + " " + member.LastName;
                            member.RoleId = reader["intRoleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intRoleId"]);
                            SetSecurityTokenCookie(reader["vchSecurityToken"].ToString());
                            SetRoleIdCookie(member.RoleId.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return member;
        }

        public static MemberInfo GetMemberInfo(string login)
        {
            MemberInfo member = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 75, login);

                    using (IDataReader reader = db.GetDataReader("up_GetMemberInfoByName", prams))
                    {
                        if (reader.Read())
                        {
                            member = new MemberInfo();
                            member.MemberId = reader["intMemberId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intMemberId"]);
                            member.FirstName = reader["vchFirstName"].ToString();
                            member.MiddleName = reader["vchMiddleName"].ToString();
                            member.LastName = reader["vchLastName"].ToString();
                            member.FullName = member.FirstName + " " + member.MiddleName + " " + member.LastName;
                            member.RoleId = reader["intRoleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intRoleId"]);
                            member.OrganizationName = clsCommon.ParseString(reader["Organization_Name"]);

                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return member;
        }

        public static void GetAuthenticationTicket(MemberInfo info, bool rememberMe)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append(Encryption.Encrypt(info.MemberId.ToString()));
            sb.Append("_!_");
            sb.Append(Encryption.Encrypt(info.FullName));
            sb.Append("_!_");
            sb.Append(Encryption.Encrypt(info.RoleId.ToString()));
            HttpCookie ck;
            FormsAuthenticationTicket tkt = new FormsAuthenticationTicket(1, sb.ToString(), DateTime.Now, DateTime.Now.AddDays(5), rememberMe, "");
            string cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (rememberMe)
                ck.Expires = tkt.Expiration;
            ck.Path = FormsAuthentication.FormsCookiePath;
            ck.Domain = SiteCookie.DomainCookie;//HttpContext.Current.Request.Url.Host;

            if (HttpContext.Current.Request.Url.Host.ToLower().Equals(UserDomain))
                ck.Domain = MemberInfo.UserDomain;
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

        private void GetUserFromCookie(string cookieValue)
        {
            try
            {
                string[] ckvalues = Regex.Split(cookieValue, "_!_");
                if (ckvalues.Length > 0)
                    _memberId = clsCommon.ParseInt(Encryption.Decrypt((ckvalues[0])));
                if (ckvalues.Length > 1)
                    _fullName = Encryption.Decrypt(ckvalues[1]);
                if (ckvalues.Length > 2)
                    _roleId = clsCommon.ParseInt(Encryption.Decrypt(ckvalues[2]));
            }
            catch
            {
                // cookie is corrupted, 
                // TODO: reload memberinfo 
            }
        }

        public static MemberInfo GetCurrentUserInfo()
        {
            MemberInfo ui = new MemberInfo();

            //if (HttpContext.Current.User.Identity.Name != null && HttpContext.Current.User.Identity.Name.Length > 0)
            //    ui.GetUserFromCookie((string)HttpContext.Current.User.Identity.Name);

            return ui;
        }


        public static void SetRoleIdCookie(string roleId)
        {
            try
            {
                HttpContext.Current.Response.Cookies.Remove("RoleId");
                HttpCookie cookie = new HttpCookie("RoleId");
                cookie.Value = Encryption.Encrypt(roleId);
                HttpContext.Current.Response.Cookies.Add(cookie);

            }
            catch (Exception)
            {
            }
        }


        public static void SetSecurityTokenCookie(string token)
        {
            try
            {
                HttpContext.Current.Response.Cookies.Remove("SecurityToken");
                HttpCookie cookie = new HttpCookie("SecurityToken");

                cookie.Value = token;//Encryption.Encrypt(token);
                //  cookie.Domain = "sr.si-sv2826.com";
                HttpContext.Current.Response.Cookies.Add(cookie);
                //cookie= HttpContext.Current.Request.Cookies.Get("SecurityToken");
                // DateTime dtExpiry = DateTime.Now.AddHours(10);
                // HttpContext.Current.Response.Cookies["SecurityToken"].Expires = dtExpiry;

            }
            catch (Exception)
            {
                //new SqlLog().InsertSqlLog(0, "Unable to Set RememberMe Coookie", ex);
            }
        }


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
                    DateTime dtExpiry = DateTime.Now.AddDays(30);
                    HttpContext.Current.Response.Cookies["RememberMe"].Expires = dtExpiry;
                }
                else
                {
                    // if the client has a cookie, expire it
                    if (HttpContext.Current.Request.Cookies["RememberMe"] != null)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                //  new SqlLog().InsertSqlLog(0, "Unable to Set RememberMe Coookie", ex);
            }
        }

        public static DataTable GetAllMembers(int pageId, int pageSize, out int totalRows)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[3];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    prams[2] = db.MakeReturnParam(SqlDbType.Int, 0);
                    DataSet ds = db.GetDataSet("up_getAllUsersPaged", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {
                    totalRows = 0;
                }
            }
            totalRows = prams[2].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[2].Value);
            return table;
        }
        public static DataTable GetAllMembers(int pageId, int pageSize, out int totalRows, bool isActive)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[4];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    prams[2] = db.MakeReturnParam(SqlDbType.Int, 0);
                    prams[3] = db.MakeInParam("@bitActive", SqlDbType.Bit, 0, isActive);
                    DataSet ds = db.GetDataSet("up_getAllUsersPaged", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {
                    totalRows = 0;
                }
            }
            totalRows = prams[2].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[2].Value);
            return table;
        }
        public static bool RemoveMember(int MemberId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, MemberId);

                    int exec = db.RunProc("up_user_delete", prams);
                    return exec >= 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                //     new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

            }
            return false;
        }

        public static bool ChangeMemberStatus(int MemberId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, MemberId);

                    int exec = db.RunProc("up_user_EnableDisable", prams);
                    return exec >= 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                //     new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

            }
            return false;
        }

        public class Logins
        {

            private int _loginId;

            public int LoginId
            {
                get { return _loginId; }
                set { _loginId = value; }
            }
            private int _memberId;

            public int MemberId
            {
                get { return _memberId; }
                set { _memberId = value; }
            }
            private string _login;

            public string Login
            {
                get { return _login; }
                set { _login = value; }
            }
            private string _password;

            public string Password
            {
                get { return _password; }
                set { _password = value; }
            }
            private string _dateLastLogin;

            public string DateLastLogin
            {
                get { return _dateLastLogin; }
                set { _dateLastLogin = value; }
            }
            private string _failedAttempts;

            public string FailedAttempts
            {
                get { return _failedAttempts; }
                set { _failedAttempts = value; }
            }
            private DateTime _dateCreated;

            public DateTime DateCreated
            {
                get { return _dateCreated; }
                set { _dateCreated = value; }
            }
            private bool _iSActive = true;

            public bool ISActive
            {
                get { return _iSActive; }
                set { _iSActive = value; }
            }











            public static int InsertLogin(Logins login)
            {
                SqlParameter[] prams = null;
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        prams = new SqlParameter[4];
                        prams[0] = db.MakeInParam("@vchLogin", SqlDbType.VarChar, 100, login.Login);
                        prams[1] = db.MakeInParam("@vchPassword", SqlDbType.VarChar, 100, login.Password);
                        prams[2] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, login.MemberId);
                        prams[3] = db.MakeInParam("@bitActive", SqlDbType.Bit, 0, login.ISActive);
                        int exec = db.RunProc("up_login_insert", prams);
                    }
                }
                catch (Exception ex)
                {
                    //  new SqlLog().InsertSqlLog(0, "Logins.InsertLogin", ex);
                }
                return prams[3].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[3].Value);
            }







            public static bool CheckEmailExists(string login)
            {
                SqlParameter[] prams = null;
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        prams = new SqlParameter[2];
                        prams[0] = db.MakeInParam("@vchLoginName", SqlDbType.VarChar, 100, login);
                        prams[1] = db.MakeReturnParam(SqlDbType.Int, 0);
                        int exec = db.RunProc("up_Check_Login_Exists", prams);
                    }
                }
                catch (Exception ex)
                {
                    //  new SqlLog().InsertSqlLog(0, "Logins.InsertLogin", ex);
                }
                return prams[1].Value.ToString().Equals("1") ? true : false;
            }

            public static Logins GetUserByUserName(string login)
            {
                IDataReader reader = null;
                Logins userInfo = new Logins();
                using (DbManager db = DbManager.GetDbManager())
                {
                    try
                    {
                        SqlParameter[] prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, login);
                        reader = db.GetDataReader("up_getUserByUserName", prams);
                        if (reader.Read())
                        {
                            userInfo.Login = reader["vchLogin"].ToString();
                            userInfo.MemberId = Convert.ToInt32(reader["intMemberId"].ToString());
                            userInfo.DateLastLogin = (reader["dtmLastLogin"].ToString());
                        }
                    }
                    catch (Exception)
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

    




            public static DataTable getUserLoginByMemberId(int memberId)
            {
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        SqlParameter[] prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, memberId);

                        return db.GetDataSet("up_getLoginByMemberId", prams).Tables[0];

                    }
                }
                catch (Exception ex)
                {
                    //     new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

                }
                return null;

            }


        }


        public class Roles
        {
            #region member

            private int _memberId;
            private string _roleName;
            private int _roleId;
            private bool _isActive;
            private int _createdById;
            private string _createdBy;
            private DateTime _dateCreated;
            private int _membertypeId;



            #endregion

            public Roles()
            {

            }


            public static int InsertMemberRoles(Roles role)
            {
                SqlParameter[] prams = null;
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        prams = new SqlParameter[3];
                        prams[0] = db.MakeInParam("@intMemberId", SqlDbType.Int, 0, role.MemberId);
                        prams[1] = db.MakeInParam("@vchRoles", SqlDbType.VarChar, 800, role.RoleName);

                        prams[2] = db.MakeReturnParam(SqlDbType.Int, 0);
                        int exec = db.RunProc("up_memberrole_insert", prams);
                    }
                }
                catch (Exception ex)
                {
                    //   new SqlLog().InsertSqlLog(0, "role.InsertMemberRoles", ex);
                }
                return prams[2].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[2].Value);
            }


            private List<Roles> LoadRole(IDataReader reader)
            {
                List<Roles> objListRole = new List<Roles>();
                while (reader.Read())
                {
                    Roles roles = new Roles();
                    roles.RoleId = reader["intRoleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intRoleId"]);
                    //  roles.MemberId = reader["intMemberId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intMemberId"]);
                    roles.RoleName = reader["vchRoleName"] == DBNull.Value ? "" : reader["vchRoleName"].ToString();
                    //   roles.IsActive = reader["bitActive"] == DBNull.Value ? false : Convert.ToBoolean(reader["bitActive"]);
                    objListRole.Add(roles);
                }

                return objListRole;
            }

            public List<Roles> GetRoles(int memberId)
            {
                IDataReader reader = null;

                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        var prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@intmemberid", SqlDbType.Int, 0, memberId);
                        reader = db.GetDataReader("up_memberrole_getbymemberid", prams);


                        List<Roles> objListRole = new List<Roles>();
                        objListRole = LoadRole(reader);
                        if (objListRole.Count > 0)
                        {
                            return objListRole;
                        }

                    }
                }
                catch (Exception e)
                {
                    //  new SqlLog().InsertSqlLog(0, "memberinfo.GetRoles", e);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }

                return null;
            }





            public static DataSet GetAllRoles(int memberId)
            {
                DataSet ds = null;

                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        var prams = new SqlParameter[1];
                        prams[0] = db.MakeInParam("@pintmemberid", SqlDbType.Int, 0, memberId);
                        ds = db.GetDataSet("up_memberrole_getbymemberid", prams);
                    }
                }
                catch (Exception e)
                {
                    //   new SqlLog().InsertSqlLog(0, "Roles.GetRoles", e);
                }
                finally
                {
                    if (ds != null)
                        ds.Dispose();
                }

                return ds;
            }

            public static DataSet GetRoleByToken(int memberId, string vchToken)
            {
                DataSet ds = null;

                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        var prams = new SqlParameter[2];
                        prams[0] = db.MakeInParam("@intmemberid", SqlDbType.Int, 0, memberId);
                        prams[1] = db.MakeInParam("@vchToken", SqlDbType.VarChar, 1000, vchToken);
                        ds = db.GetDataSet("up_memberrole_getbymemberidToken", prams);
                    }
                }
                catch (Exception e)
                {
                    // new SqlLog().InsertSqlLog(0, "Roles.GetRoles", e);
                }
                finally
                {
                    if (ds != null)
                        ds.Dispose();
                }

                return ds;
            }

            public static bool DeleteRole(int RoleId, int memberId)
            {
                try
                {
                    using (DbManager db = DbManager.GetDbManager())
                    {
                        SqlParameter[] prams = new SqlParameter[2];
                        prams[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);
                        prams[1] = db.MakeInParam("@intmemberId", SqlDbType.Int, 0, memberId);

                        int exec = db.RunProc("[up_memberRole_delete]", prams);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //   new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

                }
                return false;
            }

            #region properties


            public string RoleName
            {
                get { return _roleName; }
                set { _roleName = value; }
            }
            public int MemberId
            {
                get { return _memberId; }
                set { _memberId = value; }
            }
            public int MemberTypeId
            {
                get { return _membertypeId; }
                set { _membertypeId = value; }
            }

            public int RoleId
            {
                get { return _roleId; }
                set { _roleId = value; }
            }

            public DateTime DateCreated
            {
                get { return _dateCreated; }
                set { _dateCreated = value; }
            }

            public int CreatedById
            {
                get { return _createdById; }
                set { _createdById = value; }
            }

            public string CreatedBy
            {
                get { return _createdBy; }
                set { _createdBy = value; }
            }

            public bool IsActive
            {
                get { return _isActive; }
                set { _isActive = value; }
            }
            #endregion

        }

    }








}
