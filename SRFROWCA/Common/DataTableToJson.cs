﻿using System.Collections.Generic;
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
    }
}