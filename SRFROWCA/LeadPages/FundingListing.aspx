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

        <div class="row">
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                <asp:GridView ID="gvFunding" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                    CssClass="imagetable"
                    Width="100%">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="FTSDonorName" HeaderText="Donor Name" />
                        <asp:BoundField DataField="FTSReceipentOrganization" HeaderText="Organization" />
                        <asp:BoundField DataField="FTSEmergencyTitle" HeaderText="Emergency" />
                        <asp:BoundField DataField="FTSAppealTitle" HeaderText="Appeal" />
                        <asp:BoundField DataField="FTSProjectCode" HeaderText="FTS Project Code" />
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
                    </Columns>

                </asp:GridView>
            </div>
            <div class="space">
            </div>
        </div>
    </div>
</asp:Content>
