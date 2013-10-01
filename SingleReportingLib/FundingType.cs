using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details of FundingType
    /// </summary>
    public class FundingType
    {
        private int _Funding_Type_ID;
        private string _Funding_Type;

        public int Funding_Type_ID { get { return _Funding_Type_ID; } set { _Funding_Type_ID = value; } }
        public string Funding_Type { get { return _Funding_Type; } set { _Funding_Type = value; } }
        /// <summary>
        /// Constructor with one argument FundingTypeID
        /// IT Loads FundingTypes by id
        /// </summary>
        /// <param name="fundingTypeID"></param>
        public FundingType(int fundingTypeID)
        {
            loadFundingType(fundingTypeID);
        }

        /// <summary>
        /// Constructor with one argument fundingType
        /// IT Loads FundingTypes by funding type name
        /// </summary>
        /// <param name="fundingTypeID"></param>
        public FundingType(string fundingType)
        {
            loadFundingType(fundingType);
        }

        /// <summary>
        /// Load Funding Type by FundingType Id
        /// </summary>
        /// <param name="fundingTypeID">The id of fundingType by which it should be loaded</param>
        private void loadFundingType(int fundingTypeID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@Funding_Type_ID", SqlDbType.Int, 4, fundingTypeID);
                    reader = db.GetDataReader("UP_FundingType_GetByID", prams);
                    if (reader.Read())
                        LoadFundingTypeInfo(reader);
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

        /// <summary>
        /// Load Funding Type by FundingType name
        /// </summary>
        /// <param name="fundingType">The name of fundingType by which it should be loaded</param>
        private void loadFundingType(string fundingType)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@Funding_Type", SqlDbType.NVarChar, 50, fundingType);
                    reader = db.GetDataReader("UP_FundingType_GetByName", prams);
                    if (reader.Read())
                        LoadFundingTypeInfo(reader);
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

       
        private void LoadFundingTypeInfo(IDataReader reader)
        {
            _Funding_Type = reader["Funding_Type"] == System.DBNull.Value ? "" : reader["Funding_Type"].ToString();
            _Funding_Type_ID = reader["Funding_Type_ID"] == System.DBNull.Value ? 0 : Int32.Parse(reader["Funding_Type_ID"].ToString());
        }

        /// <summary>
        /// Insert new Funding Type by FundingType id and FundigType name
        /// </summary>
        /// <param name="FundingTypeId">The id of fundingType which should be added</param>
        /// <param name="FunidngType">The name of fundingType which should be added</param>
        /// <returns></returns>
        public static int InsertFundingTypes(int FundingTypeId, string FunidngType)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {//@pFunding_Type_ID int,@pFunding_Type
                    prams.Add(db.MakeInParam("@pFunding_Type_ID", SqlDbType.Int, 0, FundingTypeId));
                    prams.Add(db.MakeInParam("@pFunding_Type", SqlDbType.NVarChar, 0, FunidngType));
                    int exec = db.RunProc("UP_Funding_Types_InsertUpdate", prams.ToArray());
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
}
