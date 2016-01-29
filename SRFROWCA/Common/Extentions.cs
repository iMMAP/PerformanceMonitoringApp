using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRFROWCA.Common
{
    public static class Extensions
    {
        public static string NullIfWhiteSpace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }

        public static string NullIfEmpty(this string value)
        {
            if (String.IsNullOrEmpty(value)) { return null; }
            return value;
        }
    }
}