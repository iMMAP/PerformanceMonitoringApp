using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SRFROWCA.Common
{
    public static class FU
    {
        public static string UploadFile(FileUpload fu, string uploadDir)
        {
            string path = fu.PostedFile.FileName;
            string fileName = Path.GetFileNameWithoutExtension(fu.PostedFile.FileName);
            string fileExtension = Path.GetExtension(fu.PostedFile.FileName);

            // Create file name on the basis of userid and datetime.
            fileName += DateTime.Now.ToString("MMM-dd-yy-hh-mm-ss") + fileExtension;
            //string uploadDir = Server.MapPath(@"~/TEST");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            string uploadFileLocation = uploadDir + "//" + fileName;
            fu.SaveAs(uploadFileLocation);

            return uploadFileLocation;
        }

        public static string GetExcelConString(string filePath)
        {
            string con = null;
            string fileExtension = Path.GetExtension(filePath);
            if (fileExtension == ".xls")
            {
                con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else if (fileExtension == ".xlsx")
            {
                con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }
            return con;
        }

        public static String[] GetExcelSheetNames(string excelConString)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                objConn = new OleDbConnection(excelConString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);                
                if (dt == null) return null;                
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                // Loop through all of the sheets if you want too...
                //for (int j = 0; j < excelSheets.Length; j++)
                //{
                //    // Query each excel sheet.
                //}

                return excelSheets;
            }
            catch
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        // First Read data from excel sheet into data table.
        // Add UserId, Selected Emregency Id and Identity column.
        // Above added data is needed to update tables in db.
        public static DataTable ReadDataInDataTable(string excelConString, string sheetName, DataTable dt)
        {
            //Create Connection to Excel work book
            OleDbConnection excelCon = new OleDbConnection(excelConString);
            //Create OleDbCommand to fetch data from Excel
            OleDbCommand cmd = new OleDbCommand(string.Format(@"Select * from [{0}]", sheetName), excelCon);
            excelCon.Open();

            OleDbDataAdapter da = new OleDbDataAdapter();
            da.SelectCommand = cmd;

            da.Fill(dt);
            da.Dispose();
            excelCon.Close();
            return dt;
        }        

        // Write data into db using SqlBulkCopy
        public static void WriteDataInDBTable(string conString, string dbTableName, DataTable dt)
        {
            SqlBulkCopy sqlBulk = new SqlBulkCopy(conString);
            sqlBulk.DestinationTableName = dbTableName;
            sqlBulk.WriteToServer(dt);
            sqlBulk.Close();
        }

        public static void CreateTableInDB(string conString, string tableScript)
        {
            SqlConnection con = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(tableScript, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }        
    }
}