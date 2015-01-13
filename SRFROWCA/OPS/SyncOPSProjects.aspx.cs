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
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1075.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1076.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1077.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1078.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1079.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1080.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1081.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1082.xml");
            InsertUpdateProjects("http://ops.unocha.org/api/v1/project/appeal/1083.xml");

            Response.Write("All Synced!");
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
            //XDocument doc = XDocument.Load(url);
            int so = 0;
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
                    //PriorityId = (string)x.Element("priority").Attribute("id"),
                    //PriorityName = (string)x.Element("priority"),
                    CountryId = (string)x.Element("country").Attribute("id"),
                    CountryName = (string)x.Element("country"),
                    OtherFields = (string)x.Element("other_fields"),
                    EGFLocations = (string)x.Element("egf_locations")
                    ////Organizations = new OPSProjectOrganizations
                    ////{
                    ////    Organization = x.Descendants("organisations")
                    ////                      .Select(o => new OPSOrganization
                    ////                        {
                    ////                            OrganizationId = (string)o.Element("organization").Attribute("id"),
                    ////                            OrgAbbrevation = (string)o.Element("name").Attribute("abbreviation"),
                    ////                            OrganizationName = (string)o.Element("name"),
                    ////                            OriginalRequirements = (string)o.Element("original_requirement"),
                    ////                            RevisedRequirements = (string)o.Element("revised_requirement")
                    ////                        }).ToList()
                    ////}
                });

            return projects;
        }

        private void SyncProjects(IEnumerable<OPSProject> projects, XDocument doc)
        {
            int k = projects.Count();
            foreach (var project in projects)
            {
                if (project.SectorName != "SECTOR NOT YET SPECIFIED")
                {
                    object[] parameters = GetParameters(project);
                    DBContext.Update("InsertUpdateOPSProject", parameters);
                    Response.Write(project.ProjectId);
                    Response.Write("<br/>");
                    IEnumerable<OPSOrganization> organizations = GetOrganizations(doc, project.ProjectId);
                    object[] orgParameter = GetOrganizationParameter(project.ProjectId, organizations.FirstOrDefault());
                    DBContext.Update("InsertUpdateOPSProjectOrganization", orgParameter);
                }

            }
        }

        private IEnumerable<OPSOrganization> GetOrganizations(XDocument doc, string projectId)
        {
            var a = doc.Root.Elements("project")
                .Descendants("organisations").Descendants("organisation");
            var b = doc.Root.Elements("project")
                .Where(p => ((string)p.Attribute("id")) == projectId)
                .Descendants("organisations").Descendants("organisation");

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

            return new object[] { projectId, organizationId, org.OrganizationName, org.OrgAbbrevation, originalRequest, revisedReques, RC.GetCurrentUserId, DBNull.Value };
        }

        private object[] GetParameters(OPSProject project)
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

            return new object[] { RC.EmergencySahel2015, projectId, project.ProjectCode, project.ProjectTitle, project.ProjectObjective, 
                                    project.CountryName, clusterName, projectStatusId, project.OPSProjectStatus, 
                                    project.ProjectYear, project.ProjectContactName, project.ProjectContactPhone,
                                    project.ProjectContactEmail, RC.GetCurrentUserId, DBNull.Value};
        }
    }
}