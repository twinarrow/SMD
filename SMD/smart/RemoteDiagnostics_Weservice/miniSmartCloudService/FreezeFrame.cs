using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace miniSmartCloudService
{
    [DataContract]
    public class FreezeFrame
    {
        [DataMember]
        public string VINNumber { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string TimeStamp { get; set; }
        [DataMember]
        public string ECUName { get; set; }
        [DataMember]
        public string VehicleName { get; set; }
        [DataMember]
        public string LoginID { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string MobileMACAddress { get; set; }
        [DataMember]
        public string VCIID { get; set; }
        [DataMember]
        public string AppVersion { get; set; }
        [DataMember]
        public string DealerName { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public string ODOValue { get; set; }
        [DataMember]
        public List<DTCParameter> DTCParameters { get; set; }
    }

    [DataContract]
    public class CommonResponse
    {
        [DataMember]
        public bool IsSuccessful { get; set; }
        [DataMember]
        public int MessageCode { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    public class DTCParameter
    {
        public string DTCCode { get; set; }
        public string DTCDescription { get; set; }
        public string DTCStatus { get; set; }
        public List<Signal> Signals { get; set; }
    }

    public class Signal
    {
        public string SignalName { get; set; }
        public string SignalValue { get; set; }
    }
    /*******Poomari Added this line on 14 Aug 2017*********/
    [DataContract]
    public class PushNotifyAvailability
    {
        [DataMember]
        public string VINNumber { get; set; }
    }
    [DataContract]
    public class PushNotifyAvailabilityResponse
    {
        [DataMember]
        public bool IsSuccessful { get; set; }
        [DataMember]
        public int MessageCode { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
    [DataContract]
    public class PushNotifyClear
    {
        [DataMember]
        public string VINNumber { get; set; }
    }
    [DataContract]
    public class PushNotifyClearResponse
    {
        [DataMember]
        public bool IsSuccessful { get; set; }
        [DataMember]
        public int MessageCode { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}