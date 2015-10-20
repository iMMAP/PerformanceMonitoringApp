<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PasswordSendConfirmation.aspx.cs" Inherits="SRFROWCA.Account.PasswordSendConfirmation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">

                <div class="error-container">
                    <div class="well">
                        <h1 class="grey lighter smaller">Forgot Password</h1>
                        <hr />
                        <h3>We've sent password reset instructions to your email address.</h3>
                        <h3 class="lighter smaller">If you don't receive instructions within a few minutes, check your email's spam and junk filters.</h3>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
