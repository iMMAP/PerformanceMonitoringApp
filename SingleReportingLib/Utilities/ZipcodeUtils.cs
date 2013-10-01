using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReporting.Utilities
{
    public class ZipcodeUtils
    {
        public static string getZipcodeBoundries(string zipCode)
        {
            string res = string.Empty;
            SqlParameter[] prams = new SqlParameter[1];

            using (DbManager db = DbManager.GetDbManager())
            {
                try
                {
                    prams[0] = db.MakeInParam("@chrZipcode", SqlDbType.Char, 10, zipCode.Trim());
                    DataSet ds = db.GetDataSet("up_getZipcodeBoundryData", prams);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        res = ds.Tables[0].Rows[0]["result"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return res;
        }
    }

}
