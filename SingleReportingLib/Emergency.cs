using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Emergency
    /// </summary>
    public class Emergency
    {

        public Emergency()
        {

        }
        /// <summary>
        /// Get DataTable of all Emergerncies
        /// </summary>
        /// <returns>DataTable of all Emergerncies</returns>
        public static DataTable getAllEmergency()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllEmergencies", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of All Emergency Types
        /// </summary>
        /// <returns>DataTable of All Emergency Types</returns>
        public static DataTable getAllEmergencyTypes()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllEmergencyTypes", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Delete Emergency Type By Emergency Type ID
        /// </summary>
        /// <param name="EmergencyTypeID">The id of EmergencyType which should be deleted</param>
        /// <returns></returns>
        public static int deleteEmergencyType(int EmergencyTypeID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pEmergencyTypeId", SqlDbType.Int, 4, EmergencyTypeID));
                    int exec = db.RunProc("up_deleteEmergencyTypes", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Add / Update Emergency
        /// </summary>
        /// <param name="EmergencyId">The Id of Emergency by which Emergency should be Added / Updated</param>
        /// <param name="sEmergencyTitle">The Title of Emergency by which Emergency should be Added / Updated</param>
        /// <param name="EmergencyType">The Type of Emergency by which Emergency should be Added / Updateed</param>
        /// <param name="sEmergencyCountry">The Country of Emergency by which Emergency should be Added / Updated</param>
        /// <param name="sEmergencyStart_Date">The Start_Date of Emergency by which Emergency should be Added / Updated</param>
        /// <param name="sEmergencyEnd_Date">The End_Date of Emergency by which Emergency should be Added / Updated</param>
        /// <returns></returns>
        public static int AddUpdateEmergency(int EmergencyId,string sEmergencyTitle, int EmergencyType , string sEmergencyCountry, string sEmergencyStart_Date, string sEmergencyEnd_Date)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pEmergency_ID", SqlDbType.Int, 0, EmergencyId));
                    prams.Add(db.MakeInParam("@pEmergency_Type", SqlDbType.Int, 0, EmergencyType));
                    prams.Add(db.MakeInParam("@pFK_Emergency_Country", SqlDbType.NVarChar, 0, sEmergencyCountry));
                    prams.Add(db.MakeInParam("@pEmergency_Title", SqlDbType.NVarChar, 0, sEmergencyTitle));
                    prams.Add(db.MakeInParam("@pEmergency_Start_Date", SqlDbType.Date, 0, Convert.ToDateTime(sEmergencyStart_Date).Date));
                    prams.Add(db.MakeInParam("@pEmergency_End_Date", SqlDbType.Date, 0, Convert.ToDateTime(sEmergencyEnd_Date).Date));
                    int exec = db.RunProc("UP_Emergency_InsertUpdate", prams.ToArray());
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
        /// Add / Update Emergency Types by EmergencyType ID and EmergencyType Name
        /// </summary>
        /// <param name="EmergencyTypeId">The Id of EmergencyType by which EmergencyType should be Added / Updated</param>
        /// <param name="Emergency">The name of EmergencyType by which EmergencyType should be Added / Updated</param>
        /// <returns></returns>
        public static int AddUpdateEmergencyTypes(int EmergencyTypeId, string Emergency)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pEmergency_Type_ID", SqlDbType.Int, 0, EmergencyTypeId));
                    prams.Add(db.MakeInParam("@pEmergency_Type", SqlDbType.NVarChar, 0, Emergency));
                    int exec = db.RunProc("UP_Emergency_Types_InsertUpdate", prams.ToArray());
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
        /// Delete Emergency by Emergency id
        /// </summary>
        /// <param name="EmergencyID">The id of Emergency by which Emergency should be deleted</param>
        /// <returns></returns>
        public static int deleteEmergency(int EmergencyID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pEmergencyId", SqlDbType.Int, 4, EmergencyID));
                    int exec = db.RunProc("up_deleteEmergency", prams.ToArray());
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
