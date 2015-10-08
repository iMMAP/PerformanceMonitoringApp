<%@ Page Title="" Language="C#" MasterPageFile="~/NoMenue.Master" AutoEventWireup="true" CodeBehind="IndicatorTargetByProjectStatus.aspx.cs" Inherits="SRFROWCA.Reports.IndicatorTargetByProjectStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        select {
            margin: 0px;
            border: 1px solid #111;
            background: transparent;
            padding: 0px;
            font-size: 12px;
            border: 1px solid #ccc;
            height: 34px;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            background: url(../favicon.ico) 97% / 20px no-repeat #eee;
        }

        label {
            font-size: 10px;
        }

        label {
            display: inline-block;
            margin-bottom: 2px;
        }

        .details1 {
            display: none;
        }

        tdTable {
            width: 10%;
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
            //$('.ddlslk').ddslick({
            //    width: 150
            //});

            //$('.ddlslk1').ddslick({
            //    width: 250
            //});

            $('body').on('click', '#imgcnt[alt=plus]', function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../assets/orsimages/minus.png");
                $(this).attr("alt", "minus");
            });

            $('body').on('click', '#imgcnt[alt=minus]', function () {
                $(this).attr("src", "../assets/orsimages/plus.png");
                $(this).closest("tr").next().remove();
                $(this).attr("alt", "plus");
            });

            $('body').on('click', '#imgcnt1[alt=plus]', function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../assets/orsimages/minus.png");
                $(this).attr("alt", "minus");
            });

            $('body').on('click', '#imgcnt1[alt=minus]', function () {
                $(this).attr("src", "../assets/orsimages/plus.png");
                $(this).closest("tr").next().remove();
                $(this).attr("alt", "plus");
            });

            $('body').on('click', '#imgcnt2[alt=plus]', function () {
                $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
                $(this).attr("src", "../assets/orsimages/minus.png");
                $(this).attr("alt", "minus");

            });

            $('body').on('click', '#imgcnt2[alt=minus]', function () {
                $(this).attr("src", "../assets/orsimages/plus.png");
                $(this).closest("tr").next().remove();
                $(this).attr("alt", "plus");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server" border="1">
    <div class="page-content">
        <div class="row" style="display: block !important">
            <div class="col-xs-2"></div>
            <div class="col-xs-8">
                <table width="100%">
                    <tr>
                        <td>
                            <label>Country:</label></td>
                        <td>
                            <asp:DropDownList ID="ddlLocation" runat="server" Width="180px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_IndexChanged">
                            </asp:DropDownList></td>
                        <td>
                            <label>Cluster:</label></td>
                        <td>
                            <asp:DropDownList ID="ddlCluster" runat="server" Width="280px"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_IndexChanged">
                            </asp:DropDownList></td>
                    </tr>
                </table>
            </div>
            <div class="col-xs-2"></div>
        </div>
        <hr />
        <!-- /.btn-group -->
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                DataKeyNames="IndicatorId,UnitId" Width="100%"
                OnRowDataBound="gvActivities_RowDataBound"
                ShowHeaderWhenEmpty="true" EmptyDataText="No Cluster Framework Available For The Selected Country & Cluster">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altrow" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgcnt1" alt="plus" style="cursor: pointer" class="pointer1" src="../assets/orsimages/plus.png" />
                            <asp:Panel ID="pnlAdmin1" runat="server" Style="display: none" ScrollBars="Auto" Height="10px">
                                <asp:GridView ID="gvAdmin1" runat="server" AutoGenerateColumns="false"
                                    DataKeyNames="LocationId" Width="100%" CssClass="imagetable"
                                    OnRowDataBound="gvAdmin1_RowDataBound">
                                    <RowStyle CssClass="rowcolor1" />
                                    <AlternatingRowStyle CssClass="altrowcolor1" />

                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                <asp:HiddenField ID="hfIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfAdmin1IndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <img id="imgcnt2" alt="plus" style="cursor: pointer" class="pointer1" src="../assets/orsimages/plus.png" widht="20px" />
                                                <asp:Panel ID="pnlAdmin2" runat="server" Style="display: none">
                                                    <asp:GridView ID="gvAdmin2" runat="server" AutoGenerateColumns="false"
                                                        ShowHeader="false" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" ItemStyle-Width="250px" />
                                                            <asp:BoundField DataField="ClusterTotal" HeaderText="Cluster" />
                                                            <asp:BoundField DataField="DraftTotal" HeaderText="Draft" />
                                                            <asp:BoundField DataField="ApprovedTotal" HeaderText="Approved" />
                                                            <asp:BoundField DataField="CAPTotal" HeaderText="Pub CAP" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="LocationName" HeaderText="Location" />
                                        <asp:BoundField DataField="ClusterTotal" HeaderText="Cluster" />
                                        <asp:BoundField DataField="DraftTotal" HeaderText="Draft" />
                                        <asp:BoundField DataField="ApprovedTotal" HeaderText="Approved" />
                                        <asp:BoundField DataField="CAPTotal" HeaderText="Pub CAP" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                            <asp:HiddenField ID="hfCountryId" runat="server" Value='<%#Eval("CountryId")%>' />
                            <asp:HiddenField ID="hfEmgLocId" runat="server" Value='<%#Eval("EmergencyLocationId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ObjectiveId" ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="30" ItemStyle-Width="30"
                        meta:resourcekey="TemplateFieldResource2">
                        <HeaderTemplate>
                            <asp:Label ID="lblObjectiveHeader" runat="server" Text=""></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Image ID="imgObjective" runat="server" meta:resourcekey="imgRindResource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="350" ItemStyle-Width="350" meta:resourcekey="TemplateFieldResource2">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderActivity" runat="server" Text="Activity" meta:resourcekey="lblGridHeaderActivityResource1"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridActivity" runat="server" Text='<%# Eval("Activity") %>'
                                meta:resourcekey="lblGridActivityResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="350" ItemStyle-Width="350" meta:resourcekey="TemplateFieldResource3">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderIndicator" runat="server" Text="Indicator"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridIndicator" runat="server" Text='<%# Eval("Indicator") %>'
                                meta:resourcekey="lblGridIndicatorResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150px">
                        <HeaderTemplate>
                            <asp:Label ID="lblUnitHeader" runat="server" Text="Unit"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="70" ItemStyle-Width="70px">
                        <HeaderTemplate>
                            <asp:Label ID="lblCalMethodHeader" runat="server" Text="Calculation"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCalMethod" runat="server" Text='<%# Eval("CalculationType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Country" HeaderText="Country" />
                    <asp:BoundField DataField="ClusterTotal" HeaderText="Cluster" />
                    <asp:BoundField DataField="DraftTotal" HeaderText="Draft" />
                    <asp:BoundField DataField="ApprovedTotal" HeaderText="Approved" />
                    <asp:BoundField DataField="CAPTotal" HeaderText="Pub CAP" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
