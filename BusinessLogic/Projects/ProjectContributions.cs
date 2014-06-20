using DataAccessLayer.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

namespace BusinessLogic.Projects
{
    public class ProjectContributions
    {
        public ProjectContributions()
        {
            LastUpdated  = null;
            ProjectStartDate = null;
            ProjectEndDate = null;
            ContributionDecisionDate  = null;
        }

        public string AppealTitle { get; set; }
        public string ContributionRecipient { get; set; }
        public string ProjectCluster { get; set; }
        public string Sector { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectTitle { get; set; }
        public int EmergencyYear { get; set; }
        public int OriginalRequestAmount { get; set; }
        public int CurrentRequestAmount { get; set; }
        public int CommitedContributed { get; set; }
        public int FundCoverage { get; set; }
        public int AmountPledged { get; set; }
        public string Priority { get; set; }
        public string RecipientType { get; set; }
        public string RecipientAbbrev { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string GenderMarker { get; set; }
        public string ObjectiveText { get; set; }
        public string ImplementingPartners { get; set; }
        public string Emergency { get; set; }
        public string AppealCountry { get; set; }
        public string SubsetOfAppealName { get; set; }
        public int OriginalCurrencyAmount { get; set; }
        public string OriginalCurrencyUnit { get; set; }
        public string Donor { get; set; }
        public string DestinationCountry { get; set; }
        public string DestinationStatus { get; set; }
        public string DonorParent { get; set; }
        public string DonorCountry { get; set; }
        public string ContributionParentRecipient { get; set; }
        public string EmergencyRegion { get; set; }
        public string EmergencyCountry { get; set; }
        public string EmergencyType { get; set; }
        public string ContributionType { get; set; }
        public int ContributionID { get; set; }
        public string ContributionAidType { get; set; }
        public string ReportedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int AppealYear { get; set; }
        public int ProjectAmountRequested { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public DateTime? ContributionDecisionDate { get; set; }

        public static bool TruncateTable(string tableName, out string errorMessage)
        {
            errorMessage = string.Empty;

            return DBOperations.TruncateTable(CommonMethodsBL.ConnectionString, tableName, out errorMessage);
        }

        public static bool SaveContributions(List<ProjectContributions> listProjectContribution, string type, out string errorMessage)
        {
            errorMessage = string.Empty;
            List<object[]> listObj = new List<object[]>();

            foreach (ProjectContributions item in listProjectContribution)
            {
                object[] parameters = new object[46];

                parameters[0] = item.AppealTitle;
                parameters[1] = item.Donor;
                parameters[2] = item.ContributionRecipient;
                parameters[3] = item.ProjectCluster;
                parameters[4] = item.Sector;
                parameters[5] = item.ProjectCode;
                parameters[6] = item.ProjectTitle;
                parameters[7] = item.EmergencyYear;
                parameters[8] = item.OriginalRequestAmount;
                parameters[9] = item.CurrentRequestAmount;
                parameters[10] = item.CommitedContributed;
                parameters[11] = item.FundCoverage;
                parameters[12] = item.AmountPledged;
                parameters[13] = item.Priority;
                parameters[14] = item.RecipientType;
                parameters[15] = item.RecipientAbbrev;
                parameters[16] = item.Location;
                parameters[17] = item.Country;
                parameters[18] = item.GenderMarker;
                parameters[19] = item.ObjectiveText;
                parameters[20] = item.ImplementingPartners;
                parameters[21] = item.Emergency;
                parameters[22] = item.AppealCountry;
                parameters[23] = item.SubsetOfAppealName;
                parameters[24] = item.OriginalCurrencyAmount;
                parameters[25] = item.OriginalCurrencyUnit;
                parameters[26] = item.ContributionDecisionDate;
                parameters[27] = item.DonorParent;
                parameters[28] = item.DestinationCountry;
                parameters[29] = item.DestinationStatus;
                parameters[30] = item.ContributionParentRecipient;
                parameters[31] = item.EmergencyRegion;
                parameters[32] = item.EmergencyCountry;
                parameters[33] = item.EmergencyType;
                parameters[34] = item.ContributionType;
                parameters[35] = item.ContributionID;
                parameters[36] = item.ContributionAidType;
                parameters[37] = item.ReportedBy;
                parameters[38] = item.LastUpdated;
                parameters[39] = item.AppealYear;
                parameters[40] = item.ProjectAmountRequested;
                parameters[41] = item.ProjectStartDate;
                parameters[42] = item.ProjectEndDate;

                string[] projectCodeSplit = item.ProjectCode.Split('/');
                string projectID = string.Empty;
                if (projectCodeSplit.Length > 3)
                    projectID = projectCodeSplit[2];

                parameters[43] = projectID;
                parameters[44] = type;
                parameters[45] = item.DonorCountry;

                listObj.Add(parameters);
            }

            DataTable dtResult = new DataTable();
            return DBOperations.ExecuteSPROC(CommonMethodsBL.ConnectionString, "uspSaveContributions", listObj, out errorMessage, type);
        }
    }
}