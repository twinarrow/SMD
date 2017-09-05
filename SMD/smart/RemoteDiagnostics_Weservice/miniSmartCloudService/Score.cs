using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace miniSmartCloudService
{
    [DataContract]
    public class Score
    {
        [DataMember]
        public string Value { get; set; }
    }
}