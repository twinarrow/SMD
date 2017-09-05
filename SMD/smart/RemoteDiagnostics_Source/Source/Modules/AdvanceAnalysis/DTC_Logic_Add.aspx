<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="DTC_Logic_Add.aspx.cs" Inherits="Modules_AdvanceAnalysis_DTC_Logic_Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <section class="content-header">
        <h1>DTC Logic Add Section</h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-table"></i>Advance Analysis</a></li>
            <li class="active">DTC Logic Add Section</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-sm-12">
                <div class="alert alert-successs" id="dSuccessMsg" style="display: none; margin-top: 15px;">
                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                    <strong><i style="font-size: 18px" class="fa fa-1x fa-check-square-o "></i></strong>&nbsp;<span style="font-size: 18px; font-style: italic;" id="SuccessMsg"></span>
                </div>
                <div class="alert alert-danger" id="dErrorMsg" style="display: none; margin-top: 15px;">
                    <button type="button" class="close" data-dismiss="alert">&times;</button>
                    <strong><i style="font-size: 18px" class="fa fa-1x fa-warning "></i></strong>&nbsp;<span style="font-size: 18px; font-style: italic;" id="ErrorMsg"></span>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="box box-info">
                    <div class="box-header">
                        <div class="form-inline">
                            <div class="form-group pull-right">
                                <button class="btn btn-info" type="button" data-toggle="modal" data-target="#modal" onclick="javascript:ShowDealerModal()">Add Logic DTC</button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:GridView ID="gvLogicDTC" runat="server" ClientIDMode="Static" AutoGenerateColumns="false" DataKeyNames="dtc_LOGIC_ADD_PK"
                            CssClass="table table-condensed table-striped table-hover table-bordered" ShowHeaderWhenEmpty="true" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No." ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex +1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC Code" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%# Eval("dtc_DTC_CODE") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Logic" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%# Eval("dtc_LOGIC") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Threshold" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <%# Eval("dtc_LOGIC_THRESHOLD") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" ID="Edit" ImageUrl="../../Content/images/iconedit.png" OnClick="Edit_Click" />
                                        <asp:HiddenField ID="hfvAuthID" runat="server" Value='<%# Eval("dtc_LOGIC_ADD_PK") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDelete" Style="height: 20px;" runat="server" ToolTip="Delete" ImageUrl="../../Content/images/trash.png" OnClientClick="javascript:return confirm('Are you sure, do you want to delete this DTC?');" OnClick="imgDelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="modal fade" id="modal-default" style="display: none;">
                    <asp:HiddenField ID="hfvMode" runat="server" />
                    <asp:HiddenField ID="hfvAuth" runat="server" />
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button class="close" aria-label="Close" type="button" data-dismiss="modal">
                                    <span aria-hidden="true">×</span></button>
                                <h4 class="modal-title" id="myModalDLabel" runat="server"><i class="fa fa-plus-circle"></i>&nbsp;Add New Logic DTC</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box-body">
                                            <div class="form-group">
                                                <label>DTC Code <font color="red">*</font></label>
                                                <asp:TextBox ID="txtLogicDTC" runat="server" CssClass="form-control my-colorpicker1 colorpicker-element" placeholder="DTC Code" ToolTip="DTC Code" onkeypress="return onKeyValidate(event,alphanumeric);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_txtLogicDTC" runat="server" SetFocusOnError="true" ErrorMessage="DTC Code is Required" ForeColor="Red" ValidationGroup="ValGPDealer" ControlToValidate="txtLogicDTC" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </div>
                                            <div class="form-group">
                                                <label>Logic <font color="red">*</font></label>
                                                <asp:DropDownList ID="ddlDTCLogic" runat="server" CssClass="form-control select2 select2-hidden-accessible">
                                                    <asp:ListItem Selected="True" Text="Select a Logic" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="<" Value="<"></asp:ListItem>
                                                    <asp:ListItem Text=">" Value=">"></asp:ListItem>
                                                    <asp:ListItem Text="=" Value="="></asp:ListItem>
                                                    <asp:ListItem Text="!=" Value="!="></asp:ListItem>
                                                    <asp:ListItem Text="<=" Value="<="></asp:ListItem>
                                                    <asp:ListItem Text=">=" Value=">="></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFV_ddlDTCLogic" runat="server" InitialValue="0" SetFocusOnError="true" ErrorMessage="DTC Logic Required" ForeColor="Red" ValidationGroup="ValGPDealer" ControlToValidate="ddlDTCLogic" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="form-group">
                                                <label>Threshold <font color="red">*</font></label>
                                                <asp:TextBox ID="txtDTCThreshold" runat="server" CssClass="form-control my-colorpicker1 colorpicker-element" placeholder="DTC Threshold" ToolTip="DTC Threshold" onkeypress="return onKeyValidate(event,numericwithdecimal);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RFV_txtDTCThreshold" runat="server" SetFocusOnError="true" ErrorMessage="DTC Threshold is Required" ForeColor="Red" ValidationGroup="ValGPDealer" ControlToValidate="txtDTCThreshold" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="REVEtxtEDTCThreshold" runat="server" SetFocusOnError="true" ErrorMessage="Numbers,Fractions and Decimals Only Accepted" ForeColor="Red" Font-Italic="true" Font-Size="Small" ControlToValidate="txtDTCThreshold" Display="Dynamic" ValidationGroup="ValGPDealer" ValidationExpression="^[0-9/.]+$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" Text="Save" ValidationGroup="ValGPDealer" />
                                    <button class="btn btn-default pull-right" type="button" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script src='<%= Page.ResolveUrl("~/Content/js/jquery.min.js") %>'></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var totalRows = $("#<%=gvLogicDTC.ClientID%> thead tr").length;
            if (totalRows > 0) {
                $('#gvLogicDTC').DataTable({
                    "iDisplayLength": 10,
                    "bStateSave": true,
                    "scrollX": true,
                    "scrollY": "600px",
                    "scrollCollapse": true,
                    "aLengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                });
            }
        });

        function ShowDealerModal() {
            $('#<%=txtLogicDTC.ClientID %>').val('');
            $('#<%=ddlDTCLogic.ClientID %>').val(0);
            $('#<%=txtDTCThreshold.ClientID %>').val('');
            $('#<%=myModalDLabel.ClientID %>').html("<i class='fa fa-plus-circle'></i>&nbsp;Add New Logic DTC");
            $('#<%=btnSave.ClientID %>').val('Save');
            $('#<%=hfvMode.ClientID %>').val('Save');
            $('#<%=txtLogicDTC.ClientID %>').removeAttr("disabled");
            $('#<%=ddlDTCLogic.ClientID %>').removeAttr("disabled");
            $('#modal-default').modal('show');
        }

        function ShowModal() {
            $('#modal-default').modal('show');
        }

        function ShowEditModal() {
            $('#modal-default').modal('show');
        }

        var alpha = "[A-Za-z ]";
        var numeric = "[0-9/]";
        var numericwithdecimal = "[0-9.]";
        var numericwithBackslash = "[0-9./]";

        var alphanumericwithspecialchar = "[A-Za-z0-9()&. ]";
        var alphanumeric = "[A-Za-z0-9]";
        var timevalidate = "[0-9:-/]";
        var alphaSpclChar = "[A-Za-z()&., ]";

        function onKeyValidate(e, charVal) {
            var keynum;
            var keyChars = /[\x00\x08]/;
            var validChars = new RegExp(charVal);
            if (window.event) {
                keynum = e.keyCode;
            }
            else if (e.which) {
                keynum = e.which;
            }
            var keychar = String.fromCharCode(keynum);
            if (!validChars.test(keychar) && !keyChars.test(keychar)) {
                return false
            } else {
                return keychar;
            }
        }

        function funShowMessage(sparam) {
            $("#SuccessMsg").text(sparam); $("#dSuccessMsg").show();
            setTimeout(function () {
                $("#SuccessMsg").text(sparam);
                $("#dSuccessMsg").hide();
                window.location.href = "DTC_Logic_Add.aspx";
            }, 1500);
        }

        function funShowErrorMessage(sparam) {
            $("#ErrorMsg").text(sparam); $("#dErrorMsg").show();
            setTimeout(function () {
                $("#ErrorMsg").text(sparam);
                $("#dErrorMsg").hide();
                window.location.href = "DTC_Logic_Add.aspx";
            }, 1500);
        }
    </script>
</asp:Content>
