using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;

namespace SingleReportingLib
{
    public class Projects
    {


        private int _project_ID;
        private string _project_Code;
        private string _project_Title;
        private int _sectorId;
        private int _project_StatusId;
        private int _project_AppealId;
        private int _project_OrganizationId;
        private int _project_TypeId;
        private int _project_Cost_CurrencyId;
        private DateTime _project_Start_Date;
        private DateTime _project_End_Date;
        private string _project_Contact_Name;
        private string _project_Contact_phone;
        private string _project_Contact_Email;
        private string _project_GoP_Budgetary_System_Used;
        private string _project_GoP_procurement_System_Used;
        private string _answer;
        private int _owner_ID;
        private double _project_Cost;

        public int Project_ID
        {
            get { return _project_ID; }
            set { _project_ID = value; }
        }
        public int Owner_ID
        {
            get { return _owner_ID; }
            set { _owner_ID = value; }
        }
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
        public string Project_Code
        {
            get { return _project_Code; }
            set { _project_Code = value; }
        }
        public string Project_Title
        {
            get { return _project_Title; }
            set { _project_Title = value; }
        }
        public int SectorId
        {
            get { return _sectorId; }
            set { _sectorId = value; }
        }
        public int Project_StatusId
        {
            get { return _project_StatusId; }
            set { _project_StatusId = value; }
        }
        public int Project_AppealId
        {
            get { return _project_AppealId; }
            set { _project_AppealId = value; }
        }
        public int Project_OrganizationId
        {
            get { return _project_OrganizationId; }
            set { _project_OrganizationId = value; }
        }
        public int Project_TypeId
        {
            get { return _project_TypeId; }
            set { _project_TypeId = value; }
        }
        public int Project_Cost_CurrencyId
        {
            get { return _project_Cost_CurrencyId; }
            set { _project_Cost_CurrencyId = value; }
        }
        public DateTime Project_Start_Date
        {
            get { return _project_Start_Date; }
            set { _project_Start_Date = value; }
        }
        public DateTime Project_End_Date
        {
            get { return _project_End_Date; }
            set { _project_End_Date = value; }
        }
        public string Project_Contact_Name
        {
            get { return _project_Contact_Name; }
            set { _project_Contact_Name = value; }
        }
        public string Project_Contact_Phone
        {
            get { return _project_Contact_phone; }
            set { _project_Contact_phone = value; }
        }
        public string Project_Contact_Email
        {
            get { return _project_Contact_Email; }
            set { _project_Contact_Email = value; }
        }
        public string Project_GoP_Budgetary_System_Used
        {
            get { return _project_GoP_Budgetary_System_Used; }
            set { _project_GoP_Budgetary_System_Used = value; }
        }
        public string Project_GoP_Procurement_System_Used
        {
            get { return _project_GoP_procurement_System_Used; }
            set { _project_GoP_procurement_System_Used = value; }
        }
        public double Project_Cost
        {
            get { return _project_Cost; }
            set { _project_Cost = value; }
        }
        public Projects()
        {

        }

        public Projects(int projectId)
        {
            loadProjects(projectId);
        }

        private void loadProjects(int projectId)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, projectId);
                    reader = db.GetDataReader("up_getProjectsById", prams);
                    if (reader.Read())
                        LoadProjectsInfo(reader);
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
      
        private void LoadProjectsInfo(IDataReader reader)
        {

            _project_ID = reader["Project_ID"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Project_ID"]);
            //_project_ID = reader["Ower"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Project_ID"]);
            _project_Code = reader["Project_Code"].ToString();
            _project_Title = reader["Project_Title"].ToString();
            _sectorId = reader["FK_Sector"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Sector"]);
            _project_StatusId = reader["FK_Project_Status"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Project_Status"]);
            _project_AppealId = reader["FK_Project_Appeal"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Project_Appeal"]);
            _project_OrganizationId = reader["FK_Project_Organization"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Project_Organization"]);
            _project_TypeId = reader["FK_Project_Type"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Project_Cost_Currency"]);
            _project_Cost_CurrencyId = reader["FK_Project_Cost_Currency"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["FK_Project_Cost_Currency"]);
            _project_Start_Date = Convert.ToDateTime(reader["Project_Start_Date"]);
            _project_End_Date = Convert.ToDateTime(reader["Project_End_Date"]);
            _project_Contact_Name = reader["Project_Contact_Name"].ToString();
            _project_Contact_phone = reader["Project_Contact_Phone"].ToString();
            _project_Contact_Email = reader["Project_Contact_Email"].ToString();
            _project_GoP_Budgetary_System_Used = reader["Project_GoP_Budgetary_System_Used"].ToString();
            _project_GoP_procurement_System_Used = reader["Project_GoP_Procurement_System_Used"].ToString();
            _project_Cost = reader["Project_Cost"] == System.DBNull.Value ? 0 : Convert.ToInt32(reader["Project_Cost"]);
            _answer = reader["Answer"].ToString();

        }

        /// <summary>
        /// Insert / Update Projects with object of class Projects
        /// </summary>
        /// <param name="p">The object of class Projects which has to be inserted / updated</param>
        /// <returns></returns>
        public static int insertUpdate(Projects p)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {

                    prams.Add(db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, p.Project_ID));
                    prams.Add(db.MakeInParam("@pOwnerId", SqlDbType.Int, 0, p.Owner_ID));
                    prams.Add(db.MakeInParam("@pProject_Code", SqlDbType.NVarChar, 0, p.Project_Code));
                    prams.Add(db.MakeInParam("@pProject_Title", SqlDbType.NVarChar, 0, p.Project_Title));
                    prams.Add(db.MakeInParam("@pFK_Sector", SqlDbType.Int, 0, p.SectorId));
                    prams.Add(db.MakeInParam("@pFK_Project_Status", SqlDbType.Int, 0, p.Project_StatusId));
                    if (p.Project_AppealId <= 0)
                    {
                        prams.Add(db.MakeInParam("@pFK_Project_Appeal", SqlDbType.Int, 0, DBNull.Value));

                    }
                    else
                    {
                        prams.Add(db.MakeInParam("@pFK_Project_Appeal", SqlDbType.Int, 0, clsCommon.ParseDBNullInt(p.Project_AppealId)));
                    }
                    prams.Add(db.MakeInParam("@pFK_Project_Organization", SqlDbType.Int, 0, p.Project_OrganizationId));
                    prams.Add(db.MakeInParam("@pFK_Project_Type", SqlDbType.Int, 0, p.Project_TypeId));
                    prams.Add(db.MakeInParam("@pFK_Project_Cost_Currency", SqlDbType.Int, 0, p.Project_Cost_CurrencyId));
                    prams.Add(db.MakeInParam("@pProject_Start_Date", SqlDbType.DateTime, 0, p.Project_Start_Date));
                    prams.Add(db.MakeInParam("@pProject_End_Date", SqlDbType.DateTime, 0, p.Project_End_Date));
                    prams.Add(db.MakeInParam("@pProject_Contact_Name", SqlDbType.NVarChar, 0, p.Project_Contact_Name));
                    prams.Add(db.MakeInParam("@pProject_Contact_Phone", SqlDbType.NVarChar, 0, p.Project_Contact_Phone));
                    prams.Add(db.MakeInParam("@pProject_Contact_Email", SqlDbType.NVarChar, 0, p.Project_Contact_Email));
                    prams.Add(db.MakeInParam("@pProject_GoP_Budgetary_System_Used", SqlDbType.NVarChar, 3, p.Project_GoP_Budgetary_System_Used));
                    prams.Add(db.MakeInParam("@pProject_GoP_Procurement_System_Used", SqlDbType.NVarChar, 3, p.Project_GoP_Procurement_System_Used));
                    prams.Add(db.MakeInParam("@pProject_Cost", SqlDbType.Int, 0, p.Project_Cost));
                    prams.Add(db.MakeInParam("@pAnswer", SqlDbType.NVarChar, 0, p.Answer));
                    int exec = db.RunProc("UP_Projects_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }


        //public static void MakeActive(bool active, int projectId)
        //{
        //    List<SqlParameter> prams = new List<SqlParameter>();
        //    try
        //    {
        //        using (DbManager db = DbManager.GetDbManager())
        //        {
        //            prams.Add(db.MakeInParam("@intProjectsId", SqlDbType.Int, 0, projectId));
        //            prams.Add(db.MakeInParam("@bitActive", SqlDbType.Bit, 0, active));
        //            db.RunProc("up_property_Projects_MakeActive", prams.ToArray());
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get DataTable of Dashboard Groups with groupType
        /// </summary>
        /// <param name="groupType">The groupType whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataTable getDashboardGroups(string groupType)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@GroupType", SqlDbType.NVarChar, 50, groupType);
                dt = db.GetDataSet("WEB_SP_DashboardGroups_For_IPDashboard", prams).Tables[0];
            }
            return dt;
        }

        /// <summary>
        /// Get DataTable of Projects with Group Type and Group ID
        /// </summary>
        /// <param name="groupType">The GroupType whose data needs to be fetched</param>
        /// <param name="groupID">The Group id whose data needs to be fetched</param>
        /// <returns>DataTable of Projects</returns>
        public static DataTable getProjectsByGroupTypeAndID(string groupType, int groupID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[2];
                prams[0] = db.MakeInParam("@GroupID", SqlDbType.Int, 4, groupID);
                prams[1] = db.MakeInParam("@GroupType", SqlDbType.NVarChar, 50, groupType);

                dt = db.GetDataSet("WEB_SP_GroupWise_Projects", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        ///  Get DataTable of Projects with Group Type , Group ID and Owner Id
        /// </summary>
        /// <param name="groupType">he GroupType whose data needs to be fetched</param>
        /// <param name="groupID">The Group id whose data needs to be fetched</param>
        /// <param name="OwnerId">The Owner id whose data needs to be fetched</param>
        /// <returns>DataTable of Projects</returns>
        public static DataTable getProjectsByGroupTypeAndID(string groupType, int groupID, int OwnerId)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[3];
                prams[0] = db.MakeInParam("@GroupID", SqlDbType.Int, 4, groupID);
                prams[1] = db.MakeInParam("@GroupType", SqlDbType.NVarChar, 50, groupType);
                prams[2] = db.MakeInParam("@OwnerId", SqlDbType.Int, 9, OwnerId);

                dt = db.GetDataSet("WEB_SP_GroupWise_Projects_Owner", prams).Tables[0];
            }

            return dt;
        }

        public static DataTable getProjectsByGroupTypeAndIDByIP(string groupType, int groupID, int OwnerId)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[3];
                prams[0] = db.MakeInParam("@GroupID", SqlDbType.Int, 4, groupID);
                prams[1] = db.MakeInParam("@GroupType", SqlDbType.NVarChar, 50, groupType);
                prams[2] = db.MakeInParam("@OwnerId", SqlDbType.Int, 9, OwnerId);

                dt = db.GetDataSet("WEB_SP_GroupWise_Projects_IP", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Get Data Table of Cluster Lead Projects with GroupId, Owner id and Group Type
        /// </summary>
        /// <param name="GroupID">The Group id whose data needs to be fetched</param>
        /// <param name="OwnerId">The Owner id whose data needs to be fetched</param>
        /// <param name="GroupType">The Group Type whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataTable getClusterLeadProjects(int GroupID, int OwnerId, string GroupType)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[3];
                prams[0] = db.MakeInParam("@OwnerId", SqlDbType.Int, 9, OwnerId);
                prams[1] = db.MakeInParam("@GroupID", SqlDbType.Int, 9, GroupID);
                prams[2] = db.MakeInParam("@GroupType", SqlDbType.NVarChar, 50, GroupType);



                DataTableCollection tables = db.GetDataSet("SP_ClusterLead_Projects", prams).Tables;
                DataTable clusterProjects = null;
                if (tables != null && tables.Count > 0)
                {
                    clusterProjects = tables[0];
                }

                dt = clusterProjects;
            }

            return dt;
        }

        /// <summary>
        /// Get DataTable of Project Details with Project Id
        /// </summary>
        /// <param name="projectID">Project Id whose data needs to be fetched</param>
        /// <returns>DataTable of Project Details</returns>
        public static DataTable getProjectDetailsByProjectID(int projectID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);


                dt = db.GetDataSet("Web_SP_GetProjectDetails", prams).Tables[0];
            }

            return dt;
        }


        /// <summary>
        /// Get DataSet of All Project Types
        /// </summary>
        /// <returns> DataSet of Project Types</returns>
        public static DataSet getAllProjectTpyes()
        {
            DataSet ds = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                ds = db.GetDataSet("up_getAllProjectTypes", null);
            }
            return ds;
        }

        /// <summary>
        /// This Get the DataSet of All Project Statuses
        /// </summary>
        /// <returns>DataSet of Project Statuses</returns>
        public static DataSet getAllProjectStatus()
        {
            DataSet ds = null;
            using (DbManager db = DbManager.GetDbManager())
            {
                ds = db.GetDataSet("up_getAllProjectStatus", null);
            }
            return ds;
        }


        #region getReportingFrequencyBySectorId(int sectorId)
        /// <summary>
        /// This Get String of Reporting Frequency with Sector Id
        /// </summary>
        /// <param name="sectorId">Sector id whose record needs to be fetched</param>
        /// <returns></returns>
        public static string getReportingFrequencyBySectorId(int sectorId) 
        {

            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@sectorId", SqlDbType.Int, 4, sectorId);


                dt = db.GetDataSet("up_getReportingFrequencyBySubsectorId", prams).Tables[0];
                if (dt != null) 
                {

                    if (dt.Rows.Count > 0) 
                    {
                        return dt.Rows[0]["Name"].ToString();
                    
                    }
                }
            }

            return "";
        }
        #endregion


        /// <summary>
        /// Get DataTable of Service Providers with Project iD
        /// </summary>
        /// <param name="projectID">The Project Id whose data needs to be fetched</param>
        /// <returns>DataSet of All Project Statuses</returns>
        public static DataTable getProjectServiceProviders(int projectID)
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[1];
                prams[0] = db.MakeInParam("@ProjectID", SqlDbType.Int, 4, projectID);


                dt = db.GetDataSet("UP_ServiceProvider_GetByProjectID", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// This Adds new Sub Sector with SubSector id, Sector Id, SubSector Description, Subsector Name
        /// </summary>
        /// <param name="SubSector_ID">The SubSector Id which has to be Added</param>
        /// <param name="Sector_ID">The Sector Id which has to be Added</param>
        /// <param name="SubSector_Description">The SubSector Description which has to be Added</param>
        /// <param name="SubSector_Name">The SubSector Name which has to be Added</param>
        /// <returns></returns>
        public static int AddSubSector(int SubSector_ID, int Sector_ID, string SubSector_Description, string SubSector_Name)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[5];
                    prams[0] = db.MakeInParam("@pSubSector_ID", SqlDbType.Int, 0, SubSector_ID);
                    prams[1] = db.MakeInParam("@pSubSector_Name", SqlDbType.NVarChar, 50, SubSector_Name);
                    prams[2] = db.MakeInParam("@pSubSector_Description", SqlDbType.NVarChar, 500, SubSector_Description);
                    prams[3] = db.MakeInParam("@pSubSector_Icon", SqlDbType.NVarChar, 4000, null);
                    prams[4] = db.MakeInParam("@pSector_ID", SqlDbType.Int, 0, Sector_ID);
                    return db.RunProc("UP_SubSectors_InsertUpdate", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }
        
        /// <summary>
        /// This Loads DataSet of Sector wich Sector id
        /// </summary>
        /// <param name="SectorId">The Sector id whose record needs to be fetched</param>
        /// <returns>DataSet of Sector</returns>
        public static DataSet LoadSector(int SectorId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@sectorid", SqlDbType.Int, 4, SectorId);
                    ds = db.GetDataSet("up_loadSector", prams);

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
        /// This Loads DataSet of SubSector wich Sub Sector id
        /// </summary>
        /// <param name="SectorId">The SubSector id whose record needs to be fetched</param>
        /// <returns>DataSet of SubSector</returns>
        public static DataSet LoadSubSector(int subsecotId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@subsectorid", SqlDbType.Int, 4, subsecotId);
                    ds = db.GetDataSet("up_loadSubSector", prams);

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
        /// Get DataSet of All Sub Sectors
        /// </summary>
        /// <returns>DataSet of SubSecotors</returns>
        public static DataSet GetAllSubSectors()
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[0];

                    ds = db.GetDataSet("up_getAllSubSectorsNames", prams);

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
        public static DataSet GetAllSubSectorsbyIndicators()
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[0];

                    ds = db.GetDataSet("up_getSubSectorswithIndicators", prams);

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
        /// This Get DataSet of All SubSectors with Indicators by sector Id
        /// </summary>
        /// <param name="Sectorid">The sector id whose records needs to be fetched</param>
        /// <returns></returns>
        public static DataSet GetAllSubSectorsbyIndicators(int Sectorid)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pSector_id", SqlDbType.Int, 4, Sectorid);
                    ds = db.GetDataSet("up_getSubSectorswithIndicatorsBySectorId", prams);

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
        /// Get DataSet of All Sectors
        /// </summary>
        /// <returns>DataSet of Sectors</returns>
        public static DataSet GetAllSectors()
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@username", SqlDbType.VarChar, 100, HttpContext.Current.User.Identity.Name);
                    ds = db.GetDataSet("up_getAllSectorsNames", prams);

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
        /// This Get DataTable of Project Details with Project Id and Oraganization Id
        /// </summary>
        /// <param name="projectID">Project Id whose data needs to be fetched</param>
        /// <param name="OID">Organization Id whose data needs to be fetched</param>
        /// <returns></returns>
        public static DataTable getProjectDetailsByOIDandPID(int projectID, int OID)
        {
            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, projectID);
                    prams[1] = db.MakeInParam("@pOrgnaizationId_ID", SqlDbType.Int, 0, OID);
                    dt = db.GetDataSet("up_getProjectsByIdAndOrgniazionId", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;

            }

            return dt;
        }

        /// <summary>
        /// This Get DataTable of Union Councils with Project id, Organiztion Id , Reporting Frequency Id and Service Provider Target Id
        /// </summary>
        /// <param name="projectID">Project Id whose data needs to be fetched</param>
        /// <param name="OrganizationId">Organization Id whose data needs to be fetched</param>
        /// <param name="reportingFreqId">Reporting Freqency Id whose data needs to be fetched</param>
        /// <param name="sp_targetId">Service Provider Target Id whose data needs to be fetched</param>
        /// <returns>DataTable of Union Councils</returns>
        public static DataTable getUcsByRptandPID(int projectID, int OrganizationId, int reportingFreqId, int sp_targetId)
        {
            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[5];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, projectID);
                    prams[1] = db.MakeInParam("@pOrgnaizationId_ID", SqlDbType.Int, 0, OrganizationId);
                    prams[2] = db.MakeInParam("@pUCId", SqlDbType.Int, 0, DBNull.Value);
                    prams[3] = db.MakeInParam("@pReportingFreq", SqlDbType.Int, 0, reportingFreqId);
                    prams[4] = db.MakeInParam("@sp_targetId", SqlDbType.Int, 0, sp_targetId);
                    dt = db.GetDataSet("up_getLocationsByProjectIdAndOrgniazionId", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;

            }

            return dt;
        }

        /// <summary>
        /// This Get DataTable of Union Councils with Project id, Organiztion Id and Union Council Id
        /// </summary>
        /// <param name="projectID">Project Id whose data needs to be fetched</param>
        /// <param name="OID">Organization Id whose data needs to be fetched</param>
        /// <param name="ucId">Union Council Id whose data needs to be fetched</param>
        /// <returns>DataTable of Union Council</returns>
        public static DataTable getUcsByOIDandPID(int projectID, int OID, int ucId)
        {
            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[5];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, projectID);
                    prams[1] = db.MakeInParam("@pOrgnaizationId_ID", SqlDbType.Int, 0, OID);
                    prams[2] = db.MakeInParam("@pUCId", SqlDbType.Int, 0, ucId);
                    prams[3] = db.MakeInParam("@pReportingFreq", SqlDbType.Int, 0, DBNull.Value);
                    prams[4] = db.MakeInParam("@sp_targetId", SqlDbType.Int, 0, DBNull.Value);
                    dt = db.GetDataSet("up_getLocationsByProjectIdAndOrgniazionId", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;

            }

            return dt;
        }

        /// <summary>
        /// Get DataSet of SubSector by Project Id and Service Provider Target Id
        /// </summary>
        /// <param name="ProjectId">The Project Id whose record needs to be fetched</param>
        /// <param name="sp_targetId">The Service Provider Target Id whose record needs to be fetched</param>
        /// <returns>DataSet of SubSector</returns>
        public static DataSet GetSubsectorByProjectId(int ProjectId, int sp_targetId)
        {
            DataSet ds = null;

            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    var prams = new SqlParameter[2];

                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 4, ProjectId);
                    prams[1] = db.MakeInParam("@sp_targetId", SqlDbType.Int, 4, sp_targetId);
                    ds = db.GetDataSet("up_getSubSectorsByProjectId", prams);

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
        /// Get DataTable of Project Frequency by Project Id
        /// </summary>
        /// <param name="ProjectId">The Project Id whose record needs to be fetched</param>
        /// <returns></returns>
        public static DataTable GetProjectFrequencyByProjectId(int ProjectId)
        {

            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pProject_ID", SqlDbType.Int, 0, ProjectId);
                    dt = db.GetDataSet("up_getProjectFrequencyByProjectId", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                // new SqlLog().InsertSqlLog(0, "Projects DataTable GetProjectFrequencyByProjectId(int ProjectId) ", ex);

                return null;
            }
            return dt;



        }

        /// <summary>
        /// Get DataTable of Projects Codes by Appeal id
        /// </summary>
        /// <param name="AppealId">Appeal Id whose data needs to be fetched</param>
        /// <returns>DataTable of Projects Codes</returns>
        public static DataTable GetProjectsCodesByAppealId(int AppealId)
        {
            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pAppeal_ID", SqlDbType.Int, 0, AppealId);
                    dt = db.GetDataSet("up_getProjectsTitlesByAppealId", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                // new SqlLog().InsertSqlLog(0, "Projects DataTable GetProjectsTitlesByAppealId(int AppealId) ", ex);

                return null;
            }
            return dt;

        }

        /// <summary>
        /// Get DataTable of Project Titles by Project Code
        /// </summary>
        /// <param name="ProjectCode">The name of project code whose data needs to be fetched</param>
        /// <returns>DataTable of Project Titles</returns>
        public static DataTable GetProjectsTitlesByProjectCode(string ProjectCode)
        {
            DataTable dt = null;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@pProject_Code", SqlDbType.VarChar, 50, ProjectCode);
                    dt = db.GetDataSet("up_getProjectsTitlesByProjectCode", prams).Tables[0];
                }
            }
            catch (Exception ex)
            {
                // new SqlLog().InsertSqlLog(0, "Projects DataTable GetProjectsTitlesByAppealId(int AppealId) ", ex);

                return null;
            }
            return dt;

        }

        /// <summary>
        /// This delete Sector with sector id
        /// </summary>
        /// <param name="sectorID">The sector Id whose data needs to be deleted</param>
        /// <returns></returns>
        public static int deleteSector(int sectorID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pSector_ID", SqlDbType.Int, 4, sectorID));
                    int exec = db.RunProc("up_DeleteSectorById", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This delete SubSector with Subsector id
        /// </summary>
        /// <param name="subsectorID">The Subsector Id whose data needs to be deleted</param>
        /// <returns></returns>
        public static int deleteSubSector(int subsectorID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pSubSector_ID", SqlDbType.Int, 4, subsectorID));
                    int exec = db.RunProc("up_DeleteSubSectorById", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This Delete Project by Project id
        /// </summary>
        /// <param name="projectID">The Project Id whose data needs to be deleted</param>
        /// <returns></returns>
        public static int deleteProject(int projectID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pProject_ID", SqlDbType.Int, 4, projectID));
                    int exec = db.RunProc("up_DeleteProjectById", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This Get DataTable of All Appeal Projects
        /// </summary>
        /// <returns>DataTable of Appeal Projects</returns>
        public static DataTable getAllAppealProjects()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                dt = db.GetDataSet("up_getAllAppealProjects", prams).Tables[0];
            }

            return dt;
        }

        /// <summary>
        /// Insert / Update Appeal Projects with Appeal Project id, appeal id, Project Title, Project Cod, Sector ID
        /// </summary>
        /// <param name="pAppealProjectId">The Appeal Project id which needs to be Inserted / Updated</param>
        /// <param name="pAppeal_ID">The Appeal id which needs to be Inserted / Updated</param>
        /// <param name="pProject_Title">The Project Title which needs to be Inserted / Updated</param>
        /// <param name="pProject_Code">The Project code which needs to be Inserted / Updated</param>
        /// <param name="pSector_ID">The projcet Sector id which needs to be Inserted / Updated</param>
        /// <returns></returns>
        public static int insertUpdateAppealProjects(int pAppealProjectId, int pAppeal_ID, string pProject_Title, string pProject_Code, int pSector_ID)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pAppealProjectId", SqlDbType.Int, 0, pAppealProjectId));
                    prams.Add(db.MakeInParam("@pAppeal_ID", SqlDbType.Int, 0, pAppeal_ID));
                    prams.Add(db.MakeInParam("@pProject_Title", SqlDbType.NVarChar, 0, pProject_Title));
                    prams.Add(db.MakeInParam("@pProject_Code", SqlDbType.NVarChar, 0, pProject_Code));
                    prams.Add(db.MakeInParam("@pSector_ID", SqlDbType.Int, 0, pSector_ID));
                    int exec = db.RunProc("UP_Appeal_Project_InsertUpdate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
                return -1;
            }
            return 0;

        }

        /// <summary>
        /// This Deletes Appeal Projects by Appeal Project id
        /// </summary>
        /// <param name="AppealProjectId">Appeal Project Id whose data needs to be Deleted</param>
        /// <returns></returns>
        public static int deleteAppealProjects(int AppealProjectId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pAppealProjectId", SqlDbType.Int, 4, AppealProjectId));
                    int exec = db.RunProc("up_deleteAppealProject", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }



        /// <summary>
        /// This Deletes Project Type by ProjectType Id 
        /// </summary>
        /// <param name="ProjectTypeId">ProjectType Id whose data needs to be Deleted</param>
        /// <returns></returns>
        public static int deleteProjectType(int ProjectTypeId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pProjectTypeId", SqlDbType.Int, 4, ProjectTypeId));
                    int exec = db.RunProc("up_deleteProjectType", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// This Deletes Project Status by ProjectStatus Id 
        /// </summary>
        /// <param name="ProjectStatusId">ProjectStatus Id whose Status needs to be Deleted</param>
        /// <returns></returns>
        public static int deleteProjectStatus(int ProjectStatusId)
        {

            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pProjectStatusId", SqlDbType.Int, 4, ProjectStatusId));
                    int exec = db.RunProc("up_deleteProjectStatus", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This Insert new Project Type with Project Type Id and Project Type
        /// </summary>
        /// <param name="ProjectTypeId">The Project Type Id which should be Inserted</param>
        /// <param name="ProjectType">The Project Type name which should be Inserted</param>
        /// <returns></returns>
        public static int InsertProjectTypes(int ProjectTypeId, string ProjectType)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pProject_Type_ID", SqlDbType.Int, 0, ProjectTypeId));
                    prams.Add(db.MakeInParam("@pProject_Type", SqlDbType.NVarChar, 0, ProjectType));
                    int exec = db.RunProc("UP_Project_Types_InsertUpdate", prams.ToArray());
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
        /// / This Insert new Project Status with ProjectStatus Id and ProjectStatus
        /// </summary>
        /// <param name="ProjectStatusId">The ProjectStatus Id which should be Inserted</param>
        /// <param name="ProjectStatus">The ProjectStatus name which should be Inserted</param>
        /// <returns></returns>
        public static int InsertProjectStatus(int ProjectStatusId, string ProjectStatus)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@pProject_Status_ID", SqlDbType.Int, 0, ProjectStatusId));
                    prams.Add(db.MakeInParam("@pProject_Status", SqlDbType.NVarChar, 0, ProjectStatus));
                    int exec = db.RunProc("UP_Project_Status_InsertUpdate", prams.ToArray());
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
        /// This Add new Sector with Sector id, Sector Description , Sector Name and Frequency Template Id
        /// </summary>
        /// <param name="Sector_ID">The Sector Id which needs to be Inserted</param>
        /// <param name="Sector_Description">The Sector Description which needs to be Inserted</param>
        /// <param name="Sector_Name">The Sector Name which needs to be Inserted</param>
        /// <param name="Frequency_TemplateID">The Frequency Template Id which needs to be Inserted</param>
        /// <returns></returns>
        public static int AddSectors(int Sector_ID, string Sector_Description, string Sector_Name, int Frequency_TemplateID)
        {
            SqlParameter[] prams;
            try
            {

                using (DbManager db = DbManager.GetDbManager())
                {//@pSubSector_ID int,@pSubSector_Name nvarchar(50),@pSubSector_Description nvarchar(500),@pSubSector_Icon nvarchar(4000),@pSector_ID int	

                    prams = new SqlParameter[4];
                    prams[0] = db.MakeInParam("@pSector_ID", SqlDbType.Int, 0, Sector_ID);
                    prams[1] = db.MakeInParam("@pSector_Name", SqlDbType.NVarChar, 50, Sector_Name);
                    prams[2] = db.MakeInParam("@pSector_Description", SqlDbType.NVarChar, 500, Sector_Description);
                    prams[3] = db.MakeInParam("@pFrequency_TemplateID", SqlDbType.Int, 0, Frequency_TemplateID);
                    return db.RunProc("UP_Sectors_InsertUpdate", prams);
                }

            }
            catch (Exception ex)
            {

                return -1;

            }


        }

        /// <summary>
        /// This Get Service Provider Status from ProjectsReporing with frequncy id and service Provider targer Id
        /// </summary>
        /// <param name="freqId">The Frequecnty Id whose record needs to be fetched</param>
        /// <param name="spTargetId">The Service provider Id whose record needs to be tetched</param>
        /// <returns>Service Provider Status</returns>
        public static ServiceProviderStatus GetProjectsReportingStatus(int freqId, int spTargetId)
        {
            DataTable dt = null;
            ServiceProviderStatus s;
            SqlParameter[] prams;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams = new SqlParameter[2];
                    prams[0] = db.MakeInParam("@sptargetId", SqlDbType.Int, 0, spTargetId);
                    prams[1] = db.MakeInParam("@rptFreqId", SqlDbType.Int, 0, freqId);
                    dt = db.GetDataSet("up_getReportStatus", prams).Tables[0];
                }
            }
            catch (Exception)
            {
                // new SqlLog().InsertSqlLog(0, "Projects DataTable GetProjectsTitlesByAppealId(int AppealId) ", ex);

                return ServiceProviderStatus.Draft;
            }

            if (dt != null && dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];

                switch (clsCommon.ParseString(dr["status"]).ToLower())
                {

                    case "draft":
                        s = ServiceProviderStatus.Draft;
                        break;
                    case "pendingaproval":
                        s = ServiceProviderStatus.PendingAproval;
                        break;
                    case "nil reported":
                        s = ServiceProviderStatus.Nil;
                        break;
                    case "approved":
                        s = ServiceProviderStatus.Approved;
                        break;
                    case "rejected":
                        s = ServiceProviderStatus.Rejected;
                        break;
                    default:
                        s = ServiceProviderStatus.Draft;
                        break;


                }

            }
            else
            {

                s = ServiceProviderStatus.Draft;
            }

            return s;

        }

    }
}
