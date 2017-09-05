using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace miniSmartCloudService
{
    [DataContract]
    public class LeaderFrame
    {
        [DataMember]
        public string VINNumber { get; set; }
        [DataMember]
        public string Score { get; set; }
    }
}