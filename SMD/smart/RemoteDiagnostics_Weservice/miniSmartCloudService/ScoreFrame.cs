using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace miniSmartCloudService
{
    [DataContract]
    public class ScoreFrame
    {
        [DataMember]
        public string VINNumber { get; set; }
        [DataMember]
        public decimal Score { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember(IsRequired=false)]
        public string Rank { get; set; }
    }
}