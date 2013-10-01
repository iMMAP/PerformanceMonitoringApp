using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details of ProjectBeneficiary
    /// </summary>
    public class ProjectBeneficiary
    {

        #region Members and Properties

        private int _SP_Target_ID;
        private int _FK_SP;
        private int _FK_Beneficiary_Type;
        private int _Beneficiary_HH_Target;
        private int _Beneficiary_HH_Value;
        private int _Beneficiary_Male_Target;
        private int _Beneficiary_Female_Target;
        private int _Beneficiary_Children_Target;
        private int _Beneficiary_Male_Value;
        private int _Beneficiary_Female_Value;
        private int _Beneficiary_Children_Value;
        private int _FK_OrganizationID;
        private int _FK_LocationID;
        private int _FK_ProjectID;


        public int SP_Target_ID { get { return _SP_Target_ID; } set { _SP_Target_ID = value; } }
        public int FK_SP { get { return _FK_SP; } set { _FK_SP = value; } }
        public int FK_Beneficiary_Type { get { return _FK_Beneficiary_Type; } set { _FK_Beneficiary_Type = value; } }
        public int Beneficiary_HH_Target { get { return _Beneficiary_HH_Target; } set { _Beneficiary_HH_Target = value; } }
        public int Beneficiary_HH_Value { get { return _Beneficiary_HH_Value; } set { _Beneficiary_HH_Value = value; } }
        public int Beneficiary_Male_Target { get { return _Beneficiary_Male_Target; } set { _Beneficiary_Male_Target = value; } }
        public int Beneficiary_Female_Target { get { return _Beneficiary_Female_Target; } set { _Beneficiary_Female_Target = value; } }
        public int Beneficiary_Children_Target { get { return _Beneficiary_Children_Target; } set { _Beneficiary_Children_Target = value; } }
        public int Beneficiary_Male_Value { get { return _Beneficiary_Male_Value; } set { _Beneficiary_Male_Value = value; } }
        public int Beneficiary_Female_Value { get { return _Beneficiary_Female_Value; } set { _Beneficiary_Female_Value = value; } }
        public int Beneficiary_Children_Value { get { return _Beneficiary_Children_Value; } set { _Beneficiary_Children_Value = value; } }
        public int FK_OrganizationID { get { return _FK_OrganizationID; } set { _FK_OrganizationID = value; } }
        public int FK_LocationID { get { return _FK_LocationID; } set { _FK_LocationID = value; } }
        public int FK_ProjectID { get { return _FK_ProjectID; } set { _FK_ProjectID = value; } }

        #endregion


        public ProjectBeneficiary()
        {

        }

        public ProjectBeneficiary(int projectBeneficiaryId)
        {
            loadProjectBeneficiary(projectBeneficiaryId);
        }

        /// <summary>
        /// This Loads ProjectBeneficiary by ProjectBeneficiary ID
        /// </summary>
        /// <param name="projectBeneficiaryId">The projectBeneficiary id whose record to be fetched</param>
        private void loadProjectBeneficiary(int projectBeneficiaryId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@BeneficiaryID", SqlDbType.Int, 4, projectBeneficiaryId);
                    reader = db.GetDataReader("UP_Beneficiary_GetByID", prams);
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

            _SP_Target_ID = reader["SP_Target_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["SP_Target_ID"]);
            _FK_SP = reader["FK_SP"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_SP"]);
            _FK_Beneficiary_Type = reader["FK_Beneficiary_Type"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Beneficiary_Type"]);
            _Beneficiary_HH_Target = reader["Beneficiary_HH_Target"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_HH_Target"]);
            _Beneficiary_HH_Value = reader["Beneficiary_HH_Value"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_HH_Value"]);
            _Beneficiary_Male_Target = reader["Beneficiary_Male_Target"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Male_Target"]);
            _Beneficiary_Female_Target = reader["Beneficiary_Female_Target"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Female_Target"]);
            _Beneficiary_Children_Target = reader["Beneficiary_Children_Target"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Children_Target"]);
            _Beneficiary_Male_Value = reader["Beneficiary_Male_Value"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Male_Value"]);
            _Beneficiary_Female_Value = reader["Beneficiary_Female_Value"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Female_Value"]);
            _Beneficiary_Children_Value = reader["Beneficiary_Children_Value"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Beneficiary_Children_Value"]);
            _FK_OrganizationID = reader["FK_OrganizationID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_OrganizationID"]);
            _FK_LocationID = reader["FK_LocationID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_LocationID"]);
            _FK_ProjectID = reader["FK_ProjectID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_ProjectID"]);


        }

        /// <summary>
        /// Insert Beneficiary by object of class ProjectBeneficiary
        /// </summary>
        /// <param name="p">Object of Class ProjectBeneficiary which has to be inserted</param>
        /// <returns></returns>
        public static int insertBeneficiary(ProjectBeneficiary p)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@FK_SP", SqlDbType.Int, 4, p.FK_SP));
                    prams.Add(db.MakeInParam("@FK_OrganizationID", SqlDbType.Int, 4, p.FK_OrganizationID));
                    prams.Add(db.MakeInParam("@FK_LocationID", SqlDbType.Int, 4, p.FK_LocationID));
                    prams.Add(db.MakeInParam("@FK_ProjectID", SqlDbType.Int, 4, p.FK_ProjectID));
                    prams.Add(db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 4, p.SP_Target_ID));


                    int exec = db.RunProc("UP_Projects_InsertBeneficiary", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update Beneficiary by object of class ProjectBeneficiary
        /// </summary>
        /// <param name="p">Object of Class ProjectBeneficiary which has to be Updated</param>
        /// <returns></returns>
        public static int updateBeneficiary(ProjectBeneficiary p)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@FK_SP", SqlDbType.Int, 4, p.FK_SP));
                    prams.Add(db.MakeInParam("@FK_OrganizationID", SqlDbType.Int, 4, p.FK_OrganizationID));
                    prams.Add(db.MakeInParam("@FK_LocationID", SqlDbType.Int, 4, p.FK_LocationID));
                    prams.Add(db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 4, p.SP_Target_ID));


                    int exec = db.RunProc("UP_Projects_UpdateBeneficiary", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
    

        /// <summary>
        /// Delete Beneficiary by Beneficiary ID
        /// </summary>
        /// <param name="beneficiaryID">The Beneficiary id whose data should be fetched</param>
        /// <returns></returns>
        public static int deleteBeneficiary(int beneficiaryID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 4, beneficiaryID));
                    int exec = db.RunProc("up_Projects_DeleteBeneficiary", prams.ToArray());






                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get DataTable of  Beneficiary by Beneficiary ID
        /// </summary>
        /// <param name="beneficiaryID">The Beneficiary id whose data should be fetched</param>
        /// <returns></returns>
        public static DataTable getBeneficiaryByID(int beneficiaryID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@BeneficiaryID", SqlDbType.Int, 4, beneficiaryID);

                DataSet ds = db.GetDataSet("UP_Beneficiary_GetByID", prams);
                if (ds != null && ds.Tables.Count != 0)
                {
                    dt = ds.Tables[0];
                }
            }

            return dt;
        }
        /// <summary>
        /// Get Project Reporting Status  project ID
        /// </summary>
        /// <param name="beneficiaryID">The Project id whose data should be fetched</param>
        /// <returns></returns>
        public static int getProjectReportingStatus(int projectID)
        {

            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                return db.RunProc("UP_Projects_GetIPs_ByProjectID", prams);
            }

        }

        /// <summary>
        /// Get Projects Implementation Partners by Project ID
        /// </summary>
        /// <param name="projectID">The project id whose data to be fetched</param>
        /// <returns></returns>
        public static DataTable getProjectIPs_ByProjectID(int projectID)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                ds = db.GetDataSet("UP_Projects_GetIPs_ByProjectID", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }













        /// <summary>
        /// Get Projects Implementation Partners by Project ID, Member id
        /// </summary>
        /// <param name="projectID">The project id whose data to be fetched</param>
        /// <param name="memberId">The memeber id whose data to be fetched</param>
        /// <returns></returns>
        public static DataTable getProjectIPs_ByProjectID(int projectID, int memberId)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@MemberID", SqlDbType.Int, 4, memberId);
                ds = db.GetDataSet("UP_Projects_GetIPs_ByProjectID_forIP", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// Get Project Union Council by Project Spid and project id
        /// </summary>
        /// <param name="projectID">project id whose data to be fetched</param>
        /// <param name="SPid">Spid whose data to be fetched</param>
        /// <returns></returns>
        public static DataTable getProjectUCs_ByProject_SP(int projectID, int SPid)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@SPid", SqlDbType.Int, 4, SPid);
                ds = db.GetDataSet("UP_Projects_GetUCs_ByProject_BySP", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        public static DataTable getProjectUCs_ByProject_SP_grd(int projectID, int SPid)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@SPid", SqlDbType.Int, 4, SPid);
                ds = db.GetDataSet("UP_Projects_GetUCs_ByProject_BySP_grd", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// Insert ServiceProvider by object of class PfojectBeneficiary
        /// </summary>
        /// <param name="p">Object of clase ProjectBeneficiary which has to be inserted</param>
        /// <returns></returns>
        public static int insertServiceProvider(ProjectBeneficiary p)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                  
                    prams.Add(db.MakeInParam("@Service_Provider_ID", SqlDbType.Int, 4, p.FK_SP));
                    prams.Add(db.MakeInParam("@FK_OrganizationID", SqlDbType.Int, 4, p.FK_OrganizationID));
                    prams.Add(db.MakeInParam("@FK_LocationID", SqlDbType.Int, 4, p.FK_LocationID));
                    prams.Add(db.MakeInParam("@FK_ProjectID", SqlDbType.Int, 4, p.FK_ProjectID));
                   

                    int exec = db.RunProc("UP_Projects_InsertServiceProvider", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get Service Provider by Organization Id
        /// </summary>
        /// <param name="organizationID">Organization id whose data to be fetched</param>
        /// <returns></returns>
        public static int getServiceProvider_ByOrganizationID(int organizationID)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@OrganizationID", SqlDbType.Int, 4, organizationID);
                ds = db.GetDataSet("UP_GetServiceProvider_ByOrganizationID", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable t = ds.Tables[0];
                DataRow r = t.Rows[0];
                return clsCommon.ParseInt(r["Service_Provider_ID"].ToString());

            }
            return -1;
        }

        public static int getOrganization_BySpidAndProjectID(int projectID, int spid)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@ServiceProviderID", SqlDbType.Int, 4, spid);
                ds = db.GetDataSet("UP_ServiceProvider_GetBySpid_ByProjectID", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable t = ds.Tables[0];
                DataRow r = t.Rows[0];
                return clsCommon.ParseInt(r["FK_Organization"].ToString());

            }
            return -1;
        }

        /// <summary>
        /// Get SPid by Union council id
        /// </summary>
        /// <param name="ucID">Union council id whose data to be fetched</param>
        /// <returns>Spid</returns>
        public static int getSPid_ByUCid(int ucID)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@UCid", SqlDbType.Int, 4, ucID);
                ds = db.GetDataSet("UP_Projects_GetSpid_ByUCid", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable t = ds.Tables[0];
                DataRow r = t.Rows[0];
                return clsCommon.ParseInt(r["FK_SP"].ToString());

            }
            return -1;
        }
        /// <summary>
        /// Delete Service Provider by Service Provider id
        /// </summary>
        /// <param name="Spid">Service Provider id whose data should be deleted</param>
        /// <returns></returns>
        public static int deleteServiceProvider(int Spid)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@SPid", SqlDbType.Int, 4, Spid));
                    int exec = db.RunProc("UP_ServiceProvider_DeleteByID", prams.ToArray());


                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get DataTable of UnionCouncil by Organization id and Project id
        /// </summary>
        /// <param name="organizationID">The id of organization whose data shoulde be fetched</param>
        /// <param name="projectID">The id of Project whose data shoulde be fetched</param>
        /// <returns>DataTable Of UnionCouncil</returns>
        public static DataTable getUCsbyOrganizationID_ByProjectID(int organizationID, int projectID)
        {
            DataSet ds = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@OrganizationID", SqlDbType.Int, 4, organizationID);
                ds = db.GetDataSet("UP_ServiceProvider_GetByOid_ByProjectID", prams);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


    }
}
