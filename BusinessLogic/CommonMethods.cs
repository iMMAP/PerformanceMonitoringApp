using System.Data;


namespace BusinessLogic
{
    public static class CommonMethodsBL
    {
        public static DataTable GetGeneralSectors()
        {
            return DBContext.GetData("GetGeneralSectors");
        }

        public static DataTable GetDonors()
        {
            return GetDonors((int?)null);
        }

        public static DataTable GetDonors(int? isReallyDonor)
        {
            return DBContext.GetData("GetDonors", new object[] { isReallyDonor });
        }

        public static string ConnectionString
        {
            get
            {
                return "Live";
            }
        }
    }
}
