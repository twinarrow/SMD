using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for Minismart
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Minismart : System.Web.Services.WebService
{
    [WebMethod]
    public string GetDTC(string ID)
    {
        string freezeList = string.Empty;
        List<Freeze> data = new List<Freeze>();

        var ConStr = ConnectionString.GetDBConnectionString();
        string Query = "GetDTCValue";

        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", SqlDbType.Int);
        param[0].Value = Convert.ToInt32(ID);
        DataSet ds = new DataSet();
        try
        {
            ds = DBL.executeDataSet(ConStr, CommandType.StoredProcedure, Query, param);
        }
        catch (Exception ex)
        {

        }
        var Serializer = new JavaScriptSerializer();
        freezeList = JsonConvert.SerializeObject(ds, Formatting.Indented);
        return freezeList;
    }

    public class Freeze
    {
        public string DTCCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Signal { get; set; }
        public string Value { get; set; }
    }

    [WebMethod]
    public string GetAAA(string ID)
    {
        string freezeList = string.Empty;
        DataSet dsC = new DataSet();

        var ConStr = ConnectionString.GetDBConnectionString();
        string Query = "Get_Analysis_Value";

        SqlParameter[] param = new SqlParameter[1];
        param[0] = new SqlParameter("@ID", SqlDbType.Int);
        param[0].Value = Convert.ToInt32(ID);
        DataSet ds = new DataSet();

        SqlConnection _objdtcBuild = new SqlConnection(ConStr);
        try
        {
            if (_objdtcBuild.State != ConnectionState.Open)
            {
                string BuildStatus = "";
                _objdtcBuild.Open();
                ds = DBL.executeDataSet(ConStr, CommandType.StoredProcedure, Query, param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataSet dsInvert = new DataSet();
                    string SP = "Get_Invert_Value";
                    SqlParameter[] param1 = new SqlParameter[1];
                    param1[0] = new SqlParameter("@ID", SqlDbType.Int);
                    param1[0].Value = Convert.ToInt32(ID);

                    dsInvert = DBL.executeDataSet(ConStr, CommandType.StoredProcedure, SP, param1);
                    if (dsInvert != null && dsInvert.Tables.Count > 0 && dsInvert.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dsInvert.Tables[0].Rows)
                        {
                            int check_A = 0;
                            int check_B = 0;
                            int check_C = 0;
                            int check_D = 0;

                            string BuildID = row1["BuildID"].ToString();
                            string Opreator = row1["Opreator"].ToString();

                            string DTCCODE_A = row1["DTCCODE_A"] + "";
                            string DTCLOGIC_A = row1["DTCLOGIC_A"] + "";
                            bool DTCCODE_A_INVERT = false;
                            if (row1["DTCCODE_A_INVERT"] != DBNull.Value)
                            {
                                DTCCODE_A_INVERT = Convert.ToBoolean(row1["DTCCODE_A_INVERT"]);
                            }

                            string DTCCODE_B = row1["DTCCODE_B"] + "";
                            string DTCLOGIC_B = row1["DTCLOGIC_B"] + "";
                            bool DTCCODE_B_INVERT = false;
                            if (row1["DTCCODE_B_INVERT"] != DBNull.Value)
                            {
                                DTCCODE_B_INVERT = Convert.ToBoolean(row1["DTCCODE_B_INVERT"]);
                            }

                            string DTCCODE_C = row1["DTCCODE_C"] + "";
                            string DTCLOGIC_C = row1["DTCLOGIC_C"] + "";
                            bool DTCCODE_C_INVERT = false;
                            if (row1["DTCCODE_C_INVERT"] != DBNull.Value)
                            {
                                DTCCODE_C_INVERT = Convert.ToBoolean(row1["DTCCODE_C_INVERT"]);
                            }

                            string DTCCODE_D = row1["DTCCODE_D"] + "";
                            string DTCLOGIC_D = row1["DTCLOGIC_D"] + "";
                            bool DTCCODE_D_INVERT = false;
                            if (row1["DTCCODE_D_INVERT"] != DBNull.Value)
                            {
                                DTCCODE_D_INVERT = Convert.ToBoolean(row1["DTCCODE_D_INVERT"]);
                            }

                            if (DTCCODE_A != "" && DTCLOGIC_A != "")
                            {
                                check_A = 1;

                                var dtcValue = ds.Tables[0]
                                    .AsEnumerable()
                                    .Where(x => x.Field<string>("DTCCODE") == DTCCODE_A)
                                    .Select(x => x.Field<decimal>("Result"))
                                    .FirstOrDefault();

                                if (CheckLogic(DTCCODE_A, DTCLOGIC_A, Convert.ToDouble(dtcValue), DTCCODE_A_INVERT))
                                {
                                    check_A = 2;
                                }
                            }
                            if (DTCCODE_B != "" && DTCLOGIC_B != "")
                            {
                                check_B = 1;

                                var dtcValue = ds.Tables[0]
                                    .AsEnumerable()
                                    .Where(x => x.Field<string>("DTCCODE") == DTCCODE_B)
                                    .Select(x => x.Field<decimal>("Result"))
                                    .FirstOrDefault();

                                if (CheckLogic(DTCCODE_B, DTCLOGIC_B, Convert.ToDouble(dtcValue), DTCCODE_B_INVERT))
                                {
                                    check_B = 2;
                                }
                            }
                            if (DTCCODE_C != "" && DTCLOGIC_C != "")
                            {
                                check_C = 1;

                                var dtcValue = ds.Tables[0]
                                    .AsEnumerable()
                                    .Where(x => x.Field<string>("DTCCODE") == DTCCODE_C)
                                    .Select(x => x.Field<decimal>("Result"))
                                    .FirstOrDefault();

                                if (CheckLogic(DTCCODE_C, DTCLOGIC_C, Convert.ToDouble(dtcValue), DTCCODE_C_INVERT))
                                {
                                    check_C = 2;
                                }
                            }
                            if (DTCCODE_D != "" && DTCLOGIC_D != "")
                            {
                                check_D = 1;

                                var dtcValue = ds.Tables[0]
                                    .AsEnumerable()
                                    .Where(x => x.Field<string>("DTCCODE") == DTCCODE_D)
                                    .Select(x => x.Field<decimal>("Result"))
                                    .FirstOrDefault();

                                if (CheckLogic(DTCCODE_D, DTCLOGIC_D, Convert.ToDouble(dtcValue), DTCCODE_D_INVERT))
                                {
                                    check_D = 2;
                                }
                            }

                            if (Opreator == "AND")
                            {
                                if ((check_A == 0 || check_A == 2) && (check_B == 0 || check_B == 2) && (check_C == 0 || check_C == 2) && (check_D == 0 || check_D == 2))
                                {
                                    BuildStatus = BuildStatus + "," + BuildID;
                                }
                            }
                            if (Opreator == "OR")
                            {
                                if (check_A == 2 || check_B == 2 || check_C == 2 || check_D == 2)
                                {
                                    BuildStatus = BuildStatus + "," + BuildID;
                                }
                            }
                            if (Opreator == " ")
                            {
                                if (check_A == 2)
                                    BuildStatus = BuildStatus + "," + BuildID;
                            }
                        }
                        if (BuildStatus != "")
                        {
                            BuildStatus = BuildStatus.Remove(0, 1);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(BuildStatus) && BuildStatus != string.Empty)
                {
                    string ConQuery = string.Empty;

                    ConQuery = " select distinct b.dtc_LOGIC_CONCLUSION,b.dtc_LOGIC_BUILD_PK,b.dtc_LOGIC_SMILEY from tab_CA_EMS_DTC_LOGIC_ADD a ";
                    ConQuery += " inner join tab_CA_EMS_DTC_LOGIC_BUILD b on a.dtc_DTC_CODE+'_'+a.dtc_LOGIC=b.dtc_DTC_CODE_A+'_'+b.dtc_DTC_LOGIC_A or a.dtc_DTC_CODE+'_'+a.dtc_LOGIC=b.dtc_DTC_CODE_B+'_'+b.dtc_DTC_LOGIC_B or a.dtc_DTC_CODE+'_'+a.dtc_LOGIC=b.dtc_DTC_CODE_c+'_'+b.dtc_DTC_LOGIC_C or a.dtc_DTC_CODE+'_'+a.dtc_LOGIC=b.dtc_DTC_CODE_D+'_'+b.dtc_DTC_LOGIC_D";
                    ConQuery += " where b.dtc_LOGIC_BUILD_PK in (" + BuildStatus + ")";

                    dsC = DBL.executeDataSet(ConStr, CommandType.Text, ConQuery, null);
                    var Serializer = new JavaScriptSerializer();
                    freezeList = JsonConvert.SerializeObject(dsC, Formatting.Indented);
                }
                else
                {
                    freezeList = "{\r\n  \"Table\": []\r\n}";
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (_objdtcBuild.State != ConnectionState.Closed)
                _objdtcBuild.Close();
            _objdtcBuild.Dispose();
        }

        return freezeList;
    }

    private static bool CheckLogic(string dtcCode, string dtcLogic, double dtcValue, bool InvertStatus)
    {
        bool Result = false;
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            var logicData = datacontext.tab_CA_EMS_DTC_LOGIC_ADDs.FirstOrDefault(x => x.dtc_DTC_CODE == dtcCode && x.dtc_LOGIC == dtcLogic);

            if (logicData != null)
            {
                string DTCLogic = logicData.dtc_LOGIC;
                double Threshold = logicData.dtc_LOGIC_THRESHOLD ?? 0;

                if (DTCLogic != string.Empty)
                {
                    if (DTCLogic == ">")
                    {
                        if (dtcValue > Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }

                    if (DTCLogic == "<")
                    {
                        if (dtcValue < Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }

                    if (DTCLogic == "=")
                    {
                        if (dtcValue == Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }

                    if (DTCLogic == "!=")
                    {
                        if (dtcValue != Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }

                    if (DTCLogic == "<=")
                    {
                        if (dtcValue <= Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }

                    if (DTCLogic == ">=")
                    {
                        if (dtcValue >= Threshold)
                        {
                            Result = true;
                            if (InvertStatus)
                            {
                                Result = false;
                            }
                        }
                        else
                        {
                            Result = false;
                            if (InvertStatus)
                            {
                                Result = true;
                            }
                        }
                    }
                }
            }
        }
        return Result;
    }

    [WebMethod]
    public string InsertUpdate(string ADTCCode, string BDTCCode, string CDTCCode, string DDTCCode, bool DTCCodeAI, bool DTCCodeBI, bool DTCCodeCI, bool DTCCodeDI, string Operation, string Conclusion, string Smiley, string btnSave, string HFV)
    {
        string insQry = "Failure";
        bool Status = false;
        bool StatusA = false;
        bool StatusB = false;
        bool StatusC = false;
        bool StatusD = false;
        bool temp = false;
        using (miniSmartDataContext datacontext = new miniSmartDataContext())
        {
            tab_CA_EMS_DTC_LOGIC_BUILD build = new tab_CA_EMS_DTC_LOGIC_BUILD();
            string Result = "0";
            SqlDataReader objDR = null;

            string DTCCodeA = "";
            string DTCCodeB = "";
            string DTCCodeC = "";
            string DTCCodeD = "";

            string DTClogicA = "";
            string DTClogicB = "";
            string DTClogicC = "";
            string DTClogicD = "";

            string[] wordA = ADTCCode.Split('_');
            DTCCodeA = Convert.ToString(wordA[0]);
            DTClogicA = Convert.ToString(wordA[1]);

            string[] wordB = BDTCCode.Split('_');
            DTCCodeB = Convert.ToString(wordB[0]);
            if (wordB.Length > 1)
                DTClogicB = Convert.ToString(wordB[1]);

            string[] wordC = CDTCCode.Split('_');
            DTCCodeC = Convert.ToString(wordC[0]);
            if (wordC.Length > 1)
                DTClogicC = Convert.ToString(wordC[1]);

            string[] wordD = DDTCCode.Split('_');
            DTCCodeD = Convert.ToString(wordD[0]);
            if (wordD.Length > 1)
                DTClogicD = Convert.ToString(wordD[1]);


            string ConQuery = string.Empty;
            if (btnSave == "Add")
            {
                if ((DTCCodeB == "0" || DTCCodeB == "-1") && (DTCCodeC == "0" || DTCCodeC == "-1") && (DTCCodeD == "0" || DTCCodeD == "-1"))
                {
                    ConQuery = "select count(dtc_LOGIC_BUILD_PK) as BuildCount from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_DTC_CODE_A = '" + DTCCodeA + "' and dtc_DTC_LOGIC_A = '" + DTClogicA + "'";
                    ConQuery += " and dtc_DTC_CODE_B is null and dtc_DTC_CODE_C is null and dtc_DTC_CODE_D is null ";
                }
                else
                {
                    ConQuery = "select count(dtc_LOGIC_BUILD_PK) as BuildCount from tab_CA_EMS_DTC_LOGIC_BUILD where ";
                    if (DTCCodeA != "0" && DTCCodeA != "-1")
                        ConQuery += " dtc_DTC_CODE_A = '" + DTCCodeA + "' and dtc_DTC_LOGIC_A = '" + DTClogicA + "'";
                    else
                        ConQuery += " dtc_DTC_CODE_A = is null";
                    if (DTCCodeB != "0" && DTCCodeB != "-1")
                        ConQuery += " and dtc_DTC_CODE_B ='" + DTCCodeB + "' and dtc_DTC_LOGIC_B = '" + DTClogicB + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_B is null";
                    if (DTCCodeC != "0" && DTCCodeC != "-1")
                        ConQuery += " and dtc_DTC_CODE_C ='" + DTCCodeC + "' and dtc_DTC_LOGIC_C = '" + DTClogicC + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_C is null";
                    if (DTCCodeD != "0" && DTCCodeD != "-1")
                        ConQuery += " and dtc_DTC_CODE_D ='" + DTCCodeD + "' and dtc_DTC_LOGIC_D = '" + DTClogicD + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_D is null";
                    ConQuery += " and dtc_LOGIC_OPERATION='" + Operation + "'";
                }
            }
            else
            {
                if ((DTCCodeB == "0" || DTCCodeB == "-1") && (DTCCodeC == "0" || DTCCodeC == "-1") && (DTCCodeD == "0" || DTCCodeD == "-1"))
                {
                    ConQuery = "select count(dtc_LOGIC_BUILD_PK) as BuildCount from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_DTC_CODE_A = '" + DTCCodeA + "' and dtc_DTC_LOGIC_A = '" + DTClogicA + "' and dtc_DTC_CODE_A_INVERT = '" + DTCCodeAI + "'";
                    ConQuery += " and dtc_DTC_CODE_B is null and dtc_DTC_CODE_C is null and dtc_DTC_CODE_D is null ";
                    ConQuery += " and dtc_LOGIC_BUILD_PK <> " + Convert.ToInt32(HFV) + " ";
                }
                else
                {
                    ConQuery = "select count(dtc_LOGIC_BUILD_PK) as BuildCount from tab_CA_EMS_DTC_LOGIC_BUILD where ";
                    if (DTCCodeA != "0" && DTCCodeA != "-1")
                        ConQuery += " dtc_DTC_CODE_A = '" + DTCCodeA + "' and dtc_DTC_LOGIC_A = '" + DTClogicA + "' and dtc_DTC_CODE_A_INVERT = '" + DTCCodeAI + "'";
                    else
                        ConQuery += " dtc_DTC_CODE_A = is null";
                    if (DTCCodeB != "0" && DTCCodeB != "-1")
                        ConQuery += " and dtc_DTC_CODE_B ='" + DTCCodeB + "' and dtc_DTC_LOGIC_B = '" + DTClogicB + "' and dtc_DTC_CODE_B_INVERT = '" + DTCCodeBI + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_B is null";
                    if (DTCCodeC != "0" && DTCCodeC != "-1")
                        ConQuery += " and dtc_DTC_CODE_C ='" + DTCCodeC + "' and dtc_DTC_LOGIC_C = '" + DTClogicC + "' and dtc_DTC_CODE_C_INVERT = '" + DTCCodeCI + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_C is null";
                    if (DTCCodeD != "0" && DTCCodeD != "-1")
                        ConQuery += " and dtc_DTC_CODE_D ='" + DTCCodeD + "' and dtc_DTC_LOGIC_D = '" + DTClogicD + "' and dtc_DTC_CODE_D_INVERT = '" + DTCCodeDI + "'";
                    else
                        ConQuery += " and dtc_DTC_CODE_D is null";
                    ConQuery += " and dtc_LOGIC_OPERATION='" + Operation + "'";
                    ConQuery += " and dtc_LOGIC_BUILD_PK <> " + Convert.ToInt32(HFV) + " ";
                }
            }
            objDR = DBL.ExecuteReader(Config.getConnString(), CommandType.Text, ConQuery);
            if (objDR.HasRows)
            {
                objDR.Read();
                Result = Convert.ToString(objDR[0]);
            }
            else Result = "0";
            if (!objDR.IsClosed) objDR.Close();
            if (Result != "0")
            {
                insQry = "DTC Build Logic already Exist";
            }
            else
            {
                if (btnSave == "Add")
                {
                    if (DTCCodeA != "0" && DTCCodeA != "-1")
                    {
                        build.dtc_DTC_CODE_A = DTCCodeA;
                        build.dtc_DTC_LOGIC_A = DTClogicA;
                        string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeA + "' and dtc_LOGIC = '" + DTClogicA + "'";
                        StatusA = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                        if (DTCCodeAI) StatusA = !StatusA;
                        build.dtc_DTC_CODE_A_INVERT = DTCCodeAI;
                    }

                    if (DTCCodeB != "0" && DTCCodeB != "-1")
                    {
                        build.dtc_DTC_CODE_B = DTCCodeB;
                        build.dtc_DTC_LOGIC_B = DTClogicB;
                        string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeB + "' and dtc_LOGIC = '" + DTClogicB + "'";
                        StatusB = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                        if (DTCCodeBI) StatusB = !StatusB;
                        build.dtc_DTC_CODE_B_INVERT = DTCCodeBI;
                    }

                    if (DTCCodeC != "0" && DTCCodeC != "-1")
                    {
                        build.dtc_DTC_CODE_C = DTCCodeC;
                        build.dtc_DTC_LOGIC_C = DTClogicC;
                        string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeC + "' and dtc_LOGIC = '" + DTClogicC + "'";
                        StatusC = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                        if (DTCCodeCI) StatusC = !StatusC;
                        build.dtc_DTC_CODE_C_INVERT = DTCCodeCI;
                    }

                    if (DTCCodeD != "0" && DTCCodeD != "-1")
                    {
                        build.dtc_DTC_CODE_D = DTCCodeD;
                        build.dtc_DTC_LOGIC_D = DTClogicD;
                        string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeD + "' and dtc_LOGIC = '" + DTClogicD + "'";
                        StatusC = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                        if (DTCCodeDI) StatusD = !StatusD;
                        build.dtc_DTC_CODE_D_INVERT = DTCCodeDI;
                    }
                    if (Operation == "AND")
                    {
                        if (StatusA)
                        {
                            Status = true;
                            if (DTCCodeB != "0" && DTCCodeB != "-1")
                            {
                                if (StatusB)
                                    Status = true;
                                else
                                    Status = false;
                            }
                        }
                        else Status = false;

                        if (DTCCodeC != "0" && DTCCodeC != "-1")
                        {
                            if (StatusC && Status)
                                Status = true;
                            else
                                Status = false;
                        }

                        if (DTCCodeD != "0" && DTCCodeD != "-1")
                        {
                            if (StatusD && Status)
                                Status = true;
                            else
                                Status = false;
                        }
                    }
                    else if (Operation == "OR")
                    {
                        if (StatusA || StatusB || StatusC || StatusD)
                            Status = true;
                        else
                            Status = false;
                    }
                    else
                    {
                        if (StatusA || StatusB || StatusC || StatusD)
                            Status = true;
                        else
                            Status = false;
                    }

                    build.dtc_LOGIC_SMILEY = Smiley;
                    build.dtc_LOGIC_OPERATION = Operation;
                    build.dtc_LOGIC_CONCLUSION = Conclusion;
                    build.dtc_LOGIC_STATUS = Status;
                    datacontext.tab_CA_EMS_DTC_LOGIC_BUILDs.InsertOnSubmit(build);
                    datacontext.SubmitChanges();
                    insQry = "DTC Logic Build added successfully";
                }
                else if (btnSave == "Update")
                {
                    var buildDetails = datacontext.tab_CA_EMS_DTC_LOGIC_BUILDs.FirstOrDefault(x => x.dtc_LOGIC_BUILD_PK == Convert.ToInt32(HFV));
                    if (buildDetails != null)
                    {
                        if (DTCCodeA != "0" && DTCCodeA != "-1")
                        {
                            Status = true;
                            buildDetails.dtc_DTC_CODE_A = DTCCodeA;
                            buildDetails.dtc_DTC_LOGIC_A = DTClogicA;
                            buildDetails.dtc_DTC_CODE_A_INVERT = DTCCodeAI;

                            string qrytemp = "select dtc_DTC_CODE_A_INVERT from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_LOGIC_BUILD_PK = " + Convert.ToInt32(HFV) + " and dtc_DTC_CODE_A = '" + DTCCodeA + "'";
                            temp = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qrytemp));

                            string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeA + "' and dtc_LOGIC = '" + DTClogicA + "'";
                            StatusA = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                            bool vartemp = temp;
                            if (DTCCodeAI != temp)
                            {
                                vartemp = !temp;
                            }
                            if (vartemp) StatusA = !StatusA;
                        }

                        if (DTCCodeB != "0" && DTCCodeB != "-1")
                        {
                            buildDetails.dtc_DTC_CODE_B = DTCCodeB;
                            buildDetails.dtc_DTC_LOGIC_B = DTClogicB;
                            buildDetails.dtc_DTC_CODE_B_INVERT = DTCCodeBI;

                            string qrytemp = "select dtc_DTC_CODE_B_INVERT from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_LOGIC_BUILD_PK = " + Convert.ToInt32(HFV) + " and dtc_DTC_CODE_B = '" + DTCCodeB + "'";
                            temp = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qrytemp));

                            string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeB + "' and dtc_LOGIC = '" + DTClogicB + "'";
                            StatusB = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                            bool vartemp = temp;
                            if (DTCCodeBI != temp)
                            {
                                vartemp = !temp;
                            }
                            if (vartemp) StatusB = !StatusB;
                        }

                        else buildDetails.dtc_DTC_CODE_B = null;
                        if (DTCCodeC != "0" && DTCCodeC != "-1")
                        {
                            buildDetails.dtc_DTC_CODE_C = DTCCodeC;
                            buildDetails.dtc_DTC_LOGIC_C = DTClogicC;
                            buildDetails.dtc_DTC_CODE_C_INVERT = DTCCodeCI;

                            string qrytemp = "select dtc_DTC_CODE_C_INVERT from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_LOGIC_BUILD_PK = " + Convert.ToInt32(HFV) + " and dtc_DTC_CODE_C = '" + DTCCodeC + "'";
                            temp = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qrytemp));

                            string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeC + "' and dtc_LOGIC = '" + DTClogicC + "'";
                            StatusC = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                            bool vartemp = temp;
                            if (DTCCodeCI != temp)
                            {
                                vartemp = !temp;
                            }
                            if (vartemp) StatusC = !StatusC;
                        }

                        else buildDetails.dtc_DTC_CODE_C = null;
                        if (DTCCodeD != "0" && DTCCodeD != "-1")
                        {
                            buildDetails.dtc_DTC_CODE_D = DTCCodeD;
                            buildDetails.dtc_DTC_LOGIC_D = DTClogicD;
                            buildDetails.dtc_DTC_CODE_D_INVERT = DTCCodeDI;

                            string qrytemp = "select dtc_DTC_CODE_D_INVERT from tab_CA_EMS_DTC_LOGIC_BUILD where dtc_LOGIC_BUILD_PK = " + Convert.ToInt32(HFV) + " and dtc_DTC_CODE_D = '" + DTCCodeD + "'";
                            temp = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qrytemp));

                            string qryStatus = "select dtc_LOGIC_RESULT from tab_CA_EMS_DTC_LOGIC_Add where dtc_DTC_CODE = '" + DTCCodeD + "' and dtc_LOGIC = '" + DTClogicD + "'";
                            StatusD = Convert.ToBoolean(DBL.executeScalar(ConnectionString.GetDBConnectionString(), CommandType.Text, qryStatus));
                            bool vartemp = temp;
                            if (DTCCodeDI != temp)
                            {
                                vartemp = !temp;
                            }
                            if (vartemp) StatusD = !StatusD;
                        }

                        else buildDetails.dtc_DTC_CODE_D = null;

                        if (Operation == "AND")
                        {
                            if (StatusA)
                            {
                                Status = true;
                                if (DTCCodeB != "0" && DTCCodeB != "-1")
                                {
                                    if (StatusB)
                                        Status = true;
                                    else
                                        Status = false;
                                }
                            }
                            else
                                Status = false;

                            if (DTCCodeC != "0" && DTCCodeC != "-1")
                            {
                                if (StatusC && Status)
                                    Status = true;
                                else
                                    Status = false;
                            }

                            if (DTCCodeD != "0" && DTCCodeD != "-1")
                            {
                                if (StatusD && Status)
                                    Status = true;
                                else
                                    Status = false;
                            }
                        }
                        else if (Operation == "OR")
                        {
                            if (StatusA || StatusB || StatusC || StatusD)
                                Status = true;
                            else
                                Status = false;
                        }
                        else
                        {
                            if (StatusA || StatusB || StatusC || StatusD)
                                Status = true;
                            else
                                Status = false;
                        }
                        buildDetails.dtc_LOGIC_SMILEY = Smiley;
                        buildDetails.dtc_LOGIC_OPERATION = Operation;
                        buildDetails.dtc_LOGIC_CONCLUSION = Conclusion;
                        buildDetails.dtc_LOGIC_STATUS = Status;
                        datacontext.SubmitChanges();
                        insQry = "DTC Logic Build Updated successfully";
                    }
                }
            }
            return insQry;
        }
    }
}
