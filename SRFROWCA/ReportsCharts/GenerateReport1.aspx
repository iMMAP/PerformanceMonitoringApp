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
        #modal-overlay
        {
            position: fixed;
            z-index: 10;
            background: black;
            display: block;
            opacity: .60;            
            width: 100%;
            height: 100%;
            font-size:xx-large;
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

        function occurrences(string, subString, allowOverlapping) {
            string += ""; subString += "";
            if (subString.length <= 0) return string.length + 1;

            var n = 0, pos = 0;
            var step = (allowOverlapping) ? (1) : (subString.length);

            while (true) {
                pos = string.indexOf(subString, pos);
                if (pos >= 0) { n++; pos += step; } else break;
            }
            return (n);
        }

        function getSVG1() {
            try {
                var j = 0;

                $(".chartsclass").each(function (i, obj) {
                    var svg = $(obj).html();
                    var id = obj.id;
                    var durationType = '';
                    var yearId = '';

                    var logFrameId = id.substring(5, id.indexOf('_'));
                    if (id.indexOf('ye') != -1) {
                        durationType = id.substring(id.indexOf('_') + 1, id.indexOf('deys'));
                        yearId = id.substring(id.indexOf('ys') + 2, id.indexOf('ye'));
                    }
                    var chartType = id.substring(4, 5);

                    var indexOf = svg.indexOf('<svg');
                    var len = svg.length;
                    svg = svg.substr(indexOf);
                    var lastIndexOf = svg.lastIndexOf("</div>");
                    svg = svg.substring(0, lastIndexOf);
                    var xmlnsString = 'xmlns="http://www.w3.org/2000/svg"';
                    var n = occurrences(svg, xmlnsString, false);
                    if (n > 1) {
                        svg = svg.replace(xmlnsString, '');
                    }

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: "../WebService2.asmx/SaveSVGOnDisk",
                        data: "{'svg':'" + svg + "', logFrameId:'" + logFrameId + "', durationType:'" + durationType + "', yearId:'" + yearId + "', chartType:'" + chartType + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                        },
                        error: function (msg) {
                            alert('Failure');
                        }
                    });
                    j = i;
                });
            }
            finally {
                AjaxFinished();
            }
        }

        function AjaxFinished() {
            $("#modal-overlay").hide();
            $('.classbtnprevious').show()
            $(".classbtndownload").show();            
            $(".classusermessage").text("Please download your report by clicking on 'Download Report' button!");
        }

        $(document).ajaxStart(function () {
            $(".classusermessage").text("");
            $("#modal-overlay").show();
            $('#btnExport').hide();
            $('.classbtnprevious').hide();
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="wzrdReport" runat="server" OnNextButtonClick="wzrdReport_NextButtonClick"
        OnPreviousButtonClick="wzrdReport_PreviousButtonClick">
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
                            Chart Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlChartType" runat="server">
                                <asp:ListItem Text="Area" Value="2"></asp:ListItem>
                                <asp:ListItem Text="AreaSpline" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Bar" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Column" Value="4" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Line" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Spline" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Scatter" Value="7"></asp:ListItem>
                            </asp:DropDownList>
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
                <div id="modal-overlay" style="display: none;">
                    Please Wait!<br />
                    Report Generation Is In Progress... <br />
                    This may take a while, depending on your selected options.
                </div>
                <div class="classusermessage">
                    <b>You have successfully selected all the options. Please click on 'Generate Report'
                        button.
                        <br />
                        It will take a while to generate your report, depending on the options you have
                        selected!</b>
                </div>
                <div style="display: none;">
                    <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
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
            <asp:Button ID="btnPreviousFinish" runat="server" Text="<< Previous" CausesValidation="false" class="classbtnprevious"
                CommandName="MovePrevious" />
            <input type="button" name="btnname" value="Prepare Report" id="btnExport" />
            <asp:Button ID="btnDownload" runat="server" Text="Download Report" CausesValidation="false"
                Style="display: none;" class="classbtndownload" OnClick="btnDownload_Click" />
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
