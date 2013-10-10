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
    <link rel="stylesheet" href="../Styles/ui-lightness/jquery-ui-1.10.3.custom.min.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker();
            $("#<%=txtToDate.ClientID%>").datepicker();
        });
    </script>
    <script>
        function radioMe(e) {
            if (!e) e = window.event;
            var sender = e.target || e.srcElement;

            if (sender.nodeName != 'INPUT') return;
            var checker = sender;
            var chkBox = document.getElementById('<%= chkDuration.ClientID %>');
            var chks = chkBox.getElementsByTagName('INPUT');
            for (i = 0; i < chks.length; i++) {
                if (chks[i] != checker)
                    chks[i].checked = false;
            }
        }
    </script>
    <script>
        function getSVG1() {
            var j = 0;

            $(".chartsclass").each(function (i, obj) {

                var svg = $(obj).html();

//                var indexOf = svg.indexOf('class="highcharts-container">') + 29;
//                var len = svg.length;
//                svg = svg.substr(indexOf, len);
//                var lastIndexOf = svg.lastIndexOf("</div>");
//                svg = svg.substring(0, lastIndexOf);
                
                $.ajax({
                    type: "POST",
                    url: "../WebService2.asmx/SaveSVG1",
                    data: "{'svg':'" + svg + "', i:'" + i + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                    },
                    error: function (msg) {
                        alert('failuer');
                    }
                });
                j = i;
            });

            setTimeout(function () { genPDF(j); }, 9000)

        }

        function genPDF(j) {

            $.ajax({
                type: "POST",
                url: "../WebService2.asmx/GeneratePDF",
                data: "{'j':'" + j + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                },
                error: function (msg) {

                }
            });
        }
        
    </script>
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
                            <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            TO:
                            <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
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
                            <cc:DropDownCheckBoxes ID="ddlData" runat="server" CssClass="ddlWidth" AddJQueryReference="True" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlData_SelectedIndexChanged"
                                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
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
                    <tr>
                        <td>
                            Duration Type:
                        </td>
                        <td>
                            <asp:CheckBoxList ID="chkDuration" runat="server" RepeatColumns="3">
                                <asp:ListItem Text="Monthly" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Quarterly" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Yearly" Value="3"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdTest" runat="server">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30" HeaderText="#">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep>
                <div style="width: 100%;">
                    <div style="width: 100%; float: left;">
                        <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
                    </div>
                    <div style="width: 40%; float: left;">
                        <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
                    </div>
                </div>
            </asp:WizardStep>
        </WizardSteps>
        <StartNavigationTemplate>
            <asp:Button ID="btnNext" runat="server" Text="Next >>" CausesValidation="true" CommandName="MoveNext" />
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <asp:Button ID="btnPrevious" runat="server" Text="<< Previous" CausesValidation="false"
                CommandName="MovePrevious" />
            <asp:Button ID="btnNext" runat="server" Text="Next >>" CausesValidation="true" CommandName="MoveNext" />
        </StepNavigationTemplate>
        <FinishNavigationTemplate>
            <asp:Button ID="btnPreviousFinish" runat="server" Text="<< Previous" CausesValidation="false"
                CommandName="MovePrevious" />
            <input type="button" name="btnname" value="Generate PDF" id="btnExport" />            
        </FinishNavigationTemplate>
    </asp:Wizard>
    <script>
        $(function () {
            $('#btnExport').click(function () {
                getSVG1();
            });
        });
    </script>
</asp:Content>
