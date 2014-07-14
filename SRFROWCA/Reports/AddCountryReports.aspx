<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCountryReports.aspx.cs" Inherits="SRFROWCA.Reports.AddCountryReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        Location:<asp:TextBox ID="txtEnLocationId" runat="server" CssClass="width-20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnLocationId"></asp:RequiredFieldValidator>
            <%--Type:<asp:TextBox ID="txtEnReportTypeId" runat="server" CssClass="width-20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnReportTypeId"></asp:RequiredFieldValidator>--%>
        Public:<asp:TextBox ID="txtPublic" runat="server"></asp:TextBox>
        <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
        </asp:Panel>
        <div class="pull-right">
            <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click"
                causesvalidation="false" class="btn spinner-down btn-xs btn-danger" type="button"
                visible="false">
                <i class="icon-minus smaller-75"></i>
            </button>
            <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click"
                causesvalidation="false" class="btn spinner-up btn-xs btn-success" type="button">
                <i class="icon-plus smaller-75"></i>
            </button>
        </div>
        <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
            title="Save">
            <i class="icon-ok bigger-110"></i>Save
        </button>
    </div>

</asp:Content>
