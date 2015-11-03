<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlCountryTargets.ascx.cs" Inherits="SRFROWCA.Controls.ctlCountryTargets" %>
<div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
    <div class="widget-box no-border">
        <div class="widget-body">
            <div id="divAdmin2Targets" runat="server">
                <div class="widget-main no-padding-bottom no-padding-top targetCtl">
                    <div class="content">
                        <asp:Repeater ID="rptCountry" runat="server">
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
                                            CssClass="numeric1 trgtCountry txt"  meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="widget-main no-padding-bottom no-padding-top hidden targetGenderCtl">
                    <div class="content">
                        <asp:Repeater ID="rptCountryGender" runat="server">
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
                                            CssClass="numeric1 trgtCountryGenderMale txt" meta:resourcekey="txtTargetMaleResource1"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTargetFemale" runat="server" Text='<%# Eval("CountryTargetFeMale") %>' ToolTip="Country Female Total"
                                            CssClass="numeric1 trgtCountryGenderFemale txt" meta:resourcekey="txtTargetFemaleResource1"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("CountryTarget") %>'
                                            CssClass="numeric1 trgtCountryGenderTotal txt" ToolTip="Country Total"
                                            Enabled="False" meta:resourcekey="txtTargetResource4"></asp:TextBox>
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
</div>
