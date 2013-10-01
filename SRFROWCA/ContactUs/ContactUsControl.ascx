<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUsControl.ascx.cs"
    Inherits="SRFROWCA.ContactUs.ContactUsControl" %>
<table style="margin: auto;">
    <tr>
        <td>
            <span class="label1">Please use following fields to send us your query, request, comments
                etc.</span>
        </td>
    </tr>
    <tr>
        <td>
            </br>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label1">Name:</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="addfields2" MaxLength="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label1">Email:</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="500px" CssClass="addfields2" MaxLength="100"></asp:TextBox>            
        </td>
    </tr>
    <tr>
        <td>
            <span class="label1">Subject:</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtSubject" runat="server" Width="500px" CssClass="addfields2" MaxLength="100"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <span class="label1">Message (300 chars max):</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtMessage" runat="server" Width="500px" Height="150px" CssClass="addfields1"
                TextMode="MultiLine" MaxLength="300"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right">
            <asp:Button ID="btnSend" runat="server" Text="Send Message" CausesValidation="false"
                OnClick="txtSend_Click" CssClass="buttonA" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                ViewStateMode="Disabled"></asp:Label>
        </td>
    </tr>
</table>
