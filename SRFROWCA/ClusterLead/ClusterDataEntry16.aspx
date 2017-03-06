<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClusterDataEntry16.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .page-content {
            margin: 0;
            padding: 8px 12px 24px;
        }

        .widget-main {
            padding: 2px;
        }

        table.imagetable2 td {
            padding: 2px 0px 2px 0px;
        }

        .padding1 {
            padding: 2px;
        }

        table.imagetable2 td {
            padding: 2px 0px 2px 0px;
        }

        .padding1 {
            padding: 2px;
        }

        .lblnotarget {
            color: red;
            font-weight: bold;
        }

        .details1 {
            display: none;
        }

        .txtalign {
            text-align: right;
        }

        .details0 {
            display: none;
        }

        #MainContent_cblLocations td {
            padding: 0 40px 0 0;
        }

        textarea, input[type="text"] {
            border: 1px solid #D5D5D5;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 11px;
            line-height: 1.2;
            padding: 2px 1px;
            transition-duration: 0.1s;
            text-align: right;
            width: 70px;
        }

        .commentstext {
            border: 1px solid #D5D5D5;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 12px;
            line-height: 1.2;
            padding: 0 0;
            transition-duration: 0.1s;
            text-align: left;
        }

        .langlinks {
            color: white;
        }
    </style>

    <script type="text/javascript">

        $(function () {

            $(".numeric1").wholenumber();
            $('.showDetails1').click(function () {
                $(this).parent().parent().next('tr.details1').toggle();
                $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                                                             '../assets/orsimages/minus.png' :
                                                              '../assets/orsimages/plus.png'))
            });

            $('.showDetails0').click(function () {
                $(this).parent().parent().parent().parent().find('tr.details0').toggle();
                $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                                                             '../assets/orsimages/minus.png' :
                                                              '../assets/orsimages/plus.png'))
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">

        <div id="divMsg"></div>
        <table style="width: 100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="box widget-header widget-header-small header-color-blue2">
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 98%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label></label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label></label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <label>
                                                        Month:</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                    </asp:DropDownList>
                                                    <b>(2016 Data Entry)</b>
                                                </td>
                                                <td></td>
                                                <td class="pull-right">
                                                    <asp:Button runat="server" ID="btnSaveAll" Text="Save" class="btn btn-primary btn-sm"
                                                        OnClick="btnSaveAll_Click" OnClientClick="needToConfirm = false;" />
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ClusterIndicatorId,UnitId" CssClass="imagetable"
                Width="100%" OnRowDataBound="gvIndicators_RowDataBound">
                <HeaderStyle BackColor="Control"></HeaderStyle>
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                    <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                    <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfIndicatorId" runat="server" Value='<%#Eval("ClusterIndicatorID")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="imgRind" runat="server" />
                            <asp:Image ID="imgCind" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="250" ItemStyle-Width="250" meta:resourcekey="TemplateFieldResource3">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderIndicator" runat="server" Text="Indicator"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridIndicator" runat="server" Text='<%# Eval("Indicator") %>'
                                meta:resourcekey="lblGridIndicatorResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="50" ItemStyle-Width="50px">
                        <HeaderTemplate>
                            <asp:Label ID="lblUnitHeader" runat="server" Text="Unit"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                            <asp:Label ID="lblUnitId" runat="server" Text='<%# Eval("UnitId") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="50" ItemStyle-Width="50px">
                        <HeaderTemplate>
                            <asp:Label ID="lblCalMethodHeader" runat="server" Text="Calc Method"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCalMethod" runat="server" Text='<%# Eval("IndicatorCalculationType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="8%" HeaderText="<span class='tooltip2' title='Caculated on the basis of Calculation Method of the Indicator.'>Running Value</span>" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblSum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="600" ItemStyle-Width="600">
                        <ItemTemplate>
                            <asp:Label ID="lblNoTarget" runat="server" CssClass="lblnotarget" Visible="false"
                                Text="No Target Provided For This Inidcator, Consult Sector Coordinator"></asp:Label>
                            <asp:Repeater ID="rptCountryGender" runat="server" OnItemDataBound="rptCountryGender_ItemDataBound">
                                <ItemTemplate>
                                    <table style="width: 600px;" class="imagetable imagetable2 tblCountryGender">
                                        <tr>
                                            <th colspan="2" style="width: 125px;" class="lightgraycolor"></th>
                                            <th class="tdHeader" style="width: 210px;" colspan="3">Sector Targets</th>
                                            <th class="tdHeader" style="width: 210px" colspan="3">Monthly Achieved</th>
                                        </tr>
                                        <tr>
                                            <th colspan="2" style="width: 125px;" class="lightgraycolor">Locaiton</th>
                                            <th class="tdHeader" style="width: 70px;">Male</th>
                                            <th class="tdHeader" style="width: 70px;">Female</th>
                                            <th class="tdHeader" style="width: 70px;">Total</th>
                                            <th class="tdHeader" style="width: 70px;">Male</th>
                                            <th class="tdHeader" style="width: 70px;">Female</th>
                                            <th class="tdHeader" style="width: 70px;">Total</th>
                                        </tr>
                                        <tr style="background-color: #C0C0C0">
                                            <td width="5px">
                                                <img src="../assets/orsimages/plus.png" class="showDetails0"
                                                    title="Click to show/hide Admin1" alt="Expand/Collapse Admin1" /></td>
                                            <td style="width: 120px;">
                                                <div style="float: left; width: 120px; padding: 2px; text-align: left;">
                                                    <%#Eval("LocationName")%>
                                                    <asp:HiddenField ID="hfCountryId" runat="server" Value='<%#Eval("LocationId")%>' />
                                                    <asp:HiddenField ID="hfCountryIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetMaleCluster" runat="server"
                                                    Text='<%#Eval("TargetMale") %>' ToolTip="Cluster Target: Country Total Male"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetFemaleCluster" runat="server"
                                                    Text='<%#Eval("TargetFemale") %>' ToolTip="Cluster Target: Country Total Female"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetCluster" runat="server"
                                                    Text='<%#Eval("TargetTotal") %>' ToolTip="Cluster Target: Country Total"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetMaleProject" runat="server"
                                                    Text='<%#Eval("AchievedMale") %>' ToolTip="Project Target: Country Total Male"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetFemaleProject" runat="server"
                                                    Text='<%#Eval("AchievedFemale") %>' ToolTip="Project Target: Country Total Female"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetProject" runat="server"
                                                    Text='<%#Eval("AchievedTotal") %>' ToolTip="Project Target: Country Total"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="details0">
                                            <td style="width: 100%;" colspan="8">
                                                <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1Gender_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="margin: 0 auto; width: 600px;" border="0" class="imagetable imagetable2 tblAdmin1Gender">
                                                            <tr style="background-color: #D8D8D8" class="trAdmin1">
                                                                <td style="width: 142px;">
                                                                    <div style="float: left; width: 100%; padding: 2px; text-align: left;"><%#Eval("LocationName")%></div>
                                                                    <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                    <asp:HiddenField ID="hfAdmin1IndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetMaleCluster" runat="server"
                                                                        Text='<%#Eval("TargetMale") %>' ToolTip="Cluster Tareget: Admin1 Total Male"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetFemaleCluster" runat="server"
                                                                        Text='<%#Eval("TargetFemale") %>' ToolTip="Cluster Target: Admin1 Total Female"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetCluster" runat="server"
                                                                        Text='<%#Eval("TargetTotal") %>' ToolTip="Cluster Target: Admin1 Total"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable" style="padding-left: 2px;">
                                                                    <asp:TextBox ID="txtAdmin1TargetMaleProject" runat="server"
                                                                        Text='<%#Eval("AchievedMale") %>' ToolTip="Project Target Admin1 Total Male"
                                                                        CssClass="trgtAdmin2GenderMale numeric1 padding1 txtalign"
                                                                        Width="70px"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable" style="padding-left: 2px;">
                                                                    <asp:TextBox ID="txtAdmin1TargetFemaleProject" runat="server"
                                                                        Text='<%#Eval("AchievedFemale") %>' ToolTip="Project Target Admin1 Total Female"
                                                                        CssClass="trgtAdmin1GenderMale numeric1 padding1 txtalign"
                                                                        Width="70px"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable" style="padding-left: 2px;">
                                                                    <asp:TextBox ID="txtAdmin1TargetProject" runat="server"
                                                                        Text='<%#Eval("AchievedTotal") %>' ToolTip="Project Target Admin1 Total"
                                                                        CssClass="trgtAdmin1GenderMale numeric1 txtalign" Style="text-align: right;"
                                                                        Width="70px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField runat="server" ID="hfLocationIdGender" Value='<%# Eval("LocationId") %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
