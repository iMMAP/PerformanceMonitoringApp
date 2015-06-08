﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddKeyFigureIndicator.aspx.cs" Inherits="SRFROWCA.KeyFigures.AddKeyFigureIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    

    <div class="page-content">
        <div id="divMsg"></div>
        <div class="alert2 alert-block alert-info">
            <h6>
                <asp:Localize ID="locMessageForUser" runat="server" Text="Select Category, Sub-Category and add Key Figure. Click on '+' button to add as many Key Figures for the selected Sub-Category you want and click save."></asp:Localize></h6>
        </div>

        <table>
            <tr>
                <td>Category:</td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="300px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </td>
                <td>Sub Category:</td>
                <td>
                    <asp:DropDownList ID="ddlSubCategory" runat="server" Width="300px"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlSubCategory"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlKeyFigCategory" runat="server">
        </asp:Panel>

        <div class="pull-right">
            <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-down btn-xs btn-danger" type="button" visible="false">
                <i class="icon-minus smaller-75"></i>
            </button>
            <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                class="btn spinner-up btn-xs btn-success" type="button">
                <i class="icon-plus smaller-75"></i>
            </button>
        </div>
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
    </div>
</asp:Content>
