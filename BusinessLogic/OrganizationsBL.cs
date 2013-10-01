using System.Data;

namespace BusinessLogic
{
    public static class OrganizationsBL
    {
        public static DataTable GetOrganizations()
        {
            return GetOrganizations((int?)null);
        }

        public static DataTable GetOrganizations(int? orgId)
        {
            return GetOrganizations(orgId, null);
        }

        public static DataTable GetOrganizations(string orgName)
        {
            return GetOrganizations((int?)null, orgName);
        }

        public static DataTable GetOrganizations(int? orgId, string orgName)
        {
            return GetOrganizations(orgId, orgName, null);
        }

        public static DataTable GetOrganizations(int? orgId, string orgName, string orgAcronym)
        {
            return GetOrganizations(orgId, orgName, orgAcronym, (int?)null);
        }

        public static DataTable GetOrganizations(int? orgId, string orgName, string orgAcronym, int? orgTypeId)
        {
            return GetOrganizations(orgId, orgName, orgAcronym, orgTypeId, null);
        }

        public static DataTable GetOrganizations(int? orgId, string orgName, string orgAcronym, int? orgTypeId, string orgType)
        {
            object[] parameters = new object[] { orgId, orgName, orgAcronym, orgTypeId, orgTypeId };
            return DBContext.GetData(CommonProperties.ConnectionString, "USP_Common_GetOrganizations", parameters);
        }
    }
}
