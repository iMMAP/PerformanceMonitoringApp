using System;

namespace SingleReportingLib
{
    public static class CommonMethods
    {
        // Parse string to int.
        public static int ParseInt(string val)
        {
            int intValue = 0;

            try
            {
                if (!(string.IsNullOrEmpty(val)))
                {
                    int.TryParse(val, out intValue);
                }

                return intValue;
            }
            catch
            {
                // if error occoured return default value.
                return intValue;
            }
        }

        // Parse string to int.
        public static int ParseInt(long val)
        {
            int intValue = 0;

            try
            {
                intValue =  int.Parse(val.ToString());
                return intValue;
            }
            catch
            {
                // if error occoured return default value.
                return intValue;
            }
        }

        // Parse string to decimal
        public static decimal ParseDecimal(string val)
        {
            decimal decimalValue = 0M;

            try
            {
                if (!(string.IsNullOrEmpty(val)))
                {
                    decimal.TryParse(val, out decimalValue);
                }

                return decimalValue;
            }
            catch
            {
                return decimalValue;
            }
        }

        public static DateTime ParseDateTime(string stringDateTime)
        {
            DateTime dtParsed = DateTime.Now;

            try
            {

                if (!(string.IsNullOrEmpty(stringDateTime)))
                {
                    DateTime.TryParse(stringDateTime, out dtParsed);
                }

                return dtParsed;
            }
            catch
            {
                return dtParsed;
            }
        }
    }
}
