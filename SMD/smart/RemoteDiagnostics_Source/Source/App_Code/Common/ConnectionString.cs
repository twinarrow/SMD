using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConnectionString
/// </summary>
public class ConnectionString
{
    public static string GetExcelConnectionString(string ExcelType, string FilePath)
    {
        var Result = string.Empty;
        switch (ExcelType)
        {
            case "application/vnd.ms-excel":
            case "application/octet-stream":
                Result = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FilePath + "';Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                break;
            case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                Result = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + FilePath + "';Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'";
                break;
        }
        return Result;
    }
    public static string GetDBConnectionString()
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings["Cloud_ConnectionString"].ToString();

    }
}