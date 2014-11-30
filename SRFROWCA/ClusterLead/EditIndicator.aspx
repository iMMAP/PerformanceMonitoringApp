<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditIndicator.aspx.cs" Inherits="SRFROWCA.ClusterLead.EditIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .accordian
        {
            height: 20px;
            width: 20px;
            border: 1px solid gray;
            font-size: 24px;
            color: gray;
            line-height: 16px;
            text-align: center;
            float: left;
        }

        input[type="checkbox"]
        {
            margin-right: 10px;
            margin-top: 2px;
        }

            input[type="checkbox"] + label
            {
                margin-top: -3px;
            }
    </style>
    <script src="../assets/orsjs/jquery.wholenumber.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $(".numeric1").wholenumber();
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Edit Indicator</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <table>
            <tr>
                <td>
                    <div style="display: block; width: 100%; margin-bottom: 20px;">
                        <div id="dvcluster" runat="server" style="float: left; width: 50%;">

                            <label>
                                Cluster:</label>
                            <div>
                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="dvCountry" runat="server" style="float: left; width: 50%;">

                            <label>
                                Country:</label>
                            <div>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: block; width: 100%;">

                        <div style="float: left; width: 50%;">

                            <label>
                                Objective:</label>

                            <div>
                                <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-90" AutoPostBack="true" OnSelectedIndexChanged="ddlObjective_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                    CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                            </div>
                        </div>




                        <div style="float: left; width: 50%;">

                            <label>
                                Activity:</label>
                            <div>
                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-90">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                    CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlActivity"></asp:RequiredFieldValidator>
                            </div>
                        </div>


                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="col-xs-12 col-sm-12" style="padding-left: 0px;">
                        <div class="widget-box no-border">
                            <div class="widget-body">

                                <div style="float: left; width: 70%;">
                                    <label>
                                        Indicator:</label>
                                    <div>
                                        <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
                                            CssClass="error2" Text="Required" ControlToValidate="txtInd1Eng"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div style="float: left; width: 27%;">
                                    <label>
                                        Unit:</label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-95"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                                            CssClass="error2" Text="Required" ControlToValidate="ddlUnit" InitialValue="0"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
                        <div class="widget-box no-border">
                            <div class="widget-body">

                                <asp:CheckBox runat="server" ID="chkGender" Text="Gender Disagregated" />

                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>

                <td>
                    <div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
                        <div class="widget-box no-border">
                            <div class="widget-body">

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
                                                    <asp:TextBox runat="server" MaxLength="8" CssClass="numeric1" ID="txtTarget" Style="width: 50px;" Text='<%#Eval("ClusterTarget") %>'></asp:TextBox>
                                                </div>
                                                <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%#Eval("LocationId")%>' />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                </td>
            </tr>
            <tr>

                <td>
                    <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
                        title="Save">
                        <i class="icon-ok bigger-110"></i>Save
                       
                    </button>
                    <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                </td>
            </tr>
        </table>

    </div>
    <script>
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
</script>

</asp:Content>
