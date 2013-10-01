using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Organizations
    /// </summary>
    public class Organizations
    {

        #region Members and Properties

        private int _Organization_ID;
        private int _FK_Organization_Type;
        private string _Organization_Name;
        private string _Organization_Acronym;

        public int Organization_ID { get { return _Organization_ID; } set { _Organization_ID = value; } }
        public int FK_Organization_Type { get { return _FK_Organization_Type; } set { _FK_Organization_Type = value; } }
        public string Organization_Name { get { return _Organization_Name; } set { _Organization_Name = value; } }
        public string Organization_Acronym { get { return _Organization_Acronym; } set { _Organization_Acronym = value; } }

        #endregion


        public Organizations()
        {

        }

        public Organizations(int OrganizationID)
        {
            loadOrganizations(OrganizationID);
        }

        /// <summary>
        /// Load Organizations by Organization Id
        /// </summary>
        /// <param name="OrganizationID">The id whose records to be fetched</param>
        private void loadOrganizations(int OrganizationID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, OrganizationID);
                    reader = db.GetDataReader("up_getProjectsById", prams);
                    if (reader.Read())
                        LoadProjectsInfo(reader);
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
       
        private void LoadProjectsInfo(IDataReader reader)
        {

            _Organization_ID = reader["Organization_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Organization_ID"]);
            _FK_Organization_Type = reader["FK_Organization_Type"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Organization_Type"]);
            _Organization_Name = reader["Organization_Name"] == System.DBNull.Value ? "" : reader["Organization_Name"].ToString();
            _Organization_Acronym = reader["Organization_Acronym"] == System.DBNull.Value ? "" : reader["Organization_Acronym"].ToString();
            

        }
        /// <summary>
        /// Delete Oranizations by Oraganization id
        /// </summary>
        /// <param name="OrganizationId">the id of Organization whose record to be fetched</param>
        /// <returns></returns>
        public static int deleteOrganization(int OrganizationId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pOrganizationId", SqlDbType.Int, 4, OrganizationId));
                    int exec = db.RunProc("up_deleteOrganization", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Insert / Update Organization by Organization id, Organiztion Type , Organization Name, Organization Acronym
        /// </summary>
        /// <param name="Organization_ID">The id of Organization by which it should be Added / Updated</param>
        /// <param name="FK_Organization_Type">The Type of Organization by which it should be Added / Updated</param>
        /// <param name="Organization_Name">The id Name Organization by which it should be Added / Updated</param>
        /// <param name="Organization_Acronym">The Actonym of Organization by which it should be Added / Updated</param>
        /// <returns></returns>
        public static int InsertUpdateOrganization(int Organization_ID, int FK_Organization_Type, string Organization_Name, string Organization_Acronym)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {

                    prams.Add(db.MakeInParam("@pOrganization_ID", SqlDbType.Int, 0, Organization_ID));
                    prams.Add(db.MakeInParam("@pFK_Organization_Type", SqlDbType.Int, 0, FK_Organization_Type));
                    prams.Add(db.MakeInParam("@pOrganization_Name", SqlDbType.NVarChar, 0, Organization_Name));
                    prams.Add(db.MakeInParam("@pOrganization_Acronym", SqlDbType.NVarChar, 0, Organization_Acronym));
                    int exec = db.RunProc("UP_Organizations_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Insert new Organization by object of class Organizations
        /// </summary>
        /// <param name="o">Object of class Organizations which should be inserted</param>
        /// <returns></returns>
        public static int insertOrganization(Organizations o)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                  
                    prams.Add(db.MakeInParam("@pOrganization_ID", SqlDbType.Int, 0, o.Organization_ID));
                    prams.Add(db.MakeInParam("@pFK_Organization_Type", SqlDbType.Int, 0, o.FK_Organization_Type));
                    prams.Add(db.MakeInParam("@pOrganization_Name", SqlDbType.NVarChar, 0, o.Organization_Name));
                    prams.Add(db.MakeInParam("@pOrganization_Acronym", SqlDbType.NVarChar, 0, o.Organization_Acronym));
                    int exec = db.RunProc("UP_Organizations_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }

        }
        public static int AddUpdateOrganizationTypes(int OrganizationTypeId, string Organization)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pOrganization_Type_ID", SqlDbType.Int, 0, OrganizationTypeId));
                    prams.Add(db.MakeInParam("@pOrganization_Type", SqlDbType.NVarChar, 0, Organization));
                    int exec = db.RunProc("UP_Organization_Types_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
            // return 0;


        }


        /// <summary>
        /// Get DataTable of all Organizations
        /// </summary>
        /// <returns>DataTable of all Organizations</returns>
        public static DataTable getAllOrganizations()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("UP_Organizations_GetAllOrganizations", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get DataTable of all OrganizationsTypes
        /// </summary>
        /// <returns>DataTable of all OrganizationsTypes</returns>
        public static DataTable getAllOrganizationsTypes()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllOrganizationTpyes", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Delete OrganiztionType by OrganizationType id
        /// </summary>
        /// <param name="OrganizationTypeId">The OrganziationType id whose record to be fetched</param>
        /// <returns></returns>
        public static int deleteOrganizationType(int OrganizationTypeId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pOrganizationTypeId", SqlDbType.Int, 4, OrganizationTypeId));
                    int exec = db.RunProc("up_deleteOrganizationType", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get DataTable of All Sectors
        /// </summary>
        /// <returns>DataTable of All Sectors</returns>
        public static DataTable getAllSectors()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllSectorsNames", prams).Tables[0];
            }

            return dt;
        }


        


    }
}
