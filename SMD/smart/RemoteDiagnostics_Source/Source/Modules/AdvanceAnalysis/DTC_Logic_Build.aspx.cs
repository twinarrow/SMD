using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

public partial class Modules_AdvanceAnalysis_DTC_Logic_Build : System.Web.UI.Page
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        makeAccessible(gvDTCLogicBuild);
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
            BindFreezeDetails();
            LoadDTC();
        }
    }

    public string getDTCCode(object _Param1, object _Param2)
    {
        string res = string.Empty;
        string temp = string.Empty;
        string DTC = string.Empty;
        bool Status = false;
        if (_Param1 != DBNull.Value)
        {
            DTC = Convert.ToString(_Param1);
            Status = Convert.ToBoolean(_Param2);
            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                var qry = from m in db.tab_CA_EMS_DTC_LOGIC_ADDs
                          where m.dtc_DTC_CODE == DTC
                          select m;

                if (qry.Count() > 0)
                {
                    foreach (var a in qry)
                    {
                        if (Status)
                            res = DTC + "_" + a.dtc_LOGIC + "_" + a.dtc_LOGIC_THRESHOLD + "_Invert";
                        else res = DTC + "_" + a.dtc_LOGIC + "_" + a.dtc_LOGIC_THRESHOLD;
                        break;
                    }
                }
            }
        }
        return res;
    }

    public void LoadDTC()
    {
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var dtc = from z in datacontext.tab_CA_EMS_DTC_LOGIC_ADDs
                      orderby z.dtc_LOGIC_ADD_PK
                      select z;

            if (dtc.Count() > 0)
            {
                ddlDTCA.Items.Add("Select a DTC Code");
                ddlDTCA.Items[ddlDTCA.Items.Count - 1].Value = "0";

                ddlDTCB.Items.Add("Select a DTC Code");
                ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = "0";

                ddlDTCC.Items.Add("Select a DTC Code");
                ddlDTCC.Items[ddlDTCA.Items.Count - 1].Value = "0";

                ddlDTCD.Items.Add("Select a DTC Code");
                ddlDTCD.Items[ddlDTCA.Items.Count - 1].Value = "0";

                foreach (var d in dtc)
                {
                    ddlDTCA.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCA.Items[ddlDTCA.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCB.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCC.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCD.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);
                }
            }
            else
            {
                ddlDTCA.Items.Add("No DTC Found");
                ddlDTCA.Items[ddlDTCA.Items.Count - 1].Value = "-1";

                ddlDTCB.Items.Add("No DTC Found");
                ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = "-1";

                ddlDTCC.Items.Add("No DTC Found");
                ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = "-1";

                ddlDTCD.Items.Add("No DTC Found");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "-1";
            }
            ddlDTCA.SelectedIndex = 0;
            ddlDTCB.SelectedIndex = 0;
            ddlDTCC.SelectedIndex = 0;
            ddlDTCD.SelectedIndex = 0;
        }
    }

    public void LoadDTCBCD(string DTCCode)
    {
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var dtc = from z in datacontext.tab_CA_EMS_DTC_LOGIC_ADDs
                      where z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCode
                      select new { z.dtc_DTC_CODE, z.dtc_LOGIC, z.dtc_LOGIC_ADD_PK, z.dtc_LOGIC_THRESHOLD };

            ddlDTCB.Items.Clear();
            ddlDTCC.Items.Clear();
            ddlDTCD.Items.Clear();

            if (dtc.Count() > 0)
            {
                ddlDTCB.Items.Add("Select a DTC Code");
                ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = "0";

                ddlDTCC.Items.Add("Select a DTC Code");
                ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = "0";

                ddlDTCD.Items.Add("Select a DTC Code");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "0";

                foreach (var d in dtc)
                {
                    ddlDTCB.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCC.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCD.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);
                }
            }
            else
            {
                ddlDTCB.Items.Add("No DTC Found");
                ddlDTCB.Items[ddlDTCB.Items.Count - 1].Value = "-1";

                ddlDTCC.Items.Add("No DTC Found");
                ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = "-1";

                ddlDTCD.Items.Add("No DTC Found");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "-1";
            }

            ddlDTCB.SelectedIndex = 0;
            ddlDTCC.SelectedIndex = 0;
            ddlDTCD.SelectedIndex = 0;
        }
    }

    public void LoadDTCACD(string DTCCodeA, string DTCCodeB)
    {
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var dtc = from z in datacontext.tab_CA_EMS_DTC_LOGIC_ADDs
                      where z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCodeA && z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCodeB
                      select new { z.dtc_DTC_CODE, z.dtc_LOGIC, z.dtc_LOGIC_ADD_PK, z.dtc_LOGIC_THRESHOLD };

            ddlDTCC.Items.Clear();
            ddlDTCD.Items.Clear();

            if (dtc.Count() > 0)
            {
                ddlDTCC.Items.Add("Select a DTC Code");
                ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = "0";

                ddlDTCD.Items.Add("Select a DTC Code");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "0";

                foreach (var d in dtc)
                {
                    ddlDTCC.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);

                    ddlDTCD.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);
                }
            }
            else
            {
                ddlDTCC.Items.Add("No DTC Found");
                ddlDTCC.Items[ddlDTCC.Items.Count - 1].Value = "-1";

                ddlDTCD.Items.Add("No DTC Found");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "-1";
            }

            ddlDTCC.SelectedIndex = 0;
            ddlDTCD.SelectedIndex = 0;
        }
    }

    public void LoadDTCABD(string DTCCodeA, string DTCCodeB, string DTCCodeC)
    {
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var dtc = from z in datacontext.tab_CA_EMS_DTC_LOGIC_ADDs
                      where z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCodeA && z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCodeB && z.dtc_DTC_CODE + "_" + z.dtc_LOGIC != DTCCodeC
                      select new { z.dtc_DTC_CODE, z.dtc_LOGIC, z.dtc_LOGIC_ADD_PK, z.dtc_LOGIC_THRESHOLD };

            ddlDTCD.Items.Clear();

            if (dtc.Count() > 0)
            {
                ddlDTCD.Items.Add("Select a DTC Code");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "0";

                foreach (var d in dtc)
                {
                    ddlDTCD.Items.Add(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC + "_" + d.dtc_LOGIC_THRESHOLD);
                    ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = Convert.ToString(d.dtc_DTC_CODE + "_" + d.dtc_LOGIC);
                }
            }
            else
            {
                ddlDTCD.Items.Add("No DTC Found");
                ddlDTCD.Items[ddlDTCD.Items.Count - 1].Value = "-1";
            }

            ddlDTCD.SelectedIndex = 0;
        }
    }

    private void BindFreezeDetails()
    {
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var Details = from f in datacontext.tab_CA_EMS_DTC_LOGIC_BUILDs
                          select f;
            if (Details.Count() > 0)
            {
                gvDTCLogicBuild.DataSource = Details;
                gvDTCLogicBuild.DataBind();
            }
            else
            {
                gvDTCLogicBuild.DataSource = null;
                gvDTCLogicBuild.DataBind();
            }
        }
    }

    public string getDateTime(DateTime timeStamp, DateTime Updated)
    {
        string FinalDate = string.Empty;
        string Date = timeStamp.ToString("dd-MMM-yyyy");
        string Time = Updated.ToString("hh:mm");
        FinalDate = Date + " " + Time;
        return FinalDate;
    }

    public static string getDataTable_Json(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName.Trim(), dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

    public void CleartextBoxes()
    {
        txtConclusion.Text = string.Empty;
        ddlDTCA.SelectedIndex = 0;
        ddlDTCB.SelectedIndex = 0;
        ddlDTCC.SelectedIndex = 0;
        ddlDTCD.SelectedIndex = 0;
        ddlOperation.SelectedIndex = 0;
        hfvMode.Value = "Add";
    }

    protected void lnk_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        GridViewRow gvr = btn.NamingContainer as GridViewRow;
        string ID = gvDTCLogicBuild.DataKeys[gvr.RowIndex].Values["dtc_LOGIC_BUILD_PK"].ToString();
        hfvMgrID.Value = ID;
        hfvMode.Value = "Edit";
        using (miniSmartDataContext db = new miniSmartDataContext())
        {
            var qry = from b in db.tab_CA_EMS_DTC_LOGIC_BUILDs
                      where b.dtc_LOGIC_BUILD_PK == Convert.ToInt32(ID)
                      select new { b.dtc_DTC_CODE_A, b.dtc_DTC_LOGIC_A, b.dtc_DTC_CODE_B, b.dtc_DTC_LOGIC_B, b.dtc_DTC_CODE_C, b.dtc_DTC_LOGIC_C, b.dtc_DTC_CODE_D, b.dtc_DTC_LOGIC_D, b.dtc_LOGIC_CONCLUSION, b.dtc_LOGIC_OPERATION, b.dtc_LOGIC_SMILEY, b.dtc_DTC_CODE_A_INVERT, b.dtc_DTC_CODE_B_INVERT, b.dtc_DTC_CODE_C_INVERT, b.dtc_DTC_CODE_D_INVERT };

            if (qry.Count() > 0)
            {
                foreach (var c in qry)
                {
                    ddlDTCA.SelectedValue = Convert.ToString(c.dtc_DTC_CODE_A + "_" + c.dtc_DTC_LOGIC_A);
                    ddlDTCA_SelectedIndexChanged(null, null);
                    if (c.dtc_DTC_CODE_B != null)
                        ddlDTCB.SelectedValue = Convert.ToString(c.dtc_DTC_CODE_B + "_" + c.dtc_DTC_LOGIC_B);
                    else
                        ddlDTCB.SelectedIndex = 0;
                    ddlDTCB_SelectedIndexChanged(null, null);

                    if (c.dtc_DTC_CODE_C != null)
                        ddlDTCC.SelectedValue = Convert.ToString(c.dtc_DTC_CODE_C + "_" + c.dtc_DTC_LOGIC_C);
                    else
                        ddlDTCC.SelectedIndex = 0;
                    ddlDTCC_SelectedIndexChanged(null, null);

                    if (c.dtc_DTC_CODE_D != null)
                        ddlDTCD.SelectedValue = Convert.ToString(c.dtc_DTC_CODE_D + "_" + c.dtc_DTC_LOGIC_D);
                    else
                        ddlDTCD.SelectedIndex = 0;

                    if (c.dtc_DTC_CODE_A_INVERT != null)
                        DTCANotGate.Checked = Convert.ToBoolean(c.dtc_DTC_CODE_A_INVERT);
                    if (c.dtc_DTC_CODE_B_INVERT != null)
                        DTCBNotGate.Checked = Convert.ToBoolean(c.dtc_DTC_CODE_B_INVERT);
                    if (c.dtc_DTC_CODE_C_INVERT != null)
                        DTCCNotGate.Checked = Convert.ToBoolean(c.dtc_DTC_CODE_C_INVERT);
                    if (c.dtc_DTC_CODE_D_INVERT != null)
                        DTCDNotGate.Checked = Convert.ToBoolean(c.dtc_DTC_CODE_D_INVERT);

                    ddlOperation.SelectedValue = Convert.ToString(c.dtc_LOGIC_OPERATION);
                    txtConclusion.Text = Convert.ToString(c.dtc_LOGIC_CONCLUSION);

                    rbtSmiley.SelectedValue = Convert.ToString(c.dtc_LOGIC_SMILEY);
                }
            }
        }
        btnSave.InnerText = "Update";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { showEditModal(); });", true);
    }

    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((ImageButton)sender).NamingContainer;
        int _iID = Convert.ToInt32(gvDTCLogicBuild.DataKeys[gvr.RowIndex].Values["dtc_LOGIC_BUILD_PK"]);
        string _sQry = "delete from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_LOGIC_BUILD_PK=" + _iID;
        SqlConnection _objCon = new SqlConnection(ConnectionString.GetDBConnectionString());
        try
        {
            if (_objCon.State != ConnectionState.Open) _objCon.Open();
            DBL.executeNonQuery(_objCon, CommandType.Text, _sQry, null);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('DTC Logic Build Deleted successfully'); });", true);
        }
        catch { }
        finally
        {
            BindFreezeDetails();
            if (_objCon.State != ConnectionState.Closed) _objCon.Close();
            _objCon.Dispose();
        }
    }

    protected void ddlDTCA_SelectedIndexChanged(object sender, EventArgs e)
    {
        var DTCCode = ddlDTCA.SelectedValue;
        //var DTCCode = string.Empty;
        //var DTCLogic = string.Empty;

        //string[] words = DTCCodeA.Split('_');
        //DTCCode = Convert.ToString(words[0]);
        //DTCLogic = Convert.ToString(words[1]);

        LoadDTCBCD(DTCCode);
    }

    protected void ddlDTCB_SelectedIndexChanged(object sender, EventArgs e)
    {
        var DTCCodeA = ddlDTCA.SelectedValue;
        var DTCCodeB = ddlDTCB.SelectedValue;

        LoadDTCACD(DTCCodeA, DTCCodeB);
    }

    protected void ddlDTCC_SelectedIndexChanged(object sender, EventArgs e)
    {
        var DTCCodeA = ddlDTCA.SelectedValue;
        var DTCCodeB = ddlDTCB.SelectedValue;
        var DTCCodeC = ddlDTCC.SelectedValue;

        LoadDTCABD(DTCCodeA, DTCCodeB, DTCCodeC);
    }

    protected void ddlDTCD_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}