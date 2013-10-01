using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace OCHA.Security.Library
{
    public class Module
    {

        public Module() { }



        public Module(string mediaId)
        {
            Load(mediaId);
        }

        private void Load(string moduleName)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@vchModuleName", SqlDbType.VarChar, 100, moduleName);
                    reader = db.GetDataReader("up_module_GetByName", prams);
                    if (reader.Read())
                        Load(reader);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }



        private void Load(IDataReader reader)
        {
            _CreatedBy = reader["intCreatedBy"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intCreatedBy"]);
            _ModuleId = reader["intModuleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intModuleId"]);
            _PermissionId = reader["intPermissionId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["intPermissionId"]);
            _ModuleName = reader["vchModuleName"] == DBNull.Value ? "" : reader["vchModuleName"].ToString();
            _dateCreated = _dateCreated = reader["dtmDateCreated"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["dtmDateCreated"]);
            _description = reader["vchDescription"] == DBNull.Value ? "" : reader["vchDescription"].ToString();
        }

        public static int Insert(Module module)
        {
            SqlParameter[] prams = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[8];
                    prams[0] = db.MakeInParam("@pintModuleId", SqlDbType.Int, 0, module.ModuleId);
                    prams[1] = db.MakeInParam("@pintPermissionId", SqlDbType.Int, 0, module.PermissionId);
                    prams[2] = db.MakeInParam("@pvchModuleName", SqlDbType.VarChar, 100, module.ModuleName);
                    prams[3] = db.MakeInParam("@pdtmDateCreated", SqlDbType.DateTime, 0, module.DateCreated);
                    prams[4] = db.MakeInParam("@pintCreatedBy", SqlDbType.Int, 0, module.CreatedBy);
                    prams[5] = db.MakeInParam("@vchModulePath", SqlDbType.VarChar, 100, module.ModulePath);
                    prams[6] = db.MakeInParam("@vchDescription", SqlDbType.VarChar, 100, module.Description);
                    prams[7] = db.MakeReturnParam(SqlDbType.Int, 0);

                    int exec = db.RunProc("UP_mODULES_InsertUpdate", prams);
                }
            }
            catch (Exception)
            {
                // new SqlLog().InsertSqlLog(0, "Media.InsertMedia", ex);

            }
            return prams[7].Value == DBNull.Value ? 0 : Convert.ToInt32(prams[7].Value);
        }

        public static DataTable GetPermissionBits(string moduleName, int RoleId)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[2];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@moduleName", SqlDbType.VarChar, 100, moduleName);
                    prams[1] = db.MakeInParam("@roleId", SqlDbType.Int, 0, RoleId);
                    DataSet ds = db.GetDataSet("up_getPermissionBits", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {

                }
            }

            return table;
        }

       //Overload GetPermissionBits()


        public static DataTable GetPermissionBits(string moduleName, int RoleId,int moduleID)
        {
            DataTable table = null;
            SqlParameter[] prams = new SqlParameter[2];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@moduleName", SqlDbType.VarChar, 100, moduleName);
                    prams[1] = db.MakeInParam("@roleId", SqlDbType.Int, 0, RoleId);
                    DataSet ds = db.GetDataSet("Usp_Users_GetPermissionByModuleNameAndRoleID", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {

                }
            }

            return table;
        }








        public static DataTable GetAllModules()
        {
            DataTable table = null;
            SqlParameter[] prams = null;

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {

                    DataSet ds = db.GetDataSet("up_GetAllModules", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        table = ds.Tables[0];
                    }
                }
                catch (Exception)
                {

                }
            }

            return table;
        }

        public static DataTable GetAllModules(int pageId, int pageSize, out int totalRows)
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
                    DataSet ds = db.GetDataSet("up_getAllModulesPaged", prams);
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

        public static bool RemoveModule(int ModuleId)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@intModuleId", SqlDbType.Int, 0, ModuleId);

                    int exec = db.RunProc("up_Module_delete", prams);
                    return exec >= 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
           //     new SqlLog().InsertSqlLog(0, "Module.RemoveModule", ex);

            }
            return false;
        }


        #region permission


        private string _ModuleName = string.Empty;
        public string ModuleName
        {
            get { return _ModuleName; }
            set { _ModuleName = value; }
        }


        private string _ModulePath = string.Empty;
        public string ModulePath
        {
            get { return _ModulePath; }
            set { _ModulePath = value; }
        }

        private int _PermissionId = -1;
        public int PermissionId
        {
            get { return _PermissionId; }
            set { _PermissionId = value; }
        }

        private int _ModuleId = 0;
        public int ModuleId
        {
            get { return _ModuleId; }
            set { _ModuleId = value; }
        }

        private int _CreatedBy = 0;
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private DateTime _dateCreated = DateTime.Now;
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        private string _description = string.Empty;
        public string Description 
        {
            get { return _description; }
            set { _description = value; } 
        }


        #endregion



    }
}
