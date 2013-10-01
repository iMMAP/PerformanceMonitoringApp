using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{

    /// <summary>
    /// Get details for cluster leads 
    /// </summary>
    public class ClusterLeads
    {

        #region Members and Properties

        private int _ClusterLeadID;
        private int _MemberID;
        private int _SectorID;

        public int ClusterLeadID { get { return _ClusterLeadID; } set { _ClusterLeadID = value; } }
        public int MemberID { get { return _MemberID; } set { _MemberID = value; } }
        public int SectorID { get { return _SectorID; } set { _SectorID = value; } }


        #endregion


        public ClusterLeads()
        {

        }
        /// <summary>
        /// Load Cluster Lead
        /// </summary>
        /// <param name="memberID">The memberID by which Cluster Lead should be loaded</param>
        public ClusterLeads(int memberID)
        {
            loadClusterLead(memberID);
        }

        private void loadClusterLead(int memberID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@MemberID", SqlDbType.Int, 4, memberID);
                    reader = db.GetDataReader("UP_ClusterLead_ByMemberID", prams);
                    if (reader.Read())
                        LoadClusterLeadInfo(reader);
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

        private void LoadClusterLeadInfo(IDataReader reader)
        {

            _ClusterLeadID = reader["ClusterLeadID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["ClusterLeadID"]);
            _MemberID = reader["MemberID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["MemberID"]);
            _SectorID = reader["SectorID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["SectorID"]);

        }
        /// <summary>
        /// Insert new Cluster Lead
        /// </summary>
        /// <param name="c">ClusterLeads class object to be inserted</param>
        /// <returns></returns>
        public static int insertClusterLead(ClusterLeads c)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@ClusterLeadID", SqlDbType.Int, 4, c.ClusterLeadID));
                    prams.Add(db.MakeInParam("@MemberID", SqlDbType.Int, 4, c.MemberID));
                    prams.Add(db.MakeInParam("@SectorID", SqlDbType.Int, 4, c.SectorID));


                    int exec = db.RunProc("UP_ClusterLeads_Insert", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Update Cluser Projects Reporting Frequncy
        /// </summary>
        /// <param name="clusterId">Cluster id which should be updated</param>
        /// <param name="templateId">Template ID</param>
        /// <returns></returns>
        public static int UpdateClusterRF(int clusterId, int templateId)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@clusterId", SqlDbType.Int, 4, clusterId));
                    prams.Add(db.MakeInParam("@templateId", SqlDbType.Int, 4, templateId));

                    int exec = db.RunProc("up_UpdateClusterProjectsReportingFrequncy", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get DataTable of All Reporting Frequncy
        /// </summary>
        /// <returns>Reporting Frequncy DataTable</returns>
        public static DataTable getAllReportingFrequncyy()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllReportingFrequncy", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get All Reporting Frequncy By Sector Id
        /// </summary>
        /// <param name="sectorId">Sector ID by which we get All Reporting Frequncy </param>
        /// <returns></returns>
        public static DataTable getAllReportingFrequncyy(int sectorId)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@SectorId", SqlDbType.Int, 4, sectorId);
                dt = db.GetDataSet("up_getAllReportingFrequncy", prams).Tables[0];
            }

            return dt;
        }
       }
}
