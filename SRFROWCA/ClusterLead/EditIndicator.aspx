<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditIndicator.aspx.cs" Inherits="SRFROWCA.ClusterLead.EditIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .accordian
    {
        height:20px;width:20px;border:1px solid gray;font-size:24px;color:gray;line-height: 16px;
text-align: center;float:left;
    }

</style>
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
        <div class="row">
            <div class="col-xs-12 col-sm-12">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main">
                              <div>
                                <div class="row" id="dvCountry" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Country</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                             <div>
                                <div class="row" id="dvcluster" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Cluster</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <h6 class="header blue bolder smaller">
                                        Select Objective</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-90" AutoPostBack="true" OnSelectedIndexChanged="ddlObjective_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                            <div>
                            <div class="row">
                                <h6 class="header blue bolder smaller">
                                    Select Activity</h6>
                                <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
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
                                    </div>
                                </div>
                                </div>
                                </div>
                            <div>
                                <div class="row">
                                     <h6 class="header blue bolder smaller">
                                    Indicator:</h6>
                                <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <label>
                                                    Unit:</label>
                                                <div>
                                                   <asp:DropDownList ID="ddlUnit" runat="server" CssClass="width-90">
                                                        </asp:DropDownList>
                                                       
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                    </div>
                                </div>
                            <div>
                                <div class="row">
                               <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <label>
                                                    Indicator:</label>
                                                <div>
                                                   <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-90"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
                                CssClass="error2" Text="Required" ControlToValidate="txtInd1Eng"></asp:RequiredFieldValidator>
                                                       
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                </div>
                            <div>
                                <div class="row">
                           <div class="col-xs-12 col-sm-12">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div>
                        <div style="float:left;cursor:pointer;" onclick="$(this).parent().find('.content').toggle();$(this).find('.accordian').text() == '+' ? $(this).find('.accordian').text('-'): $(this).find('.accordian').text('+');" >
                        <div class="accordian">+</div>
                            <div style="margin-left:8px;margin-top:0px;float:left;">
                        
                            Admin1 targets
                                </div>
                            </div>
                        <div class="content" style="float:left;clear:both;margin-top:10px;margin-left:0px;">
       <asp:Repeater runat="server" ID="rptAdmin1">
           <ItemTemplate>
               <div style="float:left;width:120px;margin-bottom:20px;margin-right:20px;"><div style="float:left;width:80px;text-align:right;margin-top:5px;" ><%#Eval("LocationName")%>&nbsp;</div><asp:TextBox runat="server" ID="txtTarget" style="width:30px;" Text='<%#Eval("ClusterTarget") %>'></asp:TextBox></div>
               <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%#Eval("LocationId")%>' />
           </ItemTemplate>
       </asp:Repeater>
    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
                        </div>
                                </div>
                            </div>
                      
                       
                        <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
                            title="Save">
                            <i class="icon-ok bigger-110"></i>Save
                        </button>
                        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                   
                </div>
            </div>
            </div>
       
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $(".content").hide();
        });

</script>

</asp:Content>
