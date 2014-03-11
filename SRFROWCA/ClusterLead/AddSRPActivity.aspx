<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddSRPActivity.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddSRPActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain2">
        <div class="graybar">
            Select Objective and Priority
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table>
                    <tr>
                        <td>
                            <label>
                                Objective:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlObjective" runat="server" Width="800px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" CssClass="error2"
                                InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Priority:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" runat="server" Width="800px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" CssClass="error2"
                                InitialValue="0" Text="Required" ControlToValidate="ddlPriority"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="graybar">
            Add Activity
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table>
                    <tr>
                        <td>
                            <label>
                                Activity (English):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtActivityEng" runat="server" Width="700px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="UserName Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtActivityEng"></asp:RequiredFieldValidator>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <label>
                                Activity (French):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtActivityFr" runat="server" Width="700px" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="UserName Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtActivityFr"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="graybar">
            Add Activity
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table>
                    <tr>
                        <td>
                            <label>
                                Indicator 1 (English):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInd1Eng" runat="server" Width="700px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="UserName Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtInd1Eng"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Indicator 1 (French):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInd1Fr" runat="server" Width="700px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="UserName Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtInd1Fr"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Indicator 1 Unit:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUnitsInd1" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required"  CssClass="error2"
                                InitialValue="0" Text="Required" ControlToValidate="ddlUnitsInd1"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Indicator 2 (English):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInd2Eng" runat="server" Width="700px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Indicator 2 (French):</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInd2Fr" runat="server" Width="700px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Indicator 2 Unit:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUnitsInd2" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button_example" />
            <asp:Button ID="btnBackToSRPList" runat="server" Text="Back TO SRP List" OnClick="btnBackToSRPList_Click"
                CssClass="button_example" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
