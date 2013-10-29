using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SRFROWCA.Common
{
    public class ROWCACommon
    {
        internal static bool IsInAdminRoles(System.Security.Principal.IPrincipal iPrincipal)
        {
            if (IsAdmin(iPrincipal) || IsCountryAdmin(iPrincipal))
                return true;
            return false;
        }

        internal static bool IsAdmin(System.Security.Principal.IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("Admin"))
                return true;
            return false;
        }

        internal static bool IsCountryAdmin(System.Security.Principal.IPrincipal iPrincipal)
        {
            if (iPrincipal.IsInRole("CountryAdmin"))
                return true;
            return false;
        }

        internal static bool IsAuthenticated(System.Security.Principal.IPrincipal iPrincipal)
        {
            return iPrincipal.Identity.IsAuthenticated;
        }

        internal static void RedirectUserToPage(HttpResponse Response)
        {
            Response.Redirect("~/Default.aspx");
        }

        internal static string GetCountryAdminRoleName
        {
            get
            {
                return "CountryAdmin";
            }
        }

        public static string CreateFolderForFiles(string dir, string sessionId)
        {
            // Concat sessionid with path to generate seperate
            // folder for each user.
            dir += "\\" + sessionId;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static bool DataInserted
        {
            get
            {
                if (HttpContext.Current.Session["DataInserted"] == null)
                {
                    HttpContext.Current.Session["DataInserted"] = false;
                }
                bool? dataInserted = HttpContext.Current.Session["DataInserted"] as bool?;

                return dataInserted.Value;
            }
            set
            {
                HttpContext.Current.Session["DataInserted"] = value;
            }
        }

        public static string ErrorMessage
        {
            get
            {
                return "error-message";
            }
        }

        public static string InfoMessage
        {
            get
            {
                return "info-message";
            }
        }

        public enum LocationTypes
        {
            Region = 1,
            National = 2,
            Governorate = 3,
            District = 4,
            Subdistrict = 5,
            Village = 6,
            Nonrepresentative = 7,
            Other = 8,
        }
    }
}