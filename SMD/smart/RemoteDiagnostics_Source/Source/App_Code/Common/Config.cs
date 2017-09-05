using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Config
/// </summary>
public class Config
{
    public static string getConnString()
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings["Cloud_ConnectionString"].ToString();
    }
}