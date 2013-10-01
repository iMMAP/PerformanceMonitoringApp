using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace OCHA.Security.Library
{
    public class RoleManagement
    {
        public RoleManagement() { }

        public RoleManagement(int roleId)
        {
            LoadRole(roleId);
        }


        public void LoadRole(int roleId)
        {
            SqlDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[1];
                    parameter[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, roleId);
                    db.RunProc("up_Role_getByRoleId", parameter, out reader);

                    if (reader.Read())
                    {
                        roleName = clsCommon.ParseString(reader["vchRoleName"]);
                        securityToken = clsCommon.ParseString(reader["vchSecurityToken"]);
                        int.TryParse(reader["intCreatedBy"].ToString(), out createdBy);
                    }
                    else
                    {
                        roleId = 0;
                    }

                    if (reader != null)
                        reader.Close();
                }
            }
            catch (Exception)
            {
                // log the error
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


        public static int InsertRole(string roleName, string securityToken, int createdBy)
        {
            SqlParameter[] prams = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[4];
                    prams[0] = db.MakeInParam("@vchRoleName", SqlDbType.VarChar, 100, roleName);
                    prams[1] = db.MakeInParam("@vchSecurityToken", SqlDbType.VarChar, 2000, securityToken);
                    prams[2] = db.MakeInParam("@intCreatedBy", SqlDbType.Int, 0, createdBy);
                    prams[3] = db.MakeReturnParam(SqlDbType.Int, 0);
                    int exec = db.RunProc("up_role_CreateRole", prams);
                }
            }
            catch (Exception)
            {

            }
            return prams[3].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[3].Value);
        }

        public static string getRoleNameById(int roleId)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pintroleid", SqlDbType.Int, 0, roleId);
                    ds = db.GetDataSet("up_role_getbyroleId", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0]["vchRoleName"].ToString();
                    }

                }
            }
            catch (Exception e)
            {
            //    new SqlLog().InsertSqlLog(0, "rolemanagement.getRoleNameById", e);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }

            return null;
        }

        public static int getRoleIdByName(string rolename)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pvchroleName", SqlDbType.VarChar, 0, rolename);
                    ds = db.GetDataSet("up_role_getbyroleName", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return Int32.Parse(ds.Tables[0].Rows[0]["intRoleId"].ToString());
                    }

                }
            }
            catch (Exception e)
            {
              //  new SqlLog().InsertSqlLog(0, "rolemanagement.getRoleIdByName", e);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return 0;
        }

        public static int UpdateSecurityToken(int roleId, string securityToken)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameter = new SqlParameter[2];

                    parameter[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, roleId);
                    parameter[1] = db.MakeInParam("@vchSecurityToken", SqlDbType.VarChar, 1200, securityToken);

                    int exec = db.RunProc("up_role_updatetoken", parameter);
                    return 1;
                }
            }
            catch (Exception ex)
            {
            //    new SqlLog().InsertSqlLog(0, "rolemanagement.UpdateSecurityToken", ex);
            }
            return 0;
        }

        public static string GetSecurityToken(int roleId)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intRoleId", SqlDbType.VarChar, 0, roleId);
                    ds = db.GetDataSet("up_role_gettokenbyId", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0]["vchSecurityToken"].ToString();
                    }

                }
            }
            catch (Exception e)
            {
             //  new SqlLog().InsertSqlLog(0, "rolemanagement.GetSecurityToken", e);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }

            return string.Empty;
        }

        public static DataSet getRolesByMemberId(int memberId)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intmemberid", SqlDbType.Int, 0, memberId);
                    ds = db.GetDataSet("up_member_roles_getbymemberid", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception e)
            {
             //   new SqlLog().InsertSqlLog(0, "rolemanagement.getRolesByMemberId", e);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }

            return null;
        }

        public static DataTable GetAllRoles(int pageId, int pageSize, out int totalRows)
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
                    DataSet ds = db.GetDataSet("up_getAllRolesPaged", prams);
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

        public static bool RemoveMemberRole(int RoleId, int memberId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);
                    prams[1] = db.MakeInParam("@intmemberId", SqlDbType.Int, 0, memberId);

                    int exec = db.RunProc("up_memberRole_delete", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
              //  new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

            }
            return false;
        }

        public static bool RemoveRole(int RoleId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intRoleId", SqlDbType.Int, 0, RoleId);

                    int exec = db.RunProc("up_Role_delete", prams);
                    return exec >= 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
             //   new SqlLog().InsertSqlLog(0, "Role.DeleteRole", ex);

            }
            return false;
        }

        #region Properties



        private int roleId;
        private string roleName;
        private string securityToken;
        private int createdBy;
        private int modifiedBy;


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

        public string SecurityToken
        {
            set { securityToken = value; }
            get
            {
                if (string.IsNullOrEmpty(securityToken))
                    return AccessToken.ReturnEmptyToken();
                else
                    return securityToken;
            }
        }

        public int CreatedBy
        {
            set { createdBy = value; }
            get { return createdBy; }
        }

        public int ModifiedBy
        {
            set { modifiedBy = value; }
            get { return modifiedBy; }
        }

        #endregion

    }
}
