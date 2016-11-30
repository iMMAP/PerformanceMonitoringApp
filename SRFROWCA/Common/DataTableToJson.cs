using BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SRFROWCA.Common
{
    public class DataTableToJson
    {
        public static string GetJSONString(DataTable Dt)
        {

            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();

            Sb.Append("{\"" + Dt.TableName + "\" : [");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;

                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());

                }
                Sb.Append(TempStr + "},");

            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            Sb.Append("]}");
            return Sb.ToString();
        }

        public static string DataTableToJsonBySerializer(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = System.Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }


        public static string GetJSONString2(DataTable dtOrigin)
        {

            //string[] StrDc = new string[Dt.Columns.Count];

            //string HeadStr = string.Empty;
            //for (int i = 0; i < Dt.Columns.Count; i++)
            //{

            //    StrDc[i] = Dt.Columns[i].Caption;
            //    HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            //}

            //HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();
            Sb.Append("[");
            int j = 0;
            foreach (DataRow dr in dtOrigin.Rows)
            {
                DataTable Dt = DBContext.GetData("GetUNHCRData", new object[] { dr[0].ToString() });
                
                if (j == 0)
                    Sb.Append("{");
                else
                    Sb.Append(",{");
                j++;
                Sb.Append("\"country\" : ");

                Sb.Append("[");
                //Sb.Append("{\"" + Dt.TableName + "\" : [");
                int k = 0;
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string TempStr = "";
                    if (k == 0)
                        TempStr = "[" + Dt.Rows[i][1].ToString() + "," + Dt.Rows[i][2].ToString() + ",\"" + Dt.Rows[i][3].ToString() + "\"] ";
                    else
                        TempStr = " ,[" + Dt.Rows[i][1].ToString() + "," + Dt.Rows[i][2].ToString() + ",\"" + Dt.Rows[i][3].ToString() + "\"] ";
                    k++;
                    Sb.Append(TempStr);
                }

                Sb.Append("], ");
                string num = dr[1].ToString();
                string name = dr[0].ToString();

                Sb.Append("\"total\" : " + num + ", \"name\" : " + string.Format("\"{0}\"", name));
                Sb.Append("} ");

            }
            Sb.Append("]");
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            Sb.Append("]");
            return Sb.ToString();
        }

        public static string GetJSONString3(DataTable dtOrigin)
        {

            //string[] StrDc = new string[Dt.Columns.Count];

            //string HeadStr = string.Empty;
            //for (int i = 0; i < Dt.Columns.Count; i++)
            //{

            //    StrDc[i] = Dt.Columns[i].Caption;
            //    HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            //}

            //HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();
            Sb.Append("[");
            foreach (DataRow dr in dtOrigin.Rows)
            {
                DataTable Dt = DBContext.GetData("GetUNHCRData", new object[] { dr[0].ToString() });
                Sb.Append("{");
                string num = dr[1].ToString();
                string name = dr[0].ToString();
                Sb.Append("\"name\" : " + string.Format("\"{0}\"", name) + "," + "\"region\" : " + string.Format("\"{0}\"", name) + "," + "\"income\" : ");
                //Sb.Append("\"articles\" : ");

                Sb.Append("[");
                //Sb.Append("{\"" + Dt.TableName + "\" : [");
                int k = 0;
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    string TempStr = "";
                    if (k == 0)
                        TempStr = "[" + Dt.Rows[i][1].ToString() + "," + Dt.Rows[i][2].ToString() + "] ";
                    else
                        TempStr = " ,[" + Dt.Rows[i][1].ToString() + "," + Dt.Rows[i][2].ToString() + "] ";
                    k++;
                    Sb.Append(TempStr);
                }

                Sb.Append("], ");
                

                
                Sb.Append("}, ");

            }
            Sb.Append("]");
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            Sb.Append("]}");
            return Sb.ToString();
        }

    }
}