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
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js"></script>
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
        Highcharts.getSVG = function (charts) {
            var svgArr = [],
        top = 0,
        width = 0;

            $.each(charts, function (i, chart) {
                var svg = chart.getSVG();
                svg = svg.replace('<svg', '<g transform="translate(0,' + top + ')" ');
                svg = svg.replace('</svg>', '</g>');

                top += chart.chartHeight;
                width = Math.max(width, chart.chartWidth);

                svgArr.push(svg);
            });

            return '<svg height="' + top + '" width="' + width + '" version="1.1" xmlns="http://www.w3.org/2000/svg">' + svgArr.join('') + '</svg>';
        };

        /**
        * Create a global exportCharts method that takes an array of charts as an argument,
        * and exporting options as the second argument
        */
        Highcharts.exportCharts = function (charts, options) {
            var form
            svg = Highcharts.getSVG(charts);

            // merge the options
            options = Highcharts.merge(Highcharts.getOptions().exporting, options);

            // create the form
            form = Highcharts.createElement('form', {
                method: 'post',
                action: options.url
            }, {
                display: 'none'
            }, document.body);

            // add the values
            Highcharts.each(['filename', 'type', 'width', 'svg'], function (name) {
                Highcharts.createElement('input', {
                    type: 'hidden',
                    name: name,
                    value: {
                        filename: options.filename || 'chart',
                        type: options.type,
                        width: options.width,
                        svg: svg
                    }[name]
                }, null, form);
            });
            //console.log(svg); return;
            // submit
            form.submit();

            // clean up
            form.parentNode.removeChild(form);
        };

        function getSVG1() {
            $(".chartsclass").each(function (i, obj) {
                var svg = $(obj).html();
                var hfValue = 0; 
                $.ajax({
                    type: "POST",
                    url: "../WebService2.asmx/SaveSVG1",
                    data: "{'svg':'" + svg + "', i:'" + i + "', dataId:'" + hfValue + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {

                    },
                    error: function (msg) {

                    }
                });
            });

            $.ajax({
                type: "POST",
                url: "../WebService2.asmx/GeneratePDF",
                data: "{}",
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
                            <cc:DropDownCheckBoxes ID="ddlData" runat="server" CssClass="ddlWidth" AddJQueryReference="True"
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
                <div style="width: 100%;">
                    <div style="width: 58%; float: left;">
                        <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>                        
                    </div>
                    <div style="width: 40%; float: left;">
                        <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
                    </div>
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
