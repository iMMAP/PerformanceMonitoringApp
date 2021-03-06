﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlAdmin1Targets.ascx.cs" Inherits="SRFROWCA.Controls.ctlAdmin1Targets" %>
<script>
    $(function () {
        $('.content').hide();
        $("#<%=pAdmin1Target.ClientID%>").click(function () {
            $(this).next(".content").toggle();
            if ($.trim($(this).text()) === "Click To Show Locations To Add/Edit Targets") {
                $(this).text("Click To Hide Locations");
            }
            else {
                $(this).text("Click To Show Locations To Add/Edit Targets");
            }
        });

        $("#<%=pAdmin1GenderTarget.ClientID%>").click(function () {
            $(this).next(".content").toggle();
            if ($.trim($(this).text()) === "Click To Show Locations To Add/Edit Targets") {
                $(this).text("Click To Hide Locations");
            }
            else {
                $(this).text("Click To Show Locations To Add/Edit Targets");
            }
        });
    });
</script>
<div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
    <div class="widget-box no-border">
        <div class="widget-body">
            <div class="widget-main no-padding-bottom no-padding-top targetCtl" id="divAdmin1Targets" runat="server">
                <a id="pAdmin1Target" runat="server" style="width: 20%">
                    <asp:Localize ID="localClickToShow1" runat="server" Text="Click To Show Locations To Add/Edit Targets" meta:resourcekey="localClickToShow1Resource1"></asp:Localize></a>
                <div class="content">
                    <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptCountry_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 500px;" class="imagetable tblCountry">
                                <tr style="background-color: gray">
                                    <td style="width: 360px;">
                                        <asp:Label ID="lblRptLoc" runat="server" Text="Location" meta:resourcekey="lblRptLocResource1"></asp:Label></td>
                                    <td style="width: 100px;">
                                        <asp:Label runat="server" Text="Target" meta:resourcekey="LabelResource1"></asp:Label></td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="background-color: #C8C8C8">
                                <td style="width: 360px;">
                                    <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                    <asp:HiddenField ID="hfCountryId" runat="server" Value='<%# Eval("LocationId") %>' />
                                </td>
                                <td class="tdTable">
                                    <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("CountryTarget") %>' ToolTip="Country Total"
                                        CssClass="numeric1 trgtCountry txt" Enabled="False" meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 500px;" colspan="2">
                                    <asp:Repeater ID="rptAdmin1" runat="server">
                                        <ItemTemplate>
                                            <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1">
                                                <tr style="background-color: #EEEEEE">
                                                    <td style="width: 378px;">
                                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                        <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                    </td>
                                                    <td class="tdTable">
                                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("Admin1Target") %>' ToolTip="Admin1 Total"
                                                            CssClass="numeric1 trgtAdmin1" Style="text-align: right;" Width="100px" meta:resourcekey="txtTargetResource2"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            </table>
        <asp:HiddenField runat="server" ID="hfLocationId" Value='<%# Eval("LocationId") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>
            <div class="widget-main no-padding-bottom no-padding-top hidden targetGenderCtl" id="divAdmin1GenderTargets" runat="server">
                <a id="pAdmin1GenderTarget" runat="server" style="width: 20%">
                    <asp:Localize ID="localClickShow2" runat="server" Text="Click To Show Locations To Add/Edit Targets" meta:resourcekey="localClickShow2Resource1"></asp:Localize></a>

                <div class="content">
                    <asp:Repeater ID="rptCountryGender" runat="server" OnItemDataBound="rptCountryGender_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 600px;" class="imagetable tblCountryGender">
                                <tr style="background-color: gray">
                                    <td style="width: 260px;">
                                        <asp:Label ID="lblRptGenLoc" runat="server" Text="Location" meta:resourcekey="lblRptGenLocResource1"></asp:Label></td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lblRptGenMale" runat="server" Text="Male" meta:resourcekey="lblRptGenMaleResource1"></asp:Label></td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lblRptGenFemale" runat="server" Text="Female" meta:resourcekey="lblRptGenFemaleResource1"></asp:Label></td>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lblRptGenTotal" runat="server" Text="Total" meta:resourcekey="lblRptGenTotalResource1"></asp:Label></td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="background-color: #C8C8C8">
                                <td style="width: 260px;">
                                    <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                    <asp:HiddenField ID="hfCountryId" runat="server" Value='<%# Eval("LocationId") %>' />
                                </td>
                                <td class="tdTable">
                                    <asp:TextBox ID="txtTargetMale" runat="server" Text='<%# Eval("CountryTargetMale") %>' ToolTip="Country Male Total"
                                        CssClass="numeric1 trgtCountryGenderMale txt" Enabled="False" meta:resourcekey="txtTargetMaleResource1"></asp:TextBox>
                                </td>
                                <td class="tdTable">
                                    <asp:TextBox ID="txtTargetFemale" runat="server" Text='<%# Eval("CountryTargetFeMale") %>' ToolTip="Country Female Total"
                                        CssClass="numeric1 trgtCountryGenderFemale txt" Enabled="False" meta:resourcekey="txtTargetFemaleResource1"></asp:TextBox>
                                </td>
                                <td class="tdTable">
                                    <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("CountryTarget") %>'
                                        CssClass="numeric1 trgtCountryGenderTotal txt" ToolTip="Country Total"
                                        Enabled="False" meta:resourcekey="txtTargetResource4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 600px;" colspan="4">
                                    <asp:Repeater ID="rptAdmin1" runat="server">
                                        <ItemTemplate>
                                            <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1Gender">
                                                <tr style="background-color: #EEEEEE" class="trAdmin1">
                                                    <td style="width: 259px;">
                                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                        <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                    </td>
                                                    <td class="tdTable">
                                                        <asp:TextBox ID="txtTargetMale" runat="server" Text='<%# Eval("Admin1TargetMale") %>' ToolTip="Admin1 Male Total"
                                                            CssClass="numeric1 trgtAdmin1GenderMale" Style="text-align: right;" Width="100px" meta:resourcekey="txtTargetMaleResource2"></asp:TextBox>
                                                    </td>
                                                    <td class="tdTable">
                                                        <asp:TextBox ID="txtTargetFeMale" runat="server" Text='<%# Eval("Admin1TargetFeMale") %>' ToolTip="Admin1 Female Total"
                                                            CssClass="numeric1 trgtAdmin1GenderFemale" Style="text-align: right;" Width="100px" meta:resourcekey="txtTargetFeMaleResource2"></asp:TextBox>
                                                    </td>
                                                    <td class="tdTable">
                                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("Admin1Target") %>' ToolTip="Admin1 Total"
                                                            CssClass="numeric1 trgtAdmin1GenderTotal" Style="text-align: right;"
                                                            Width="100px" Enabled="False" meta:resourcekey="txtTargetResource5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>
        </div>
    </div>
</div>
