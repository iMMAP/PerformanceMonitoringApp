using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Service Providers
    /// </summary>
    public enum ServiceProviderStatus
    {

        Approved,
        Rejected,
        PendingAproval,
        Nil,
        Draft

    }

    public class ServiceProvider
    {

        public ServiceProvider() { }
        /// <summary>
        /// This Update Reporting Status 
        /// </summary>
        /// <param name="FreqId"></param>
        /// <param name="sp_targetid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateStatus(int FreqId, int sp_targetid, ServiceProviderStatus status)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[3];

                    prams[0] = db.MakeInParam("@freqId", SqlDbType.Int, 0, FreqId);
                    prams[1] = db.MakeInParam("@sp_target_id", SqlDbType.Int, 0, sp_targetid);
                    prams[2] = db.MakeInParam("@status", SqlDbType.VarChar, 100, status);

                    db.RunProc("up_updateReportingStatus", prams);
                    return true;


                }
            }
            catch (Exception) { return false; }

        }

        /// <summary>
        /// This Submits report with zero value
        /// </summary>
        /// <param name="FreqId"></param>
        /// <param name="sp_targetid"></param>
        /// <returns></returns>
        public static bool SubmitNilReport(int FreqId, int sp_targetid)
        {
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[2];

                    prams[0] = db.MakeInParam("@ReportingFreqId", SqlDbType.Int, 0, FreqId);
                    prams[1] = db.MakeInParam("@sp_target_id", SqlDbType.Int, 0, sp_targetid);

                    db.RunProc("up_SubmitNillReport", prams);
                    return true;


                }
            }
            catch (Exception) { return false; }

        }

    /// <summary>
    /// Get String of Emails by Service Provider Id
    /// </summary>
    /// <param name="sp_targetid">Id of Service Provider whose data to be fetched</param>
    /// <returns>String of comma(,) separated Emails</returns>
        public static string getcommaseperatedEmailsbySpId(int sp_targetid)
        {
            DataSet ds = null;
            string s = string.Empty;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {

                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@sp_target_id", SqlDbType.Int, 0, sp_targetid);

                    ds = db.GetDataSet("up_getIPEmailByServiceProviderId", prams);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            s += clsCommon.ParseString(dr["vchEmail"]) + ";";

                        }

                    }


                }
            }
            catch (Exception) { return s; }
            return s;
        }
    }
}
