<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditOutputIndicator.aspx.cs" Inherits="SRFROWCA.ClusterLead.EditOutputIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
    <asp:HiddenField runat="server" ID="hdnIsRegional" Value="0" />
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Edit Output Indicator</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <div style="display: block; width: 100%; margin-bottom: 20px;">
            <div id="dvcluster" runat="server" style="float: left; width: 50%;">

                <label>
                    Cluster:</label>
                <div>
                    <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div id="dvCountry" runat="server" style="float: left; width: 50%;">

                <label>
                    Country:</label>
                <div>
                    <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" CssClass="width-90">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                </div>
            </div>

        </div>


        <div style="display: block; width: 100%; margin-bottom: 15px;">
            <div style="width: 80%; margin-top: 120px; margin-bottom: 0px; display: block;">
                <asp:Localize ID="localIndicatorInfo" runat="server" meta:resourcekey="localIndicatorInfoResource1"></asp:Localize>
            </div>
            <h6 class="header blue bolder smaller">Indicator<asp:Label ID="lbl1stNumber" runat="server" meta:resourcekey="lbl1stNumberResource1"></asp:Label></h6>

            <div class="col-xs-12 col-sm-12 dvIndicator" style="padding-left: 0px;">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-bottom no-padding-top">
                            <div style="float: left; width: 40%;">
                                <label>
                                    <asp:HiddenField ID="hfIndicatorId" runat="server" />
                                    (English):</label>
                                <div>
                                    <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1EngResource1" Style="height: 100px;"></asp:TextBox>
                                    <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateActivity"
                                        CssClass="error2"></asp:CustomValidator>

                                </div>
                            </div>
                            <div style="float: left; width: 40%;">
                                <label>
                                    (French):</label>
                                <div>
                                    <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1FrResource1" Style="height: 100px;"></asp:TextBox>

                                </div>
                            </div>
                            <div style="float: left; width: 15%;">
                                <label>
                                    Unit:</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-95" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
            <div class="widget-box no-border">
                <div class="widget-body">
                    <div class="widget-main no-padding-bottom no-padding-top">
                        <div>
                            <div style="float: left; cursor: pointer;" onclick="$(this).parent().find('.content').toggle();$(this).find('.accordian').text() == '+' ? $(this).find('.accordian').text('-'): $(this).find('.accordian').text('+');">
                                <div class="accordian">+</div>
                                <div style="margin-left: 8px; margin-top: 0px; float: left;">
                                    Admin1 targets
                               
                                </div>
                            </div>
                            <div class="content" style="float: left; clear: both; margin-top: 10px; margin-left: 0px;">
                                <asp:Repeater runat="server" ID="rptAdmin1">
                                    <ItemTemplate>
                                        <div style="float: left; width: 140px; margin-bottom: 20px; margin-right: 20px;">
                                            <div style="float: left; width: 80px; text-align: right; margin-top: 5px;"><%#Eval("LocationName")%>&nbsp;</div>
                                            <asp:TextBox ID="txtTarget" runat="server" MaxLength="8" Text='<%#Eval("Target") %>' Style="width: 80px;" CssClass="numeric1" meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                        </div>
                                        <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%# Eval("LocationId") %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="pull-left">
        
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
            </div>
    </div>
    <div id="divMsg">
    </div>
    <script>
        function validateActivity(sender, args) {
            var txtEng = $("[id$=txtInd1Eng]").val();
            var txtFr = $("[id$=txtInd1Fr]").val();

            if (txtEng.trim() == '' && txtFr.trim() == '') {

                alert("Please add Indicator atleast in one Language!")
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


        $(document).ready(function () {
            $(".content").hide();
        });
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


    </script>
</asp:Content>
