<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Change_Password.aspx.cs" Inherits="Modules_Common_Change_Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <section class="content-header">
        <h1>Change Password</h1>
        <ol class="breadcrumb">
            <li class="active">Change Password</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-xs-12">
                <div class="box box-info">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="margin-top:75px;">
                        <ContentTemplate>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                <div class="form-horizontal fge-st">
                                    <div class="form-group fge-st-1">
                                        <label class="col-sm-4 control-label">Old Password &nbsp;<font color="red">*</font></label>
                                        <div class="col-sm-8">
                                            <div class="input-group m-b">
                                                <span class="input-group-addon"><span class="fa fa-key"></span></span>
                                                <asp:TextBox ID="txtOldPwd" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfv_txtOldPwd" runat="server" ControlToValidate="txtOldPwd" Display="Dynamic" ErrorMessage="Old Password Required..." ForeColor="Red" SetFocusOnError="true" ValidationGroup="vgForgot"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group fge-st-1">
                                        <label class="col-sm-4 control-label">New Password &nbsp;<font color="red">*</font></label>
                                        <div class="col-sm-8">
                                            <div class="input-group m-b">
                                                <span class="input-group-addon"><span class="fa fa-key"></span></span>
                                                <asp:TextBox ID="txtNewPwd" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfv_txtNewPwd" runat="server" ControlToValidate="txtNewPwd" Display="Dynamic" ErrorMessage="New Password Required..." ForeColor="Red" SetFocusOnError="true" ValidationGroup="vgForgot"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpv_NewPass" runat="server" ErrorMessage="New Password and Confirm Password should be same." Display="Dynamic" SetFocusOnError="true" ForeColor="Red" ValidationGroup="vgForgot" ControlToValidate="txtConfirmPwd" ControlToCompare="txtOldPwd" Operator="NotEqual" Type="String"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="form-group fge-st-1">
                                        <label class="col-sm-4 control-label">Confirm Password &nbsp;<font color="red">*</font></label>
                                        <div class="col-sm-8">
                                            <div class="input-group m-b">
                                                <span class="input-group-addon"><span class="fa fa-key"></span></span>
                                                <asp:TextBox ID="txtConfirmPwd" runat="server" CssClass="form-control" TextMode="Password" MaxLength="100"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfv_txtConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd" Display="Dynamic" ErrorMessage="Confirm Password Required..." ForeColor="Red" SetFocusOnError="true" ValidationGroup="vgForgot"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cmpV_txtConfirmPwd" runat="server" ErrorMessage="New Password and Confirm Password should be same." SetFocusOnError="true" Display="Dynamic" ForeColor="Red" ControlToCompare="txtNewPwd" Operator="Equal" ControlToValidate="txtConfirmPwd" ValidationGroup="vgForgot"></asp:CompareValidator>
                                        </div>
                                    </div>
                                     <div class="modal-footer fge-st-2">
                                <asp:Button ID="btnSavePassword" runat="server" CssClass="btn btn-danger" Text="Save New Password" ValidationGroup="vgForgot" OnClick="btnSavePassword_OnClick" />
                            </div>
                                </div>
                            </div>
                           
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

