using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Modules_Common_Change_Password : System.Web.UI.Page
{
    string Connectionstring = Config.getConnString();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
    }

    protected void btnSavePassword_OnClick(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (Page.IsValid)
        {
            string _sReadPass = "", _sEncOldPass = "", _sEncNewPass = "";

            secureData _objSecure = new secureData();
            _sEncOldPass = _objSecure.DESEncrypt(txtOldPwd.Text);
            _sEncNewPass = _objSecure.DESEncrypt(txtNewPwd.Text);
            _objSecure = null;


            string _sCommandText = "select user_PASSWORD from tab_m_ADMIN_USER where user_PASSWORD ='" + _sEncOldPass + "' and user_ID_PK=" + Convert.ToInt32(Session["AccessCode"]);
            object _oRes = DBL.executeScalar(Connectionstring, CommandType.Text, _sCommandText);
            if (_oRes != DBNull.Value) _sReadPass = Convert.ToString(_oRes);

            bool _bValidatePwd = true;

            if (_bValidatePwd == true)
            {
                if (_sReadPass == _sEncOldPass)
                {
                    string _sQuery = "Update tab_m_ADMIN_USER set user_PASSWORD='" + _sEncNewPass + "' where user_ID_PK=" + Convert.ToInt32(Session["AccessCode"]);

                    int _iSuccess = 0;
                    SqlConnection _objCon = new SqlConnection(Connectionstring);
                    try
                    {
                        if (_objCon.State != ConnectionState.Open) _objCon.Open();
                        _iSuccess = DBL.executeNonQuery(_objCon, CommandType.Text, _sQuery, null);
                    }
                    catch { }
                    finally
                    {
                        if (_objCon.State != ConnectionState.Closed) _objCon.Close();
                        _objCon.Dispose();
                    }
                    if (_iSuccess > 0)
                    {
                        Session.Abandon();
                        Response.Redirect("~/default.aspx?err=SPC");
                    }
                    else { Response.Redirect("~/default.aspx?err=E00A"); }
                }
                else
                {
                    lblMsg.Text = "Old Password is wrong";
                }
            }
        }
    }
}