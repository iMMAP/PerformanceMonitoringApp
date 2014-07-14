using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.LeadPages
{
    public partial class FundingListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFunding();
            }
        }

        private void LoadFunding()
        {
            try
            {
                
                    gvFunding.DataSource = GetFundings();
                    gvFunding.DataBind();
               
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        private DataTable GetFundings()
        {
            string projectId = string.IsNullOrEmpty(txtprojectId.Text) ? null : txtprojectId.Text;
            string project = string.IsNullOrEmpty(txtProject.Text) ? null : txtProject.Text;
            string org = string.IsNullOrEmpty(txtOrg.Text) ? null : txtOrg.Text;
            string projectSector = string.IsNullOrEmpty(txtPrjSector.Text) ? null : txtPrjSector.Text;
            string projectCluster = string.IsNullOrEmpty(txtPrjCluster.Text) ? null : txtPrjCluster.Text;
            string projectCountry = string.IsNullOrEmpty(txtPrjCountry.Text) ? null : txtPrjCountry.Text;
            string apealingCountry = string.IsNullOrEmpty(txtApealingCountry.Text) ? null : txtApealingCountry.Text;
            return DBContext.GetData("GetFundings", new object[] { projectId, project, org, projectSector, projectCluster, projectCountry, apealingCountry });
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadFunding();
        }
    }
}