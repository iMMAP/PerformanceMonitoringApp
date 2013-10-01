using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Project Funding
    /// </summary>
    public class ProjectFunding
    {
        private int _funding_ID;
        private int _project_ID;        
        private int _donor_ID;
        private int _funding_type_ID;
        private int _currency_ID;
        private double _funding_Amount;
        private DateTime _funding_Date;
        private string _info_Source;

        public int Funding_ID
        {
            get { return _funding_ID ; }
            set { _funding_ID = value; }
        }
      
        public int Project_ID
        {
            get { return _project_ID; }
            set { _project_ID = value; }
        }
      
        public int Donor_ID
        {
            get { return _donor_ID ; }
            set { _donor_ID = value; }
        }
    
        public int  FundingTYPE
        {
            get { return _funding_type_ID; }
            set { _funding_type_ID = value; }
        }
    
        public int Currency_ID
        {
            get { return _currency_ID; }
            set { _currency_ID = value; }
        }

        public double Funding_Amount
        {
            get { return _funding_Amount ; }
            set { _funding_Amount = value; }
        }

        public DateTime Funding_Date { get { return _funding_Date; } set { _funding_Date = value; } }

   
        public string Source
        {
            get { return _info_Source ; }
            set { _info_Source = value; }
        }

        /// <summary>
        /// Insert new Project Funding 
        /// </summary>
        /// <param name="p">The name of ProjectFunding Class object which should be inserted</param>
        /// <returns></returns>
        public static int insertProjectFunding(ProjectFunding p)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@Project_ID", SqlDbType.Int, 0, p.Project_ID));
                    prams.Add(db.MakeInParam("@Donor_ID", SqlDbType.Int , 0, p.Donor_ID));
                    prams.Add(db.MakeInParam("@FundingType_ID", SqlDbType.Int, 0, p.FundingTYPE));
                    prams.Add(db.MakeInParam("@Currency_ID", SqlDbType.Int, 0, p.Currency_ID));
                    prams.Add(db.MakeInParam("@FundingAmount", SqlDbType.Money , 0, p.Funding_Amount));
                    prams.Add(db.MakeInParam("@FundingDate", SqlDbType.DateTime , 0, p.Funding_Date));
                    prams.Add(db.MakeInParam("@Source", SqlDbType.NVarChar , 500, p.Source));
                     int exec = db.RunProc("up_InsertFunding", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
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
        /// Get DataTable of ProjectFunding by Projectid And FundingType id
        /// </summary>
        /// <param name="projectID">Project id whose ProjectFundig to be fetched</param>
        /// <param name="fundingType">ProjectType id whose ProjecFunding to be fetched</param>
        /// <returns>DataTable of ProjectFunding</returns>
        public static DataTable getProjectFundingByProjectIDandFundingType(int projectID, int fundingType)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@FK_Funding_Type", SqlDbType.Int, 4, fundingType);


                dt = db.GetDataSet("UP_Funding_GetByProjectIDAndFundingType", prams).Tables[0];
            }

            return dt;
        }
        /// <summary>
        /// Delete ProjectFunding By ProjectFunding id
        /// </summary>
        /// <param name="projectFundingID">ProjectFunding id whose data should be deleted</param>
        /// <returns></returns>
        public static int deleteProjectFunding(int projectFundingID)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@Project_Funding_ID", SqlDbType.Int, 4, projectFundingID));
                    int exec = db.RunProc("up_Funding_DeleteByID", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }


       
    /// <summary>
    /// Get total Pledges by Project ID
    /// </summary>
    /// <param name="projectID">Project id whose data to be fetched</param>
    /// <returns>sum of Pledges</returns>
        public static double getPledgesTotal_ByProjectID(int projectID)
        {
            FundingType fType = new FundingType("Pledges");

            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@FK_Funding_Type", SqlDbType.Int, 4, fType.Funding_Type_ID);
                dt = db.GetDataSet("UP_Funding_GetTotalFunds_ByProject_ByFundType", prams).Tables[0];

                string value = dt.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    double sum = Double.Parse(value);
                    return sum;
                }                
            }

            return 0;
        }


        /// <summary>
        /// Get total Committment by Project ID
        /// </summary>
        /// <param name="projectID">Project id whose data to be fetched</param>
        /// <returns>sum of Pledges</returns>
        public static double getCommittmentsTotal_ByProjectID(int projectID)
        {
            
            FundingType fType = new FundingType("Committment");
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@FK_Funding_Type", SqlDbType.Int, 4, fType.Funding_Type_ID);
                dt = db.GetDataSet("UP_Funding_GetTotalFunds_ByProject_ByFundType", prams).Tables[0];

                string value = dt.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    double sum = Double.Parse(value);
                    return sum;
                }  
            }

            return 0;
        }

        /// <summary>
        /// Get total Disbursement by Project ID
        /// </summary>
        /// <param name="projectID">Project id whose data to be fetched</param>
        /// <returns>sum of Pledges</returns>
        public static double getDisbursementsTotal_ByProjectID(int projectID)
        {

            FundingType fType = new FundingType("Disbursement");
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);
                prams[1] = db.MakeInParam("@FK_Funding_Type", SqlDbType.Int, 4, fType.Funding_Type_ID);

                

                dt = db.GetDataSet("UP_Funding_GetTotalFunds_ByProject_ByFundType", prams).Tables[0];

                string value = dt.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    double sum = Double.Parse(value);
                    return sum;
                }  
            }

            return 0;
        }
        //public static int insertGovtDpt(int dptId,int locationId,string dptName)
        //{
        //    List<SqlParameter> prams = new List<SqlParameter>();
        //    try
        //    {
        //        using (DbManager db = DbManager.GetDbManager())
        //        {
        //            prams.Add(db.MakeInParam("@pProject_Status_ID", SqlDbType.Int, 0, ProjectStatusId));
        //            prams.Add(db.MakeInParam("@pProject_Status", SqlDbType.NVarChar, 0, ProjectStatus));
        //            int exec = db.RunProc("UP_Project_Status_InsertUpdate", prams.ToArray());
        //            return exec;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    // return 0;

        //}

    }
}
