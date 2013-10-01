<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GenerateReport1.aspx.cs" Inherits="SRFROWCA.Reports.GenerateReport1" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .ddlWidth
        {
            width: 800px;
        }
        .ddlWidth2
        {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="wzrdReport" runat="server" OnNextButtonClick="wzrdReport_NextButtonClick"
        OnPreviousButtonClick="wzrdReport_PreviousButtonClick" OnFinishButtonClick="wzrdReport_FinishButtonClick">
        <WizardSteps>
            <asp:WizardStep ID="wsFrist" runat="server" Title="Step 1">
                <table>
                    <tr>
                        <td>
                            Country:
                        </td>
                        <td>
                            <%--<cc:DropDownCheckBoxes ID="ddlCountry" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AddJQueryReference="True"
                                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>--%>
                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="300px"
                                OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCountry" runat="server" InitialValue="0" ControlToValidate="ddlCountry"
                                Text="Required" ErrorMessage="Country Required" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admin1:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlAdmin1Locations" runat="server" CssClass="ddlWidth2"
                                AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admin2:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlAdmin2Locations" runat="server" CssClass="ddlWidth2"
                                AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbCountry" runat="server" Text="Country" Checked="true" GroupName="Location" />
                            <asp:RadioButton ID="rbAdmin1" runat="server" Text="Admin 1" GroupName="Location" />
                            <asp:RadioButton ID="rbAdmin2" runat="server" Text="Admin 2" GroupName="Location" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Emergency:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmergency" runat="server" Width="300px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                <table>
                    <tr>
                        <td>
                            Clusters:
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblClusters" runat="server" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Organization:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth2"
                                AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Organization" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            From:
                            <asp:Calendar ID="fromDate" runat="server"></asp:Calendar>
                        </td>
                        <td>
                            TO:
                            <asp:Calendar ID="toDate" runat="server"></asp:Calendar>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                <table>
                    <tr>
                        <td>
                            Objectives:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlObjectives" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlObjectives_SelectedIndexChanged" AddJQueryReference="True"
                                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Inidicators:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlIndicators" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlIndicators_SelectedIndexChanged" AddJQueryReference="True"
                                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Activities:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlActivities" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlActivities_SelectedIndexChanged" AddJQueryReference="True"
                                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Data:
                        </td>
                        <td>
                            <cc:DropDownCheckBoxes ID="ddlData" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                UseSelectAllNode="True">
                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                <Texts SelectBoxCaption="Select Location" />
                            </cc:DropDownCheckBoxes>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Report ON:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblReportOn" runat="server" RepeatColumns="4">
                                <asp:ListItem Text="Objectives" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Indicators" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Activities" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Data" Value="4"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
