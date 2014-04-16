<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUsControl.ascx.cs"
    Inherits="SRFROWCA.ContactUs.ContactUsControl" %>
<div class="row">
    <div class="col-xs-6 col-sm-6">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <div>
                        <label>
                            <asp:Localize ID="localContactUsMessage" runat="server" Text="Please use following fields to send us your query, request, comments!" meta:resourcekey="localContactUsMessageResource1"></asp:Localize>
                        </label>
                    </div>
                    <hr />
                    <div>
                        <label>
                            <asp:Localize ID="localName" runat="server" Text="Name" meta:resourcekey="localNameResource1"></asp:Localize>
                        </label>
                        <div>
                            <asp:TextBox ID="txtName" runat="server" Width="300px" MaxLength="50" meta:resourcekey="txtNameResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <label>
                            <asp:Localize ID="localEmail" runat="server" Text="Your Email(Required)" meta:resourcekey="localEmailResource1"></asp:Localize>
                            
                        </label>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                                        Text="*" CssClass="error2" ControlToValidate="txtEmail" meta:resourcekey="rfvEmailResource1"></asp:RequiredFieldValidator>
                        <div>
                            <div>
                                <asp:TextBox ID="txtEmail" runat="server" Width="500px" MaxLength="100" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                                 
                            </div>
                        </div>
                    </div>
                    <div>
                        <label>
                            <asp:Localize ID="localSubject" runat="server" Text="Subject" meta:resourcekey="localSubjectResource1"></asp:Localize>
                        </label>
                        <div>
                            <div>
                                <asp:TextBox ID="txtSubject" runat="server" Width="500px" MaxLength="100" meta:resourcekey="txtSubjectResource1"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div>
                        <label>
                            <asp:Localize ID="localMessage" runat="server" Text="Message (300 char max)(Required)" meta:resourcekey="localMessageResource1"></asp:Localize>
                        </label>
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ErrorMessage="Required"
                                                        Text="*" CssClass="error2" ControlToValidate="txtMessage" meta:resourcekey="rfvMessageResource1"></asp:RequiredFieldValidator>
                        <div>
                            <div>
                                <asp:TextBox ID="txtMessage" runat="server" Width="500px" Height="150px"
                                    TextMode="MultiLine" MaxLength="300" meta:resourcekey="txtMessageResource1"></asp:TextBox>                                    
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <button runat="server" id="btnSend" onserverclick="txtSend_Click" class="btn btn-primary"
                             Title="Send">
                            <i class="fa fa-envelope-alt"></i><asp:Localize ID="LocalSend" runat="server" Text="Send" meta:resourcekey="LocalSendResource1"></asp:Localize>
                        </button>
                    </div>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessageResource1"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /contact us -->
    <%--<div class="col-xs-3 col-sm-3">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <div>
                        <lable>&nbsp;</lable>
                    </div>
                    <hr />
                    <div>
                        <label>
                            Email</label>
                        <div>
                            info@hello-media.com
                        </div>
                    </div>
                    <hr />
                    <div>
                        <label>
                            Phone</label>
                        <div>
                            000.0000.0000
                        </div>
                    </div>
                    <hr />
                </div>
            </div>
        </div>
    </div>--%>
</div>
<!-- /row -->
