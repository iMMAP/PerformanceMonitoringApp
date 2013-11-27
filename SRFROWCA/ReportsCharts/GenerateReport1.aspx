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
            z-index: 5;
            background: black;
            display: block;
            opacity: .60;
            width: 60%;
            height: 10%;
            font-size: xx-large;
        }
        #progressBar
        {
            width: 400px;
            height: 22px;
            border: 1px solid #111;
            background-color: #292929;
        }
        
        #progressBar div
        {
            height: 100%;
            color: #fff;
            text-align: right;
            line-height: 22px; /* same as #progressBar height if we want text middle aligned */
            width: 0;
            background-color: #0099ff;
        }
    </style>
    <link rel="stylesheet" href="../Styles/ui-lightness/jquery-ui-1.10.3.custom.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="wzrdReport" runat="server" OnNextButtonClick="wzrdReport_NextButtonClick"
        CssClass="containerWizard" OnPreviousButtonClick="wzrdReport_PreviousButtonClick">
        <WizardSteps>
            <asp:WizardStep ID="wsFrist" runat="server" Title="Step 1">
                <div class="containerWizardStep">
                    <div class="graybar">
                        Select Your Options To Generate Report
                    </div>
                    <div class="contentarea">
                        <div class="formdiv">
                            <table>
                                <tr>
                                    <td>
                                        <label>
                                            Country:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="300px"
                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" InitialValue="0" ControlToValidate="ddlCountry"
                                            Text="Required" ErrorMessage="Country Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Admin1:</label>
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
                                        <label>
                                            Admin2:</label>
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
                                        <label>
                                            Report Location Level:</label>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbCountry" runat="server" Text="Country" Checked="true" GroupName="Location" />
                                        <asp:RadioButton ID="rbAdmin1" runat="server" Text="Admin 1" GroupName="Location" />
                                        <asp:RadioButton ID="rbAdmin2" runat="server" Text="Admin 2" GroupName="Location" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Emergency:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmergency" runat="server" AutoPostBack="true" Width="300px"
                                            OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Organization:</label>
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
                                        <label>
                                            Clusters:</label>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="cblClusters" runat="server" RepeatColumns="3">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                    </div>
                    <div class="graybarcontainer">
                    </div>
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                <div class="containerWizardStep">
                    <div class="graybar">
                        Select Your Options To Generate Report
                    </div>
                    <div class="contentarea">
                        <div class="formdiv">
                            <table border="0">
                                <tr>
                                    <td>
                                        <label>
                                            Objectives:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlObjectives" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlObjectives_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Objectives" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Inidicators:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlIndicators" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlIndicators_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Indicators" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Activities:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlActivities" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlActivities_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Activities" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Data:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlData" runat="server" CssClass="ddlWidth" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Data" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Date:</label>
                                    </td>
                                    <td>
                                        <label>
                                            From:</label>
                                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                        <label>
                                            To:</label>
                                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Accumulate ON:</label>
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
                                        <label>
                                            Chart Type :</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlChartType" runat="server" Width="200px">
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
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                    </div>
                    <div class="graybarcontainer">
                    </div>
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </asp:WizardStep>
            <asp:WizardStep>
                <div class="containerWizardStep">
                    <div class="graybar">
                        Generate Report
                    </div>
                    <div class="contentarea">
                        <div class="formdiv">
                            <div id="progressBar" style="display: none;">
                                <div>
                                </div>
                            </div>
                            <div id="modal-overlay" style="display: none;">
                                <div style="margin: 0 auto; width: 100%">
                                    <img src="../images/ajaxlodr.gif" alt="In Progress..." />
                                </div>
                            </div>
                            <div id="divMessage" runat="server" class="classusermessage">
                                <b>You have successfully selected all the options. Please click on 'Generate Report'
                                    button.
                                    <br />
                                    It will take a while to generate your report, depending on the options you have
                                    selected!</b>
                            </div>
                            <div style="display: none;">
                                <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                    </div>
                    <div class="graybarcontainer">
                    </div>
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </asp:WizardStep>
        </WizardSteps>
        <StartNavigationTemplate>
            <asp:Button ID="btnNext" runat="server" Text="Next >>" CausesValidation="true" CommandName="MoveNext"
                CssClass="button_example" />
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <asp:Button ID="btnPrevious" runat="server" Text="<< Previous" CausesValidation="false"
                CssClass="button_example" CommandName="MovePrevious" />
            <asp:Button ID="btnNext" runat="server" Text="Next >>" CausesValidation="true" CommandName="MoveNext"
                CssClass="button_example" />
        </StepNavigationTemplate>
        <FinishNavigationTemplate>
            <asp:Button ID="btnPreviousFinish" runat="server" Text="<< Previous" CausesValidation="false"
                CssClass="button_example classbtnprevious" class="classbtnprevious" CommandName="MovePrevious" />
            <input type="button" name="btnname" value="Prepare Report" id="btnExport" class="button_example" />
            <asp:Button ID="btnDownload" runat="server" Text="Download Report" CausesValidation="false"
                CssClass="button_example classbtndownload" Style="display: none;" OnClick="btnDownload_Click"
                OnClientClick="HideButton();" />
        </FinishNavigationTemplate>
    </asp:Wizard>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#btnExport').click(function () {
                getSVG1();
            });
        });
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

        function progressBar(percent, $element) {
            var progressBarWidth = percent * $element.width() / 100;
            $element.find('div').animate({ width: progressBarWidth }, 0).html(percent + "%&nbsp;");
        }

        function getSVG1() {
            try {
                var j = 0;

                var numberOfImages = $(".chartsclass").length,
		        numberOfLoaded = 0,
		        step = (100 / numberOfImages).toFixed(2);

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
                        url: "../ChartSVGService.asmx/SaveSVG",
                        data: "{'svg':'" + svg + "', logFrameId:'" + logFrameId + "', durationType:'" + durationType + "', yearId:'" + yearId + "', chartType:'" + chartType + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            numberOfLoaded++;

                            progressBar((step * numberOfLoaded).toFixed(2), $('#progressBar'));
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
                progressBar(100, $('#progressBar'));
            }
        }

        function AjaxFinished() {
            $("#modal-overlay").hide();
            $("#progressBar").hide();
            $('.classbtnprevious').show()
            $(".classbtndownload").show();
            $(".info2").html("<h2>Report Generated.</h2> <p>Please download your report by clicking on 'Download Report' button!</p>");
        }

        $(document).ajaxStart(function () {
            $(".classusermessage").html("");
            $("#modal-overlay").show();
            $("#progressBar").show();
            $('#btnExport').hide();
            $('.classbtnprevious').hide();
        });

        function HideButton() {
            $(".info2").html("<p>Please use previous button to select options to generate report!</p>");
            $(".classbtndownload").hide();
        }
        
    </script>
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
</asp:Content>
