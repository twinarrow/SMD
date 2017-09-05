using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    string ConnectionString = Config.getConnString();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            GetLoginDetails();
        }
    }
    #region User Functions
    private void GetLoginDetails()
    {
        secureData objSecure = new secureData();
        SqlDataReader objDr = null;

        //var UserType = Convert.ToInt32(hfvUserType.Value);
        var Sautation = "";
        var UserName = txtAccessCode.Text;
        var Password = objSecure.DESEncrypt(txtPassword.Text);
        objSecure = null;

        string selectQry = "SP_CheckLogin";
        SqlParameter[] objParameter = new SqlParameter[2];
        //objParameter[0] = new SqlParameter("@userType", UserType);
        objParameter[0] = new SqlParameter("@loginID", UserName);
        objParameter[1] = new SqlParameter("@password", Password);
        try
        {
            objDr = DBL.ExecuteReader(ConnectionString, CommandType.StoredProcedure, selectQry, objParameter);

            if (objDr.HasRows)
            {
                objDr.Read();
                Session["AccessCode"] = Convert.ToInt32(objDr["user_ID_PK"]);
                Session["DealerID"] = null;
                Session["UserType"] = Convert.ToString(objDr["user_ROLE"]);
                if (objDr["user_GENDER"] != DBNull.Value)
                {
                    if (Convert.ToInt32(objDr["user_GENDER"]) == 0)
                        Sautation = "Mr.";
                    else
                        Sautation = "Ms.";
                }

                Session["LoginName"] = Convert.ToString(objDr["user_FIRST_NAME"]) + " " + Convert.ToString(objDr["user_LAST_NAME"]);
                int iRole = Convert.ToInt32(objDr["user_ROLE"]);
                Session["AdminRole"] = Convert.ToString(objDr["user_ROLE"]);

                if (objDr.IsClosed == false)
                {
                    objDr.Close();
                    Response.Redirect("~/Modules/DTC/Vehicle_DTC.aspx?MnuID=DSsID");
                }

                if (objDr.IsClosed == false)
                {
                    objDr.Close();
                    objDr.Dispose();
                }
            }
            else Response.Redirect("default.aspx?err=EUP");
        }
        catch (Exception ex) { Response.Write(ex.Message); }
        finally
        {
            if (objDr != null)
            {
                if (!objDr.IsClosed) objDr.Close();
                objDr.Dispose();
            }
        }
    }
    #endregion
}