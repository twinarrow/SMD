using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

public partial class Modules_DTC_Vehicle_DTC_Report : System.Web.UI.Page
{
    const String STR_ACCOUNTING_FORMAT = @"_(#,##0_);_(\(#,##0\);_(""-""??_);_(@_)";

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        makeAccessible(gvFreezeView);
    }

    private static void makeAccessible(GridView grid)
    {
        if (grid.Rows.Count <= 0) return;
        grid.UseAccessibleHeader = true;
        grid.HeaderRow.TableSection = TableRowSection.TableHeader;
        if (grid.ShowFooter) grid.FooterRow.TableSection = TableRowSection.TableFooter;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginName"] == null) Response.Redirect("~/default.aspx?err=AUT201");
        if (!Page.IsPostBack)
        {
            hfvDateval.Value = "0";
            BindFreezeDetails();
        }
    }

    private void BindFreezeDetails()
    {
        string _sStartDate = "";
        string _sEndDate = "";

        string _strStart = "";
        string _strEnd = "";
        int DateSelect = Convert.ToInt32(hfvDateval.Value);
        var dt = DateTime.Now;

        if (DateSelect == 0)
        {
            _sStartDate = dt.Month + "/" + dt.Day + "/" + dt.Year;
            _sEndDate = dt.Month + "/" + (dt.Day) + "/" + dt.Year;
            txtSDate.Enabled = false;
            txtEDate.Enabled = false;
            txtSDate.Text = "";
            txtEDate.Text = "";
        }
        else if (DateSelect == 1)
        {
            _sStartDate = "01/01/" + DateTime.Now.Year;
            _sEndDate = dt.Month + "/" + dt.Day + "/" + dt.Year;

            txtSDate.Enabled = false;
            txtEDate.Enabled = false;
            txtSDate.Text = "";
            txtEDate.Text = "";
        }
        else if (DateSelect == 2)
        {
            txtSDate.Enabled = true;
            txtEDate.Enabled = true;

            _strStart = txtSDate.Text;
            _strEnd = txtEDate.Text;
            if (string.IsNullOrEmpty(_strStart) || string.IsNullOrEmpty(_strEnd))
            {
                return;
            }
            else
            {
                _sStartDate = _strStart;
                _sEndDate = _strEnd;
            }
        }
        DataSet dsFreezeView = new DataSet();
        string ConQuery = string.Empty;
        var ConStr = ConnectionString.GetDBConnectionString();

        ConQuery = "SELECT * FROM tab_CA_EMS_MASTER_INFO WHERE CONVERT(date, freeze_TIMESTAMP, 101) between '" + _sStartDate + "' and '" + _sEndDate + "'";
        dsFreezeView = DBL.executeDataSet(ConStr, CommandType.Text, ConQuery, null);
        if (dsFreezeView != null && dsFreezeView.Tables[0].Rows.Count > 0)
        {
            gvFreezeView.DataSource = dsFreezeView;
            gvFreezeView.DataBind();
        }
        else
        {
            gvFreezeView.DataSource = null;
            gvFreezeView.DataBind();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        BindFreezeDetails();
    }

    public string getDTCCode(object _Param)
    {
        string res = string.Empty;
        string temp = string.Empty;
        int MasterID = 0;
        if (_Param != DBNull.Value)
        {
            MasterID = Convert.ToInt32(_Param);
            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                var qry = from m in db.tab_CA_EMS_MASTER_INFOs
                          join d in db.tab_CA_EMS_DTC_INFOs on m.freeze_MASTER_ID equals d.freeze_MASTER_ID
                          where m.freeze_MASTER_ID == MasterID
                          select new { d.freeze_DTC_CODE };
                if (qry.Count() > 0)
                {
                    foreach (var a in qry)
                    {
                        temp = temp + ", " + Convert.ToString(a.freeze_DTC_CODE);
                        res = temp.Remove(0, 1);
                    }
                }
            }
        }
        return res;
    }

    private DataTable GetDataTable(string _sStartDate, string _sEndDate)
    {
        DataTable objDT = new DataTable();
        DataSet dsFreezeView = new DataSet();
        string ConQuery = string.Empty;
        var ConStr = ConnectionString.GetDBConnectionString();

        ConQuery = "SELECT freeze_MASTER_ID,freeze_VIN_NUMBER,freeze_MOBILE_NUMBER,freeze_LATITUDE,freeze_LATITUDE,freeze_TIMESTAMP,freeze_ECUNAME,freeze_VEHICLENAME,freeze_LOGINID,freeze_LOGINNAME,freeze_MOBILE_MAC_ADDRESS,freeze_VCIID,freeze_APP_VERSION,freeze_DEALER_NAME,freeze_AREA,freeze_LOCATION,freeze_SOURCE,freeze_UPDATEDON,freeze_ODOValue FROM tab_CA_EMS_MASTER_INFO WHERE CONVERT(date, freeze_TIMESTAMP, 101) between '" + _sStartDate + "' and '" + _sEndDate + "'";
        dsFreezeView = DBL.executeDataSet(ConStr, CommandType.Text, ConQuery, null);

        objDT = dsFreezeView.Tables[0];
        return objDT;
    }

    private DataTable GetDTCVale(int ID)
    {
        DataTable objDT = new DataTable();
        var ConStr = ConnectionString.GetDBConnectionString();

        string Query = string.Empty;
        Query = " select d.freeze_DTC_CODE, d.freeze_DTC_DESCRIPTION, d.freeze_DTC_STATUS, freeze_SIGNAL, freeze_VALUE from tab_CA_EMS_DTC_INFO d ";
        Query += " join tab_CA_EMS_FREEZEFRAME_DATA fm on d.dtc_id = fm.dtc_INFO_ID where fm.MASTER_ID = '" + ID + "'";

        DataSet dsDTCCode = DBL.executeDataSet(ConStr, CommandType.Text, Query, null);
        objDT = dsDTCCode.Tables[0];
        return objDT;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        CreateExcel();
    }

    private void CreateExcel()
    {
        string _sStartDate = "";
        string _sEndDate = "";
        string _strStart = "";
        string _strEnd = "";
        int DateSelect = Convert.ToInt32(hfvDateval.Value);
        var dt = DateTime.Now;
        if (DateSelect == 0)
        {
            _sStartDate = dt.Month + "/" + dt.Day + "/" + dt.Year;
            _sEndDate = dt.Month + "/" + (dt.Day) + "/" + dt.Year;
            txtSDate.Enabled = false;
            txtEDate.Enabled = false;
            txtSDate.Text = "";
            txtEDate.Text = "";
        }
        else if (DateSelect == 1)
        {
            _sStartDate = "01/01/" + DateTime.Now.Year;
            _sEndDate = dt.Month + "/" + dt.Day + "/" + dt.Year;

            txtSDate.Enabled = false;
            txtEDate.Enabled = false;
            txtSDate.Text = "";
            txtEDate.Text = "";
        }
        else if (DateSelect == 2)
        {
            txtSDate.Enabled = true;
            txtEDate.Enabled = true;

            _strStart = txtSDate.Text;
            _strEnd = txtEDate.Text;
            if (string.IsNullOrEmpty(_strStart) || string.IsNullOrEmpty(_strEnd))
            {
                return;
            }
            else
            {
                _sStartDate = _strStart;
                _sEndDate = _strEnd;
            }
        }

        using (ExcelPackage xlPack = new ExcelPackage())
        {
            xlPack.Workbook.Properties.Title = "Remote Diagnostic Tool - Vehicle DTC Report";

            //Worksheet
            xlPack.Workbook.Worksheets.Add("Vehicle DTC Report");
            ExcelWorksheet xlSheet = xlPack.Workbook.Worksheets[1];
            xlSheet.Name = "Vehicle DTC Report";
            xlSheet.Cells["A:XFD"].Style.Font.Name = "Arial";

            DataTable ObjDT = GetDataTable(_sStartDate, _sEndDate);

            //Title 
            var cell = xlSheet.Cells[1, 1, 1, 17];
            cell.Value = "Remote Diagnostic Tool - Vehicle DTC Report";
            cell.Merge = true;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            var fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            xlSheet.Row(1).Height = 32;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

            cell = xlSheet.Cells[3, 1];
            cell.Value = "Date";
            cell.Merge = true;
            cell.Style.Font.Size = 11;
            xlSheet.Row(3).Height = 32;
            cell.Style.Font.Name = "Calibri";
            fill = cell.Style.Fill;
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

            cell = xlSheet.Cells[3, 2, 3, 3];
            cell.Value = _sStartDate + " to " + _sEndDate;
            cell.Merge = true;
            cell.Style.Font.Size = 11;
            cell.Style.Font.Name = "Calibri";
            fill = cell.Style.Fill;
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

            int RowIndex = 5; int colIndex = 1, slNo = 0;


            //Header
            cell = xlSheet.Cells[RowIndex, 1];
            cell.Value = "Sl.No.";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            xlSheet.Row(RowIndex).Height = 32;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 2];
            cell.Value = "VIN Number";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 3];
            cell.Value = "Vehicle Name";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 4];
            cell.Value = "ECU Name";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 5];
            cell.Value = "ODO Value";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 6];
            cell.Value = "App Version";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 7];
            cell.Value = "Mobile No.";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 8];
            cell.Value = "Source";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 9];
            cell.Value = "Latitude";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 10];
            cell.Value = "Longitude";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 11];
            cell.Value = "Timestamp";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 12];
            cell.Value = "Server Timestamp";
            cell.Merge = true;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 13];
            cell.Value = "DTC Code";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 14];
            cell.Value = "Description";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 15];
            cell.Value = "Status";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 16];
            cell.Value = "Signals";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);

            cell = xlSheet.Cells[RowIndex, 17];
            cell.Value = "Value";
            cell.Style.Font.Bold = true;
            cell.Merge = true;
            cell.Style.Font.Size = 10;
            cell.Style.Font.Name = "Calibri";
            cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
            cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            fill = cell.Style.Fill;
            fill.PatternType = ExcelFillStyle.Solid;
            fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 172, 214));
            cell.Style.Border.Right.Style = ExcelBorderStyle.Hair;
            cell.Style.Border.Right.Color.SetColor(System.Drawing.Color.White);


            int Count = 0;
            int subRowCount = 0;
            int mergeRowIndex = 0;

            foreach (DataRow row in ObjDT.Rows)
            {
                if (Count == 0)
                    RowIndex++;
                slNo++;
                DataTable dtSub = GetDTCVale(Convert.ToInt32(row["freeze_MASTER_ID"]));
                if (dtSub.Rows.Count > 0)
                    subRowCount = (dtSub.Rows.Count - 1) + RowIndex;
                else
                    subRowCount = RowIndex;

                DataRow(xlSheet, RowIndex, 1, subRowCount, 1, true, "Sl.No.", slNo);
                DataRow(xlSheet, RowIndex, 2, subRowCount, 2, false, "VIN Number", row["freeze_VIN_NUMBER"]);
                DataRow(xlSheet, RowIndex, 3, subRowCount, 3, false, "Vehicle Name", row["freeze_VEHICLENAME"]);
                DataRow(xlSheet, RowIndex, 4, subRowCount, 4, false, "ECU Name", row["freeze_ECUNAME"]);
                DataRow(xlSheet, RowIndex, 5, subRowCount, 5, false, "ODO Value", row["freeze_ODOValue"]);
                DataRow(xlSheet, RowIndex, 6, subRowCount, 6, false, "App Version", row["freeze_APP_VERSION"]);
                DataRow(xlSheet, RowIndex, 7, subRowCount, 7, false, "Mobile No.", row["freeze_MOBILE_NUMBER"]);
                DataRow(xlSheet, RowIndex, 8, subRowCount, 8, false, "Source", row["freeze_SOURCE"]);
                DataRow(xlSheet, RowIndex, 9, subRowCount, 9, false, "Latitude", row["freeze_LATITUDE"]);
                DataRow(xlSheet, RowIndex, 10, subRowCount, 10, false, "Longitude", row["freeze_LATITUDE"]);
                DataRow(xlSheet, RowIndex, 11, subRowCount, 11, false, "Timestamp", row["freeze_TIMESTAMP"]);
                DataRow(xlSheet, RowIndex, 12, subRowCount, 12, false, "Server Timestamp", row["freeze_UPDATEDON"]);

                if (dtSub.Rows.Count > 0)
                {
                    int vehicleMergeIndex = RowIndex;
                    int i = 1;
                    foreach (DataRow rowSub in dtSub.Rows)
                    {
                        if (Convert.ToString(rowSub["freeze_DTC_CODE"]) == (i < dtSub.Rows.Count ? Convert.ToString(dtSub.Rows[i]["freeze_DTC_CODE"]) : ""))
                        {
                            mergeRowIndex += 1;
                        }
                        else
                        {
                            if (vehicleMergeIndex != RowIndex)
                            {
                                DataRow(xlSheet, vehicleMergeIndex, 13, vehicleMergeIndex + mergeRowIndex, 13, false, "DTC Code", rowSub["freeze_DTC_CODE"]);
                                DataRow(xlSheet, vehicleMergeIndex, 14, vehicleMergeIndex + mergeRowIndex, 14, false, "Description", rowSub["freeze_DTC_DESCRIPTION"]);
                                DataRow(xlSheet, vehicleMergeIndex, 15, vehicleMergeIndex + mergeRowIndex, 15, false, "Status", rowSub["freeze_DTC_STATUS"]);
                            }
                            else
                            {
                                DataRow(xlSheet, RowIndex, 13, RowIndex, 13, false, "DTC Code", rowSub["freeze_DTC_CODE"]);
                                DataRow(xlSheet, RowIndex, 14, RowIndex, 14, false, "Description", rowSub["freeze_DTC_DESCRIPTION"]);
                                DataRow(xlSheet, RowIndex, 15, RowIndex, 15, false, "Status", rowSub["freeze_DTC_STATUS"]);
                            }
                            mergeRowIndex = 0;
                        }
                        DataRow(xlSheet, RowIndex, 13, RowIndex, 13, false, "DTC Code", rowSub["freeze_DTC_CODE"]);
                        DataRow(xlSheet, RowIndex, 14, RowIndex, 14, false, "Description", rowSub["freeze_DTC_DESCRIPTION"]);
                        DataRow(xlSheet, RowIndex, 15, RowIndex, 15, false, "Status", rowSub["freeze_DTC_STATUS"]);
                        DataRow(xlSheet, RowIndex, 16, RowIndex, 16, false, "Signals", rowSub["freeze_SIGNAL"]);
                        DataRow(xlSheet, RowIndex, 17, RowIndex, 17, false, "Value", rowSub["freeze_VALUE"]);
                        RowIndex++;
                        if (Convert.ToString(rowSub["freeze_DTC_CODE"]) != (i < dtSub.Rows.Count ? Convert.ToString(dtSub.Rows[i]["freeze_DTC_CODE"]) : ""))
                        {
                            vehicleMergeIndex = RowIndex;
                        }
                        i++;
                    }
                }
                else
                {
                    DataRow(xlSheet, RowIndex, 13, RowIndex, 13, false, "DTC Code", "");
                    DataRow(xlSheet, RowIndex, 14, RowIndex, 14, false, "Description", "");
                    DataRow(xlSheet, RowIndex, 15, RowIndex, 15, false, "Status", "");
                    DataRow(xlSheet, RowIndex, 16, RowIndex, 16, false, "Signals", "");
                    DataRow(xlSheet, RowIndex, 17, RowIndex, 17, false, "Value", "");
                    RowIndex++;
                }
                Count++;
            }
            xlSheet.View.FreezePanes(6, 7);
            //Save
            Byte[] bin = xlPack.GetAsByteArray();
            string fileName = "Vehicle_DTC_Report_" + string.Format("{0:yyyy}", DateTime.Now) + ".xlsx";
            string file = Server.MapPath("../../Resources/Reports/" + fileName);

            if (File.Exists(file))
                File.Delete(file);

            File.WriteAllBytes(file, bin);

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.TransmitFile(file);
            Response.End();
        }
    }

    private void DataRow(ExcelWorksheet Sheet, int FromRowIndex, int FromColIndex, int ToRowIndex, int ToColIndex, bool isNumberFormatRequired, string Field, object Value)
    {
        bool IsDate = false;
        var cell = Sheet.Cells[FromRowIndex, FromColIndex, ToRowIndex, ToColIndex];

        cell.Style.Font.Size = 10;
        if (Field == "Timestamp" || Field == "Server Timestamp")
        {
            if (isValidDate(Value))
            {
                cell.Value = string.Format("{0:dd-MMM-yyyy HH:mm:ss tt}", Convert.ToDateTime(Value));
            }
            IsDate = true;
        }
        else
            cell.Value = Value;
        cell.Style.Font.Name = "Calibri";
        if (FromRowIndex != ToRowIndex)
            cell.Merge = true;
        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        var fill = cell.Style.Fill;
        fill.PatternType = ExcelFillStyle.Solid;
        fill.BackgroundColor.SetColor(System.Drawing.Color.White);

        cell.Style.Border.BorderAround(ExcelBorderStyle.Hair, System.Drawing.Color.Black);
        SetColumnWidth(Sheet, FromRowIndex, FromColIndex, Field, IsDate);

        if (isNumberFormatRequired)
            cell.Style.Numberformat.Format = STR_ACCOUNTING_FORMAT;
    }

    public static bool isValidDate(object Expression)
    {
        DateTime retDate;
        bool isDate = DateTime.TryParse(Convert.ToString(Expression), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out retDate);
        return isDate;
    }

    private static void SetColumnWidth(ExcelWorksheet ws, int dataRows, int dataColumns, string textToAdd, bool IsDate)
    {
        double proposedCellSize;
        double cellSize = ws.Cells[dataRows, dataColumns].Worksheet.Column(dataColumns).Width;
        if (textToAdd == "Signals")
            proposedCellSize = 50;
        else if (textToAdd == "Value")
            proposedCellSize = 15;
        else
            proposedCellSize = textToAdd.Trim().Length * 2;
        if (IsDate == true)
            ws.Cells[dataRows, dataColumns].Worksheet.Column(dataColumns).Width = 25;
        else
            ws.Cells[dataRows, dataColumns].Worksheet.Column(dataColumns).Width = proposedCellSize;
    }

    protected void gvFreezeView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[14].Attributes.Add("style", "display:none;");
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[14].Attributes.Add("style", "display:none;");
        }
    }
}