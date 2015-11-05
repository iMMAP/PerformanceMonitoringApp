<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddActivityAndIndicators.aspx.cs"
    Inherits="SRFROWCA.ClusterLead.AddActivityAndIndicators" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

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
        // Non Gender TArget sum
        function calTotal(thisObj) {
            var countrySum = 0;
            thisObj.closest('table.tblCountry').find('.trgtAdmin2').each(function () {
                var adm2ForCountry = 0;
                adm2ForCountry = parseInt($(this).val());
                if (isNaN(adm2ForCountry))
                    adm2ForCountry = 0;
                countrySum += adm2ForCountry;
            });

            var admin1Sum = 0
            txtAdmin2 = thisObj.closest('.tblAdmin1').find('.trgtAdmin2');
            txtAdmin2.each(function () {
                var adm2ForAdm1 = 0;
                adm2ForAdm1 = parseInt($(this).val());
                if (isNaN(adm2ForAdm1))
                    adm2ForAdm1 = 0;
                admin1Sum += adm2ForAdm1;
            });
            thisObj.closest('table.tblCountry').find('.trgtCountry').val(countrySum);
            thisObj.closest('.tblAdmin1').find('.trgtAdmin1').val(admin1Sum);
        }

        // Gender Male Target Sum
        function calTotalGenderMale(thisObj) {
            var countrySumMale = 0;
            thisObj.closest('table.tblCountryGender').find('.trgtAdmin2GenderMale').each(function () {
                var adm2ForCountryMale = 0;
                adm2ForCountryMale = parseInt($(this).val());
                if (isNaN(adm2ForCountryMale))
                    adm2ForCountryMale = 0;
                countrySumMale += adm2ForCountryMale;
            });
            if (countrySumMale == 0)
                countrySumMale = ''
            thisObj.closest('table.tblCountryGender').find('.trgtCountryGenderMale').val(countrySumMale);

            var admin1GenderMaleSum = 0
            txtAdmin2GenderMale = thisObj.closest('.tblAdmin1Gender').find('.trgtAdmin2GenderMale');
            txtAdmin2GenderMale.each(function () {
                var adm2ForAdmin1Male = 0;
                adm2ForAdmin1Male = parseInt($(this).val());
                if (isNaN(adm2ForAdmin1Male))
                    adm2ForAdmin1Male = 0;
                admin1GenderMaleSum += adm2ForAdmin1Male;
            });
            if (admin1GenderMaleSum == 0)
                admin1GenderMaleSum = ''
            thisObj.closest('.tblAdmin1Gender').find('.trgtAdmin1GenderMale').val(admin1GenderMaleSum);
            rowGenderTotal(thisObj);
            rowCountryGenderTotal(thisObj);
            rowAdmin1GenderTotal(thisObj);
        }

        // Gender Female Target Sum
        function calTotalGenderFemale(thisObj) {
            var countrySumFemale = 0;
            thisObj.closest('table.tblCountryGender').find('.trgtAdmin2GenderFemale').each(function () {
                var adm2ForCountryFemale = 0;
                adm2ForCountryFemale = parseInt($(this).val());
                if (isNaN(adm2ForCountryFemale))
                    adm2ForCountryFemale = 0;
                countrySumFemale += adm2ForCountryFemale;
            });
            if (countrySumFemale == 0)
                countrySumFemale = '';
            thisObj.closest('table.tblCountryGender').find('.trgtCountryGenderFemale').val(countrySumFemale);

            var admin1GenderFemaleSum = 0
            txtAdmin2GenderFemale = thisObj.closest('.tblAdmin1Gender').find('.trgtAdmin2GenderFemale');
            txtAdmin2GenderFemale.each(function () {
                var adm2ForAdmin1Female = 0;
                adm2ForAdmin1Female = parseInt($(this).val());
                if (isNaN(adm2ForAdmin1Female))
                    adm2ForAdmin1Female = 0;
                admin1GenderFemaleSum += adm2ForAdmin1Female;
            });
            if (admin1GenderFemaleSum == 0)
                admin1GenderFemaleSum = '';
            thisObj.closest('.tblAdmin1Gender').find('.trgtAdmin1GenderFemale').val(admin1GenderFemaleSum);
            rowGenderTotal(thisObj);
            rowCountryGenderTotal(thisObj);
            rowAdmin1GenderTotal(thisObj);
        }

        function rowGenderTotal(thisObj) {
            var $row = thisObj.closest('tr'),
                maleTarget = parseInt($row.find('.trgtAdmin2GenderMale').val()),
                femaleTarget = parseInt($row.find('.trgtAdmin2GenderFemale').val());

            if (isNaN(maleTarget) || maleTarget == '')
                maleTarget = 0;

            if (isNaN(femaleTarget) || femaleTarget == '')
                femaleTarget = 0;

            var total = maleTarget + femaleTarget;
            $row.find('.trgtAdmin2GenderTotal').val(total)
        }

        function rowCountryGenderTotal(thisObj) {

            var maleTarget = parseInt(thisObj.closest('table.tblCountryGender').find('.trgtCountryGenderMale').val())
            var femaleTarget = parseInt(thisObj.closest('table.tblCountryGender').find('.trgtCountryGenderFemale').val());

            if (isNaN(maleTarget) || maleTarget == '')
                maleTarget = 0;

            if (isNaN(femaleTarget) || femaleTarget == '')
                femaleTarget = 0;

            var total = maleTarget + femaleTarget;
            thisObj.closest('table.tblCountryGender').find('.trgtCountryGenderTotal').val(total)
        }

        function rowAdmin1GenderTotal(thisObj) {
            var $row = thisObj.closest('.tblAdmin1Gender').find('tr.trAdmin1'),
                maleTarget = parseInt($row.find('.trgtAdmin1GenderMale').val()),
                femaleTarget = parseInt($row.find('.trgtAdmin1GenderFemale').val());

            if (isNaN(maleTarget) || maleTarget == '')
                maleTarget = 0;

            if (isNaN(femaleTarget) || femaleTarget == '')
                femaleTarget = 0;

            var total = maleTarget + femaleTarget;
            $row.find('.trgtAdmin1GenderTotal').val(total)
        }

        $(function () {
            $('[data-rel=popover]').popover({ html: true });
            $('.showDetails1').click(function () {
                $(this).parent().parent().next('tr.details1').toggle();
                $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                                                             '../assets/orsimages/minus.png' :
                                                              '../assets/orsimages/plus.png'))
            });

            $('.showall').click(function () {
                if ($(this).text() == 'Expand All') {
                    $('.details1').show();
                    $(this).text('Collapse All');
                    $('.showallGen').text('Collapse All');
                    $('.showDetails1').attr('src', '../assets/orsimages/minus.png');
                }
                else {
                    $('.details1').hide();
                    $(this).text('Expand All');
                    $('.showallGen').text('Expand All');
                    $('.showDetails1').attr('src', '../assets/orsimages/plus.png');
                }
            });

            $('.showallGen').click(function () {
                if ($(this).text() == 'Expand All') {
                    $('.details1').show();
                    $(this).text('Collapse All');
                    $('.showall').text('Collapse All');
                    $('.showDetails1').attr('src', '../assets/orsimages/minus.png');
                }
                else {
                    $('.details1').hide();
                    $(this).text('Expand All');
                    $('.showall').text('Expand All');
                    $('.showDetails1').attr('src', '../assets/orsimages/plus.png');
                }
            });

            // Sum without Gender
            //$(".tblAdmin2").on('change', '.trgtAdmin2', calTotal);
            $(".trgtAdmin2").on('change', function () {
                calTotal($(this));
            });

            // Sum Gender Male
            $(".trgtAdmin2GenderMale").on('change', function () {
                calTotalGenderMale($(this));
            });

            $(".trgtAdmin2GenderFemale").on('change', function () {
                calTotalGenderFemale($(this));
            });

            // Sum Gender Female
            //$(".tblAdmin2Gender").on('change', '.trgtAdmin2GenderFemale', calTotalGenderFemale);

            $('#<%=txtActivityEng.ClientID%>').autocomplete({
                minLength: 4,
                source: function (request, response) {
                    $.ajax({
                        url: "AddActivityAndIndicators.aspx/GetActivities",
                        data: JSON.stringify({
                            "cnId": $("#<%=ddlCountry.ClientID%>").val(),
                            "clId": $("#<%=ddlCluster.ClientID%>").val(),
                            "obId": $("#<%=ddlObjective.ClientID%>").val(),
                            "lngId": "1",
                            "searchTxt": request.term
                        }),
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
                    $('#<%=txtActivityFr.ClientID%>').val(ui.item.valuealt)
                }
            });

            $('#<%=txtActivityFr.ClientID%>').autocomplete({
                minLength: 4,
                source: function (request, response) {
                    $.ajax({
                        url: "AddActivityAndIndicators.aspx/GetActivities",
                        data: JSON.stringify({
                            "cnId": $("#<%=ddlCountry.ClientID%>").val(),
                            "clId": $("#<%=ddlCluster.ClientID%>").val(),
                            "obId": $("#<%=ddlObjective.ClientID%>").val(),
                            "lngId": "2",
                            "searchTxt": request.term
                        }),
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
                    <asp:Button ID="Button1" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" meta:resourcekey="Button1Resource1" />
                    <asp:Button ID="Button2" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="Button2Resource1" />
                    <asp:Button ID="Button3" runat="server" Text="Help"
                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="Button3Resource1" />
                </div>
                <div class="col-xs-4 col-sm-4">
                    <asp:Label ID="lblMSRefugee" runat="server" Text="MS for Refugees Sector Framework"
                        CssClass="label-warning label-lg" Visible="false"></asp:Label>
                    <asp:CheckBox ID="cbMSRefugees" runat="server" Text="Yes" Visible="false" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-xs-4 col-sm-4">
                    <input hidden id="hdActId" value="0" />
                    <asp:Label runat="server" ID="lblCountry" Text="Country:*" meta:resourcekey="lblCountryResource1"></asp:Label>
                    <div>
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-100" meta:resourcekey="ddlCountryResource1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry" meta:resourcekey="rfvCountryResource1"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xs-4 col-sm-4">
                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:*" meta:resourcekey="lblClusterResource1"></asp:Label>
                    <div>
                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-100" meta:resourcekey="ddlClusterResource1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster" meta:resourcekey="rfvClusterResource1"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-xs-4 col-sm-4">
                    <asp:Label ID="lblObj" runat="server" Text="Objective:" meta:resourcekey="lblObjResource1"></asp:Label>
                    <div>
                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-100" meta:resourcekey="ddlObjectiveResource1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-6 col-sm-6">
                    <asp:Label ID="lblActivityEngCap" runat="server" Text="Activity (English)" meta:resourcekey="lblActivityEngCapResource1"></asp:Label>
                    <div>
                        <asp:TextBox ID="txtActivityEng" runat="server" CssClass="width-100 textboxAuto" MaxLength="1000" TextMode="MultiLine" Height="70px" meta:resourcekey="txtActivityEngResource1"></asp:TextBox>
                        <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateActivity" ValidateEmptyText="True"
                            CssClass="error2" meta:resourcekey="cvActivityEngResource1"></asp:CustomValidator>
                    </div>
                </div>
                <div class="col-xs-6 col-sm-6">
                    <asp:Label ID="lblActivitFrCap" runat="server" Text="Activity (French):" meta:resourcekey="lblActivitFrCapResource1"></asp:Label>
                    <div>
                        <asp:TextBox ID="txtActivityFr" runat="server" CssClass="width-100" TextMode="MultiLine" MaxLength="1000" Height="70px" meta:resourcekey="txtActivityFrResource1"></asp:TextBox>

                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server" meta:resourcekey="pnlAdditionalIndicaotrsResource1">
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
        <div class="col-xs-12 col-sm-12">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" meta:resourcekey="btnSaveResource1" />
            <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                CssClass="width-10 btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnBackToSRPListResource1" />
        </div>

    </div>
    <script>
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtActivityEng]").val();
            var txtFr = $("[id$=txtActivityFr]").val();
            var countryId = $("#<%=ddlCountry.ClientID%>").val();
            var clusterId = $("#<%=ddlCluster.ClientID%>").val();
            var objId = $("#<%=ddlObjective.ClientID%>").val();
            var unitId = 0;
            $(".pullUnits").each(function () {
                if ($(this).val() == 0)
                    unitId = -1;
            });

            var calcId = 0;
            $(".pullCalc").each(function () {
                if ($(this).val() == 0)
                    calcId = -1;
            });

            if (countryId == "0" || clusterId == "0" || objId == "0" || unitId == -1 || calcId == -1) {
                args.IsValid = false;
            }
            else if (txtEng.trim() == '' && txtFr.trim() == '') {
                alert("Please add Activity!");
                args.IsValid = false;
            }
            else
                args.IsValid = true;
        }


        function validateIndicator(sender, args) {
            var counter = 0;
            $(".dvIndicator").each(function () {
                var index = 0;
                var txtEng = $(this).find("[id$=txtInd1Eng]").val();
                var txtFr = $(this).find("[id$=txtInd1Fr]").val();
                var countryId = $("#<%=ddlCountry.ClientID%>").val();
                var clusterId = $("#<%=ddlCluster.ClientID%>").val();
                var objId = $("#<%=ddlObjective.ClientID%>").val();
                var unitId = 0;
                $(".pullUnits").each(function () {
                    if ($(this).val() == 0)
                        unitId = -1;
                });

                var calcId = 0;
                $(".pullCalc").each(function () {
                    if ($(this).val() == 0)
                        calcId = -1;
                });

                if (countryId == "0" || clusterId == "0" || objId == "0" || unitId == -1 || calcId == -1) {
                    args.IsValid = false;
                }
                else if (txtEng.trim() == '' && txtFr.trim() == '') {
                    alert("Please add Indicator " + (parseInt(index) + 1));
                    args.IsValid = false;
                }
                else
                    arg.IsValid = true;

                index += 1;
            });
        }

    </script>
</asp:Content>
