using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{

    /// <summary>
    /// Get Details for Donors
    /// </summary>
    public class Donors
    {
        private int _Donor_ID;
        private string _Donor_Name;
        private string _Donor_Country;

        public int Donor_ID { get { return _Donor_ID; } set { _Donor_ID = value; } }
        public string Donor_Name { get { return _Donor_Name; } set { _Donor_Name = value; } }
        public string Donor_Country { get { return _Donor_Country; } set { _Donor_Country = value; } }
        
        public Donors(int donorID)
        {
            loadDonors(donorID);
        }


        /// <summary>
        /// Load Donors by Donor ID
        /// </summary>
        /// <param name="donorID">The id of a donor by which donors should be laoded</param>
        private void loadDonors(int donorID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@Funding_Type_ID", SqlDbType.Int, 4, donorID);
                    reader = db.GetDataReader("UP_FundingType_GetByID", prams);
                    if (reader.Read())
                        LoadDonors(reader);
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
        
        private void LoadDonors(IDataReader reader)
        {
            _Donor_ID = reader["Donor_ID"] == System.DBNull.Value ? 0 : Int32.Parse(reader["Donor_ID"].ToString());
            _Donor_Name = reader["Donor_Name"] == System.DBNull.Value ? "" : reader["Donor_Name"].ToString();
            _Donor_Country = reader["Donor_Country"] == System.DBNull.Value ? "" : reader["Donor_Country"].ToString();
        }
        /// <summary>
        /// Get DataTable of All Donors
        /// </summary>
        /// <returns>DataTable of Donors</returns>
        public static DataTable getAllDonors()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllDonors", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Add / Update Donor by Donor id, Donor Name and Donor Country
        /// </summary>
        /// <param name="iDonorId">The id of Donor which should be Added / Updated</param>
        /// <param name="sDonorName">The Name of Donor which should be Added / Updated</param>
        /// <param name="sDonorCountry">The Country of Donor which should be Added / Updated</param>
        /// <returns></returns>
        public static int AddUpdateDonor(int iDonorId,string sDonorName, string sDonorCountry)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[3];
                    prams[0] = db.MakeInParam("@pDonor_ID", SqlDbType.Int, 0, iDonorId);
                    prams[1] = db.MakeInParam("@pDonor_Name", SqlDbType.NVarChar, 500, sDonorName);
                    prams[2] = db.MakeInParam("@pDonor_Country", SqlDbType.NVarChar, 50, sDonorCountry);
                    return db.RunProc("UP_Donor_Update", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }
        /// <summary>
        /// Get DataSet of Donor By Donor ID
        /// </summary>
        /// <param name="DonorId">ID by which DataSet of Donor should be get</param>
        /// <returns>DataSet of Donor</returns>
        public static DataSet LoadDonorById(int DonorId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@DonorId", SqlDbType.Int, 4, DonorId);
                    ds = db.GetDataSet("up_loadDonorById", prams);

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
        /// Delet Donor by donor id
        /// </summary>
        /// <param name="DonorId">The id of a donor by which donor should be deleted</param>
        /// <returns></returns>
        public static int deleteDonor(int DonorId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pDonorId", SqlDbType.Int, 4, DonorId));
                    int exec = db.RunProc("up_deleteDonor", prams.ToArray());
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
