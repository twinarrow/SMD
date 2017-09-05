using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace miniSmartCloudService
{
    [DataContract]
    public class RankFrame
    {

        [DataMember]
        public int Rank { get; set; }
        [DataMember]
        public List<ScoreFrame> Top5Score { get; set; }
        [DataMember]
        public List<ScoreFrame> Top10ScoreOverall { get; set; }


    }
}