using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRFROWCA.OPS
{
    public class OPSProject
    {
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectObjective { get; set; }
        public string EmergencyLocationId { get; set; }
        public string EmergencyClusterId { get; set; }
        public string ProjectStatusId { get; set; }
        public string BeneficiaryTotalNumber { get; set; }
        public string BeneficiariesChildren { get; set; }
        public string BeneficiariesWomen { get; set; }
        public string BeneficiariesOthers { get; set; }
        public string BeneficiariesDescription { get; set; }
        public string BeneficiariesTotalDescription { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectEndDate { get; set; }
        public string ProjectImplementingpartner { get; set; }
        public string ProjectContactName { get; set; }
        public string ProjectContactEmail { get; set; }
        public string ProjectContactPhone { get; set; }
        public string RelatedURL { get; set; }
        public string ProjectType { get; set; }
        public string OPSLastUpdatedDate { get; set; }
        public string OPSLastUpdatedBy { get; set; }
        public string OPSProjectStatus { get; set; }
        public string OPSClusterName { get; set; }
        public string IsOPSProject { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedById { get; set; }
        public string ImportedById { get; set; }
        public string ImportedDate { get; set; }
        public string DonorName { get; set; }
        public string FundingStatus { get; set; }
        public string DonorName2 { get; set; }
        public string Contribution2Amount { get; set; }
        public string Contribution2CurrencyId { get; set; }
        public string RequestedAmount { get; set; }
        public string RequestedAmountCurrencyId { get; set; }
        public string Contribution1Amount { get; set; }
        public string Contribution1CurrencyId { get; set; }
        public string ProjectStatus { get; set; }
        public string Type { get; set; }
        public string ProjectShortTitle { get; set; }
        public string Pooled { get; set; }
        public string ProjectYear { get; set; }
        public string GenderMarker { get; set; }
        public string AppealId { get; set; }
        public string AppealName { get; set; }
        public string SectorId { get; set; }
        public string SectorName { get; set; }
        public string ClusterId { get; set; }
        public string ClusterName { get; set; }
        public string PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string OtherFields { get; set; }
        public string EGFLocations { get; set; }
        //public string SecondaryClusterId { get; set; }
        public string Refugee { get; set; }
        public OPSDescription OPSDescriptions { get; set; }
        public OPSProjectOrganizations Organizations { get; set; }
    }

    public class OPSDescription
    {
        public string DescriptionText { get; set; }
        public string Type { get; set; }
    }

    public class OPSProjectOrganizations
    {
        public List<OPSOrganization> Organization = new List<OPSOrganization>();
    }

    public class OPSOrganization
    {
        public string OrganizationId { get; set; }
        public string OriginalRequirements { get; set; }
        public string RevisedRequirements { get; set; }
        public string OrgAbbrevation { get; set; }
        public string OrganizationName { get; set; }
        public string BudgetLineType { get; set; }
        public string BudgetLineDescription { get; set; }
        public string BudgetLineAmount { get; set; }
    }

    public class OPSBudgetLine
    {
        public string BudgetType { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
    }
}