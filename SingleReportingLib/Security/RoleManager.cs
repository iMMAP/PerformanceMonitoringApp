using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Collections;
using System.Configuration;

namespace SingleReporting.Security
{

    public class Role
    {
        private int roleId;
        private string roleName;
        //private int moduleId;
        //private Modules modulesRole;
        private bool active;

        #region Constructors
        public Role() { }
        public Role(int roleId)
        {
            this.roleId = roleId;
            LoadRoles();
        }
        public Role(string RoleName)
        {
            this.roleName = RoleName;

        }
        #endregion

        #region Insert()

        public void Insert()
        {
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = db.MakeInParam("@vchRole", SqlDbType.VarChar, 30, roleName);


                db.RunProc("up_insert_role", parameter);
            }
        }
        #endregion

        #region AddUserToRole(int UserId, int RoleId)
        public static bool AddUserToRole(int UserId, int RoleId)
        {

            bool sucess = false;

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@intUserID", SqlDbType.Int, 0, UserId);
                    parameter[1] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);

                    db.RunProc("up_AddUserToRole", parameter);
                    sucess = true;
                }
                catch (Exception ex)
                {
                    sucess = false;

                }
            }


            return sucess;

        }
        #endregion

        #region GetUsersNotInRole(int RoleId)
        public static DataTable GetUsersNotInRole(int RoleId)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[1];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);

                    DataSet ds = db.GetDataSet("up_UsersNotInRole", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {

                    //new SqlLog().InsertSqlLog(0, "ROles.Info.GetUsers", ex);
                }
            }

            return table;
        }
        #endregion

        #region DeleteUserFromRole(int UserId, int RoleId)

        public static bool DeleteUserFromRole(int UserId, int RoleId)
        {
            bool sucess = false;

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@intUserID", SqlDbType.Int, 0, UserId);
                    parameter[1] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);

                    db.RunProc("up_delete_user_from_role", parameter);
                    sucess = true;
                }
                catch (Exception ex)
                {
                    sucess = false;

                }
            }


            return sucess;
        }
        #endregion

        #region GetRoles(int pageId, int pageSize, out int totalRows, string sortExpression, string sortDirection)
        public static DataTable GetRoles(int pageId, int pageSize, out int totalRows, string sortExpression, string sortDirection)
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
                    DataSet ds = db.GetDataSet("up_getAllRolesPaged", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    totalRows = 0;
                    //new SqlLog().InsertSqlLog(0, "ROles.Info.GetUsers", ex);
                }
            }
            totalRows = prams[4].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[4].Value);
            return table;
        }
        #endregion

        public static DataTable GetUsersInRoles(int RoleId, int pageId, int pageSize, out int totalRows, string sortExpression, string sortDirection)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[6];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@intPageId", SqlDbType.Int, 0, pageId);
                    prams[1] = db.MakeInParam("@intPageSize", SqlDbType.Int, 0, pageSize);
                    prams[2] = db.MakeInParam("@vchSortExpression", SqlDbType.VarChar, 50, sortExpression);
                    prams[3] = db.MakeInParam("@vchSortDirection", SqlDbType.VarChar, 20, sortDirection);
                    prams[4] = db.MakeReturnParam(SqlDbType.Int, 0);
                    prams[5] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);
                    DataSet ds = db.GetDataSet("getAllUsersinRolePaged", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    totalRows = 0;
                    //new SqlLog().InsertSqlLog(0, "ROles.Info.GetUsers", ex);
                }
            }
            totalRows = prams[4].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[4].Value);
            return table;
        }
        public static DataTable GetUsersInRoles(string RoleName)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[1];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {

                    prams[0] = db.MakeInParam("@vchRoleName", SqlDbType.VarChar, 0, RoleName);
                    DataSet ds = db.GetDataSet("up_getAllUsersinRole", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception ex)
                {

                    //new SqlLog().InsertSqlLog(0, "ROles.Info.GetUsers", ex);
                }
            }

            return table;
        }

        public static DataTable GetAllUsers()
        {
            DataTable table = null;
            SqlParameter[] prams = null;

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {

                    DataSet ds = db.GetDataSet("up_getAllUsers", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {

                    //new SqlLog().InsertSqlLog(0, "ROles.Info.GetUsers", ex);
                }
            }

            return table;
        }
        public static bool PageAcess(string filepath, string username)
        {
            bool PageAcess = false;
            if (isUserAdmin(username))

                PageAcess = true;
            else
            {
                DataSet ds = null;
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@vchUsername", SqlDbType.VarChar, 155, username);
                    parameter[1] = db.MakeInParam("@vchModuleName", SqlDbType.VarChar, 75, filepath);

                    ds = db.GetDataSet("up_pageAcessByUsername", parameter);
                    //db.RunProc("up_pageAcessByUsername", parameter);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PageAcess = true;
                    }
                }

            }
            return PageAcess;

        }
        public static bool IsPageInTable(DataTable dt, string page)
        {
            bool found = false;
            foreach (DataRow item in dt.Rows)
            {
                if (item["vchPagePath"].ToString().Equals(page))
                {
                    found = true;
                    break;
                }


            }
            return found;
        }
        public static DataTable PageAcessRoleDataset(string RoleName)
        {


            DataSet ds = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                SqlParameter[] parameter = new SqlParameter[1];

                parameter[0] = db.MakeInParam("@vchRoleName", SqlDbType.VarChar, 25, RoleName);
                //parameter[1] = db.MakeInParam("@vchModuleName", SqlDbType.VarChar, 75, filepath);

                ds = db.GetDataSet("up_PageAcessList", parameter);
                //db.RunProc("up_pageAcessByUsername", parameter);



            }
            return ds.Tables[0];

        }
        public static bool UpdatePageList(string PageList, string RoleName)
        {

            bool sucess = false;
            //        @vchPageList varchar(8000),
            //@vchRoleName varchar(25)
            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@vchPageList", SqlDbType.VarChar, 80000, PageList);
                    parameter[1] = db.MakeInParam("@vchRoleName", SqlDbType.VarChar, 25, RoleName);

                    db.RunProc("UpdatePageList", parameter);
                    sucess = true;
                }
                catch (Exception ex)
                {
                    sucess = false;

                }
            }


            return sucess;

        }




        public static bool isUserAdmin(string username)
        {
            bool isAdmin = false;


            if (username.ToLower().Equals(ConfigurationManager.AppSettings["superAdminEmail"].ToString()))
            {
                isAdmin = true;
            }
            return isAdmin;


        }
        public static bool isCurrentUserAdmin()
        {
            List<Role> r = Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);
            bool isAdmin = false;
            foreach (Role _r in r)
            {
                if (_r.RoleName.ToLower().Contains("admin"))
                {
                    isAdmin = true;
                }
            }
            return isAdmin;
        }

        public static bool isCurrentUserCL()
        {
            List<Role> r = Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);
            bool isAdmin = false;
            foreach (Role _r in r)
            {
                if (_r.RoleName.ToLower().Contains("cluster"))
                {
                    isAdmin = true;
                }
            }
            return isAdmin;
        }

        public static bool isCurrentUserIP()
        {
            List<Role> r = Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);
            bool isAdmin = false;
            foreach (Role _r in r)
            {
                if (_r.RoleName.ToLower().Contains("implementation partner"))
                {
                    isAdmin = true;
                }
            }
            return isAdmin;
        }


        public static bool isCurrentUserPM()
        {
            List<Role> r = Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);
            bool isAdmin = false;
            foreach (Role _r in r)
            {
                if (_r.RoleName.ToLower().Contains("project manager"))
                {
                    isAdmin = true;
                }
            }
            return isAdmin;
        }


        public static bool isCurrentUserManager()
        { 
            List<Role> r = Role.GetRoleByUser(HttpContext.Current.User.Identity.Name);
            bool isManager = false;
            foreach (Role _r in r)
            {
                if (_r.roleName.ToLower().Contains("manager"))
                {
                    isManager = true;
                }
               
            }
            return isManager;
        }


        public static bool isUserAdmin(int UserId)
        {
            bool isAdmin = false;
            if (UserId == 0)
            {
                isAdmin = true;
            }

            return isAdmin;
        }
        public static bool isPageAnynonumus(string filepath)
        {
            //to do to add the logic of pageanynomus logic
            if (filepath.ToLower().Equals("/default.aspx") || filepath.ToLower().Equals("/login.aspx"))
            {

                return true;
            }

            else
                return false;



        }

        #region LoadRoles
        public void LoadRoles()
        {
            SqlDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[1];
                    parameter[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, roleId);
                    db.RunProc("up_get_role", parameter, out reader);

                    if (reader.Read())
                    {
                        roleName = reader["vchRoleName"].ToString();


                    }
                    else
                    {
                        roleId = 0;
                    }

                    if (reader != null)
                        reader.Close();
                }
            }
            catch (Exception exception)
            {
                // log the error
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        #endregion


        public void Delete()
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, roleId);
                    db.RunProc("delete_role", param);
                }
            }
            catch
            {
                // log the error
            }
        }

        public void Update()
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@vchRole", SqlDbType.VarChar, 30, roleName);


                    parameter[1] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, roleId);

                    db.RunProc("up_update_role", parameter);
                }
            }
            catch (Exception e)
            {
                // log the error
            }
        }

        #region Properties

        public int RoleId
        {
            set { roleId = value; }
            get { return roleId; }
        }

        public string RoleName
        {
            set { roleName = value; }
            get { return roleName; }
        }





        #endregion


        public static List<Role> GetRoleByUser(string UserName)
        {
            DataSet ds = null;
            List<Role> RolesofUser = new List<Role>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[1];

                    parameter[0] = db.MakeInParam("@vchUserName", SqlDbType.VarChar, 155, UserName);
                    //parameter[1] = db.MakeInParam("@vchModuleName", SqlDbType.VarChar, 75, filepath);

                    ds = db.GetDataSet("up_GetUserRole", parameter);




                }
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    Role temp = new Role();
                    temp.RoleName = item["vchRoleName"].ToString();
                    int tempRoleid = 0;
                    int.TryParse(item["intRoleId"].ToString(), out tempRoleid);
                    temp.RoleId = tempRoleid;
                    RolesofUser.Add(temp);
                }
                return RolesofUser;
            }
            catch (Exception ex)
            {
                return RolesofUser;
            }
        }




        public static DataSet getDataCategories()
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[1];

                    parameter[0] = db.MakeReturnParam(SqlDbType.Int, 0);

                    ds = db.GetDataSet("up_load_applicationViews", parameter);




                }
            }
            catch (Exception ex)
            {
                ds = new DataSet();

            }
            return ds;

        }
        public static DataSet getDataSchema(int ApplicationViewId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[1];

                    parameter[0] = db.MakeInParam("@pintApplicationViewId", SqlDbType.Int, 0, ApplicationViewId);
                    //parameter[1] = db.MakeInParam("@vchModuleName", SqlDbType.VarChar, 75, filepath);

                    ds = db.GetDataSet("up_load_applicationviews_schema", parameter);




                }
            }
            catch (Exception ex)
            {

            }
            return ds;

        }
        public static void saveDataSchema(string RoleName, string list)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@pvchRoleName", SqlDbType.VarChar, 150, RoleName);
                    parameter[1] = db.MakeInParam("@pvchColunmList", SqlDbType.VarChar, 1000, list);

                    db.RunProc("up_applicationviews_schema_InsertUpdate", parameter);




                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}


