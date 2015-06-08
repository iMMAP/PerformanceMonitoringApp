<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewLocation.aspx.cs"  MasterPageFile="~/Site.Master" Inherits="SRFROWCA.Admin.Location.AddNewLocation" %>
<%@ MasterType virtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      
    <div class="page-content">
        <asp:HiddenField ID="hdnId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnLocationName" runat="server" />
        <asp:HiddenField ID="hdnPCode" runat="server" />
        <asp:HiddenField ID="hdnLat" runat="server" />
        <asp:HiddenField ID="hdnLong" runat="server" />
        <asp:HiddenField ID="hdnPopulation" runat="server" />
        <asp:HiddenField ID="hdnIsAccurate" runat="server" />
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">                                                    
                                                    <table class="tblLocation" border="0" class="width-70" style="margin: 10px 10px 10px 20px">
                                                        <tr><td colspan="3">
                                                            <h6 class="header blue bolder smaller LocationNumber">Location 1</h6>
                                                            </td></tr>
                                                        <tr>
                                                            <td style="width: 200px;">
                                                                <label>
                                                                    Location Name:</label>
                                                            </td>
                                                            <td style="width: 380px;">
                                                                <asp:TextBox ID="txtLocationName" runat="server" MaxLength="150" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Location Name Required"
                                                                    CssClass="error2" Text="Required" ValidationGroup="Save" ControlToValidate="txtLocationName"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr> 
                                                         <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    Location PCode:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPCode" runat="server" MaxLength="150" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                        </tr> 
                                                           <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    Estimated Population:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPopulation" runat="server" MaxLength="150" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                        </tr>                    
                                                         <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    Latitude:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLat" runat="server" MaxLength="150" CssClass="width-100" onkeypress="return filter(event);"></asp:TextBox>
                                                                
                                                            </td>
                                                            <td>
                                                               
                                                            </td>
                                                        </tr>      
                                                         <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    Longitude:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLong" runat="server" MaxLength="150" CssClass="width-100" onkeypress="return filter(event);"></asp:TextBox>
                                                                
                                                            </td>
                                                            <td>
                                                                  
                                                            </td>
                                                        </tr>      
                                                         <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    IsAccurateLatLng:</label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsAccurate" />
                                                            </td>
                                                            <td>
                                                                
                                                            </td>
                                                        </tr>  
                                                        </table>
                                                    <div id="dvButtons" class="pull-right" style="margin-right:450px;">
                            <div id="btnRemove"  onclick="removeLocation();"
                                class="btn spinner-down btn-xs btn-danger">
                                <i class="icon-minus smaller-75"></i>
                            </div>
                            <div id="btnAdd"  onclick="addLocation();"
                                class="btn spinner-up btn-xs btn-success">
                                <i class="icon-plus smaller-75"></i>
                            </div>
                        </div>
                                                    <asp:UpdatePanel runat="server" ID="updPanel1" UpdateMode="Always">
                                                        <ContentTemplate>
                                                     <table border="0" class="width-70" style="margin: 10px 10px 10px 20px;clear:both;">                                                       
                                                        <tr>
                                                            <td style="width:200px;">Location Type:</td>
                                                            <td style="width:380px;">
                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="width-100" AppendDataBoundItems="true" AutoPostBack="true"
                                                                    DataTextField="LocationType" DataValueField="LocationTypeId" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select Type" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Location Type Required" InitialValue="-1"
                                                                    CssClass="error2" ValidationGroup="Save" Text="Required" ControlToValidate="ddlType"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trRegion" Visible="false">
                                                            <td>Region:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRegion" AutoPostBack="true" runat="server" CssClass="width-100" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" >
                                                                    <asp:ListItem Text="Select Region" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                
                                                            </td>
                                                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Region Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlRegion" Display="Dynamic"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                         <tr runat="server" id="trNational" Visible="false">
                                                            <td>National:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlNational" runat="server" AutoPostBack="true" CssClass="width-100" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlNational_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select National" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                 
                                                            </td>
                                                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="National Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlNational" Display="Dynamic"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                          <tr runat="server" id="trGovernorate" Visible="false">
                                                            <td>Governorate:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlGovernorate" AutoPostBack="true" runat="server" CssClass="width-100" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlGovernorate_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select Governorate" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                
                                                            </td>
                                                            <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Governorate Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlGovernorate" Display="Dynamic"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                          <tr runat="server" id="trDistrict" Visible="false">
                                                            <td>District:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDistrict" AutoPostBack="true" runat="server" CssClass="width-100" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select District" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               
                                                            </td>
                                                            <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="District Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlDistrict" Display="Dynamic"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                          <tr runat="server" id="trSubDistrict" Visible="false">
                                                            <td>Sub-District:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSubDistrict" runat="server" CssClass="width-100" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="Select Sub-District" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               
                                                            </td>
                                                            <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Sub-District Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlSubDistrict" Display="Dynamic"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                         <tr runat="server" id="trVillage" Visible="false">
                                                            <td>Village:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlVillage" AutoPostBack="true" runat="server" CssClass="width-100" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="Select Village" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               
                                                            </td>
                                                            <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Village Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlVillage"></asp:RequiredFieldValidator></td>
                                                        </tr>
                                                         <tr runat="server" visible="false">
                                                            <td>Non-Representative:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlNonRepresentative"  runat="server" CssClass="width-100" AppendDataBoundItems="true" Visible="false">
                                                                    <asp:ListItem Text="Select Non-Representative" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                         
                                                        <tr>
                                                            <td></td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Button ID="btnSave" ValidationGroup="Save" runat="server" Text="Save" CssClass="width-20 btn btn-sm btn-success" OnClick="btnSave_Click" OnClientClick="return GetLocationValues();" />
                                                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-sm" Text="Back To Location List" OnClick="btnCancel_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                            </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlRegion" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlNational" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlGovernorate" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlDistrict" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSubDistrict" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                        </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {           
            showHideRemoveButton();
        });

        function showHideRemoveButton() {
            if ($("[id$=hdnId]").val() != "0") {
                $("#dvButtons").hide();
                return;
            }
            if ($(".tblLocation").length <= 1) {
                $("#btnRemove").hide();
            } else {
                $("#btnRemove").show();
            }
        }
        function removeLocation() {
            $(".tblLocation:Last").remove();
            showHideRemoveButton();
        }

        function addLocation() {
            if($(".tblLocation:last").find("[id$=txtLocationName]").val() == "")
            {
                alert("Please fill out " + $(".LocationNumber:Last").text() + " information!");
                return;
            }
            $(".tblLocation:first").clone().insertAfter(".tblLocation:last").find("input").val("");
            $(".tblLocation:last").find("input").removeAttr('checked');
            
            $(".LocationNumber:Last").text("Location " + $(".tblLocation").length);
            showHideRemoveButton();

        }

        function GetLocationValues() {
            $("#divMsg").empty();
            $("#divMsg").removeClass();
            $("[id$=hdnLocationName]").val("");
            $("[id$=hdnPCode]").val("");
            $("[id$=hdnPopulation]").val("");
            $("[id$=hdnLat]").val("");
            $("[id$=hdnLong]").val("");
            $("[id$=hdnIsAccurate]").val("");
            if (Page_ClientValidate()) {
               
                $(".tblLocation").each(function () {
                    if ($("[id$=hdnLocationName]").val() != "") {
                        $("[id$=hdnLocationName]").val($("[id$=hdnLocationName]").val() + "|" + $(this).find("[id$=txtLocationName]").val());
                    } else {
                        $("[id$=hdnLocationName]").val($("[id$=txtLocationName]").val());
                    }
                    if ($("[id$=hdnPCode]").val() != "") {
                        $("[id$=hdnPCode]").val($("[id$=hdnPCode]").val() + "|" + $(this).find("[id$=txtPCode]").val());
                    }
                    else {
                        $("[id$=hdnPCode]").val($("[id$=txtPCode]").val() == "" ? " " : $("[id$=txtPCode]").val());
                    }
                    if ($("[id$=hdnPopulation]").val() != "") {
                        $("[id$=hdnPopulation]").val($("[id$=hdnPopulation]").val() + "|" + $(this).find("[id$=txtPopulation]").val());
                    } else {
                        $("[id$=hdnPopulation]").val($("[id$=txtPopulation]").val() == "" ? " " : $("[id$=txtPopulation]").val());
                    }
                    if ($("[id$=hdnLat]").val() != "") {
                        $("[id$=hdnLat]").val($("[id$=hdnLat]").val() + "|" + $(this).find("[id$=txtLat]").val());
                    } else {
                        $("[id$=hdnLat]").val($("[id$=txtLat]").val() == "" ? "0" : $("[id$=txtLat]").val());
                    }
                    if ($("[id$=hdnLong]").val() != "") {
                        $("[id$=hdnLong]").val($("[id$=hdnLong]").val() + "|" + $(this).find("[id$=txtLong]").val());
                    } else {
                        $("[id$=hdnLong]").val($("[id$=txtLong]").val() == "" ? "0" :$("[id$=txtLong]").val());
                    }
                    if ($("[id$=hdnIsAccurate]").val() != "") {
                        $("[id$=hdnIsAccurate]").val($("[id$=hdnIsAccurate]").val() + "|" + $(this).find("[id$=chkIsAccurate]").val());
                    } else {
                        $("[id$=hdnIsAccurate]").val($("[id$=chkIsAccurate]").val());
                    }
                });
             
            }
           
            $("[id$=RequiredFieldValidator2]").each(function () {
                if ($(this).parent().parent().find("[id$=txtLocationName]").val() == "") {
                    $(this).css("visibility", "visible");
                   // return false;
                }
                else {
                    $(this).hide();
                }
            });
            if ($("[id$=RequiredFieldValidator2]").is(":visible").length == 0 || $("[id$=RequiredFieldValidator2]").is(":visible").length == undefined) {

                return true;
            }
            else {
                return false;
            }
        }

        function filter(e) {
            var a = [];
            var k = e.which;
            
            for (i = 48; i < 58; i++)
                a.push(i);
            a.push(45);
            a.push(46);
            a.push(61);
            if (!($.inArray(k, a) >= 0))
                e.preventDefault();
        }


    </script>
</asp:Content>
