using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Modules_AdvanceAnalysis_DTC_Logic_Add : System.Web.UI.Page
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        makeAccessible(gvLogicDTC);
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
            BindLogicDTC();
        }
        lblMsg.Text = string.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var LogicDTC = txtLogicDTC.Text;
            if (string.IsNullOrEmpty(LogicDTC))
                return;

            var Logic = ddlDTCLogic.SelectedValue;

            var DTCThreshold = txtDTCThreshold.Text;
            if (string.IsNullOrEmpty(DTCThreshold))
                return;

            double Threshold = Convert.ToDouble(txtDTCThreshold.Text);
            double thResult = 0;
            thResult = Math.Round(Threshold, 3);

            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                try
                {
                    if (hfvMode.Value == "Save")
                    {
                        tab_CA_EMS_DTC_LOGIC_ADD add = new tab_CA_EMS_DTC_LOGIC_ADD();
                        var addLogic = from u in db.tab_CA_EMS_DTC_LOGIC_ADDs
                                       where u.dtc_DTC_CODE == LogicDTC && u.dtc_LOGIC == Logic
                                       select u;

                        if (addLogic.Count() > 0)
                        {
                            lblMsg.Text = "DTC Logic already Exist";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowDealerModal(); });", true);
                        }
                        else
                        {
                            add.dtc_DTC_CODE = LogicDTC;
                            add.dtc_LOGIC = Logic;
                            add.dtc_LOGIC_THRESHOLD = thResult;
                            add.dtc_LOGIC_RESULT = false;
                            db.tab_CA_EMS_DTC_LOGIC_ADDs.InsertOnSubmit(add);
                            db.SubmitChanges();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('DTC Logic added successfully');  });", true);
                        }
                    }
                    else
                    {
                        if (hfvAuth != null)
                        {
                            var crtDTCID = Convert.ToInt32(hfvAuth.Value);
                            var query = from cd in db.tab_CA_EMS_DTC_LOGIC_ADDs
                                        where cd.dtc_DTC_CODE == LogicDTC && cd.dtc_LOGIC == Logic && cd.dtc_LOGIC_ADD_PK != Convert.ToUInt32(hfvAuth.Value)
                                        select new { cd.dtc_DTC_CODE };

                            if (query != null)
                            {
                                if (query.Count() > 0)
                                {
                                    lblMsg.Text = "DTC Logic already Exist";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
                                }
                                else
                                {
                                    var DTCCode = from d in db.tab_CA_EMS_DTC_LOGIC_ADDs
                                                  where d.dtc_LOGIC_ADD_PK == crtDTCID
                                                  select d;
                                    foreach (var d in DTCCode)
                                    {
                                        d.dtc_DTC_CODE = LogicDTC;
                                        d.dtc_LOGIC = Logic;
                                        d.dtc_LOGIC_THRESHOLD = thResult;
                                        d.dtc_LOGIC_RESULT = false;
                                        break;
                                    }
                                    db.SubmitChanges();
                                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('Logic DTC Code has been updated successfully!'); });", true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowErrorMessage('" + Convert.ToString(ex) + "'); });", true);
                }
                finally
                {
                    BindLogicDTC();
                    btnSave.Text = "Save";
                }
            }
        }
        else
        {
            if (hfvMode.Value == "Save")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowModal(); });", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
            }
        }
    }

    protected void Edit_Click(object sender, ImageClickEventArgs e)
    {
        hfvMode.Value = "EDIT";
        myModalDLabel.InnerHtml = "<i class='fa fa-plus-circle'></i>&nbsp;Edit Logic DTC";
        GridViewRow rows = ((GridViewRow)((ImageButton)sender).NamingContainer);
        HiddenField hFvAuthID = (HiddenField)rows.FindControl("hfvAuthID");
        ClearControls();
        DisplayDeviceDetails(Convert.ToInt32(hFvAuthID.Value));
        hfvAuth.Value = hFvAuthID.Value;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
    }

    private void ClearControls()
    {
        txtLogicDTC.Text = string.Empty;
        ddlDTCLogic.SelectedIndex = 0;
        txtDTCThreshold.Text = string.Empty;
    }

    private void DisplayDeviceDetails(int DTCID)
    {
        using (miniSmartDataContext db = new miniSmartDataContext())
        {
            var DTCAdd = from d in db.tab_CA_EMS_DTC_LOGIC_ADDs
                         where d.dtc_LOGIC_ADD_PK == DTCID
                         select new { d.dtc_DTC_CODE, d.dtc_LOGIC, d.dtc_LOGIC_THRESHOLD };
            if (DTCAdd != null)
            {
                foreach (var d in DTCAdd)
                {
                    txtLogicDTC.Text = d.dtc_DTC_CODE;
                    ddlDTCLogic.SelectedValue = d.dtc_LOGIC;
                    txtDTCThreshold.Text = Convert.ToString(d.dtc_LOGIC_THRESHOLD);
                    btnSave.Text = "Update";

                    #region Enable

                    var DTCCount = (from c in db.tab_CA_EMS_DTC_LOGIC_BUILDs
                                    where c.dtc_DTC_CODE_A + "_" + c.dtc_DTC_LOGIC_A == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_B + "_" + c.dtc_DTC_LOGIC_B == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_C + "_" + c.dtc_DTC_LOGIC_C == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_D + "_" + c.dtc_DTC_LOGIC_D == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC
                                    select c.dtc_LOGIC_BUILD_PK).Count();

                    if (DTCCount > 0)
                    {
                        txtLogicDTC.Enabled = false;
                        ddlDTCLogic.Enabled = false;
                    }
                    else
                    {
                        txtLogicDTC.Enabled = true;
                        ddlDTCLogic.Enabled = true;
                    }
                    #endregion

                    break;
                }
            }
        }
    }

    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((ImageButton)sender).NamingContainer;
        int _iID = Convert.ToInt32(gvLogicDTC.DataKeys[gvr.RowIndex].Values["dtc_LOGIC_ADD_PK"]);

        using (miniSmartDataContext db = new miniSmartDataContext())
        {
            var DTCAdd = from d in db.tab_CA_EMS_DTC_LOGIC_ADDs
                         where d.dtc_LOGIC_ADD_PK == _iID
                         select new { d.dtc_DTC_CODE, d.dtc_LOGIC, d.dtc_LOGIC_THRESHOLD };
            if (DTCAdd != null)
            {
                foreach (var d in DTCAdd)
                {
                    var DTCCount = (from c in db.tab_CA_EMS_DTC_LOGIC_BUILDs
                                    where c.dtc_DTC_CODE_A + "_" + c.dtc_DTC_LOGIC_A == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_B + "_" + c.dtc_DTC_LOGIC_B == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_C + "_" + c.dtc_DTC_LOGIC_C == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC || c.dtc_DTC_CODE_D + "_" + c.dtc_DTC_LOGIC_D == d.dtc_DTC_CODE + "_" + d.dtc_LOGIC
                                    select c.dtc_LOGIC_BUILD_PK).Count();

                    if (DTCCount > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('DTC Logic Builded Already'); });", true);
                    }
                    else
                    {
                        string _sQry = "delete from tab_CA_EMS_DTC_LOGIC_ADD where dtc_LOGIC_ADD_PK=" + _iID;
                        SqlConnection _objCon = new SqlConnection(ConnectionString.GetDBConnectionString());
                        try
                        {
                            if (_objCon.State != ConnectionState.Open) _objCon.Open();
                            DBL.executeNonQuery(_objCon, CommandType.Text, _sQry, null);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('DTC Logic Deleted successfully'); });", true);
                        }
                        catch { }
                        finally
                        {
                            BindLogicDTC();
                            if (_objCon.State != ConnectionState.Closed) _objCon.Close();
                            _objCon.Dispose();
                        }
                    }
                    break;
                }
            }
        }
    }

    private void BindLogicDTC()
    {
        string sqry = "select * from tab_CA_EMS_DTC_LOGIC_ADD";
        DataSet objDs = DBL.executeDataSet(Config.getConnString(), CommandType.Text, sqry, null);
        if (objDs != null && objDs.Tables[0].Rows.Count > 0)
        {
            gvLogicDTC.DataSource = objDs.Tables[0];
            gvLogicDTC.DataBind();
        }
        else
        {
            gvLogicDTC.DataSource = null;
            gvLogicDTC.DataBind();
        }
    }
}