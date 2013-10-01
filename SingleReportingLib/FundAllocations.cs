using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for FundAllocations
    /// </summary>
    public class FundAllocations
    {
        
        private int _ProjectID;
        private int _service_provider_ID;        
        private double _TotalFundsAllocation;
        private string _funding_Year;
        private double _Amount_Q1;
        private double _Amount_Q2;
        private double _Amount_Q3;
        private double _Amount_Q4;
        private double _Total;
        private int _Fund_Allocation_ID;
        private bool isActive;
        
            
        public int Service_Provider_ID
        {
            get { return _service_provider_ID; }
            set { _service_provider_ID = value; }
        }
        public string Funding_Year 
        { get { return _funding_Year; } set { _funding_Year = value; } 
        }
        public int Fund_Allocation_ID { get { return _Fund_Allocation_ID; } set { _Fund_Allocation_ID = value; } }
        public int ProjectID { get { return _ProjectID; } set { _ProjectID = value; } }
        public double TotalFundsAllocation { get { return _TotalFundsAllocation; } set { _TotalFundsAllocation = value; } }
        public double Amount_Q1 { get { return _Amount_Q1; } set { _Amount_Q1 = value; } }
        public double Amount_Q2 { get { return _Amount_Q2; } set { _Amount_Q2 = value; } }
        public double Amount_Q3 { get { return _Amount_Q3; } set { _Amount_Q3 = value; } }
        public double Amount_Q4 { get { return _Amount_Q4; } set { _Amount_Q4 = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public double Total { get { return _Total; } set { _Total=value; } }


        /// <summary>
        /// Insert new FundAllocations
        /// </summary>
        /// <param name="f">The Object of class FundAllocations which should be inserted</param>
        /// <returns></returns>
        public static int insertFundAllocations(FundAllocations f)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@FK_Service_Provider", SqlDbType.Int, 4, f.Service_Provider_ID));
                    prams.Add(db.MakeInParam("@FK_ProjectID", SqlDbType.Int, 4, f.ProjectID));
                    prams.Add(db.MakeInParam("@Funding_Year", SqlDbType.VarChar, 50, f.Funding_Year));
                    prams.Add(db.MakeInParam("@Amount_Q1", SqlDbType.Money, 0, f.Amount_Q1));
                    prams.Add(db.MakeInParam("@Amount_Q2", SqlDbType.Money, 0, f.Amount_Q2));
                    prams.Add(db.MakeInParam("@Amount_Q3", SqlDbType.Money, 0, f.Amount_Q3));
                    prams.Add(db.MakeInParam("@Amount_Q4", SqlDbType.Money, 0, f.Amount_Q4));
                    prams.Add(db.MakeInParam("@TotalFundsAllocation", SqlDbType.Money, 0, f.TotalFundsAllocation));
                    prams.Add(db.MakeInParam("@Total", SqlDbType.Money, 0, f.Total));

                    int exec = db.RunProc("UP_FundAllocation_Insert", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update FundAllocations by FundAllocations Class Object
        /// </summary>
        /// <param name="f">The Object of class FundAllocations which should be Updated</param>
        /// <returns></returns>
        public static int UpdateFundAllocations(FundAllocations f)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@Fund_Allocation_ID", SqlDbType.Int, 4, f.Fund_Allocation_ID));
                    prams.Add(db.MakeInParam("@FK_Service_Provider", SqlDbType.Int, 4, f.Service_Provider_ID));
                    prams.Add(db.MakeInParam("@FK_ProjectID", SqlDbType.Int, 4, f.ProjectID));
                    prams.Add(db.MakeInParam("@Funding_Year", SqlDbType.VarChar, 50, f.Funding_Year));
                    prams.Add(db.MakeInParam("@Amount_Q1", SqlDbType.Money, 0, f.Amount_Q1));
                    prams.Add(db.MakeInParam("@Amount_Q2", SqlDbType.Money, 0, f.Amount_Q2));
                    prams.Add(db.MakeInParam("@Amount_Q3", SqlDbType.Money, 0, f.Amount_Q3));
                    prams.Add(db.MakeInParam("@Amount_Q4", SqlDbType.Money, 0, f.Amount_Q4));
                    prams.Add(db.MakeInParam("@TotalFundsAllocation", SqlDbType.Money, 0, f.TotalFundsAllocation));
                    prams.Add(db.MakeInParam("@Total", SqlDbType.Money, 0, f.Total));

                    int exec = db.RunProc("UP_FundAllocation_Update", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get List of All Funding Years by Project Id
        /// </summary>
        /// <param name="ProjectId">The ProjectID by which Funding Years List should be get</param>
        /// <param name="serviceProviderId">The serviceProviderId by which Funding Years List should be get</param>
        /// <returns></returns>
        public static List<string> getAllFunddingYearsByProjectId(int ProjectId,int serviceProviderId) 
        {

            List<string> lst = new List<string>();
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, ProjectId);
                prams[1] = db.MakeInParam("@FK_Service_Provider", SqlDbType.Int, 4, serviceProviderId);
                dt = db.GetDataSet("UP_FundAllocations_GetFundingYearsByProjectID", prams).Tables[0];
            }
            if (dt != null) 
            {
                if (dt.Rows.Count > 0) 
                {

                    foreach (DataRow dr in dt.Rows) 
                    {
                        //ListItem li = new ListItem(dr["Funding_Year"].ToString(), dr["Funding_Year"].ToString());
                        lst.Add(dr["Funding_Year"].ToString());
                    
                    }
                
                }
            
            }
            return lst;
        
        }

        /// <summary>
        /// Delete Funding by Funding ID
        /// </summary>
        /// <param name="fundingId">Id of FundingAllocations by which FundingAllocation should be deleted</param>
        /// <returns>True for successful delete / False when exception cought</returns>
        public static bool DeleteFunding(int fundingId) 
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@Fund_Allocation_ID", SqlDbType.Int, 4, fundingId));
                    int exec = db.RunProc("UP_FundAllocation_Delete", prams.ToArray());
                    if (exec > 0) 
                    {

                        return true;
                    }
                }
            }
            catch
            {
                throw;
                
            }
            return false;
        
        }
        /// <summary>
        /// Get DataTable of FundAllocation By Project id
        /// </summary>
        /// <param name="projectID">The id of FundAllocation by which FundAllocation should get</param>
        /// <returns></returns>
        public static DataTable getFundAllocationByProjectID(int projectID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                dt = db.GetDataSet("UP_FundAllocations_GetByProjectID", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Get The Sum of Total FundsAllocation By Project Id
        /// </summary>
        /// <param name="projectID">The id of fundsAllocations by which toatal sum should be get</param>
        /// <returns>Sum of Total FundsAllocatin</returns>
        public static double getTotalFundsAllocation_ByProjectID(int projectID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);               

                dt = db.GetDataSet("UP_Funding_GetTotalFundAllocation_ByProjectID", prams).Tables[0];

                double sum = Double.Parse(dt.Rows[0][0].ToString());
                return sum;
            }
        }

    }
}
