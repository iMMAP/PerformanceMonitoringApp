<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddContryReportControl.ascx.cs" Inherits="SRFROWCA.Controls.AddContryReportControl" %>


    <div class="col-xs-12 col-sm-12">
        <label>
            (English):</label>
        <div>
            Title:
                            <asp:TextBox ID="txtEnReportTitle" runat="server" CssClass="width-30"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnReportTitle"></asp:RequiredFieldValidator>
            URL:
                            <asp:TextBox ID="txtEnReportURL" runat="server" CssClass="width-30"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnReportURL"></asp:RequiredFieldValidator>
            
            <%--Location:<asp:TextBox ID="txtEnLocationId" runat="server" CssClass="width-20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnLocationId"></asp:RequiredFieldValidator>--%>
            Type:<asp:TextBox ID="txtEnReportTypeId" runat="server" CssClass="width-20"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtEnReportTypeId"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="col-xs-12 col-sm-12">

        <label>
            (French):</label>
        <div>
            Title:
                            <asp:TextBox ID="txtFrReportTitle" runat="server" CssClass="width-30"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtFrReportTitle"></asp:RequiredFieldValidator>
            URL:
                            <asp:TextBox ID="txtFrReportURL" runat="server" CssClass="width-30"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                CssClass="error2" Text="Required" ControlToValidate="txtFrReportURL"></asp:RequiredFieldValidator>
            <%--Location:<asp:TextBox ID="txtFrLocationId" runat="server" CssClass="width-20"></asp:TextBox>            
            Type:<asp:TextBox ID="txtFrReportTypeId" runat="server" CssClass="width-20"></asp:TextBox>--%>
            
        </div>
    </div>


