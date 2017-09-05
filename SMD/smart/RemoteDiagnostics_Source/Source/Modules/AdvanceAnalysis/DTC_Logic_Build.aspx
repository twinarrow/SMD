<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="DTC_Logic_Build.aspx.cs" Inherits="Modules_AdvanceAnalysis_DTC_Logic_Build" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <section class="content-header">
        <h1>DTC Logic Build Section</h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-table"></i>Advance Analysis</a></li>
            <li class="active">DTC Logic Build Section</li>
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
                                <button class="btn btn-info" type="button" data-toggle="modal" data-target="#modal" onclick="javascript:showModal()">Add Logic DTC</button>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:HiddenField ID="hfvMgrID" runat="server" />
                        <asp:HiddenField ID="hfvMode" Value="Add" runat="server" />
                        <asp:GridView ID="gvDTCLogicBuild" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="dtc_LOGIC_BUILD_PK" CssClass="table table-condensed table-striped table-hover table-bordered"
                            ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="#F0F0F0" HeaderStyle-HorizontalAlign="Center" ClientIDMode="Static">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No." ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC A" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span>
                                            <%# getDTCCode(Eval("dtc_DTC_CODE_A"),Eval("dtc_DTC_CODE_A_INVERT"))%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC B" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span>
                                            <%# getDTCCode(Eval("dtc_DTC_CODE_B"),Eval("dtc_DTC_CODE_B_INVERT"))%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC C" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span>
                                            <%# getDTCCode(Eval("dtc_DTC_CODE_C"),Eval("dtc_DTC_CODE_C_INVERT"))%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC D" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span>
                                            <%# getDTCCode(Eval("dtc_DTC_CODE_D"),Eval("dtc_DTC_CODE_D_INVERT"))%>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC Operation" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <span>
                                            <%# Eval("dtc_LOGIC_OPERATION").ToString() == "0" ? "--" : Eval("dtc_LOGIC_OPERATION") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Smiley" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Image ID="smileyImage" runat="server" ImageUrl='<%# Eval("dtc_LOGIC_SMILEY") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Conclusion" ItemStyle-Width="500px" DataField="dtc_LOGIC_CONCLUSION" />
                                <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lnk" runat="server" AlternateText="btnDetails" ImageUrl="../../Content/images/iconedit.png" ImageAlign="Middle" ToolTip="Edit" OnClick="lnk_Click" />
                                        <asp:HiddenField ID="hfvDTCID" Value='<%#Convert.ToString(Eval("dtc_LOGIC_BUILD_PK"))%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" Visible="true">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDelete" Style="width: 20px; height: 20px;" runat="server" ToolTip="Delete" ImageUrl="../../Content/images/trash.png" OnClientClick="javascript:return confirm('Are you sure, do you want to delete this DTC Build?');" OnClick="imgDelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-danger">
                                    <strong>Oops</strong> No Details Found
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="modal fade" id="modal-default" style="display: none;">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button class="close" aria-label="Close" type="button" data-dismiss="modal">
                                    <span aria-hidden="true">×</span></button>
                                <h4 class="modal-title" id="myModalDLabel" runat="server"><i class="fa fa-plus-circle"></i>&nbsp;DTC Logic Build</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="box-body">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC A :&nbsp;</span>
                                                        <input type="checkbox" runat="server" style="margin-left: 4%; margin-top: 2%" id="DTCANotGate" />
                                                        <label for="DTCANotGate">Not Gate</label>
                                                        <asp:UpdatePanel ID="dtcApnl" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlDTCA" CssClass="form-control input-medium" runat="server" class="span10" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlDTCA_SelectedIndexChanged" Style="margin-left: 45%; margin-top: -6%"></asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <label id="lblddlDTCA" style="display: none; color: red; font-size: 13px; font-style: italic; margin-left: 45%;"><i class='fa fa-warning'></i>DTC A is Required!</label>
                                                    </div>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC B :&nbsp;</span>
                                                        <input type="checkbox" runat="server" style="margin-left: 4%; margin-top: 2%" id="DTCBNotGate" />
                                                        <label for="DTCBNotGate">Not Gate</label>
                                                        <asp:UpdatePanel ID="dtcBpnl" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlDTCB" CssClass="form-control input-medium" runat="server" class="span10" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlDTCB_SelectedIndexChanged" Style="margin-left: 45%; margin-top: -6%"></asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCC" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCD" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC C :&nbsp;</span>
                                                        <input type="checkbox" runat="server" style="margin-left: 4%; margin-top: 2%" id="DTCCNotGate" />
                                                        <label for="DTCCNotGate">Not Gate</label>
                                                        <asp:UpdatePanel ID="dtcCpnl" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlDTCC" CssClass="form-control input-medium" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDTCC_SelectedIndexChanged" class="span10" Width="180px" Style="margin-left: 45%; margin-top: -6%"></asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCB" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCD" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC D :&nbsp;</span>
                                                        <input type="checkbox" runat="server" style="margin-left: 4%; margin-top: 2%" id="DTCDNotGate" />
                                                        <label for="DTCDNotGate">Not Gate</label>
                                                        <asp:UpdatePanel ID="dtcDpnl" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlDTCD" CssClass="form-control input-medium" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDTCD_SelectedIndexChanged" class="span10" Width="180px" Style="margin-left: 45%; margin-top: -6%"></asp:DropDownList>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCC" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlDTCB" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC Operation :&nbsp;</span>
                                                        <asp:DropDownList ID="ddlOperation" CssClass="form-control input-medium" runat="server" class="span10" Width="180px" Style="margin-left: 45%; margin-top: -5%">
                                                            <asp:ListItem Selected="True" Text="Select an Operation" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                                            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <label id="lblddlOperation" style="display: none; color: red; font-size: 13px; font-style: italic; margin-left: 45%;"><i class='fa fa-warning'></i>Operation is Required!</label>
                                                        <label id="lblddlOperationDTC" style="display: none; color: red; font-size: 13px; font-style: italic; margin-left: 45%;"><i class='fa fa-warning'></i>Select any other DTC!</label>
                                                    </div>
                                                    <div class="form-group">
                                                        <span class="help-inline" style="font-size: 12px; color: black; margin-left: 15%;">DTC Conclusion :&nbsp;</span>
                                                        <asp:TextBox runat="server" MaxLength="250" ID="txtConclusion" CssClass="form-control input-medium" Rows="4" Width="180px" class="span5" placeholder="DTC Conclusion" Style="margin-left: 45%; margin-top: -5%"></asp:TextBox>
                                                        <label id="lbltxtConclusion" style="display: none; color: red; font-size: 13px; font-style: italic; margin-left: 45%;"><i class='fa fa-warning'></i>DTC Conclusion is Required!</label>
                                                    </div>
                                                    <div class="form-group" style="margin-left: 14px;">
                                                        <asp:RadioButtonList ID="rbtSmiley" RepeatDirection="Horizontal" RepeatLayout="Flow" runat="server" RepeatColumns="5">
                                                            <asp:ListItem Value="../../../Smileys/Average.png" style="margin-right: 30px" Selected="True"><img src="../../../Smileys/Average.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Bad.png" style="margin-right: 30px"><img src="../../../Smileys/Bad.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Better.png" style="margin-right: 30px"><img src="../../../Smileys/Better.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Excellent.png" style="margin-right: 30px"><img src="../../../Smileys/Excellent.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Good.png" style="margin-right: 30px"><img src="../../../Smileys/Good.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Normal.png" style="margin-right: 30px"><img src="../../../Smileys/Normal.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Okay.png" style="margin-right: 30px"><img src="../../../Smileys/Okay.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Poor.png" style="margin-right: 30px"><img src="../../../Smileys/Poor.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/Terrible.png" style="margin-right: 30px"><img src="../../../Smileys/Terrible.png" /></asp:ListItem>
                                                            <asp:ListItem Value="../../../Smileys/VeryPoor.png" style="margin-right: 30px"><img src="../../../Smileys/VeryPoor.png" /></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="form-group">
                                    <button id="btnSave" runat="server" type="button" clientidmode="static" value="Add" onclick="javascript:validation();" class="btn btn-danger">Add</button>
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
            var totalRows = $("#<%=gvDTCLogicBuild.ClientID%> thead tr").length;
            if (totalRows > 0) {
                $('#gvDTCLogicBuild').DataTable({
                    "iDisplayLength": 5,
                    "bStateSave": true,
                    "scrollX": true,
                    "scrollY": "600px",
                    "scrollCollapse": true,
                    "aLengthMenu": [[5, 10, 25, 50, 100, -1], [5, 10, 25, 50, 100, "All"]],
                });
            }
        });

        function showModal() {
            $('#<%=hfvMode.ClientID%>').val('Add');
            var mode = $('#<%=hfvMode.ClientID%>').val();
            $("#modal-default").modal('show');
            if (mode == "Add") {
                document.getElementById('<%=DTCANotGate.ClientID%>').checked = false;
                document.getElementById('<%=DTCBNotGate.ClientID%>').checked = false;
                document.getElementById('<%=DTCCNotGate.ClientID%>').checked = false;
                document.getElementById('<%=DTCDNotGate.ClientID%>').checked = false;
                document.getElementById('<%=ddlDTCA.ClientID%>').value = "0";
                document.getElementById('<%=ddlDTCB.ClientID%>').value = "0";
                document.getElementById('<%=ddlDTCC.ClientID%>').value = "0";
                document.getElementById('<%=ddlDTCD.ClientID%>').value = "0";
                document.getElementById('<%=txtConclusion.ClientID%>').value = "";

                document.getElementById('<%=ddlOperation.ClientID%>').value = "0";
                $('#<%=rbtSmiley.ClientID %>').find("input[value='../../../Smileys/Average.png']").prop("checked", true);
                $('#<%=btnSave.ClientID %>').text("Add");
            }
        }

        function showEditModal() { $("#modal-default").modal('show'); }


        var jsonVal = null;
        function validation() {
            var conclusion = document.getElementById('<%=txtConclusion.ClientID%>').value;

            var dtca = document.getElementById('<%=ddlDTCA.ClientID%>').value;
            var dtcb = document.getElementById('<%=ddlDTCB.ClientID%>').value;
            var dtcc = document.getElementById('<%=ddlDTCC.ClientID%>').value;
            var dtcd = document.getElementById('<%=ddlDTCD.ClientID%>').value;
            var dtcaI = document.getElementById('<%=DTCANotGate.ClientID%>').checked;
            var dtcbI = document.getElementById('<%=DTCBNotGate.ClientID%>').checked;
            var dtccI = document.getElementById('<%=DTCCNotGate.ClientID%>').checked;
            var dtcdI = document.getElementById('<%=DTCDNotGate.ClientID%>').checked;
            
            var smiley = ($('#<%=rbtSmiley.ClientID %> input[type=radio]:checked').val());
            var btntext = $('#<%=btnSave.ClientID %>').text();
            var hidden = $('#<%=hfvMgrID.ClientID %>').val();

            var operation = document.getElementById('<%=ddlOperation.ClientID%>').value;

            if (dtca == null || (dtca == "0" || dtca == "-1") || document.getElementById('<%=ddlDTCA.ClientID%>').length == 1) {
                $('#lblddlDTCA').css("display", "block");
            }
            if (conclusion == null || conclusion == "") {
                $('#lbltxtConclusion').css("display", "block");
            }

            if ((operation == "0") && ((dtcb == "0" || dtcb == "-1") && (dtcc == "0" || dtcc == "-1") && (dtcd == "0" || dtcd == "-1"))) {
                $('#lblddlOperation').css("display", "none");
            }
            if (operation != "0" && (dtca != "0" && dtca != "-1") && ((dtcb == "0" || dtcb == "-1") && (dtcc == "0" || dtcc == "-1") && (dtcd == "0" || dtcd == "-1"))) {
                $('#lblddlOperationDTC').css("display", "block");
            }
            else {
                $('#lblddlOperationDTC').css("display", "none");
            }

            if (operation == "0" && (dtca != "0" && dtca != "-1") && ((dtcb != "0" && dtcb != "-1") || (dtcc != "0" && dtcc != "-1") || (dtcd != "0" && dtcd != "-1"))) {
                $('#lblddlOperation').css("display", "block");
            }
            if (operation != "0" && (dtca != "0" && dtca != "-1") && ((dtcb != "0" && dtcb != "-1") || (dtcc != "0" && dtcc != "-1") || (dtcd != "0" && dtcd != "-1")) && conclusion != "") {
                if (document.getElementById('<%=ddlDTCA.ClientID%>').length == 1) {
                    $('#lblddlDTCA').css("display", "block");
                    $('#lblddlOperation').css("display", "none");
                    $('#lblddlOperationDTC').css("display", "none");
                    $('#lbltxtConclusion').css("display", "none");
                } else {
                    $.ajax({
                        type: "POST",
                        url: '<%= ResolveUrl("~/Minismart.asmx/InsertUpdate") %>',

                        contentType: "application/json; charset=utf-8",
                        data: '{"ADTCCode":"' + dtca + '","BDTCCode":"' + dtcb + '","CDTCCode":"' + dtcc + '","DDTCCode":"' + dtcd + '","DTCCodeAI":"' + dtcaI + '","DTCCodeBI":"' + dtcbI + '","DTCCodeCI":"' + dtccI + '","DTCCodeDI":"' + dtcdI + '","Operation":"' + operation + '","Conclusion":"' + conclusion + '","Smiley":"' + smiley + '","btnSave":"' + btntext + '","HFV":"' + hidden + '"}',
                        dataType: "json",
                        success: function (response) {
                            jsonVal = JSON.stringify(response.d);
                            OnSuccess(jsonVal);
                        },
                        failure: function (response) {
                            jsonVal = JSON.stringify(response.d);
                            OnFailed(jsonVal);
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            }

            if (operation == "0" && dtca != "0" && (dtcb == "0" || dtcb == "-1") && (dtcc == "0" || dtcc == "-1") && (dtcd == "0" || dtcd == "-1") && conclusion != "") {
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("~/Minismart.asmx/InsertUpdate") %>',
                    contentType: "application/json; charset=utf-8",
                    data: '{"ADTCCode":"' + dtca + '","BDTCCode":"' + dtcb + '","CDTCCode":"' + dtcc + '","DDTCCode":"' + dtcd + '","DTCCodeAI":"' + dtcaI + '","DTCCodeBI":"' + dtcbI + '","DTCCodeCI":"' + dtccI + '","DTCCodeDI":"' + dtcdI + '","Operation":"' + operation + '","Conclusion":"' + conclusion + '","Smiley":"' + smiley + '","btnSave":"' + btntext + '","HFV":"' + hidden + '"}',
                    dataType: "json",
                    success: function (response) {
                        jsonVal = JSON.stringify(response.d);
                        OnSuccess(jsonVal);
                    },
                    failure: function (response) {
                        jsonVal = JSON.stringify(response.d);
                        OnFailed(jsonVal);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }
        }

        function OnSuccess(sparam) {
            $('#modal-default').modal('hide');
            if (sparam == "DTC Build Logic already Exist") {
                $("#ErrorMsg").text(sparam); $("#dErrorMsg").show();
                setTimeout(function () {
                    $("#ErrorMsg").text(sparam);
                    $("#dErrorMsg").hide();
                    window.location.href = "DTC_Logic_Build.aspx";
                }, 1500);
            }
            else {
                $("#SuccessMsg").text(sparam); $("#dSuccessMsg").show();
                setTimeout(function () {
                    $("#SuccessMsg").text(sparam);
                    $("#dSuccessMsg").hide();
                    window.location.href = "DTC_Logic_Build.aspx";
                }, 1500);
            }
        }

        function OnFailed(sparam) {
            $('#modal-default').modal('hide');
            $("#ErrorMsg").text(sparam); $("#dErrorMsg").show();
            setTimeout(function () {
                $("#ErrorMsg").text(sparam);
                $("#dErrorMsg").hide();
                window.location.href = "DTC_Logic_Build.aspx";
            }, 1500);
        }

        function funShowMessage(sparam) {
            $("#SuccessMsg").text(sparam); $("#dSuccessMsg").show();
            setTimeout(function () {
                $("#SuccessMsg").text(sparam);
                $("#dSuccessMsg").hide();
                window.location.href = "DTC_Logic_Build.aspx";
            }, 1500);
        }

        function funShowErrorMessage(sparam) {
            $("#ErrorMsg").text(sparam); $("#dErrorMsg").show();
            setTimeout(function () {
                $("#ErrorMsg").text(sparam);
                $("#dErrorMsg").hide();
                window.location.href = "DTC_Logic_Build.aspx";
            }, 1500);
        }

    </script>
</asp:Content>
