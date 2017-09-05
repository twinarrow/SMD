using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Modules_DTC_Vehicle_DTC : System.Web.UI.Page
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        makeAccessible(gvCriticalDTC);
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
            BindCriticalDTC();
        }
        lblMsg.Text = string.Empty;
    }

    protected void gvCriticalDTC_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btnCriticalDTC = (Button)e.Row.FindControl("btnCriticalDTC");
            bool IsCritical = Convert.ToBoolean(gvCriticalDTC.DataKeys[e.Row.RowIndex].Values["crt_ISCRITICAL"]);
            if (IsCritical == true)
                btnCriticalDTC.Visible = false;
            else btnCriticalDTC.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var CriticalDTC = txtVehicleDTC.Text;
            if (string.IsNullOrEmpty(CriticalDTC))
                return;
            var Logic = ddlDTCLogic.SelectedValue;
            var DTCThreshold = txtDTCThreshold.Text;
            bool Status = true;

            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                try
                {
                    if (hfvMode.Value == "Save")
                    {
                        tab_CA_CRITICAL_DTC_ADD add = new tab_CA_CRITICAL_DTC_ADD();
                        var addLogic = from u in db.tab_CA_CRITICAL_DTC_ADDs
                                       where u.crt_DTC_CODE == CriticalDTC
                                       select u;

                        if (addLogic.Count() > 0)
                        {
                            lblMsg.Text = "DTC Logic already Exist";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowDealerModal(); });", true);
                        }
                        else
                        {
                            add.crt_DTC_CODE = CriticalDTC;
                            add.crt_TYPE_OF_LOGIC = Logic;
                            add.crt_DTC_THRESHOLD = DTCThreshold;
                            add.crt_ISCRITICAL = false;
                            add.crt_DTC_STATUS = Status;
                            add.crt_DTC_CREATED_ON = DateTime.Now;
                            db.tab_CA_CRITICAL_DTC_ADDs.InsertOnSubmit(add);
                            db.SubmitChanges();
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('Vehicle DTC Logic added successfully');  });", true);
                        }
                    }
                    else
                    {
                        if (hfvAuth != null)
                        {
                            var crtDTCID = Convert.ToInt32(hfvAuth.Value);
                            var query = from cd in db.tab_CA_CRITICAL_DTC_ADDs
                                        where cd.crt_DTC_CODE == CriticalDTC && cd.crt_DTC_PK != Convert.ToUInt32(hfvAuth.Value)
                                        select new { cd.crt_DTC_CODE };

                            if (query != null)
                            {
                                if (query.Count() > 0)
                                {
                                    lblMsg.Text = "DTC Logic already Exist";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
                                }
                                else
                                {
                                    var DTCCode = from d in db.tab_CA_CRITICAL_DTC_ADDs
                                                  where d.crt_DTC_PK == crtDTCID
                                                  select d;
                                    foreach (var d in DTCCode)
                                    {
                                        d.crt_DTC_CODE = CriticalDTC;
                                        d.crt_TYPE_OF_LOGIC = Logic;
                                        d.crt_DTC_THRESHOLD = DTCThreshold;
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
                    BindCriticalDTC();
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
        myModalDLabel.InnerHtml = "<i class='fa fa-plus-circle'></i>&nbsp;Vehicle DTC";
        GridViewRow rows = ((GridViewRow)((ImageButton)sender).NamingContainer);
        bool IsCritical = Convert.ToBoolean(gvCriticalDTC.DataKeys[rows.RowIndex].Values["crt_ISCRITICAL"]);
        HiddenField hFvAuthID = (HiddenField)rows.FindControl("hfvAuthID");
        if (IsCritical == true)
        {
            ClearCriticalControls();
            DisplayCriticalDTCDetails(Convert.ToInt32(hFvAuthID.Value));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowCriticalModal(); });", true);
        }
        else
        {
            ClearControls();
            DisplayDeviceDetails(Convert.ToInt32(hFvAuthID.Value));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
        }

        hfvAuth.Value = hFvAuthID.Value;
        hfvCriticalAuth.Value = hFvAuthID.Value;
    }

    private void ClearControls()
    {
        txtVehicleDTC.Text = string.Empty;
        ddlDTCLogic.SelectedIndex = 0;
        txtDTCThreshold.Text = string.Empty;
    }

    private void ClearCriticalControls()
    {
        //txtCrtDTC.Text = string.Empty;
        //ddlCrtDTCLogic.SelectedIndex = 0;
        //txtCrtDTCThreshold.Text = string.Empty;
        txtCrtECUName.Text = string.Empty;
        txtCrtPlatformName.Text = string.Empty;
        ddlCrtDTCOperation.SelectedIndex = 0;
        chkCrtInvertOperation.Checked = false;
    }

    private void DisplayDeviceDetails(int DTCID)
    {
        using (miniSmartDataContext db = new miniSmartDataContext())
        {
            var DTCAdd = from d in db.tab_CA_CRITICAL_DTC_ADDs
                         where d.crt_DTC_PK == DTCID
                         select new { d.crt_DTC_CODE, d.crt_TYPE_OF_LOGIC, d.crt_DTC_THRESHOLD, d.crt_DTC_STATUS, d.crt_ISCRITICAL };
            if (DTCAdd != null)
            {
                foreach (var d in DTCAdd)
                {
                    txtVehicleDTC.Text = d.crt_DTC_CODE;
                    ddlDTCLogic.SelectedValue = d.crt_TYPE_OF_LOGIC;
                    txtDTCThreshold.Text = d.crt_DTC_THRESHOLD;
                    btnSave.Text = "Update";
                    if (d.crt_ISCRITICAL == true) ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowCriticalModal(); });", true);
                    else ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowEditModal(); });", true);
                    break;
                }
            }
        }
    }

    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvr = (GridViewRow)((ImageButton)sender).NamingContainer;
        int _iID = Convert.ToInt32(gvCriticalDTC.DataKeys[gvr.RowIndex].Values["crt_DTC_PK"]);
        string _sQry = "delete from tab_CA_CRITICAL_DTC_ADD where crt_DTC_PK=" + _iID;
        SqlConnection _objCon = new SqlConnection(ConnectionString.GetDBConnectionString());
        try
        {
            if (_objCon.State != ConnectionState.Open) _objCon.Open();
            DBL.executeNonQuery(_objCon, CommandType.Text, _sQry, null);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('Vehicle DTC Logic Deleted successfully'); });", true);
        }
        catch { }
        finally
        {
            BindCriticalDTC();
            if (_objCon.State != ConnectionState.Closed) _objCon.Close();
            _objCon.Dispose();
        }
    }

    private void BindCriticalDTC()
    {
        string sqry = "select * from tab_CA_CRITICAL_DTC_ADD";
        DataSet objDs = DBL.executeDataSet(Config.getConnString(), CommandType.Text, sqry, null);
        if (objDs != null && objDs.Tables[0].Rows.Count > 0)
        {
            gvCriticalDTC.DataSource = objDs.Tables[0];
            gvCriticalDTC.DataBind();
        }
        else
        {
            gvCriticalDTC.DataSource = null;
            gvCriticalDTC.DataBind();
        }
    }

    protected void btnCriticalDTC_Click(object sender, EventArgs e)
    {
        myCriticalModalDLabel.InnerHtml = "<i class='fa fa-plus-circle'></i>&nbsp;Critical DTC";
        GridViewRow rows = ((GridViewRow)((Button)sender).NamingContainer);
        HiddenField hfvCriticalDTCID = (HiddenField)rows.FindControl("hfvAuthID");
        ClearCriticalControls();
        DisplayCriticalDTCDetails(Convert.ToInt32(hfvCriticalDTCID.Value));
        hfvCriticalAuth.Value = hfvCriticalDTCID.Value;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowCriticalModal(); });", true);
    }

    private void DisplayCriticalDTCDetails(int CriticalDTCID)
    {
        using (miniSmartDataContext db = new miniSmartDataContext())
        {
            var DTCAdd = from d in db.tab_CA_CRITICAL_DTC_ADDs
                         where d.crt_DTC_PK == CriticalDTCID
                         select new { d.crt_DTC_CODE, d.crt_TYPE_OF_LOGIC, d.crt_DTC_THRESHOLD, d.crt_ECU_NAME, d.crt_PLATFORM_NAME, d.crt_TYPE_OF_OPERATION, d.crt_INVERT_OPERATON, d.crt_DTC_STATUS };
            if (DTCAdd != null)
            {
                foreach (var d in DTCAdd)
                {
                    txtCrtDTC.Text = d.crt_DTC_CODE;
                    ddlCrtDTCLogic.SelectedValue = d.crt_TYPE_OF_LOGIC;
                    txtCrtDTCThreshold.Text = d.crt_DTC_THRESHOLD;

                    txtCrtDTC.Enabled = false;
                    //ddlCrtDTCLogic.Enabled = false;
                    //txtCrtDTCThreshold.Enabled = false;

                    txtCrtECUName.Text = d.crt_ECU_NAME;
                    txtCrtPlatformName.Text = d.crt_PLATFORM_NAME;
                    ddlCrtDTCOperation.SelectedValue = d.crt_TYPE_OF_OPERATION;
                    if (d.crt_INVERT_OPERATON == true)
                        chkCrtInvertOperation.Checked = true;
                    else
                        chkCrtInvertOperation.Checked = false;
                    break;
                }
            }
        }
    }

    protected void btnCrtSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                if (hfvCriticalAuth != null)
                {
                    var criticalDTCID = Convert.ToInt32(hfvCriticalAuth.Value);
                    var DTCCode = from d in db.tab_CA_CRITICAL_DTC_ADDs
                                  where d.crt_DTC_PK == criticalDTCID
                                  select d;
                    foreach (var d in DTCCode)
                    {
                        d.crt_DTC_CODE = txtCrtDTC.Text;
                        d.crt_TYPE_OF_LOGIC = ddlCrtDTCLogic.SelectedValue;
                        d.crt_DTC_THRESHOLD = txtCrtDTCThreshold.Text;
                        d.crt_ECU_NAME = txtCrtECUName.Text;
                        d.crt_PLATFORM_NAME = txtCrtPlatformName.Text;
                        d.crt_TYPE_OF_OPERATION = ddlCrtDTCOperation.SelectedValue;
                        if (chkCrtInvertOperation.Checked == true)
                            d.crt_INVERT_OPERATON = true;
                        else
                            d.crt_INVERT_OPERATON = false;
                        if (txtCrtECUName.Text == "EMS" || txtCrtECUName.Text == "ems")
                            d.crt_DTC_TYPE = "OCC_COUNT";
                        else
                            d.crt_DTC_TYPE = "ODO_VALUE";
                        d.crt_ISCRITICAL = true;

                        break;
                    }
                    db.SubmitChanges();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowStatus", "$(function() { funShowMessage('Critical DTC Code saved successfully!'); });", true);
                }
            }
            BindCriticalDTC();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { ShowCriticalModal(); });", true);
        }
    }
}