<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUsControl.ascx.cs"
    Inherits="SRFROWCA.ContactUs.ContactUsControl" %>
<div class="row">
    <div class="col-xs-6 col-sm-6">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main">
                    <div>
                        <label>
                            Please use following fields to send us your query, request, comments!</label>
                    </div>
                    <hr />
                    <div>
                        <label>
                            Name</label>
                        <div>
                            <asp:TextBox ID="txtName" runat="server" Width="300px" CssClass="addfields2" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <label>
                            Your Email(Required)</label>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                                        Text="*" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                        <div>
                            <div>
                                <asp:TextBox ID="txtEmail" runat="server" Width="500px" CssClass="addfields2" MaxLength="100"></asp:TextBox>
                                 
                            </div>
                        </div>
                    </div>
                    <div>
                        <label>
                            Subject</label>
                        <div>
                            <div>
                                <asp:TextBox ID="txtSubject" runat="server" Width="500px" CssClass="addfields2" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div>
                        <label>
                            Message (300 char max)(Required)</label>
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ErrorMessage="Required"
                                                        Text="*" CssClass="error2" ControlToValidate="txtMessage"></asp:RequiredFieldValidator>
                        <div>
                            <div>
                                <asp:TextBox ID="txtMessage" runat="server" Width="500px" Height="150px" CssClass="addfields1"
                                    TextMode="MultiLine" MaxLength="300"></asp:TextBox>                                    
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div>
                        <button runat="server" id="btnSend" onserverclick="txtSend_Click" class="btn btn-primary"
                            title="Send">
                            <i class="icon-envelope-alt"></i>Send
                        </button>
                    </div>
                    <div>
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
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
