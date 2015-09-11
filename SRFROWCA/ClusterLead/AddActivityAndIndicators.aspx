<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivityAndIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddActivityAndIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css">
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <style>
        .ui-autocomplete {
            font-size: 11px;
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#<%=txtActivityEng.ClientID%>').autocomplete({
                minLength: 4,
                source: function (request, response) {
                    $.ajax({
                        url: "AddActivityAndIndicators.aspx/GetActivitiesEn",
                        onSelect: function (suggestion) {
                            alert('You selected');
                        },
                        data: "{ 'cnId': '" + $("#<%=ddlCountry.ClientID%>").val() +
                            "', 'clId': '" + $("#<%=ddlCluster.ClientID%>").val() +
                            "', 'obId': '" + $("#<%=ddlObjective.ClientID%>").val() +
                            "', 'pre':'" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item.Activity, valuealt:item.ActivityAlt }
                            }))

                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    $('#<%=txtActivityFr.ClientID%>').val(ui.item.valuealt)
                }
            });

            $('#<%=txtActivityFr.ClientID%>').autocomplete({
                minLength: 4,
                source: function (request, response) {
                    $.ajax({
                        url: "AddActivityAndIndicators.aspx/GetActivitiesFr",
                        data: "{ 'cnId': '" + $("#<%=ddlCountry.ClientID%>").val() +
                            "', 'clId': '" + $("#<%=ddlCluster.ClientID%>").val() +
                            "', 'obId': '" + $("#<%=ddlObjective.ClientID%>").val() +
                            "', 'pre':'" + request.term + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return { value: item.Activity, valuealt: item.ActivityAlt }
                            }))
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (e, ui) {
                    $('#<%=txtActivityEng.ClientID%>').val(ui.item.valuealt)
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <div class="col-xs-12 col-sm-12">
            <div class="row">
                <div class="col-xs-4 col-sm-4">

                    <label>
                        <input hidden id="hdActId" value="0" />
                        Country:</label>
                    <div>
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-100" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xs-4 col-sm-4">
                    <label>
                        Cluster:</label>
                    <div>
                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-100">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xs-4 col-sm-4">

                    <label>
                        Objective:</label>

                    <div>
                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-100">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6 col-sm-6">
                    <label>
                        Activity (English):</label>
                    <div>
                        <asp:TextBox ID="txtActivityEng" runat="server" CssClass="width-100 textboxAuto" TextMode="MultiLine" Height="70px"></asp:TextBox>
                        <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateActivity"
                            CssClass="error2"></asp:CustomValidator>
                    </div>
                </div>
                <div class="col-xs-6 col-sm-6">
                    <label>Activity (French):</label>
                    <div>
                        <asp:TextBox ID="txtActivityFr" runat="server" CssClass="width-100" TextMode="MultiLine" Height="70px"></asp:TextBox>

                    </div>
                </div>
            </div>
        </div>


        <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
        </asp:Panel>

        <div class="pull-right">
            <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-down btn-xs btn-danger" type="button" visible="false">
                <i class="icon-minus smaller-75"></i>
            </button>
            <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-up btn-xs btn-success" type="button">
                <i class="icon-plus smaller-75"></i>
            </button>
        </div>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />

    </div>
    <script>
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtActivityEng]").val();
            var txtFr = $("[id$=txtActivityFr]").val();

            if (txtEng.trim() == '' && txtFr.trim() == '') {

                alert("Please add Activity atleast in one Language!")
                return false;
            }
            else {
                validateUnit();
            }
        }

        function validateUnit() {
            var counter = 0;
            $(".dvIndicator").each(function (index) {
                var txtEng = $(this).find("[id$=txtInd1Eng]").val();
                var txtFr = $(this).find("[id$=txtInd1Fr]").val();
                var unitId = $(this).find("[id$=ddlUnit]").val();

                if (txtEng.trim() !== '' || txtFr.trim() !== '') {
                    if (unitId == "0") {
                        alert("Please select Unit!");
                        args.IsValid = false;
                        return false;
                    }
                }
            });
        }

        //function validateIndicator() {

        //    var counter = 0;
        //    $(".dvIndicator").each(function (index) {
        //        var txtEng = $(this).find("[id$=txtInd1Eng]").val();
        //        var txtFr = $(this).find("[id$=txtInd1Fr]").val();

        //        if (txtEng.trim() == '' && txtFr.trim() == '') {

        //            alert("Please add Indicator " + (parseInt(index)+1) + " atleast in one Language!")
        //            return false;
        //        }
        //    });

        //}
    </script>
</asp:Content>
