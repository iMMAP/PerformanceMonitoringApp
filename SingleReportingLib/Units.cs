using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Units
    /// </summary>
    public class Units
    {
        private int _UnitID;
        private string _UnitTitle;
        private string _UnitDescription;
        private bool _UnitActive;

        public int UnitID { get { return _UnitID; } set { _UnitID = value; } }
        public string UnitTitle { get { return _UnitTitle; } set { _UnitTitle = value; } }
        public string UnitDescription { get { return _UnitDescription; } set { _UnitDescription = value; } }
        public bool UnitActive { get { return _UnitActive; } set { _UnitActive = value; } }



        public Units(int unitID)
        {
            loadUnits(unitID);
        }


        /// <summary>
        /// Load Units by Unit ID
        /// </summary>
        /// <param name="unitID">the id of unit whose records to be fetched</param>
        private void loadUnits(int unitID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@Unit_ID", SqlDbType.Int, 4, unitID);
                    reader = db.GetDataReader("up_Units_GetByID", prams);
                    if (reader.Read())
                        LoadUnits(reader);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

       
        private void LoadUnits(IDataReader reader)
        {
            _UnitID = reader["Unit_ID"] == System.DBNull.Value ? 0 : Int32.Parse(reader["Unit_ID"].ToString());
            _UnitTitle = reader["Unit_Title"] == System.DBNull.Value ? "" : reader["Unit_Title"].ToString();
            _UnitDescription = reader["Unit_Description"] == System.DBNull.Value ? "" : reader["Unit_Description"].ToString();
            _UnitActive = reader["Unit_Active"] == System.DBNull.Value ? false : bool.Parse(reader["Unit_Active"].ToString());
        }

        /// <summary>
        /// Get DataTable of All Units
        /// </summary>
        /// <returns></returns>
        public static DataTable getAllUnits()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllUnits", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Enable / Disable Unit by Unit id
        /// </summary>
        /// <param name="unit_ID">The id of Unit whose record to be fetched</param>
        /// <returns></returns>
        public static bool enableDisableUnit(int unit_ID)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pUnit_ID", SqlDbType.Int, 0, unit_ID);

                    int exec = db.RunProc("UP_Units_EnableDisable", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }
        /// <summary>
        /// Add / Update Unit by Unit id. unit Tiltle, unit Description
        /// </summary>
        /// <param name="iUnitId">The id of Unit which has to be added / updated</param>
        /// <param name="sUnitTitle">The Title of Unit which has to be added / updated</param>
        /// <param name="sUnitDescription">The Description of Unit which has to be added / updated</param>
        /// <returns></returns>
        public static int AddUpdateUnit(int iUnitId, string sUnitTitle, string sUnitDescription)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[3];
                    prams[0] = db.MakeInParam("@pUnit_ID", SqlDbType.Int, 0, iUnitId);
                    prams[1] = db.MakeInParam("@pUnit_Title", SqlDbType.NVarChar, 50, sUnitTitle);
                    prams[2] = db.MakeInParam("@pUnit_Description", SqlDbType.NVarChar, 50, sUnitDescription);
                    return db.RunProc("UP_Unit_InsertUpdate", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }

        /// <summary>
        /// Delete Unit by unit id
        /// </summary>
        /// <param name="unit_ID">The id of unit whose record should be fetched</param>
        /// <returns></returns>
        public static bool deleteUnit(int unit_ID)
        {
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pUnit_ID", SqlDbType.Int, 0, unit_ID);

                    int exec = db.RunProc("UP_Units_DeleteByID", prams);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

    }
}
