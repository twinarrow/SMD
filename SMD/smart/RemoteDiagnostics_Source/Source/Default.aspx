<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>


<!DOCTYPE html>
<!--[if IE 8]><html class="ie8" lang="en"><![endif]-->
<!--[if IE 9]><html class="ie9" lang="en"><![endif]-->
<!--[if !IE]><!-->
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<!--<![endif]-->
<head runat="server">
    <title>.::Remote Diagnostics Web Portal::.</title>
    <meta charset="utf-8" />
    <!--[if IE]><meta http-equiv='X-UA-Compatible' content="IE=edge,IE=9,IE=8,chrome=1" /><![endif]-->
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0" />

    <link rel="stylesheet" href="Content/css/Login/bootstrap.min.css" />
    <link rel="stylesheet" href="Content/css/Login/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="Content/css/Login/animate.min.css" />
    <link rel="stylesheet" href="Content/css/Login/styles.css" />
    <link rel="stylesheet" href="Content/css/Login/styles-responsive.css" />
</head>
<body class="login">
    <form runat="server">
        <div class="row">
            <div class="main-login col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
                <div class="logo">
                    <img src="Content/images/main_logo.png" class="img-responsive" alt="" /></div>
                <!-- start: LOGIN BOX -->
                <div class="box-login" style="margin-top: 20%;">
                    <h3>Sign in to your account</h3>
                    <p>Please Enter your Access Code and Password to Log in.</p>
                    <div class="errorHandler alert alert-danger no-display">
                        <i class="fa fa-remove-sign"></i>You have some form errors. Please check below.
                    </div>
                    <fieldset>
                        <div class="form-group">
                            <span class="input-icon">
                                <asp:TextBox ID="txtAccessCode" runat="server" CssClass="form-control" placeholder="Access Code" TabIndex="1" MaxLength="10"></asp:TextBox>
                                <i class="fa fa-user"></i>
                            </span>
                            <asp:RequiredFieldValidator ID="RFV_txtAccessCode" runat="server" ControlToValidate="txtAccessCode" Display="Dynamic" ErrorMessage="Access Code is Required" ValidationGroup="ValGPLogin" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <span class="input-icon">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control password" placeholder="Password" TabIndex="2" MaxLength="50"></asp:TextBox>
                                <i class="fa fa-lock"></i>
                            </span>
                            <asp:RequiredFieldValidator ID="RFV_txtPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="Password is Required" ValidationGroup="ValGPLogin" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-actions">
                            <span class="input-icon input-icon-right">
                                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-green pull-right" Text="Login" ValidationGroup="ValGPLogin" OnClick="btnLogin_Click" />
                                <i class="fa fa-arrow-circle-right" style="color: #ffffff"></i>
                            </span>
                        </div>
                        <div class="form-group">
                            <span class="input-icon">
                                <asp:Label ID="lblErrMsg" Font-Italic="true" Font-Size="17px" ForeColor="Red" runat="server" Style="display: none;"></asp:Label>
                            </span>
                        </div>
                    </fieldset>
                    <!-- start: COPYRIGHT -->
                    <div class="copyright">2017 &copy; Mahindra & Mahindra .</div>
                    <!-- end: COPYRIGHT -->
                </div>
            </div>
        </div>
    </form>
    <!--[if gte IE 9]><!-->
    <script src="Content/css/Login/js/jquery-2.1.1.min.js"></script>
    <!--<![endif]-->
    <script src="Content/css/Login/js/bootstrap.min.js"></script>
    <script src="Content/css/Login/js/main.js"></script>
    <!-- end: MAIN JAVASCRIPTS -->
    <!-- start: JAVASCRIPTS REQUIRED FOR THIS PAGE ONLY -->
    <script src="Content/css/Login/js/jquery.validate.min.js"></script>
    <script src="Content/css/Login/js/login.js"></script>
    <!-- end: JAVASCRIPTS REQUIRED FOR THIS PAGE ONLY -->
    <script>
        jQuery(document).ready(function () {
            Main.init();
            Login.init();
        });
    </script>
</body>
</html>

