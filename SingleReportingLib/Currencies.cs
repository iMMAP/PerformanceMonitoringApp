using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Currencies
    /// </summary>
    public class Currencies
    {
        public Currencies()
        {

        }
        /// <summary>
        /// Get DataTable of All Currencies
        /// </summary>
        /// <returns>DataTable of All Currencies</returns>
        public static DataTable getAllCurrencies()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllCurrencies", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Add new and Update Currency
        /// </summary>
        /// <param name="CurrencyId">The id of Currency which should be Added / Updated</param>
        /// <param name="CurrencyTitle">The Title of Currency which should be Added / Updated</param>
        /// <param name="CurrencyCountry">The Country of Currency which should be Added / Updated</param>
        /// <param name="CurrencyAcronym">The Acronym of Currency which should be Added / Updated</param>
        /// <returns></returns>
        public static int AddUpdateCurrency(int CurrencyId, string CurrencyTitle,string CurrencyCountry,string CurrencyAcronym)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pCurrency_ID", SqlDbType.Int, 0, CurrencyId));
                    prams.Add(db.MakeInParam("@pCurrency_Title", SqlDbType.NVarChar, 50, CurrencyTitle));
                    prams.Add(db.MakeInParam("@pCurrency_Country", SqlDbType.NVarChar, 50, CurrencyCountry));
                    prams.Add(db.MakeInParam("@pCurrency_Acronym", SqlDbType.NVarChar, 50, CurrencyAcronym));
                    int exec = db.RunProc("UP_Currency_InsertUpdate", prams.ToArray());
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
        /// Delete Currency
        /// </summary>
        /// <param name="CurrencyID">Id of a Currency which should be deleted</param>
        /// <returns></returns>
        public static int deleteCurrency(int CurrencyID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pCurrencyId", SqlDbType.Int, 4, CurrencyID));
                    int exec = db.RunProc("up_deleteCurrency", prams.ToArray());
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
