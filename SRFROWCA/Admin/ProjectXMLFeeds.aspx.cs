using BusinessLogic.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SRFROWCA.Admin
{
    public partial class ProjectXMLFeeds : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void LoadXML(string xmlURL, string headNode)
        {
            XmlTextReader reader = new XmlTextReader(xmlURL);
            reader.WhitespaceHandling = WhitespaceHandling.Significant;
            List<ProjectContributions> projectList = new List<ProjectContributions>();
            string date = string.Empty;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals(headNode))
                    {
                        ProjectContributions projectData = new ProjectContributions();

                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals(headNode))
                            {
                                projectList.Add(projectData);
                                break;
                            }

                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name.Equals("appeal-title"))
                                {
                                    reader.Read();
                                    projectData.AppealTitle = reader.Value;
                                }
                                if (reader.Name.Equals("donor"))
                                {
                                    reader.Read();
                                    projectData.Donor = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-recipient"))
                                {
                                    reader.Read();
                                    projectData.ContributionRecipient = reader.Value;
                                }
                                if (reader.Name.Equals("project-cluster"))
                                {
                                    reader.Read();
                                    projectData.ProjectCluster = reader.Value;
                                }
                                if (reader.Name.Equals("sector"))
                                {
                                    reader.Read();
                                    projectData.Sector = reader.Value;
                                }
                                if (reader.Name.Equals("project-code"))
                                {
                                    reader.Read();
                                    projectData.ProjectCode = reader.Value;
                                }
                                if (reader.Name.Equals("project-title"))
                                {
                                    reader.Read();
                                    projectData.ProjectTitle = reader.Value;
                                }
                                if (reader.Name.Equals("emergency-year"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.EmergencyYear = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("project-amount-originalrequest"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.OriginalRequestAmount = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("project-amount-currentrequest"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.CurrentRequestAmount = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("contribution-amount-commited-contributed"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.CommitedContributed = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("project-fund-coverage"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.FundCoverage = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("contribution-amount-pledged"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.AmountPledged = Convert.ToInt32(reader.Value);
                                }

                                if (reader.Name.Equals("project-priority"))
                                {
                                    reader.Read();
                                    projectData.Priority = reader.Value;
                                }

                                if (reader.Name.Equals("contribution-recipient-type"))
                                {
                                    reader.Read();
                                    projectData.RecipientType = reader.Value;
                                }

                                if (reader.Name.Equals("contribution-recipient-abbrev"))
                                {
                                    reader.Read();
                                    projectData.RecipientAbbrev = reader.Value;
                                }
                                if (reader.Name.Equals("project-location"))
                                {
                                    reader.Read();
                                    projectData.Location = reader.Value;
                                }
                                if (reader.Name.Equals("project-country"))
                                {
                                    reader.Read();
                                    projectData.Country = reader.Value;
                                }
                                if (reader.Name.Equals("gender-marker"))
                                {
                                    reader.Read();
                                    projectData.GenderMarker = reader.Value;
                                }
                                if (reader.Name.Equals("objective-text"))
                                {
                                    reader.Read();
                                    projectData.ObjectiveText = reader.Value;
                                }
                                if (reader.Name.Equals("implementing_partners"))
                                {
                                    reader.Read();
                                    projectData.ImplementingPartners = reader.Value;
                                }
                                if (reader.Name.Equals("emergency"))
                                {
                                    reader.Read();
                                    projectData.Emergency = reader.Value;
                                }
                                if (reader.Name.Equals("appeal-country"))
                                {
                                    reader.Read();
                                    projectData.AppealCountry = reader.Value;
                                }
                                if (reader.Name.Equals("SubsetOfAppealName"))
                                {
                                    reader.Read();
                                    projectData.SubsetOfAppealName = reader.Value;
                                }
                                if (reader.Name.Equals("original-currency-amount"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.OriginalCurrencyAmount = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("original-currency-unit"))
                                {
                                    reader.Read();
                                    projectData.OriginalCurrencyUnit = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-decision-date"))
                                {
                                    reader.Read();

                                    date = FormatDate(reader.Value);
                                    if (!string.IsNullOrEmpty(date))
                                        projectData.ContributionDecisionDate = Convert.ToDateTime(date);
                                }
                                if (reader.Name.Equals("donor-parent"))
                                {
                                    reader.Read();
                                    projectData.DonorParent = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-destination-country"))
                                {
                                    reader.Read();
                                    projectData.DestinationCountry = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-parent-recipient"))
                                {
                                    reader.Read();
                                    projectData.ContributionParentRecipient = reader.Value;
                                }
                                if (reader.Name.Equals("emergencyregion"))
                                {
                                    reader.Read();
                                    projectData.EmergencyRegion = reader.Value;
                                }
                                if (reader.Name.Equals("emergencycountry"))
                                {
                                    reader.Read();
                                    projectData.EmergencyCountry = reader.Value;
                                }
                                if (reader.Name.Equals("emergencytype"))
                                {
                                    reader.Read();
                                    projectData.EmergencyType = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-type"))
                                {
                                    reader.Read();
                                    projectData.ContributionType = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-id"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.ContributionID = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("contribution-aidtype"))
                                {
                                    reader.Read();
                                    projectData.ContributionAidType = reader.Value;
                                }
                                if (reader.Name.Equals("contribution-reportedby"))
                                {
                                    reader.Read();
                                    projectData.ReportedBy = reader.Value;
                                }
                                if (reader.Name.Equals("LastUpdated"))
                                {
                                    reader.Read();

                                    date = FormatDate(reader.Value);
                                    if (!string.IsNullOrEmpty(date))
                                        projectData.LastUpdated = Convert.ToDateTime(date);
                                }
                                if (reader.Name.Equals("appeal-year"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.AppealYear = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("project-amount-requested"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.ProjectAmountRequested = Convert.ToInt32(reader.Value);
                                }
                                if (reader.Name.Equals("project-start-date"))
                                {
                                    reader.Read();

                                    date = FormatDate(reader.Value);
                                    if (!string.IsNullOrEmpty(date))
                                        projectData.ProjectStartDate = Convert.ToDateTime(date);
                                }
                                if (reader.Name.Equals("project-end-date"))
                                {
                                    reader.Read();

                                    date = FormatDate(reader.Value);
                                    if (!string.IsNullOrEmpty(date))
                                        projectData.ProjectEndDate = Convert.ToDateTime(date);
                                }
                                if (reader.Name.Equals("donor-country"))
                                {
                                    reader.Read();

                                    if (!string.IsNullOrEmpty(reader.Value))
                                        projectData.DonorCountry = reader.Value;
                                }
                            }
                        }
                    }
                }
            }

            string type = "Project";
            if (!headNode.Equals("project"))
                type = "Contribution";

            string errorMessage = string.Empty;
            if (ProjectContributions.SaveContributions(projectList, type, out errorMessage))
                lblMessage.Text += "XML Feeds Project fed successfully into database<br><br>";
            else
                lblMessage.Text += "Error: XML feeding unsuccessful - " + errorMessage + "<br><br>";


            //Response.Write(type + " count in Feed: " + projectList.Count.ToString() + "<br>");
            //Response.Write(type + " already exist in DB: 0" + "<br>");
            //Response.Write(type + " IDs already exist in DB: 0" + "<br>");
            //Response.Write(type + " rows inserted in DB: 0" + "<br>");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                LoadXML("http://fts.unocha.org/common/CustomSearchXmlRss.aspx?output=0&CQ=cq1606141529408X1rn1Kc2n", "project");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error (Project feed): Connectivity issues - " + ex.Message+"<br><br>";
            }

            try
            {
                LoadXML("http://fts.unocha.org/common/CustomSearchXmlRss.aspx?output=0&CQ=cq1606141458492On03qSddb", "contribution");
            }
            catch (Exception ex)
            {
                lblMessage.Text += "Error (Contribution feed): Connectivity issues - " + ex.Message;
            }
        }

        private string FormatDate(string dateValue)
        {
            string[] dateSplitted = dateValue.Split('/');

            if (dateSplitted.Length > 1)
                return dateSplitted[1] + "/" + dateSplitted[0] + "/" + dateSplitted[2];
            else
                return string.Empty;
        }
    }
}