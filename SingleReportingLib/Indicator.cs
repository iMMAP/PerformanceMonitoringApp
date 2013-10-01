using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Indicator
    /// </summary>
    public class Indicator
    {
        public Indicator()
        {

        }

        public Indicator(int indId)
        {

            loadIndicator(indId);
        }

        /// <summary>
        /// Get the indicate with its stauts ID
        /// </summary>
        /// <param name="indicatorId">Indicator ID whose status neeeds to be featched</param>
        /// <returns></returns>
        public static bool getIndicatorStatusById(int indicatorId)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pIndicatorId", SqlDbType.Int, 0, indicatorId);
                    ds = db.GetDataSet("up_getIndicatorStatusById", prams);
                    if (ds != null)
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsPublic"].ToString()))
                                {
                                    return clsCommon.ParseBool(ds.Tables[0].Rows[0]["IsPublic"].ToString());
                                }

                            }

                        }
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            return false;

        }

        /// <summary>
        /// Get the Indicate Status with its Activity ID
        /// </summary>
        /// <param name="Activity_Id">Activity Id whose status needs to be fetched</param>
        /// <returns></returns>
        public static bool getIndicatorStatusByActivityId(int Activity_Id)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pActivity_Id", SqlDbType.Int, 0, Activity_Id);
                    ds = db.GetDataSet("up_getIndicatorStatus", prams);
                    if (ds != null)
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsPublic"].ToString()))
                                {
                                    return clsCommon.ParseBool(ds.Tables[0].Rows[0]["IsPublic"].ToString());
                                }

                            }

                        }
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            return false;

        }

        /// <summary>
        /// Get Acitivity Status with Activity Id
        /// </summary>
        /// <param name="Activity_Id">Activity Id whose status needs to be fetched</param>
        /// <returns></returns>
        public static bool getActivityStatusByActivityId(int Activity_Id)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pActivity_Id", SqlDbType.Int, 0, Activity_Id);
                    ds = db.GetDataSet("up_getActivityStatus", prams);
                    if (ds != null)
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsPublic"].ToString()))
                                {
                                    return clsCommon.ParseBool(ds.Tables[0].Rows[0]["IsPublic"].ToString());
                                }

                            }

                        }
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            return false;

        }
        /// <summary>
        /// Get the Indicator Status with Data iD
        /// </summary>
        /// <param name="Data_Id">Data Id Whose status needs to be fetched</param>
        /// <returns></returns>
        public static bool getIndicatorStatusByDataId(int Data_Id)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pData_Id", SqlDbType.Int, 0, Data_Id);
                    ds = db.GetDataSet("up_getIndicatorStatusByDataId", prams);
                    if (ds != null)
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsPublic"].ToString()))
                                {
                                    return clsCommon.ParseBool(ds.Tables[0].Rows[0]["IsPublic"].ToString());
                                }

                            }

                        }
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            return false;

        }

        /// <summary>
        /// Get the Activity Status with Data iD
        /// </summary>
        /// <param name="Data_Id">Data Id Whose status needs to be fetched</param>
        /// <returns></returns>
        public static bool getActivityStatusByDataId(int Data_Id)
        {
            DataSet ds = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pData_Id", SqlDbType.Int, 0, Data_Id);
                    ds = db.GetDataSet("up_getActivityStatusByDataId", prams);
                    if (ds != null)
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsPublic"].ToString()))
                                {
                                    return clsCommon.ParseBool(ds.Tables[0].Rows[0]["IsPublic"].ToString());
                                }

                            }

                        }
                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
            }
            return false;

        }
        /// <summary>
        /// This change The Indicate Status with Indicator id and new Status
        /// </summary>
        /// <param name="IndicatorId">Indicator iD whose Status to be changed</param>
        /// <param name="status">The new Status of Indicator</param>
        /// <returns></returns>
        public static int ChangeIndicatorStatus(int IndicatorId, bool status)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@pIndicator_Id", SqlDbType.Int, 0, IndicatorId);
                    prams[1] = db.MakeInParam("@pStatus", SqlDbType.Bit, 0, status);
                    return db.RunProc("up_changeIndicatorStatusById", prams);

                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
                return -1;
            }
            return 0;

        }

        /// <summary>
        /// This change The Acitivty Status with Indicator id and new Status
        /// </summary>
        /// <param name="IndicatorId">Acitivity iD whose Status to be changed</param>
        /// <param name="status">The new Status of Acitivity</param>
        /// <returns></returns>
        public static int ChangeActivityStatus(int ActivityId, bool status)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@pActivity_Id", SqlDbType.Int, 0, ActivityId);
                    prams[1] = db.MakeInParam("@pStatus", SqlDbType.Bit, 0, status);
                    return db.RunProc("up_changeActivityStatusById", prams);

                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
                return -1;
            }
            return 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IndicatorId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static int ChangeIndicatorActiveStatus(int IndicatorId, bool isActive)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@pIndicator_Id", SqlDbType.Int, 0, IndicatorId);
                    prams[1] = db.MakeInParam("@pIsActive", SqlDbType.Bit, 0, isActive);
                    return db.RunProc("up_changeIndicatorActiveStatusById", prams);

                }
            }
            catch (Exception ex)
            {
                //new SqlLog().InsertSqlLog(propertyId, "PropertyInfo.loadPropertyInfo", ex);
                throw;
                return -1;
            }
            return 0;

        }

        /// <summary>
        /// This Gets the Indicate with Indicator id
        /// </summary>
        /// <param name="IndicatorId">Indicator id whose data needs to be fetched</param>
        private void loadIndicator(int IndicatorId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pIndicator_ID", SqlDbType.Int, 0, IndicatorId);
                    reader = db.GetDataReader("up_getIndicatorById", prams);
                    if (reader.Read())
                        LoadIndicatorInfo(reader);
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

      
        private void LoadIndicatorInfo(IDataReader reader)
        {

            _Subsector = clsCommon.ParseDBNullString(reader["Subsector"]);
            _Sector = clsCommon.ParseDBNullString(reader["Sector"]);
            _Title = clsCommon.ParseDBNullString(reader["Indicator_Title"]);
            _IndicatorId = clsCommon.ParseDBNullInt(reader["Indicator_ID"]);
            _Weight = clsCommon.ParseDBNullString(reader["Weight"]);

        }

        //public static DataSet GetAllIndicators()
        //{
        //    DataSet ds = null;

        //    try
        //    {
        //        using (DbManager db = DbManager.GetDbManager())
        //        {
        //            var prams = new SqlParameter[0];

        //            ds = db.GetDataSet("Web_SP_GetAllIndicators", prams);

        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                return ds;
        //            }

        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        if (ds != null)
        //            ds.Dispose();
        //    }
        //    return null;
        //}

        /// <summary>
        /// Get the DataSet of All Indicators
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllIndicators()
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@username", SqlDbType.VarChar, 100, HttpContext.Current.User.Identity.Name);
                    ds = db.GetDataSet("Web_SP_GetAllIndicators", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// Get DataSet of All Indicates by Project Sector with Project Id
        /// </summary>
        /// <param name="ProjectId">Project id whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllIndicatorsByProjectSector(int ProjectId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];

                    prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@ProjectId", SqlDbType.Int, 4, ProjectId);
                    ds = db.GetDataSet("up_getAllIndicatorByProjectSector", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }


        /// <summary>
        /// Pool Id is not Implemented Yet
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="IpId"></param>
        /// <param name="PoolId">Always set as zero</param>
        /// <returns></returns>
        public static DataSet GetAllIndicatorsByProjectIPActivity(int ProjectId, int IpId, int PoolId, int locId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[4];

                    prams[0] = db.MakeInParam("@ProjectId", SqlDbType.Int, 4, ProjectId);
                    prams[1] = db.MakeInParam("@poolId", SqlDbType.Int, 4, PoolId);
                    prams[2] = db.MakeInParam("@IpId", SqlDbType.Int, 4, IpId);
                    prams[3] = db.MakeInParam("@spTargetId", SqlDbType.Int, 4, locId);
                    ds = db.GetDataSet("up_getPoolActivityIndicator", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// Add the Indicate with Subsector ID, Sector Id, Indicator Name, Weight
        /// </summary>
        /// <param name="SubSector_ID">The Subsector id which has to be added</param>
        /// <param name="Sector_ID">The Sector Id which has to be Added</param>
        /// <param name="Indicator_Name">The Name of Indicator which has to be added</param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int AddIndicator(int SubSector_ID, int Sector_ID, string Indicator_Name, int weight)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[4];
                    prams[0] = db.MakeInParam("@pSubSector_ID", SqlDbType.Int, 0, SubSector_ID);
                    prams[1] = db.MakeInParam("@Indicator_Title", SqlDbType.NVarChar, 250, Indicator_Name);
                    prams[2] = db.MakeInParam("@SectorId", SqlDbType.Int, 0, Sector_ID);
                    prams[3] = db.MakeInParam("@weight", SqlDbType.Int, 0, weight);
                    return db.RunProc("Web_SP_InsertIndicator", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }

        /// <summary>
        /// Add / Update the Indicate with Subsector ID, Sector Id, Indicator Name, Weight
        /// </summary>
        /// <param name="SubSector_ID">The Subsector id which has to be added / Updated</param>
        /// <param name="Sector_ID">The Sector Id which has to be Added / Updated</param>
        /// <param name="Indicator_Name">The Name of Indicator which has to be added / Updated</param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static int AddUpdateIndicator(int Indicator_ID, int SubSector_ID, int Sector_ID, string Indicator_Name, int weight)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[5];
                    prams[0] = db.MakeInParam("@pIndicator_ID", SqlDbType.Int, 0, Indicator_ID);
                    prams[1] = db.MakeInParam("@pIndicator_SubSector_ID", SqlDbType.Int, 0, SubSector_ID);
                    prams[2] = db.MakeInParam("@pIndicator_Title", SqlDbType.NVarChar, 250, Indicator_Name);
                    prams[3] = db.MakeInParam("@pIndicator_Sector_ID", SqlDbType.Int, 0, Sector_ID);
                    prams[4] = db.MakeInParam("@pWeight", SqlDbType.Int, 0, weight);
                    return db.RunProc("Web_SP_InsertUpdateIndicator", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }


        /// <summary>
        /// This Submits report with zero value
        /// </summary>
        /// <param name="Sp_target_ID"></param>
        /// <returns></returns>
        public static int SubmitNilReport(int Sp_target_ID)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@Sp_target_Id", SqlDbType.Int, 0, Sp_target_ID);
                    return db.RunProc("up_SubmitNilReport", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }


        #region Properties

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }

        }

        string _Subsector = string.Empty;
        public string Subsector
        {
            get { return _Subsector; }
            set { _Subsector = value; }

        }

        string _Sector = string.Empty;
        public string Sector
        {
            get { return _Sector; }
            set { _Sector = value; }

        }

        int _IndicatorId = 0;
        public int IndicatorId
        {
            get { return _IndicatorId; }
            set { _IndicatorId = value; }
        }


        string _Weight = string.Empty;
        public string Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }



        #endregion

    }
    /// <summary>
    /// Get Details for Activity
    /// </summary>
    public class Activity
    {

        public Activity() { }

        public Activity(int activityId)
        {
            loadActivity(activityId);
        }

        /// <summary>
        /// This Load Acitivty with Activity id
        /// </summary>
        /// <param name="activityId">Acitivity id whose data needs to be fetched</param>
        private void loadActivity(int activityId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pActivity_ID", SqlDbType.Int, 0, activityId);
                    reader = db.GetDataReader("up_getActivityById", prams);
                    if (reader.Read())
                        LoadActivityInfo(reader);
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

        /// <summary>
        /// This deletes Activity with Acitivity Id
        /// </summary>
        /// <param name="Id">The Acitivity id whose record needs to be deleted</param>
        /// <returns>True when successful delete / Fasle when Exception cought</returns>
        public static bool DeleteActivity(int Id)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@intActivityId", SqlDbType.Int, 0, Id);

                    db.RunProc("up_deleteActivityById", prams);
                    return true;


                }
            }
            catch (Exception ex) { return false; }

        }
        public static Activity loadActivityWithPool(int activityId, int poolId, int sp_target, int RptRreqId)
        {
            IDataReader reader = null;
            Activity a = new Activity();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[4];
                    prams[0] = db.MakeInParam("@pActivity_ID", SqlDbType.Int, 0, activityId);
                    prams[1] = db.MakeInParam("@pActivityPoolId", SqlDbType.Int, 0, poolId);
                    prams[2] = db.MakeInParam("@spTarget", SqlDbType.Int, 0, sp_target);
                    prams[3] = db.MakeInParam("@reportingFrequncyId", SqlDbType.Int, 0, RptRreqId);
                    reader = db.GetDataReader("up_getActivityByIdandPool", prams);
                    if (reader.Read())
                    {
                        a.Unit = reader["Unit_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Unit_ID"]);
                        a.UnitDetails = clsCommon.ParseString(reader["Unit_Title"]);
                        a.Data = reader["Activity_Data"].ToString();
                        a.Title = reader["Activity_Title"].ToString();
                        a.ActivityId = reader["Activity_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Activity_ID"]);
                        a.ActivityPoolTargetId = reader["ActivityPoolTargetId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["ActivityPoolTargetId"]);
                        a.Target = reader["Target"].ToString();
                        a.Value = clsCommon.ParseString(reader["ActValue"]);
                        a.ActivityTargetId = clsCommon.ParseInt(reader["intActivityTargetId"]);
                    }

                    return a;
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

      
        private void LoadActivityInfo(IDataReader reader)
        {

            _Unit = reader["FK_Unit"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Unit"]);
            _Data = reader["Activity_Data"].ToString();
            _Title = reader["Activity_Title"].ToString();
            _ActivityId = reader["Activity_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Activity_ID"]);
            _activityPoolTargetId = reader["ActivityPoolTargetId"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["ActivityPoolTargetId"]);
            _target = reader["Target"].ToString();


        }

        /// <summary>
        /// Get DataSet of All Acitivies with Indicator Id
        /// </summary>
        /// <param name="IndicatorId">Indicator id whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllActivity(int IndicatorId)
        {
            return GetAllActivity(IndicatorId, 0);
        }

        public static DataSet GetAllActivity(int IndicatorId, int sp_target_id)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[2];

                    prams[0] = db.MakeInParam("@IndicatorId", SqlDbType.Int, 0, IndicatorId);
                    prams[1] = db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, sp_target_id);
                    ds = db.GetDataSet("Web_SP_GetAllAcitvity", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// PoolId not implemented
        /// </summary>
        /// <param name="IndicatorId"></param>
        /// <param name="IpId"></param>
        /// <param name="ProjectId"></param>
        /// <param name="PoolId"></param>
        /// <returns></returns>
        public static DataSet GetAllActivityByProjectIp(int IndicatorId, int IpId, int ProjectId, int PoolId, int locationId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[5];

                    prams[0] = db.MakeInParam("@IndicatorId", SqlDbType.Int, 0, IndicatorId);
                    prams[1] = db.MakeInParam("@IpId", SqlDbType.Int, 0, IpId);
                    prams[2] = db.MakeInParam("@ProjectId", SqlDbType.Int, 0, ProjectId);
                    prams[3] = db.MakeInParam("@poolId", SqlDbType.Int, 0, PoolId);
                    prams[4] = db.MakeInParam("@spTargetId", SqlDbType.Int, 0, locationId);
                    ds = db.GetDataSet("up_ActivitiesByProjectIp", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// Get DataSet of Sub Sector Activity by Project Implementation Partners with subSector id, Service Provider Target Id ,ReportingFreq id
        /// </summary>
        /// <param name="subsectorId">Subsetctor Id whose record needs to be fetched</param>
        /// <param name="spTargetId">Service Provider Target Id whose record needs to be fetched</param>
        /// <param name="reportingFreqId">Reporting Frequency id whose record needs to be fetched</param>
        /// <returns>DataSet of Sub Sector Activity</returns>
        public static DataSet GetSubsectorActivityByProjectIp(int subsectorId, int spTargetId, int reportingFreqId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[3];

                    prams[0] = db.MakeInParam("@SubsectorId", SqlDbType.Int, 0, subsectorId);
                    prams[1] = db.MakeInParam("@spTargetId", SqlDbType.Int, 0, spTargetId);
                    prams[2] = db.MakeInParam("@reportingFreq", SqlDbType.Int, 0, reportingFreqId);

                    ds = db.GetDataSet("up_SubsectorActivitiesByProjectIp", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// This Insert / Update Activity with Activity class object  
        /// </summary>
        /// <param name="activity">The Object of Activity Class which has to be Inserted / Updated</param>
        /// <returns></returns>
        public static int InsertUpdate(Activity activity)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@Activity_Data", SqlDbType.VarChar,250, activity.Data),
                            db.MakeInParam("@Activity_Title", SqlDbType.NVarChar, 250, activity.Title), 
                            db.MakeInParam("@FK_Unit", SqlDbType.Int, 0, activity.Unit),
                            db.MakeInParam("@Activity_ID", SqlDbType.Int, 0, activity.ActivityId),
                            db.MakeInParam("@Indicator_ID", SqlDbType.Int, 0, activity.IndicatorId),
                            db.MakeInParam("@Activity_Weight", SqlDbType.Int, 0, activity.Weight),
                             db.MakeInParam("@Is_Recurring", SqlDbType.Bit, 0, activity.IsRecurring)
                        };

                    return db.RunProc("UP_Activity_InsertUpdate", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        
        public static int UpdateTargetValue(int PoolId, string value, int ActivityId, int freqId, int spTargetId, string target)
        {

            try 
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@value", SqlDbType.VarChar,100, value),
                            db.MakeInParam("@poolId", SqlDbType.Int, 0, PoolId),
                            db.MakeInParam("@id", SqlDbType.VarChar, 30, ActivityId), 
                            db.MakeInParam("@freqId", SqlDbType.Int, 0, freqId),
                            db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@target", SqlDbType.VarChar,100, target)
                        };

                    return db.RunProc("up_updateActivityTargetValue", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }


        public static int CreateProjectActivityPool(int ProjectId, int ActivityPoolId, int PoolLevel, int IPId, string ActivityIds, int SpTargetID)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@pActivityPoolId", SqlDbType.Int,0, ActivityPoolId),
                            db.MakeInParam("@pActivityIds", SqlDbType.VarChar, 1000, ActivityIds), 
                            db.MakeInParam("@pintPoolLevel", SqlDbType.Int, 0, PoolLevel),
                            db.MakeInParam("@pIPId", SqlDbType.Int, 0, IPId),
                            db.MakeInParam("@pProjectId", SqlDbType.Int, 0, ProjectId),
                            db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 0, SpTargetID)
                        };

                    return db.RunProc("UP_Activity_pool_Insert", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        public static int SetProjectActivityTarget(int poolTargetId, int ProjectId, int ActivityId, int PoolId,
                                                    int IPId, string target, string value, int spTargetId, bool isAdded)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@pActivityPoolTargetId", SqlDbType.Int,0, poolTargetId),
                            db.MakeInParam("@pActivityId", SqlDbType.Int,0, ActivityId),
                            db.MakeInParam("@pTarget", SqlDbType.VarChar, 1000, target), 
                            db.MakeInParam("@pValue", SqlDbType.VarChar, 1000, value), 
                            db.MakeInParam("@pPoolId", SqlDbType.Int, 0, PoolId),
                            db.MakeInParam("@pIPId", SqlDbType.Int, 0, IPId),
                            db.MakeInParam("@pProjectId", SqlDbType.Int, 0, ProjectId),
                            db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@isAdded", SqlDbType.Bit, 0, isAdded)
                        };

                    return db.RunProc("UP_Activity_pool_target_InsertUpdate", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        #region Properties

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }

        }

        string _Data = string.Empty;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }

        }

        int _ActivityId = 0;
        public int ActivityId
        {
            get { return _ActivityId; }
            set { _ActivityId = value; }
        }

        int _IndicatorId = 0;
        public int IndicatorId
        {
            get { return _IndicatorId; }
            set { _IndicatorId = value; }
        }


        int _Weight = 0;
        public int Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }

        int _Unit = 0;
        public int Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        string _UnitDetails = string.Empty;
        public string UnitDetails
        {
            get { return _UnitDetails; }
            set { _UnitDetails = value; }
        }

        int _activityPoolTargetId = 0;
        public int ActivityPoolTargetId
        {
            get { return _activityPoolTargetId; }
            set { _activityPoolTargetId = value; }
        }
        private string _target;
        public string Target
        {
            get { return _target; }
            set { _target = value; }

        }
        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }

        }

        int _ActivityTargetId = 0;
        public int ActivityTargetId
        {
            get { return _ActivityTargetId; }
            set { _ActivityTargetId = value; }
        }

        private bool _isRecurring;
        public bool IsRecurring
        {
            get { return _isRecurring; }
            set { _isRecurring = value; }

        }
        #endregion

    }

    /// <summary>
    /// Gets Details for Data
    /// </summary>
    public class Data
    {

        public Data() { }

        /// <summary>
        /// Get DataSet of All Data with Activity Id
        /// </summary>
        /// <param name="ActivityId">Activity Id whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllData(int ActivityId)
        {
            return GetAllData(ActivityId, 0);
        }

        /// <summary>
        /// This Deletes Data wich Data Id
        /// </summary>
        /// <param name="Id">Data Id whose data needs to be deleted</param>
        /// <returns></returns>
        public static bool DeleteData(int Id)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@intDataId", SqlDbType.Int, 0, Id);

                    db.RunProc("up_deleteDataById", prams);
                    return true;


                }
            }
            catch (Exception ex) { return false; }

        }

        /// <summary>
        /// This Delete DataType with DataType Id
        /// </summary>
        /// <param name="DataTypeId">DataType Id whose data needs to be deleted</param>
        /// <returns></returns>
        public static bool DeleteDataType(int DataTypeId)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@pData_TypeId", SqlDbType.Int, 0, DataTypeId);

                    db.RunProc("up_deleteDataTypes", prams);
                    return true;


                }
            }
            catch (Exception ex) { return false; }

        }

        /// <summary>
        /// This Gets DataSet of All Data with Acitivity ID and Service Provider Target ID
        /// </summary>
        /// <param name="ActivityId">Activity Id whose data needs to be fetched</param>
        /// <param name="sp_target_id">Service Provider Target Id whose Data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllData(int ActivityId, int sp_target_id)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[2];

                    prams[0] = db.MakeInParam("@intActivityId", SqlDbType.Int, 0, ActivityId);
                    prams[1] = db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, sp_target_id);
                    ds = db.GetDataSet("Web_SP_GetAllDataByActivityId", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        public static DataSet GetAllDataByActivityPool(int ActivityPoolId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@ActivityPoolTargetId", SqlDbType.Int, 0, ActivityPoolId);
                    ds = db.GetDataSet("up_ActivitiesDataByProjectIp", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }
        public static DataSet GetAllDataByActivityPool(int ActivityPoolId, int spTargetId, int rptFqId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[3];

                    prams[0] = db.MakeInParam("@ActivityPoolTargetId", SqlDbType.Int, 0, ActivityPoolId);
                    prams[1] = db.MakeInParam("@spTargetId", SqlDbType.Int, 0, spTargetId);
                    prams[2] = db.MakeInParam("@reportingFrequncyId", SqlDbType.Int, 0, rptFqId);
                    ds = db.GetDataSet("up_ActivitiesDataByProjectIp_pool", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// This Insert / Update Data with object of class Data
        /// </summary>
        /// <param name="data">The object of class Data which has to be Updated / Inserted</param>
        /// <returns></returns>
        public static int InsertUpdate(Data data)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@Data_Text", SqlDbType.NVarChar,250, data.DataText), 
                            db.MakeInParam("@Data_Unit", SqlDbType.Int, 0, data.Unit),
                            db.MakeInParam("@Data_ID", SqlDbType.Int, 0, data.DataId),
                            db.MakeInParam("@Activity_ID", SqlDbType.Int, 0, data.ActivityId),
                            db.MakeInParam("@DataType", SqlDbType.Int, 0, data.DataType),
                            db.MakeInParam("@Data_Type", SqlDbType.VarChar, 20, data.Data_Type),
                             db.MakeInParam("@IsUsedToCalculateIndicator", SqlDbType.Bit, 0, data.IsUsedToCalculateIndicator)
                        };

                    return db.RunProc("UP_Activity_Data_InsertUpdate", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        public static int SetProjectDataTarget(int DataTargetId, int ActivityPoolTargetId, int DataId, string target, string value, int spTargetId, bool isAdded)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@pintDataTargetId", SqlDbType.Int,0, DataTargetId),
                            db.MakeInParam("@pActivityPoolTargetId", SqlDbType.Int,0, ActivityPoolTargetId),
                            db.MakeInParam("@pTarget", SqlDbType.VarChar, 1000, target), 
                            db.MakeInParam("@pValue", SqlDbType.VarChar, 1000, value), 
                            db.MakeInParam("@pDataId", SqlDbType.Int, 0, DataId),
                            db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@isAdded", SqlDbType.Bit, 0, isAdded)
                        };

                    return db.RunProc("UP_Acticity_Data_Pool_Target_InsertUpdate", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        public static int UpdateTargetValue(int PoolId, string value, int DataId, int freqId, int spTargetId, string target)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@value", SqlDbType.VarChar,100, value),
                            db.MakeInParam("@poolId", SqlDbType.Int, 0, PoolId),
                            db.MakeInParam("@Id", SqlDbType.Int, 0, DataId),
                            db.MakeInParam("@freqId", SqlDbType.Int, 0, freqId),
                            db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@target", SqlDbType.VarChar,100, target)
                        };

                    return db.RunProc("up_updateDataTargetValue", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        #region Properties

        string _Data = string.Empty;
        public string DataText
        {
            get { return _Data; }
            set { _Data = value; }
        }

        string _Data_Type = string.Empty;
        public string Data_Type
        {
            get { return _Data_Type; }
            set { _Data_Type = value; }
        }
        int _DatType = 0;
        public int DataType
        {
            get { return _DatType; }
            set { _DatType = value; }
        }
        int _DataId = 0;
        public int DataId
        {
            get { return _DataId; }
            set { _DataId = value; }
        }

        int _ActivityId = 0;
        public int ActivityId
        {
            get { return _ActivityId; }
            set { _ActivityId = value; }
        }


        int _Unit = 0;
        public int Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }
        bool _IsUsedToCalculateIndicator;
        public bool IsUsedToCalculateIndicator
        {
            get
            {
                return _IsUsedToCalculateIndicator;
            }
            set
            {
                _IsUsedToCalculateIndicator = value;
            }
        }
        #endregion

        /// <summary>
        /// This Gets DataSet of All DataTypes
        /// </summary>
        /// <returns>DataSet of All DataTypes</returns>
        public static DataSet getAllDataTpyes()
        {
            DataSet ds = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                ds = db.GetDataSet("up_getAllDataTypes", null);
            }
            return ds;
        }

        /// <summary>
        /// This Insert DataType with DataType ID and DataType name
        /// </summary>
        /// <param name="DataTypeId">The DataType Id which has to be Inserted</param>
        /// <param name="DataType">The DataType name which has to be Inserted</param>
        /// <returns></returns>
        public static int InsertDataTypes(int DataTypeId, string DataType)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pData_Type_ID", SqlDbType.Int, 0, DataTypeId));
                    prams.Add(db.MakeInParam("@pData_Type", SqlDbType.NVarChar, 0, DataType));
                    int exec = db.RunProc("UP_Data_Types_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
            // return 0;
        }
    }

    /// <summary>
    /// Get Details for Elements
    /// </summary>
    public class Elements
    {

        public Elements() { }

        /// <summary>
        /// Get DataSet of All Elements with Data id
        /// </summary>
        /// <param name="DataId">The Data Id whose record needs to be fetched</param>
        /// <returns>DataSet of All Elements</returns>
        public static DataSet GetAllElements(int DataId)
        {
            return GetAllElements(DataId, 0);
        }
        public static DataSet GetAllElements(int DataId, int sp_target_id)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[2];

                    prams[0] = db.MakeInParam("@intDataId", SqlDbType.Int, 0, DataId);
                    prams[1] = db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, sp_target_id);
                    ds = db.GetDataSet("Web_SP_GetAllElementByDataId", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// Get DataSet of Data Elements by Project Implementaion Partner with DataTarget ID
        /// </summary>
        /// <param name="DataTargetId">DatTarget ID whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllDataElementByProjectIp(int DataTargetId)
        {

            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@intDataTargetId", SqlDbType.Int, 0, DataTargetId);
                    ds = db.GetDataSet("up_ActivitiesDataElementByProjectIp", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        ///  Get DataSet of Data Elements by Project Implementaion Partner with DataTarget ID, Service Provider Target Id and Reporting Frequency Id
        /// </summary>
        /// <param name="DataTargetId">DatTarget ID whose data needs to be fetched</param>
        /// <param name="spTargetId">Service Provider ID whose data needs to be fetched</param>
        /// <param name="reportingFreqId">Reporting Frequency  ID whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllDataElementByProjectIp(int DataTargetId, int spTargetId, int reportingFreqId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[3];

                    prams[0] = db.MakeInParam("@intDataTargetId", SqlDbType.Int, 0, DataTargetId);
                    prams[1] = db.MakeInParam("@spTargetId", SqlDbType.Int, 0, spTargetId);
                    prams[2] = db.MakeInParam("@reportingFrequncyId", SqlDbType.Int, 0, reportingFreqId);
                    ds = db.GetDataSet("up_ActivitiesDataElementByProjectIp_Pool", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return null;
        }

        /// <summary>
        /// Insert / Update Element with object of class Elements
        /// </summary>
        /// <param name="element">The Object of class Elements which has to be Inserted / Updated</param>
        /// <returns></returns>
        public static int InsertUpdate(Elements element)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;
                    SqlParameter pram;
                    if (element.Unit == 0)
                    {
                        pram = db.MakeInParam("@Data_Unit", SqlDbType.Int, 0, DBNull.Value);

                    }
                    else
                    {

                        pram = db.MakeInParam("@Data_Unit", SqlDbType.Int, 0, element.Unit);

                    }


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@Data_Element", SqlDbType.NVarChar,250, element.Data), 
                            pram,
                            db.MakeInParam("@Element_DataType", SqlDbType.Int, 0, element.DataTypeId),
                            db.MakeInParam("@Data_Id", SqlDbType.Int, 0, element.DataId),
                            db.MakeInParam("@Data_Element_ID", SqlDbType.Int, 0, element.ElementId)
                        };

                    return db.RunProc("UP_Data_Elements_InsertUpdate", parameters);

                }

            }
            catch (Exception)
            {

            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementTargetId"></param>
        /// <param name="DataTargetId"></param>
        /// <param name="ElementId"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="spTargetId"></param>
        /// <param name="isAdded"></param>
        /// <returns></returns>
        public static int SetProjectElementTarget(int ElementTargetId, int DataTargetId, int ElementId, string target, string value, int spTargetId, bool isAdded)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@pintElementTargetId", SqlDbType.Int,0, ElementTargetId),
                            db.MakeInParam("@pintDataTargetId", SqlDbType.Int,0, DataTargetId),
                            db.MakeInParam("@pTarget", SqlDbType.VarChar, 1000, target), 
                            db.MakeInParam("@pValue", SqlDbType.VarChar, 1000, value), 
                            db.MakeInParam("@pElementId", SqlDbType.Int, 0, ElementId),
                            db.MakeInParam("@SP_Target_ID", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@isAdded", SqlDbType.Bit, 0, isAdded)
                        };

                    return db.RunProc("UP_Data_Element_Pool_Target_InsertUpdate", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        /// <summary>
        /// Insert / Update Unit with Unit Name and Unit Description
        /// </summary>
        /// <param name="UnitName">The name of Unit which has to be Inserted / Updated</param>
        /// <param name="UnitDescription">The Description of Unit which has to be Inserted / Updated</param>
        /// <returns></returns>
        public static int insertUpdateUnit(string UnitName, string UnitDescription)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pUnit_ID", SqlDbType.Int, 0, 0));
                    prams.Add(db.MakeInParam("@pUnit_Title", SqlDbType.NVarChar, 0, UnitName));
                    prams.Add(db.MakeInParam("@pUnit_Description", SqlDbType.NVarChar, 0, UnitDescription));
                    int exec = db.RunProc("UP_Units_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }


        }
        /// <summary>
        /// Insert / Update Unit with Unit Id, Unit Name and Unit Description
        /// </summary>
        /// <param name="unitId">The id of Unit which has to be Inserted / Updated</param>
        /// <param name="UnitName">The name of Unit which has to be Inserted / Updated</param>
        /// <param name="UnitDescription">The Description of Unit which has to be Inserted / Updated</param>
        /// <returns></returns>
        public static int insertUpdateUnit(int unitId,string UnitName, string UnitDescription)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pUnit_ID", SqlDbType.Int, 0, unitId));
                    prams.Add(db.MakeInParam("@pUnit_Title", SqlDbType.NVarChar, 0, UnitName));
                    prams.Add(db.MakeInParam("@pUnit_Description", SqlDbType.NVarChar, 0, UnitDescription));
                    int exec = db.RunProc("UP_Units_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }


        }
        public static int UpdateTargetValue(int PoolId, string value, int elementId, int freqId, int spTargetId, string target)
        {

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] parameters;


                    parameters = new SqlParameter[]
                        {   	
                            db.MakeInParam("@value", SqlDbType.VarChar,100, value),
                            db.MakeInParam("@poolId", SqlDbType.Int, 0, PoolId),
                            db.MakeInParam("@Id", SqlDbType.Int, 0, elementId),
                            db.MakeInParam("@freqId", SqlDbType.Int, 0, freqId),
                            db.MakeInParam("@sp_target_Id", SqlDbType.Int, 0, spTargetId),
                            db.MakeInParam("@target", SqlDbType.VarChar,100, target)

                        };

                    return db.RunProc("up_updateElementTargetValue", parameters);

                }

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        /// <summary>
        /// This Delete Elements with Element Id
        /// </summary>
        /// <param name="Id">The Id of Element whose record needs to be deleted</param>
        /// <returns></returns>
        public static bool DeleteElement(int Id)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[1];

                    prams[0] = db.MakeInParam("@intElementId", SqlDbType.Int, 0, Id);

                    db.RunProc("up_deleteElementById", prams);
                    return true;


                }
            }
            catch (Exception ex) { return false; }

        }
        #region Properties

        string _Data = string.Empty;
        public string Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        int _DataId = 0;
        public int DataId
        {
            get { return _DataId; }
            set { _DataId = value; }
        }


        int _DataTypeId = 0;
        public int DataTypeId
        {
            get { return _DataTypeId; }
            set { _DataTypeId = value; }
        }

        int _ElementId = 0;
        public int ElementId
        {
            get { return _ElementId; }
            set { _ElementId = value; }
        }


        int _Unit = 0;
        public int Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        #endregion

    }
}
