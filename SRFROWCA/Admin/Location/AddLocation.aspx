<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"
    CodeBehind="AddLocation.aspx.cs" Inherits="SRFROWCA.Admin.Location.AddLocation" %>

<%@ Register Assembly="Artem.Google" Namespace="Artem.Google.UI" TagPrefix="map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width: 50%; float: left;">
        <map:GoogleMap id="gmapView" runat="server" key="" width="750px" height="550" address="West Africa"
            zoom="5">
        </map:googlemap>
    </div>
    <div style="width: 35%; float: right;">
        <table>
            <tr>
                <td height="45">
                    <div class="poph41">
                        Location Level:</div>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="input1" Width="200px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlLocationType_SelectedIndexChanged">
                        <asp:ListItem Text="Select Location Type" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Country" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Governorate" Value="3"></asp:ListItem>
                        <asp:ListItem Text="District" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Sub-district" Value="5"></asp:ListItem>
                        <asp:ListItem Text="Village" Value="6"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td height="45">
                    <div class="poph41">
                        Country:</div>
                </td>
                <td width="77%" height="45">
                    <label for="textfield3">
                    </label>
                    <asp:DropDownList ID="ddlProvince" runat="server" Width="200px" CssClass="input"
                        OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" AutoPostBack="true"
                        Enabled="false">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtProvince" runat="server" Width="200px" CssClass="input1" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="45">
                    <div class="poph41">
                        Governorate:</div>
                </td>
                <td height="45">
                    <asp:DropDownList ID="ddlDistrict" runat="server" Width="200px" CssClass="input"
                        OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true"
                        Enabled="false">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtDistrict" runat="server" Width="200px" CssClass="input1" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="45">
                    <div class="poph41">
                        District:</div>
                </td>
                <td height="45">
                    <asp:DropDownList ID="ddlTehsil" runat="server" Width="200px" CssClass="input" Enabled="false"
                        OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtTehsil" runat="server" Width="200px" CssClass="input1" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="45">
                    <div class="poph41">
                        Sub-Distirct:</div>
                </td>
                <td height="45">
                    <asp:DropDownList ID="ddlUC" runat="server" Width="200px" CssClass="input" Enabled="false"
                        OnSelectedIndexChanged="ddlUC_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtUC" runat="server" Width="200px" CssClass="input1" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="45">
                    <div class="poph41">
                        Village:</div>
                </td>
                <td height="45">
                    <asp:DropDownList ID="ddlVillage" runat="server" Width="200px" CssClass="input" Enabled="false">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtVillage" runat="server" Width="200px" CssClass="input1" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="20" colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td height="50" colspan="2">
                    <asp:Button ID="btnAddLocation" runat="server" CssClass="button1" Text="Add Location"
                        OnClick="btnAddLocation_Click" ValidationGroup="vgNewActivity" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
