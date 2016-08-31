using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using BusinessLogic;
using SRFROWCA.Common;
using System.Transactions;

namespace SRFROWCA.OPS
{
    public partial class SyncOPSProjects : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
        }

        protected void btnImportProjects_Click(object sender, EventArgs e)
        {
            //DBContext.Update("UpdateProjectsTableWithToDeleteNull", new object[] { DBNull.Value });

            //using (TransactionScope scope = new TransactionScope())
            {
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1105/published.xml"); // Burkina
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1107/published.xml"); // Cameroon
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1108/published.xml"); // Chad
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1109/published.xml"); // Gambia
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1110/published.xml"); // Mali
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1111/published.xml"); // Mauritania
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1112/published.xml"); // Niger
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1113/published.xml"); // Nigeria
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1114/published.xml"); // Sahel
                InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1115/published.xml"); // Senegal

                //DBContext.Delete("DeleteTempProjectsOPS", new object[] { DBNull.Value });
                //scope.Complete();
                Response.Write("All Synced!");
            }
        }

        private void InsertUpdateProjects(string url)
        {
            try
            {
                Response.Write("<br/>");
                Response.Write(url);
                Response.Write("<br/>");
                XDocument doc = XDocument.Load(url);
                IEnumerable<OPSProject> projects = GetProjects(doc);
                SyncProjects(projects, doc);
            }
            catch (Exception ex)
            {
                Response.Write(url);
                Response.Write("<br/>");
                Response.Write(ex.ToString());
                Response.Write("<br/>");
            }
        }

        private IEnumerable<OPSProject> GetProjects(XDocument doc)
        {
            var projects = doc.Root
                .Elements("project")
                .Select(x => new OPSProject
                {
                    ProjectId = (string)x.Attribute("id"),
                    OPSProjectStatus = (string)x.Element("status"),
                    Type = (string)x.Element("type"),
                    ProjectCode = (string)x.Element("code"),
                    ProjectTitle = (string)x.Element("title"),
                    Pooled = (string)x.Element("pooled"),
                    ProjectStartDate = (string)x.Element("startdate"),
                    ProjectEndDate = (string)x.Element("enddate"),
                    ProjectYear = (string)x.Element("year"),
                    BeneficiariesChildren = (string)x.Element("beneficiaries_children"),
                    BeneficiariesWomen = (string)x.Element("beneficiaries_women"),
                    BeneficiaryTotalNumber = (string)x.Element("beneficiaries_total"),
                    BeneficiariesTotalDescription = (string)x.Element("beneficiaries_total_description"),
                    BeneficiariesOthers = (string)x.Element("beneficiaries_others"),
                    BeneficiariesDescription = (string)x.Element("beneficiaries_others_description"),
                    ProjectImplementingpartner = (string)x.Element("implementing_partners"),
                    GenderMarker = (string)x.Element("gender_marker"),
                    ProjectContactName = (string)x.Element("contact_name"),
                    ProjectContactPhone = (string)x.Element("contact_phone"),
                    ProjectContactEmail = (string)x.Element("contact_email"),
                    AppealId = (string)x.Element("appeal").Attribute("id"),
                    AppealName = (string)x.Element("appeal"),
                    SectorId = (string)x.Element("sector").Attribute("id"),
                    SectorName = (string)x.Element("sector"),
                    ClusterId = (string)x.Element("cluster").Attribute("id"),
                    ClusterName = (string)x.Element("cluster"),
                    PriorityId = (string)x.Element("priority").Attribute("id"),
                    PriorityName = (string)x.Element("priority"),
                    CountryId = (string)x.Element("country").Attribute("id"),
                    CountryName = (string)x.Element("country"),
                    OtherFields = (string)x.Element("other_fields"),
                    EGFLocations = (string)x.Element("egf_locations"),
                    //SecondaryClusterId = (string)x.Element("subset").Attribute("id"),
                    Refugee = (string)x.Element("refugee")

                });

            return projects;
        }

        private void SyncProjects(IEnumerable<OPSProject> projects, XDocument doc)
        {
            foreach (var project in projects)
            {
                try
                {
                    if (project.SectorName != "SECTOR NOT YET SPECIFIED")
                    {
                        IEnumerable<OPSDescription> descriptions = GetProjectDescriptions(doc, project.ProjectId);
                        object[] parameters = GetParameters(project, descriptions);
                        DBContext.Update("InsertUpdateOPSProject", parameters);
                        Response.Write(project.ProjectId);
                        Response.Write("<br/>");
                        IEnumerable<OPSOrganization> organizations = GetOrganizations(doc, project.ProjectId);
                        object[] orgParameter = GetOrganizationParameter(project.ProjectId, organizations.FirstOrDefault());
                        DBContext.Update("InsertUpdateOPSProjectOrganization", orgParameter);
                    }
                }
                catch (Exception ex)
                {
                    //Response.Write(url);
                    Response.Write("<br/>");
                    Response.Write(ex.ToString());
                    Response.Write("<br/>");
                }

            }
        }

        private IEnumerable<OPSDescription> GetProjectDescriptions(XDocument doc, string projectId)
        {
            var descriptions = doc.Root.Elements("project")
                .Where(p => ((string)p.Attribute("id")) == projectId)
                .Descendants("descriptions").Elements("description")
                .Select(d => new OPSDescription
                {
                    Type = (string)d.Attribute("type"),
                    DescriptionText = (string)d,

                });

            return descriptions;
        }

        private IEnumerable<OPSOrganization> GetOrganizations(XDocument doc, string projectId)
        {

            var organizations = doc.Root.Elements("project")
                .Where(p => ((string)p.Attribute("id")) == projectId)
                .Descendants("organisations").Descendants("organisation")
                .Select(o => new OPSOrganization
                {
                    OrganizationId = (string)o.Attribute("id"),
                    OrgAbbrevation = (string)o.Element("name").Attribute("abbreviation"),
                    OrganizationName = (string)o.Element("name"),
                    OriginalRequirements = (string)o.Element("original_requirement"),
                    RevisedRequirements = (string)o.Element("revised_requirement")
                });

            return organizations;
        }

        private object[] GetOrganizationParameter(string projId, OPSOrganization org)
        {
            int projectId = 0;
            int.TryParse(projId, out projectId);
            int organizationId = 0;
            int.TryParse(org.OrganizationId, out organizationId);
            int revisedReques = 0;
            int.TryParse(org.RevisedRequirements, out revisedReques);
            int originalRequest = 0;
            int.TryParse(org.OriginalRequirements, out originalRequest);

            if (org.OrganizationName == "Food And Agriculture Organization of the United Nations")
            {
                org.OrganizationName = "Food & Agriculture Organization of the United Nations";
            }

            return new object[] { projectId, organizationId, org.OrganizationName, org.OrgAbbrevation, originalRequest, revisedReques, RC.GetCurrentUserId, DBNull.Value };
        }

        private object[] GetParameters(OPSProject project, IEnumerable<OPSDescription> descriptions)
        {
            int projectId = 0;
            int.TryParse(project.ProjectId, out projectId);
            int projectStatusId = 1;

            string clusterName = "";
            if (project.ClusterName == "WATER AND SANITATION")
            {
                clusterName = "Water Sanitation & Hygiene";
            }
            else if (project.ClusterName == "MULTI-SECTOR FOR REFUGEES")
            {
                clusterName = "Multi Sector for Refugees";
            }
            else
            {
                clusterName = project.ClusterName;
            }

            //string secClusterName = "";
            //if (project.SecondaryClusterName == "WATER AND SANITATION")
            //{
            //    secClusterName = "Water Sanitation & Hygiene";
            //}
            //else if (project.SecondaryClusterName == "MULTI-SECTOR FOR REFUGEES")
            //{
            //    secClusterName = "Multi Sector for Refugees";
            //}
            //else
            //{
            //    secClusterName = project.SecondaryClusterName;
            //}

            int tempVal = 0;
            int? benChildren = null;
            if (project.BeneficiariesChildren != "0")
            {
                int.TryParse(project.BeneficiariesChildren, out tempVal);
                benChildren = tempVal > 0 ? tempVal : (int?)null;
            }
            else
            {
                benChildren = 0;
            }
            tempVal = 0;

            int? benWomen = null;
            if (project.BeneficiariesWomen != "0")
            {
                int.TryParse(project.BeneficiariesWomen, out tempVal);
                benWomen = tempVal > 0 ? tempVal : (int?)null;
            }
            else
            {
                benWomen = 0;
            }
            tempVal = 0;

            int? benOther = null;
            if (project.BeneficiariesOthers != "0")
            {
                int.TryParse(project.BeneficiariesOthers, out tempVal);
                benOther = tempVal > 0 ? tempVal : (int?)null;
            }
            else
            {
                benOther = 0;
            }
            tempVal = 0;

            int? benTotal = null;
            if (project.BeneficiaryTotalNumber != "0")
            {
                int.TryParse(project.BeneficiaryTotalNumber, out tempVal);
                benTotal = tempVal > 0 ? tempVal : (int?)null;
            }
            else
            {
                benTotal = 0;
            }
            tempVal = 0;

            string objectives = null;
            string needs = null;
            string activites = null;
            string indicatorOutcomes = null;
            foreach (var description in descriptions)
            {
                switch (description.Type)
                {
                    case "objectives":
                        {
                            objectives = description.DescriptionText;
                            break;
                        }
                    case "needs":
                        {
                            needs = description.DescriptionText;
                            break;
                        }
                    case "activities":
                        {
                            activites = description.DescriptionText;
                            break;
                        }
                    case "indicators/outcomes":
                        {
                            indicatorOutcomes = description.DescriptionText;
                            break;
                        }
                    default:
                        break;
                }
            }

            return new object[] { RC.EmergencySahel2015, projectId, project.ProjectCode, project.ProjectTitle, objectives, 
                                    project.CountryName, clusterName, projectStatusId, project.OPSProjectStatus, 
                                    project.ProjectYear, project.ProjectContactName, project.ProjectContactPhone,
                                    project.ProjectContactEmail, RC.GetCurrentUserId,
                                    needs, activites, indicatorOutcomes,
                                    project.ProjectStartDate,
                                    project.ProjectEndDate,
                                    benChildren, benWomen, benTotal,
                                    project.BeneficiariesTotalDescription,
                                    benOther,
                                    project.BeneficiariesDescription,
                                    project.ProjectImplementingpartner,
                                    project.RelatedURL,
                                    project.Type,
                                    project.OPSLastUpdatedDate,
                                    project.OPSLastUpdatedBy,
                                    project.OPSClusterName,
                                    project.GenderMarker,
                                    project.EGFLocations,
                                    project.PriorityName,
                                    project.Refugee,
                                    (int)RC.Year._2016,
                                    DBNull.Value};
        }


    }
}