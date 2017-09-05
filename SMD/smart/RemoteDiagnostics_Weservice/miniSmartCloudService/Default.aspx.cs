using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace miniSmartCloudService
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Service1 obj = new Service1();

            FreezeFrame devpt = new FreezeFrame();

            devpt.VINNumber = "MA1WA2ES1H2E43941";
            devpt.MobileNumber = "8939447923";
            devpt.Latitude = "33.33";
            devpt.Longitude = "33.33";
            devpt.TimeStamp = "01/01/2017";
            devpt.ECUName = "EMS";
            devpt.VehicleName = "TUV3OO";
            devpt.LoginID = "11aab5504";
            devpt.LoginName = "MALLAPUMADHAVAN";
            devpt.MobileMACAddress = "704843977c7f4e29";
            devpt.VCIID = "111010283540";
            devpt.AppVersion = "4.0";
            devpt.DealerName = "MRV";
            devpt.Area = "MRV_testing";
            devpt.Location = "NELLORE";
            devpt.Source = "miniSMART";
            devpt.ODOValue = "8500";

            List<DTCParameter> DTCParameterss = new List<DTCParameter>();
            DTCParameter param = new DTCParameter();
            param.DTCCode = "P0341";
            DTCParameterss.Add(param);
            param.DTCDescription = "wewe14";
            DTCParameterss.Add(param);
            param.DTCStatus = "1";
            DTCParameterss.Add(param);

            List<Signal> Signals = new List<Signal>();
            Signal sg = new Signal();
            sg.SignalName = "OccurrenceCounter";
            Signals.Add(sg);
            sg.SignalValue = "100";
            Signals.Add(sg);
            param.Signals = Signals;
            DTCParameterss.Add(param);

            devpt.DTCParameters = DTCParameterss;
            obj.FreezeFrameData(devpt);
        }
    }
}