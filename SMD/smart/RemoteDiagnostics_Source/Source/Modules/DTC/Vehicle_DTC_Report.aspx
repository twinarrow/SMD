<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Vehicle_DTC_Report.aspx.cs" Inherits="Modules_DTC_Vehicle_DTC_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <section class="content-header">
        <h1>Vehicle DTC Report</h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-table"></i>Reports</a></li>
            <li class="active">Vehicle DTC Report</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-info">
                    <div class="box-header">
                        <div class="form-inline">
                            <div class="form-group">
                                <asp:HiddenField ID="hfvDateval" runat="server" />
                                <input type="radio" name="radiog_lite" id="rdCurDate" class="iradio_minimal-blue" checked="checked" onchange="javascript:CheckDate();" />
                                <label for="rdCurDate" class="css-label radGroup1">&nbsp;&nbsp;Current Date</label>
                                <input type="radio" name="radiog_lite" id="rdYTD" class="iradio_minimal-blue" onchange="javascript:CheckDate();" />
                                <label for="rdYTD" class="css-label radGroup1">&nbsp;&nbsp;Year to Date</label>
                                <input type="radio" name="radiog_lite" id="rdDateRange" class="iradio_minimal-blue" onchange="javascript:CheckDate();" />
                                <label for="rdDateRange" class="css-label radGroup1">&nbsp;&nbsp;Date Range</label>
                            </div>
                            <div class="form-group">
                                <div class="input-group m-b">
                                    <asp:TextBox ID="txtSDate" runat="server" TabIndex="1" placeholder="Start Date [MM/dd/yyyy]" CssClass="form-control col-lg-1" Enabled="false"></asp:TextBox>
                                    <span class="input-group-addon"><span class="fa fa-calendar"></span></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group m-b">
                                    <asp:TextBox ID="txtEDate" runat="server" TabIndex="2" placeholder="End Date [MM/dd/yyyy]" CssClass="form-control col-lg-1" Enabled="false"></asp:TextBox>
                                    <span class="input-group-addon"><span class="fa fa-calendar"></span></span>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="btn  btn-info" ValidationGroup="ValAppInstall" OnClick="btnGo_Click" />
                            </div>
                            <div class="form-group pull-right">
                                <asp:Button ID="btnExcel" runat="server" Text="Download Excel" CssClass="btn  btn-info" OnClick="btnExcel_Click" />
                            </div>
                        </div>
                        <div class="form-inline">
                            <div class="form-group" style="width: 15%">&nbsp;</div>
                            <div class="form-group">
                                <asp:RequiredFieldValidator ID="RFV_txtSDate" runat="server" Enabled="false" SetFocusOnError="true" ErrorMessage="Start Date is Required" ForeColor="Red" Font-Italic="true" Display="Dynamic" ControlToValidate="txtSDate" ValidationGroup="ValAppInstall"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="rev_txtSDate" runat="server" SetFocusOnError="true" ErrorMessage="Invalid start Date [mm/dd/yyyy]" ForeColor="Red" Font-Italic="true" Display="Dynamic" ControlToValidate="txtSDate" ValidationExpression="^(((((((0?[13578])|(1[02]))[/]+((0?[1-9])|([12]\d)|(3[01])))|(((0?[469])|(11))[/]+((0?[1-9])|([12]\d)|(30)))|((0?2)[/]+((0?[1-9])|(1\d)|(2[0-8]))))[/]+(((19)|(20))?([\d][\d]))))|((0?2)[/]+(29)[/]?(((19)|(20))+(([02468][048])|([13579][26])))))$" ValidationGroup="ValAppInstall"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="custom_St_Date" runat="server" ClientValidationFunction="StartDateCheck" ControlToValidate="txtSDate" ForeColor="Red" Display="Dynamic" ErrorMessage="From date should be less than or equal to current date & To date" SetFocusOnError="true" ValidationGroup="ValAppInstall"></asp:CustomValidator>
                            </div>
                            <div class="form-group">
                                <asp:RequiredFieldValidator ID="RFV_txtEDate" runat="server" Enabled="false" SetFocusOnError="true" ErrorMessage="End Date is Required" ForeColor="Red" Font-Italic="true" Display="Dynamic" ControlToValidate="txtEDate" ValidationGroup="ValAppInstall"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="rev_txtEDate" runat="server" SetFocusOnError="true" ErrorMessage="Invalid End Date [mm/dd/yyyy]" ForeColor="Red" Font-Italic="true" Display="Dynamic" ControlToValidate="txtEDate" ValidationExpression="^(((((((0?[13578])|(1[02]))[/]+((0?[1-9])|([12]\d)|(3[01])))|(((0?[469])|(11))[/]+((0?[1-9])|([12]\d)|(30)))|((0?2)[/]+((0?[1-9])|(1\d)|(2[0-8]))))[/]+(((19)|(20))?([\d][\d]))))|((0?2)[/]+(29)[/]?(((19)|(20))+(([02468][048])|([13579][26])))))$" ValidationGroup="ValAppInstall"></asp:RegularExpressionValidator>
                                <asp:CustomValidator ID="custom_Et_Date" runat="server" ClientValidationFunction="EndDateCheck" ControlToValidate="txtEDate" ForeColor="Red" Display="Dynamic" ErrorMessage="To date should be Greater than or equal to current date & From date " SetFocusOnError="true" ValidationGroup="ValAppInstall"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                    <div class="box-body">
                        <asp:GridView ID="gvFreezeView" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="freeze_MASTER_ID"
                            CssClass="table table-condensed table-striped table-hover table-bordered" ShowHeaderWhenEmpty="true" OnRowDataBound="gvFreezeView_RowDataBound" ClientIDMode="Static">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl.No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="VIN Number" DataField="freeze_VIN_NUMBER" />
                                <asp:BoundField HeaderText="Vehicle " DataField="freeze_VEHICLENAME" />
                                <asp:BoundField HeaderText="ECU " DataField="freeze_ECUNAME" />
                                <asp:BoundField HeaderText="ODO Value" DataField="freeze_ODOValue" />
                                <asp:BoundField HeaderText="App Version" DataField="freeze_APP_VERSION" />
                                <asp:BoundField HeaderText="Mobile No." DataField="freeze_MOBILE_NUMBER" />
                                <asp:BoundField HeaderText="Source" DataField="freeze_SOURCE" />

                                <asp:BoundField HeaderText="Latitude" DataField="freeze_LATITUDE" />
                                <asp:BoundField HeaderText="Longitude" DataField="freeze_LONGITUDE" />
                                <asp:TemplateField HeaderText="Timestamp" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd-MMM-yyyy hh:mm:ss tt}",Convert.ToDateTime(Eval("freeze_TIMESTAMP"))) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Server Timestamp" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd-MMM-yyyy hh:mm:ss tt}",Convert.ToDateTime(Eval("freeze_UPDATEDON"))) %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <input type="button" class="btn btn-info btn-xs" value="DTC" onclick="javascript: showModal(this);" />
                                        <asp:HiddenField ID="hfvDTCID" Value='<%#Convert.ToString(Eval("freeze_MASTER_ID"))%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Adv. Analysis" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <input type="button" class="btn bg-slate-400 btn-xs" value="Adv. Analysis" onclick="javascript: showAAModal(this);" />
                                        <asp:HiddenField ID="hfvDTCBuildID" Value='<%#Convert.ToString(Eval("freeze_MASTER_ID"))%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DTC Code" ItemStyle-Wrap="true" ItemStyle-Height="1px" HeaderStyle-Height="1px">
                                    <ItemTemplate>
                                        <span>
                                            <%#getDTCCode(Eval("freeze_MASTER_ID"))%>
                                        </span>
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
            <div class="modal fade" id="ViewDTC" tabindex="-1" role="dialog" aria-labelledby="ViewDTCLabel" aria-hidden="true">
                <div class="modal-dialog" style="width: 900px; height: 350px">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #00acd6; color: #FCFCFC;">
                            <button type="button" id="btnModalClose" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h3 class="modal-title" id="myModalDLabel" runat="server">&nbsp;DTC View</h3>
                        </div>
                        <div class="modal-body">
                            <div class="row" style="margin-top: 1%">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <table id="gvDTC" class="display" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>DTC Code</th>
                                                <th>Description</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer"></div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="modal fade" id="ViewAA" tabindex="-1" role="dialog" aria-labelledby="ViewAALabel" aria-hidden="true">
                <div class="modal-dialog" style="width: 440px; height: 350px">
                    <div class="modal-content">
                        <div class="modal-header" style="background-color: #00acd6; color: #FCFCFC;">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h3 class="modal-title" id="H1" runat="server">&nbsp;Advance Analysis</h3>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <table id="gvAA" class="display" cellspacing="0" width="100%">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Smiley </th>
                                                <th>Description </th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer"></div>
                    </div>
                </div>
            </div>
        </div>

    </section>

    <style>
        .small {
            width: 0px;
            min-width: 0px;
            max-width: 0px;
        }

        td.details-control {
            background: url('/Content/images/details_open.png') no-repeat center center;
            padding-left: 45px;
        }

        tr.shown td.details-control {
            background: url('/Content/images/details_close.png') no-repeat center center;
        }

        #gvDTC tbody, #gvAA tbody {
            display: table-row-group;
            vertical-align: middle;
            border-color: inherit;
        }

        #gvDTC tr, #gvAA tr {
            display: table-row;
            vertical-align: inherit;
            border-color: inherit;
        }

        .dataTables_length > label > span:first-child {
            margin-top: 1px !important;
        }

        #gvDTC {
            border-collapse: collapse;
            width: 100%;
        }

            #gvDTC td, #gvAA td {
                text-align: left;
                padding-left: 12px;
                padding-top: 12px;
                padding-right: 15px;
            }

        #gvAA th {
            background-color: transparent;
            color: white !important;
            text-align: center;
            padding-top: 12px;
            padding-right: 15px;
            cursor: pointer;
        }

        #gvDTC th {
            /*background-color: #00acd6;
            color: white !important;*/
            text-align: center;
            padding-top: 12px;
            padding-right: 15px;
            cursor: pointer;
        }

        #gvDTC tbody tr, #gvAA tbody tr {
            top: 2%;
            text-align: center;
        }

        #gvDTC td {
            border: 1px solid #DDDDDD;
        }

        #gvDTC tr {
            border: 1px solid #DDDDDD;
        }

        .tabst td {
            border-bottom: dotted 1px #c4c4c4 !important;
            border-top: dotted 1px #c4c4c4 !important;
            border-left: none;
            border-right: none;
        }

        .label-yellow {
            background-color: #FFFF00 !important;
        }

        #gvAA_filter, #gvAA_length, #gvAA_info {
            display: none;
        }

        .red {
            color: white;
            background-image: url("/Content/images/red.jpg");
        }

        .Yellow {
            background-image: url("/Content/images/yellow.jpg");
        }

        .green {
            background-image: url("/Content/images/green.jpg");
        }

        .gray {
            background-image: url("/Content/images/gray.jpg");
        }
    </style>
    <script src='<%= Page.ResolveUrl("~/Content/js/jquery.min.js") %>'></script>
    <script src='<%= Page.ResolveUrl("~/Content/js/bootstrap-datepicker.js") %>'></script>
    <script>
        //iCheck for checkbox and radio inputs
        //$('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        //    checkboxClass: 'icheckbox_minimal-blue',
        //    radioClass: 'iradio_minimal-blue',
        //});


        $(document).ready(function () {
            var totalRows = $("#<%=gvFreezeView.ClientID%> thead tr").length;
            if (totalRows > 0) {
                dt = $('#<%=gvFreezeView.ClientID%>').dataTable({
                    "iDisplayLength": 50,
                    "aaSorting": [[0, 'asc']],
                    "bStateSave": true,
                    "scrollX": true,
                    "scrollY": "500px",
                    "scrollCollapse": true,
                    "aLengthMenu": [[50, 200, 300, -1], [50, 200, 300, "All"]],
                });
            }
            if ($("#<%=hfvDateval.ClientID %>").val() == 0) $('#rdCurDate').prop('checked', true);
            else if ($("#<%=hfvDateval.ClientID %>").val() == 1) $('#rdYTD').prop('checked', true);
            else if ($("#<%=hfvDateval.ClientID %>").val() == 2) $('#rdDateRange').prop('checked', true);
        });

    $('#gvDTC').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dtTable.api().row(tr);
        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    function format(d) {
        var tableData = "";
        var obj = Object.keys(d);
        tableData = '<table id="rowTable" cellpadding="5" cellspacing="5" border="0" style="width: 90%;margin-bottom:12px" align="center">';
        var i = 0;
        for (var key in d) {
            if (key == "freeze_DTC_CODE" || key == "freeze_DTC_DESCRIPTION" || key == "freeze_DTC_STATUS")
                continue;
            else if (d[key] == null || d[key] == "")
                continue;
            else if (d[key] != null || d[key] != "") {
                if (i == 0)
                    tableData += '<tr >';

                while (i < 2) {
                    if (d[key] == 0)
                        tableData += "<td style='text-align:left;'><b>" + key + "</b></td><td style='text-align:left;'>&nbsp;&nbsp;" + "--" + '</td>';
                    else
                        tableData += "<td style='text-align:left;'><b>" + key + "</b></td><td style='text-align:left;'>&nbsp;&nbsp;" + d[key] + '</td>';
                    i++;
                    break;
                }
                if (i == 2) {
                    tableData += '</tr>';
                    i = 0;
                }
            }
        }
        tableData += '</table>';
        return tableData;
    }

    var jsonVal = null;
    var responseString = null;
    var finalString = null;
    var dt = null;
    var dtTable = null;
    var dtAATable = null;

    function close() {
        dtAATable = $('#gvAA').dataTable();
        dtAATable.fnDestroy();
    }

    function check() {
        console.log($('#gvDTC tbody td.details-control'));
    }

    function CheckDate() {
        if ($('#rdCurDate').is(':checked')) {
            $("#<%=hfvDateval.ClientID %>").val('0');
            $("#<%=txtSDate.ClientID %>").val('');
            $("#<%=txtEDate.ClientID %>").val('');

            $("#<%=txtSDate.ClientID %>").attr("disabled", "disabled");
            $("#<%=txtEDate.ClientID %>").attr("disabled", "disabled");

            ValidatorEnable(document.getElementById('<%=RFV_txtSDate.ClientID%>'), false);
            ValidatorEnable(document.getElementById('<%=RFV_txtEDate.ClientID%>'), false);
        }
        else if ($('#rdYTD').is(':checked')) {
            $("#<%=hfvDateval.ClientID %>").val('1');
            $("#<%=txtSDate.ClientID %>").val('');
            $("#<%=txtEDate.ClientID %>").val('');
            $("#<%=txtSDate.ClientID %>").attr("disabled", "disabled");
            $("#<%=txtEDate.ClientID %>").attr("disabled", "disabled");

            ValidatorEnable(document.getElementById('<%=RFV_txtSDate.ClientID%>'), false);
            ValidatorEnable(document.getElementById('<%=RFV_txtEDate.ClientID%>'), false);
        }
        else if ($('#rdDateRange').is(':checked')) {
            $("#<%=hfvDateval.ClientID %>").val('2');
            $("#<%=txtSDate.ClientID %>").removeAttr("disabled");
            $("#<%=txtEDate.ClientID %>").removeAttr("disabled");

            ValidatorEnable(document.getElementById('<%=RFV_txtSDate.ClientID%>'), true);
            ValidatorEnable(document.getElementById('<%=RFV_txtEDate.ClientID%>'), true);
        }
}
$('#<%=txtSDate.ClientID %> ').datepicker({
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: false,
            autoclose: true
        });
        $('#<%=txtEDate.ClientID %> ').datepicker({
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: false,
            autoclose: true
        });

        var TodayDate = new Date();
        var day = TodayDate.getDate();
        var Month = TodayDate.getMonth() + 1;
        var Year = TodayDate.getFullYear();

        function StartDateCheck(sender, args) {
            var CurrentDate = Month + "/" + day + "/" + Year;
            var StartDate = args.Value;
            var EndDate = $('#<%=txtEDate.ClientID %> ').val();

            if (Date.parse(StartDate) > Date.parse(CurrentDate)) args.IsValid = false;
            else {
                if ((EndDate.length) > 0) {
                    if (Date.parse(EndDate) < Date.parse(StartDate)) args.IsValid = false;
                    else args.IsValid = true;
                }
                else args.IsValid = true;
            }
        }
        function EndDateCheck(sender, args) {
            var CurrentDate = Month + "/" + day + "/" + Year;
            var StartDate = $('#<%=txtSDate.ClientID %> ').val();
            var EndDate = args.Value;

            if (Date.parse(EndDate) > Date.parse(CurrentDate)) args.IsValid = false;
            else {
                if ((EndDate.length) > 0) {
                    if (Date.parse(EndDate) < Date.parse(StartDate)) args.IsValid = false;
                    else args.IsValid = true;
                }
                else args.IsValid = true;
            }
        }

        function showAAModal(Param) {
            var idVal = $(Param).next().val();
            dtAATable = $('#gvAA').dataTable();
            dtAATable.fnDestroy();
            $.ajax({
                type: "POST",
                <%--url: '<%= ResolveUrl("Vehicle_DTC_Report.aspx/GetAAA") %>',--%>
                url: '<%= ResolveUrl("~/Minismart.asmx/GetAAA") %>',

                contentType: "application/json; charset=utf-8",
                data: '{"ID":"' + idVal + '"}',
                dataType: "json",
                success: function (response) {
                    jsonVal = JSON.stringify(response.d);
                    responseString = response.d.replace('{\r\n  \"Table\":', "");
                    finalString = responseString.slice(0, -1);
                    data1 = JSON.parse(response.d);
                    dtTable = $('#gvAA').dataTable({
                        "aaData": JSON.parse(finalString),
                        "language": {
                            "emptyTable": "No Data Found!"
                        },
                        "iDisplayLength": 5,
                        "bAutoWidth": false,
                        "columns": [
                              {
                                  "className": "DTStyle",
                                  "visible": false,
                                  "orderable": false,
                                  "data": null,
                                  "defaultContent": ""
                              },
                                {
                                    "render": function (data, type, full, meta) {
                                        if (full.dtc_LOGIC_SMILEY != null)
                                            return '<img style="margin-left: 0%;" src=' + full.dtc_LOGIC_SMILEY.slice(3) + '>';
                                    }
                                },
                            { "data": "dtc_LOGIC_CONCLUSION" }
                        ],
                        "fnDrawCallback": function (oSettings) {
                            $('#gvAA thead').remove();
                        }
                    });
                },

                failure: function (response) {
                    console.log(response);
                },
                error: function (response) {
                    console.log(response);
                }
            });
            $("#ViewAA").modal('show');
        }

        function showModal(Param) {
            var idVal = $(Param).next().val();
            dtTable = $('#gvDTC').dataTable();
            dtTable.fnDestroy();
            $.ajax({
                type: "POST",
                <%--url: '<%= ResolveUrl("Vehicle_DTC_Report.aspx/GetDTC") %>',--%>
                url: '<%= ResolveUrl("~/Minismart.asmx/GetDTC") %>',

                contentType: "application/json; charset=utf-8",
                data: '{"ID":"' + idVal + '"}',
                dataType: "json",
                success: function (response) {
                    jsonVal = JSON.stringify(response.d);
                    responseString = response.d.replace('{\r\n  \"Table\":', "");
                    finalString = responseString.slice(0, -1);
                    data1 = JSON.parse(response.d);
                    var jsonResult = JSON.parse(finalString);
                    if (dtTable) {
                        if ($.fn.DataTable.isDataTable('#gvDTC')) {
                            $('#gvDTC').DataTable().destroy();
                        }
                        $('#gvDTC tbody').empty();
                    }
                    if (finalString.length > 5) {
                        dtTable = $('#gvDTC').dataTable({
                            "aaData": JSON.parse(finalString),
                            "bAutoWidth": false,
                            "aoColumns": [
                                {
                                    "sClass": "details-control",
                                    "orderable": false,
                                    "data": null,
                                    "defaultContent": ""
                                },
                                { "mData": "freeze_DTC_CODE" },
                                { "mData": "freeze_DTC_DESCRIPTION" },
                                { "mData": "freeze_DTC_STATUS" }
                            ],
                            "fnDrawCallback": function (oSettings) {
                                $('#gvDTC tbody tr').each(function () {
                                    $(this).find('td:gt(3)').remove();
                                });
                                var totaltd = $('#gvDTC tbody td');
                                for (var i = 0, j = totaltd.length; i < j; ++i) {
                                    if ($(totaltd[i]).text() == "Current")
                                        $(totaltd[i]).addClass("red");
                                    else if ($(totaltd[i]).text() == "Healed")
                                        $(totaltd[i]).addClass("Yellow");
                                    else if ($(totaltd[i]).text() == "Others")
                                        $(totaltd[i]).addClass("gray");
                                }
                            },
                            "order": [[1, 'asc']]
                        });
                    }
                    else {
                        dtTable = $('#gvDTC').dataTable({
                            "aaData": null,
                            "language": {
                                "emptyTable": "No Data Found!"
                            }
                        });
                    }
                },

                failure: function (response) {
                    console.log(response);
                },
                error: function (response) {
                    console.log(response);
                }
            });
            $("#ViewDTC").modal('show');
        }
    </script>
</asp:Content>

