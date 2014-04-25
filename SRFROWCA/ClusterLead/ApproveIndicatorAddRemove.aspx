<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApproveIndicatorAddRemove.aspx.cs" Inherits="SRFROWCA.ClusterLead.ApproveIndicatorAddRemove" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbCountryIndicators" runat="server" Text="Approve/Reject Requested Add/Delete indicators"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-sm-3">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="Localize2" runat="server" Text="Projects"></asp:Localize>
                                        </h5>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                                                    >
                                                </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-9">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <button runat="server" id="btnApprove" onserverclick="btnApprove_Click" class="btn btn-success">
                            <i class="icon-ok bigger-110"></i>Approve
                        </button>
                        <button runat="server" id="btnReject" onserverclick="btnReject_Click" class="btn btn-danger">
                            <i class="icon-remove bigger-110"></i>Reject
                        </button>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="Localize1" runat="server" Text="Indicaotrs Added"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:GridView ID="gvAdded" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId,ProjectIndicatorId" CssClass="imagetable"
                                                Width="100%" OnRowDataBound="gvAdded_RowDataBound" EmptyDataText="No Add Indicator request">
                                                <HeaderStyle BackColor="Control"></HeaderStyle>
                                                <RowStyle CssClass="istrow" />
                                                <AlternatingRowStyle CssClass="altcolor" />
                                                <Columns>
                                                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px">
                                                        <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId">
                                                        <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                                            <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ProjectCode" HeaderText="Project">
                                                        <ItemStyle CssClass="testact"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                                        SortExpression="ActivityName" meta:resourcekey="BoundFieldResource4">
                                                        <ItemStyle CssClass="testact"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DataName" HeaderText="Indicator" ItemStyle-CssClass="testind"
                                                        SortExpression="Indicator" ItemStyle-Wrap="true">
                                                        <ItemStyle Wrap="True" CssClass="testind"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="40px">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkApproved" runat="server" CssClass="testcb" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndProjects" runat="server"
                                                Text="Indicators Removed"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="slim-scroll" data-height="200">
                                            <div class="widget-main">
                                                <asp:GridView ID="gvDeleted" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                                    HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId,ProjectIndicatorId" CssClass="imagetable"
                                                    Width="100%" OnRowDataBound="gvDeleted_RowDataBound" EmptyDataText="No Remove Indicator request">
                                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                                    <RowStyle CssClass="istrow" />
                                                    <AlternatingRowStyle CssClass="altcolor" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px">
                                                            <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                            <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId">
                                                            <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                            <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                                                <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ProjectCode" HeaderText="Project">
                                                            <ItemStyle CssClass="testact"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                                            SortExpression="ActivityName">
                                                            <ItemStyle CssClass="testact"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DataName" HeaderText="Indicator" ItemStyle-CssClass="testind"
                                                            ItemStyle-Wrap="true">
                                                            <ItemStyle Wrap="True" CssClass="testind"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="40px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkApproved" runat="server" CssClass="testcb" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
