using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{   
    /// <summary>
    /// Get Details for Counterpart
    /// </summary>
    public class Counterpart
    {

        #region Members and Properties

        private int _CounterpartID;
        private int _CounterpartTypeID;
        private int _LocationID;
        private int _GovtDeptID;
        private int _ProjectID;



        public int CounterpartID { get { return _CounterpartID; } set { _CounterpartID = value; } }
        public int CounterpartTypeID { get { return _CounterpartTypeID; } set { _CounterpartTypeID = value; } }
        public int LocationID { get { return _LocationID; } set { _LocationID = value; } }
        public int GovtDeptID { get { return _GovtDeptID; } set { _GovtDeptID = value; } }
        public int ProjectID { get { return _ProjectID; } set { _ProjectID = value; } }
        

        #endregion


        public Counterpart()
        {

        }
        /// <summary>
        /// Load Counterpart by counterpart ID
        /// </summary>
        /// <param name="counterpartID">counterpart ID with wich Counterpart should be loaded</param>
        public Counterpart(int counterpartID)
        {
            loadCounterpart(counterpartID);
        }

        private void loadCounterpart(int counterpartID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@BeneficiaryID", SqlDbType.Int, 4, counterpartID);
                    reader = db.GetDataReader("UP_Beneficiary_GetByID", prams);
                    if (reader.Read())
                        LoadCounterpartInfo(reader);
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
        
        private void LoadCounterpartInfo(IDataReader reader)
        {

            _CounterpartID = reader["CounterpartID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["CounterpartID"]);
            _CounterpartTypeID = reader["CounterpartTypeID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["CounterpartTypeID"]);
            _LocationID = reader["LocationID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["LocationID"]);
            _GovtDeptID = reader["GovtDeptID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["GovtDeptID"]);
            _ProjectID = reader["ProjectID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["ProjectID"]);
            


        }
        /// <summary>
        /// Insert new Counter part
        /// </summary>
        /// <param name="c">Counterpart class object which should be inserted</param>
        /// <returns></returns>
        public static int insertCounterpart(Counterpart c)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@CounterpartTypeID", SqlDbType.Int, 4, c.CounterpartTypeID));
                    prams.Add(db.MakeInParam("@LocationID", SqlDbType.Int, 4, c.LocationID));
                    prams.Add(db.MakeInParam("@GovtDeptID", SqlDbType.Int, 4, c.GovtDeptID));
                    prams.Add(db.MakeInParam("@ProjectID", SqlDbType.Int, 4, c.ProjectID));
                    prams.Add(db.MakeInParam("@CounterpartID", SqlDbType.Int, 4, c.CounterpartID));

                    int exec = db.RunProc("UP_Counterpart_Insert", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete Government Department by GovtDptID
        /// </summary>
        /// <param name="GovtDptId">the id of GovtDpt which should be deleted</param>
        /// <returns></returns>
        public static int deleteGovtDpt(int GovtDptId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pGovtDptId", SqlDbType.Int, 4, GovtDptId));
                    int exec = db.RunProc("up_deleteGovtDpt", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Insert new Government Department
        /// </summary>
        /// <param name="Govt_Dept_ID">new Government Department id</param>
        /// <param name="Govt_Dept_Name">new Government Department Name</param>
        /// <param name="FK_Govt_Loc_ID">Government Location Id</param>
        /// <returns></returns>
        public static int InsertGovtDpt(int Govt_Dept_ID, string Govt_Dept_Name, int FK_Govt_Loc_ID)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pGovt_Dept_ID", SqlDbType.Int, 0, Govt_Dept_ID));
                    prams.Add(db.MakeInParam("@pGovt_Dept_Name", SqlDbType.NVarChar, 0, Govt_Dept_Name));
                    prams.Add(db.MakeInParam("@pFK_Govt_Loc_ID", SqlDbType.Int, 0, FK_Govt_Loc_ID));
                    int exec = db.RunProc("UP_Govt_Dept_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Get DataTable of Counterpart by Project ID
        /// </summary>
        /// <param name="projectID">Project id by which counterpart Datatable should be laoded</param>
        /// <returns>Couterpart DataTable</returns>

        public static DataTable getCounterpart_ByProjectID(int projectID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                dt = db.GetDataSet("UP_Counterpart_ByProjectID", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of Counterpart by ID
        /// </summary>
        /// <param name="beneficiaryID">id by which counterpart Datatable should be laoded</param>
        /// <returns>DataTable of Couterpart</returns>
        public static DataTable getCounterpartByID(int beneficiaryID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@BeneficiaryID", SqlDbType.Int, 4, beneficiaryID);
                dt = db.GetDataSet("UP_Beneficiary_GetByID", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get All Couterpart Types
        /// </summary>
        /// <returns>Datatable of Couterpart Types</returns>
        public static DataTable getAllCounterpartTypes()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];                
                dt = db.GetDataSet("UP_CounterpartType_GetAll", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get All Government Departments
        /// </summary>
        /// <returns>DataTable of All Government Departments</returns>
        public static DataTable getAllGovtDpt()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("UP_GovtDpt_GetAll", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get All Government Departments by Location ID
        /// </summary>
        /// <param name="locationID">Location Id by which Government Departments should be get</param>
        /// <returns>Data Table of Government Departments</returns>
        public static DataTable getAllGovtDept_ByLocationID(int locationID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@LocationID", SqlDbType.Int, 4, locationID);
                dt = db.GetDataSet("UP_GovtDept_GetAllByLocationID", prams).Tables[0];
            }

            return dt;
        }
 
        /// <summary>
        /// Delete Counterpart by Counterpart ID
        /// </summary>
        /// <param name="counterpartID">The Id of Counterpart by which counterpart should be deleted</param>
        /// <returns></returns>
        public static int deleteCounterpart(int counterpartID)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@CounterpartID", SqlDbType.Int, 4, counterpartID));
                    int exec = db.RunProc("up_Counterpart_DeleteByID", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Delete CounterpartType by CounterpartType ID
        /// </summary>
        /// <param name="counterpartTypeID">The Id of CounterpartType by which counterpartType should be deleted</param>
        /// <returns></returns>
        public static int deleteCounterpartType(int counterpartTypeID)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pCounterPartTypeId", SqlDbType.Int, 4, counterpartTypeID));
                    int exec = db.RunProc("up_deleteCounterPartType", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Insert new CounterpartType
        /// </summary>
        /// <param name="CounterPartTypeId">The id of new CounterPartType</param>
        /// <param name="CounterPartType">The name of new CounterPartType</param>
        /// <returns></returns>
        public static int InsertCounterpartTypes(int CounterPartTypeId, string CounterPartType)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pCounterpartTypeID", SqlDbType.Int, 0, CounterPartTypeId));
                    prams.Add(db.MakeInParam("@pCounterpartTypeName", SqlDbType.NVarChar, 0, CounterPartType));
                    int exec = db.RunProc("UP_CounterpartType_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
