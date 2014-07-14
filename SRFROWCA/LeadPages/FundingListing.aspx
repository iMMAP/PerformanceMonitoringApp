<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FundingListing.aspx.cs" Inherits="SRFROWCA.LeadPages.FundingListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbDataEntry" runat="server" Text="Data Entry" meta:resourcekey="localBreadCrumbDataEntryResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
         <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                   <h6>Search Filters</h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Project ID:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtprojectId" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Project:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                  <asp:TextBox ID="txtProject" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                             
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Organization:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtOrg" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Project Sector:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtPrjSector" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                           
                                                        </tr>
                                                            <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Project Country:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtPrjCountry" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Appealing Country:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtApealingCountry" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                           
                                                        </tr>
                                                         <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Project Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:TextBox ID="txtPrjCluster" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                             <td colspan="2">
                                                              <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnSearch_Click" />
                                                            </td>
                                                        </tr>
                                                   
                                                   
                                                        
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>
        <div class="row">
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                <asp:GridView ID="gvFunding" runat="server" AutoGenerateColumns="True" HeaderStyle-BackColor="ButtonFace"
                    CssClass="imagetable"
                    Width="100%">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <%--<Columns>
                        <asp:BoundField DataField="FTSDonorName" HeaderText="Donor Name" />
                        <asp:BoundField DataField="FTSReceipentOrganization" HeaderText="Organization" />
                        <asp:BoundField DataField="FTSEmergencyTitle" HeaderText="Emergency" />
                        <asp:BoundField DataField="FTSAppealTitle" HeaderText="Appeal" />                        
                        <asp:BoundField DataField="EmergencyYear" HeaderText="Emergency Year" />
                        <asp:BoundField DataField="USDCommitedContributed" HeaderText="USD Commited Contributed" />
                        <asp:BoundField DataField="USDPledged" HeaderText="USD Pledged" />
                        <asp:BoundField DataField="OriginalCurrencyAmount" HeaderText="Original Amount" />
                        <asp:BoundField DataField="OriginalCurrencyUnit" HeaderText="Currency Unit" />
                        <asp:BoundField DataField="DecisionDate" HeaderText="Decision Date" />
                        <asp:BoundField DataField="IASCStandardSector" HeaderText="IASC Standard Sector" />
                        <asp:BoundField DataField="DeistinationCountry" HeaderText="Country" />
                        <asp:BoundField DataField="ContributionStatus" HeaderText="Status" />
                        <asp:BoundField DataField="ReportedBy" HeaderText="Reported By" />
                    </Columns>--%>

                </asp:GridView>
            </div>
            <div class="space">
            </div>
        </div>
    </div>
</asp:Content>
