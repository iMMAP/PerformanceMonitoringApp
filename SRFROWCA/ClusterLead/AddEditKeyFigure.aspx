<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddEditKeyFigure.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddEditKeyFigure" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        $(function () {
            $("#expand1").click(function () {
                $("#content1").toggle();
                if ($(this).find("div").text() == "+") {
                    $(this).find("div").text("-").css("line-height", "24px");
                } else {
                    $(this).find("div").text("+").css("line-height", "28px");
                }
            });
            $("#expand2").click(function () {
                $("#content2").toggle();
                if ($(this).find("div").text() == "+") {
                    $(this).find("div").text("-").css("line-height", "24px");
                } else {
                    $(this).find("div").text("+").css("line-height", "28px");
                }
            });
            $("#<%=txtFromDate.ClientID%>").datepicker({
                dateFormat: "mm-dd-yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                    LoadData();
                }
            });

        });

        function ShowOther(obj) {
            if ($(obj).val() == 'Other') {
                $("[id$=txtOther]").removeAttr("disabled");
            } else {
                $("[id$=txtOther]").attr("disabled", "disabled");
            }

        }
        function LoadData() {
            if ($("[id$=ddlCountry]").val() != "0" && $("[id$=ddlCategory]").val() != "0" && $("[id$=txtFromDate]").val() != "") {
                $("[id$=btnLoadData]").click();
            }
        }
    </script>
    <style>
        .accordian {
            height: 20px;
            width: 20px;
            border: 1px solid gray;
            font-size: 24px;
            color: gray;
            line-height: 16px;
            text-align: center;
            float: left;
        }

        input[type="checkbox"] {
            margin-right: 10px;
            margin-top: 2px;
        }

            input[type="checkbox"] + label {
                margin-top: -3px;
            }
    </style>
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
    <asp:Button runat="server" ID="btnLoadData" Style="display: none;" OnClick="btnLoadData_Click" CausesValidation="false" />
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add/Edit Key Figure</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">

        <div style="float: left; width: 100%; margin-top: 20px;">
            <div style="float: left; width: 70px;">
                <label>
                    Date:                                                  
                </label>
            </div>
            <div style="float: left; width: 259px;">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="width-100"></asp:TextBox>

            </div>

        </div>
        <div style="float: left; width: 100%; margin-top: 20px;">
            <div class="col-xs-12 col-sm-12 dvIndicator" style="padding-left: 0px;">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-bottom no-padding-top">
                            <div style="display: block; width: 100%; margin-bottom: 20px;">
                                <div id="dvcluster" runat="server" style="float: left; width: 40%;">
                                    <label>
                                        Category:
                                    </label>
                                    <div>
                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="width-70" onchange="LoadData();">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div id="dvCountry" runat="server" style="float: left; width: 30%;">
                                    <label>
                                        Country:
                                    </label>

                                    <div>
                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-80" onchange="LoadData();">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>

                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div style="float: left; width: 100%; margin-top: 20px;">
            <div class="col-xs-12 col-sm-12 dvIndicator" style="padding-left: 0px;">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-bottom no-padding-top">
                            <div style="float: left; width: 40%;">
                                <label>
                                    Key Figure (English):</label>
                                <div>
                                    <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1EngResource1" Style="height: 100px;"></asp:TextBox>
                                    <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateActivity"
                                        CssClass="error2"></asp:CustomValidator>

                                </div>
                            </div>
                            <div style="float: left; width: 40%;">
                                <label>
                                    Key Figure (French):</label>
                                <div>
                                    <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1FrResource1" Style="height: 100px;"></asp:TextBox>

                                </div>
                            </div>
                            <div style="float: left; width: 15%;">
                                <label>
                                    Unit:</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-95" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" Display="Dynamic"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlUnit"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div style="float: left; width: 100%; margin-top: 20px;">
            <div style="float: left; width: 140px;">
                Population In Need:
            </div>
            <div style="float: left; width: 28.2%;">
                <asp:TextBox runat="server" ID="txtNeed" CssClass="width-90" onkeypress="return isNumber(event)"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" Display="Dynamic"
                                        CssClass="error2" Text="Required" ControlToValidate="txtNeed"></asp:RequiredFieldValidator>
            </div>

            <div style="float: left; width: 120px;">
                Population Target:
            </div>
            <div style="float: left; width: 314px;">
                <asp:TextBox runat="server" ID="txtPopulationTarget" CssClass="width-90" onkeypress="return isNumber(event)"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required" Display="Dynamic"
                                        CssClass="error2" Text="Required" ControlToValidate="txtPopulationTarget"></asp:RequiredFieldValidator>
            </div>

        </div>

        <div style="float: left; width: 37.9%;">
            
            <div id="expand1" style="float: right; width: 67%; margin-top: 20px; height: 30px; border: solid 1px #B5B5B5; cursor: pointer;display:none;">
                Disaggregated Figures
                <div style="float: left; width: 50px; line-height: 28px; height: 29px; border-right: solid 1px #B5B5B5; font-size: 24px; text-align: center; color: #B5B5B5;">+</div>
                
            </div>
            
            <div style="float: left; width: 100%; margin-top: 10px;" id="content1">
                
                <div style="float: right; width: 200px; text-align: right;">
                    Men:&nbsp;<asp:TextBox runat="server" ID="txtNeedMen" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Women:&nbsp;<asp:TextBox runat="server" ID="txtNeedWomen" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Girls:&nbsp;<asp:TextBox runat="server" ID="txtNeedGirls" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Boys:&nbsp;<asp:TextBox runat="server" ID="txtNeedBoys" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
            </div>
        </div>
        <div style="float: left; width: 38.9%;">
            <div id="expand2" style="float: right; width: 65%; margin-top: 20px; height: 30px; border: solid 1px #B5B5B5; cursor: pointer; display:none;">
                Disaggregated Figures
                <div style="float: left; width: 50px; line-height: 28px; height: 29px; border-right: solid 1px #B5B5B5; font-size: 24px; text-align: center; color: #B5B5B5;">+</div>
            </div>

            <div style="float: left; width: 100%; margin-top: 10px;" id="content2">
                <div style="float: right; width: 200px; text-align: right;">
                    Men:&nbsp;<asp:TextBox runat="server" ID="txtTargetMen" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Women:&nbsp;<asp:TextBox runat="server" ID="txtTargetWomen" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Girls:&nbsp;<asp:TextBox runat="server" ID="txtTargetGirls" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
                <div style="float: right; width: 200px; text-align: right; clear: both; margin-top: 5px;">
                    Boys:&nbsp;<asp:TextBox runat="server" ID="txtTargetBoys" CssClass="width-50" Style="float: right;" onkeypress="return isNumber(event)"></asp:TextBox>
                </div>
            </div>
        </div>

        <div style="float: left; width: 100%; margin-top: 20px;">
            <div style="float: left; width: 140px;">
                Source:
            </div>
            <div style="float: left; width: 315px;">
                <asp:DropDownList ID="ddlSource" runat="server" CssClass="width-90" onchange="ShowOther(this);">
                    <asp:ListItem Text="--- Select Source ---" Value="0"></asp:ListItem>
                    <asp:ListItem Text="SRP" Value="SRP"></asp:ListItem>
                    <asp:ListItem Text="HNO" Value="HNO"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required" Display="Dynamic"
                                        CssClass="error2" Text="Required" InitialValue="0" ControlToValidate="ddlSource"></asp:RequiredFieldValidator>
            </div>
             <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="validateSource"
                                        CssClass="error2"></asp:CustomValidator>
            <div style="float: left; width: 120px;">
                Other:
            </div>
            <div style="float: left; width: 314px;">
                <asp:TextBox runat="server" ID="txtOther" CssClass="width-90" Enabled="false"></asp:TextBox>

            </div>

        </div>



        <div style="float: left; margin-top: 30px;">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
            <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
        </div>
    </div>
    <div id="divMsg">
    </div>
    <script>
        function validateSource(sender, args) {
            if ($("[id$=ddlSource]").val() == "Other" && $("[id$=txtOther]").val() == "") {
                alert("Please enter other source!")
                args.IsValid = false;
                return false;
            }
        }
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtInd1Eng]").val();
            var txtFr = $("[id$=txtInd1Fr]").val();

            if (txtEng.trim() == '' && txtFr.trim() == '') {

                alert("Please add Key Figure atleast in one Language!")
                args.IsValid = false;
                return false;
            }
            else {
                // validateUnit();


            }
        }

        function validateUnit() {
            var counter = 0;
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

        }



        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
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

        if ($("[id$=ddlSource]").val() == "Other") {
            $("[id$=txtOther]").removeAttr("disabled");
        }
    </script>
</asp:Content>
