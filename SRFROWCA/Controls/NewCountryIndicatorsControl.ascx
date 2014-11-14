﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewCountryIndicatorsControl.ascx.cs" Inherits="SRFROWCA.Controls.NewCountryIndicatorsControl" %>

<div class="row no-padding-bottom no-padding-top">
    <h6 class="header blue bolder smaller">Indicator<asp:Label ID="lbl1stNumber" runat="server" Text=""></asp:Label></h6>
    <div class="col-sm-12">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="col-sm-4">
                    <div class="widget-box no-border">
                        <div class="widget-body">
                            <div class="widget-main no-padding-bottom no-padding-top">
                                <div>
                                    <label>
                                        (English):</label>
                                    <div>
                                        <asp:TextBox ID="txtInd1Eng" MaxLength="4000" runat="server" TextMode="MultiLine" CssClass="width-100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtInd1Eng" runat="server" ErrorMessage="Indicator Required"
                                            CssClass="error2" Text="Required" ControlToValidate="txtInd1Eng"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="widget-box no-border">
                        <div class="widget-body">
                            <div class="widget-main no-padding-bottom no-padding-top">
                                <div>
                                    <label>
                                        (French):</label>
                                    <div>
                                        <asp:TextBox ID="txtInd1Fr" MaxLength="4000" runat="server" TextMode="MultiLine" CssClass="width-100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtInd1Fr" runat="server" ErrorMessage="Indicator Required"
                                            CssClass="error2" Text="Required" ControlToValidate="txtInd1Fr"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="widget-box no-border">
                        <div class="widget-body">
                            <div class="widget-main no-padding-bottom no-padding-top">
                                <div>
                                    <label>
                                        Target:</label>
                                    <div>
                                        <asp:TextBox ID="txtTarget" MaxLength="10" runat="server" CssClass="width-100"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtTarget" runat="server" ErrorMessage="Target Required"
                                            CssClass="error2" Text="Required" ControlToValidate="txtTarget"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="widget-box no-border">
                        <div class="widget-body">
                            <div class="widget-main no-padding-bottom no-padding-top">
                                <div>
                                    <label>
                                        Unit:</label>
                                    <div>
                                        <asp:DropDownList runat="server" ID="ddlUnits" AppendDataBoundItems="true" CssClass="width-100">
                                            <asp:ListItem Selected="True" Text="--- Select Unit ---" Value="0"></asp:ListItem>

                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Unit Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlUnits"></asp:RequiredFieldValidator>
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
