using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SingleReportingLib
{
    /// <summary>
    /// Get Details for Frequency Template
    /// </summary>
    public class FrequencyTemplate
    {
        private int _TemplateID;
        private string _Name;
        private string _Frequency;
        private int _RGap;
        private string _Alias;

        public int TemplateID { get { return _TemplateID; } set { _TemplateID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Frequency { get { return _Frequency; } set { _Frequency = value;  } }
        public int RGap { get { return _RGap; } set { _RGap = value; } }
        public string Alias { get { return _Alias; } set { _Alias = value; } }

        /// <summary>
        /// Constructor with no arguments
        /// </summary>
        public FrequencyTemplate()
        {
         
        }
        /// <summary>
        /// Constructor with only one argument
        /// </summary>
        /// <param name="templateID"></param>
        public FrequencyTemplate(int templateID)
        {
            loadFrequencyTemplate(templateID);
        }


        /// <summary>
        /// Insert new Frequency Template
        /// </summary>
        /// <param name="ft">The Object of class Frequency Template which should be inserted</param>
        /// <returns></returns>
        public static int insertFrequencyTemplate(FrequencyTemplate ft)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@TemplateID", SqlDbType.VarChar, 50, ft.TemplateID));
                    prams.Add(db.MakeInParam("@Name", SqlDbType.VarChar, 50, ft.Name));
                    prams.Add(db.MakeInParam("@Frequency", SqlDbType.VarChar, 50, ft.Frequency));
                    prams.Add(db.MakeInParam("@RGap", SqlDbType.Int, 4, ft.RGap));
                    prams.Add(db.MakeInParam("@Alias", SqlDbType.VarChar, 50, ft.Alias));                    

                    int exec = db.RunProc("up_InsertFrequencyTemplate", prams.ToArray());
                    return exec;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Load FrequencyTemplate by TemplateID
        /// </summary>
        /// <param name="templateID">The id of FrequencyTemplate by which Frequency Template should be loaded</param>
        private void loadFrequencyTemplate(int templateID)
        {
            IDataReader reader = null;
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    SqlParameter[] prams = new SqlParameter[1];
                    prams[0] = db.MakeInParam("@FrequencyTemplate_ID", SqlDbType.Int, 4, templateID);
                    reader = db.GetDataReader("UP_FrequencyTemplate_GetByID", prams);
                    if (reader.Read())
                        LoadFrequencyTemplates(reader);
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
        
        private void LoadFrequencyTemplates(IDataReader reader)
        {
            _TemplateID = reader["TemplateID"] == System.DBNull.Value ? 0 : clsCommon.ParseInt(reader["TemplateID"].ToString());
            _Name = reader["Name"] == System.DBNull.Value ? "" : reader["Name"].ToString();
            _Frequency = reader["Frequency"] == System.DBNull.Value ? "" : reader["Frequency"].ToString();
            _RGap = reader["RGap"] == System.DBNull.Value ? 0 : clsCommon.ParseInt(reader["RGap"].ToString());
            _Alias = reader["Alias"] == System.DBNull.Value ? "" : reader["Alias"].ToString();
        }

        /// <summary>
        /// Get DataTable of All Frequency Templates
        /// </summary>
        /// <returns> DataTable of All FrequencyTemplates</returns>
        public static DataTable getAllFrequencyTemplates()
        {
            DataTable dt = null;
            SqlParameter[] prams;

            using (DbManager db = DbManager.GetDbManager())
            {
                prams = new SqlParameter[0];
                DataTableCollection tables = db.GetDataSet("UP_FrequencyTemplate_GetAll", prams).Tables;
                if (tables != null && tables.Count > 0)
                {
                    dt = tables[0];
                }
                
            }

            return dt;
        }
        /// <summary>
        /// Delete FrequencyTemplate by template ID
        /// </summary>
        /// <param name="templateID">Id of FrequencyTemplate by which Frequency Template should be deleted</param>
        /// <returns>True for successful delete / False when exception cought</returns>
        public static bool deleteFrequencyTemplate(int templateID)
        {
            List<SqlParameter> prams = new List<SqlParameter>();
            try
            {
                using (DbManager db = DbManager.GetDbManager())
                {
                    prams.Add(db.MakeInParam("@TemplateID", SqlDbType.Int, 4, templateID));
                    int exec = db.RunProc("up_FrequencyTemplate_DeleteByID", prams.ToArray());
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }
    }
}
