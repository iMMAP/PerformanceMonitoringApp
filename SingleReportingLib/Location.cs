using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Location
    /// </summary>
    public class Location
    {


        #region Members and Properties

        private int _Location_ID;
        private int _FK_Location_Level;
        private string _Location_Name;
        private int _Location_PCode;
        private int _Location_Linked_ID;

        public int Location_ID { get { return _Location_ID; } set { _Location_ID = value; } }
        public int FK_Locaiton_Level { get { return _FK_Location_Level; } set { _FK_Location_Level = value; } }
        public string Location_Name { get { return _Location_Name; } set { _Location_Name = value; } }
        public int Location_PCode { get { return _Location_PCode; } set { _Location_PCode = value; } }
        public int Location_Linked_ID { get { return _Location_Linked_ID; } set { _Location_Linked_ID = value; } }


        #endregion

        public Location()
        {

        }

        public Location(int locationID)
        {
            loadLocation(locationID);
        }

        /// <summary>
        /// Load Location by Location Id
        /// </summary>
        /// <param name="locationID">the name of Location id whose data to be fetched</param>
        private void loadLocation(int locationID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pLocationId", SqlDbType.Int, 0, locationID);
                    reader = db.GetDataReader("up_getLocationById", prams);
                    if (reader.Read())
                        LoadLocationInfo(reader);
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



       
        private void LoadLocationInfo(IDataReader reader)
        {
            //Location_ID,Location_level_ID,Location_Name,Location_Pcode,Location_Linked_ID,isActive
            _Location_ID = reader["Location_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Location_ID"]);
            _FK_Location_Level = reader["Location_level_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Location_level_ID"]);
            _Location_Linked_ID = reader["Location_Linked_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Location_Linked_ID"]);
            _Location_Name = reader["Location_Name"] == System.DBNull.Value ? "" : reader["Location_Name"].ToString();
            _Location_PCode = reader["Location_Pcode"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Location_Pcode"]);
            //_Location_ID = reader["Project_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Project_ID"]);

        }


        /// <summary>
        /// Enalbe / Disable Location by Location Id
        /// </summary>
        /// <param name="Location_ID"></param>
        /// <returns>True for successful / False</returns>
        public static bool enableDisableLocation(int Location_ID)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pLocation_ID", SqlDbType.Int, 0, Location_ID);

                    int exec = db.RunProc("UP_Locations_EnableDisable", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        /// <summary>
        /// Insert Update by object of class Location
        /// </summary>
        /// <param name="l">The name of object of class Locatin which has to be inserted</param>
        /// <returns></returns>
        public static int insertUpdate(Location l)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    //(@pLocation_ID int,@pFK_Location_Level int,@pLocation_Name nvarchar(500),@pLocation_PCode numeric,@pLocation_Linked_ID int	

                    prams.Add(db.MakeInParam("@pLocation_ID", SqlDbType.Int, 0, l.Location_ID));
                    prams.Add(db.MakeInParam("@pFK_Location_Level", SqlDbType.Int, 0, l.FK_Locaiton_Level));
                    prams.Add(db.MakeInParam("@pLocation_Name", SqlDbType.NVarChar, 500, l.Location_Name));
                    prams.Add(db.MakeInParam("@pLocation_PCode", SqlDbType.Int, 0, l.Location_PCode));
                    if (l.Location_Linked_ID == 0)
                        prams.Add(db.MakeInParam("@pLocation_Linked_ID", SqlDbType.Int, 0, DBNull.Value));
                    else
                        prams.Add(db.MakeInParam("@pLocation_Linked_ID", SqlDbType.Int, 0, l.Location_Linked_ID));
                    int exec = db.RunProc("UP_Locations_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get DataTable of All Locations
        /// </summary>
        /// <returns>DataTable of All Locations</returns>
        public static DataTable getAllLocations()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllLocations", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of All Provinces
        /// </summary>
        /// <returns>DataTable of All Provinces</returns>
        public static DataTable getAllProvinces()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("UP_Location_GetAllProvinces", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get DataTable of All Sub Locations by Parent id
        /// </summary>
        /// <param name="parentID">The name of parent id whose data to be fetched</param>
        /// <returns>DataTable of All Sub Locations</returns>
        public static DataTable getAllSubLocations(int parentID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ParentID", SqlDbType.Int, 4, parentID);
                dt = db.GetDataSet("UP_Location_GetAllSubLocations", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Delete Location Level by Location level id
        /// </summary>
        /// <param name="LocationLevelId">The name of LocationLevelId whose data should be deleted</param>
        /// <returns></returns>
        public static int deleteLocationLevel(int LocationLevelId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pLocationLevelId", SqlDbType.Int, 4, LocationLevelId));
                    int exec = db.RunProc("up_deleteLocationLevel", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get DataTable of AllLocationLevels
        /// </summary>
        /// <returns>DataTable of AllLocationLevels</returns>
        public static DataTable getAllLocationLevels()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllLocationLevels", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Add / Update Location Levels by LocationLevelsTypeId,LocationLevels,Order
        /// </summary>
        /// <param name="LocationLevelsTypeId"></param>
        /// <param name="LocationLevels"></param>
        /// <param name="Order"></param>
        /// <returns></returns>
        public static int AddUpdateLocationLevels(int LocationLevelsTypeId, string LocationLevels, int Order)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pLocation_Level_ID", SqlDbType.Int, 0, LocationLevelsTypeId));
                    prams.Add(db.MakeInParam("@pLocation_Level_Name", SqlDbType.NVarChar, 0, LocationLevels));
                    prams.Add(db.MakeInParam("@pLocation_Order", SqlDbType.Int, 0, Order));
                    int exec = db.RunProc("up_LocationLevel_InsertUpdate", prams.ToArray());
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
        /// Get ParentLocationID by LocationID
        /// </summary>
        /// <param name="locationID">The name of location id whose data to be fetched</param>
        /// <returns></returns>
        public static int getParentLocationID(int locationID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@LocationID", SqlDbType.Int, 4, locationID);
                dt = db.GetDataSet("UP_Location_GetParentLocationID", prams).Tables[0];
            }

            return Int32.Parse(dt.Rows[0][0].ToString());
        }
        /// <summary>
        /// Get Location Name by Location Id
        /// </summary>
        /// <param name="locationID">The name of location id whose data to be fetched</param>
        /// <returns></returns>
        public static string getLocationName(int locationID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@LocationID", SqlDbType.Int, 4, locationID);
                dt = db.GetDataSet("UP_Location_GetLocationNameByLocationID", prams).Tables[0];
            }

            return dt.Rows[0][0].ToString();
        }
        /// <summary>
        /// Delete Comp by Comp ID
        /// </summary>
        /// <param name="CampId">The name of comp id whose data to be fetched</param>
        /// <returns></returns>
        public static int deleteCamp(int CampId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pCampId", SqlDbType.Int, 4, CampId));
                    int exec = db.RunProc("up_deleteCamp", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get DataTable of Comps
        /// </summary>
        /// <returns>DataTable of Comps</returns>
        public static DataTable getAllCamps()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("UP_Location_GetAllCamps", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get DataTable of All Comps Details
        /// </summary>
        /// <returns> DataTable of All Comps Details</returns>
        public static DataTable getAllCampsDetails()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllCapms", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of Camp by camp id
        /// </summary>
        /// <param name="campID">The Camp id whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataTable getCampById(int campID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@Camp_ID", SqlDbType.Int, 4, campID);
                dt = db.GetDataSet("UP_Camp_GetByID", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Insert new Camp  with camp id , location id and Camp title
        /// </summary>
        /// <param name="campId">Camp id which needs to be inserted</param>
        /// <param name="locationId">Camp Location id which needs to be inserted</param>
        /// <param name="campTitle">Camp Title which needs to be inserted</param>
        /// <returns></returns>
        public static int InsertCamp(int campId, int locationId, string campTitle)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pLocationID", SqlDbType.Int, 0, locationId));
                    prams.Add(db.MakeInParam("@pCampID", SqlDbType.Int, 0, campId));
                    prams.Add(db.MakeInParam("@pCamp_Title", SqlDbType.NVarChar, 0, campTitle));
                    int exec = db.RunProc("UP_Camp_InsertUpdate", prams.ToArray());
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
        /// Get DataTable of Camps by Tehsil id
        /// </summary>
        /// <param name="tehsilID">Tehsil id whose data needs to be fetched</param>
        /// <returns>DataTable of Camps</returns>
        public static DataTable getCamps_ByTehsilID(int tehsilID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@Tehsil_ID", SqlDbType.Int, 4, tehsilID);
                dt = db.GetDataSet("UP_GetCamps_ByTehsilID", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of Parent Location Levels by Location Level Id
        /// </summary>
        /// <param name="Id">The Location Level Id whose data needs to be fetched</param>
        /// <returns>DataTable of Parent Location Levels</returns>
        public static DataTable getParentLocationLevelsById(int Id)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@pLocationLevelId", SqlDbType.Int, 4, Id);
                dt = db.GetDataSet("up_getParentLocationLevelsById", prams).Tables[0];
            }

            return dt;
        }
    }
}
