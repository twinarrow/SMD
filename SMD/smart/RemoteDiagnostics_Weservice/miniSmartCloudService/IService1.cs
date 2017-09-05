using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace miniSmartCloudService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(UriTemplate = "/FreezeFrame",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        CommonResponse FreezeFrameData(FreezeFrame InputData);

        [OperationContract]
        [WebInvoke(UriTemplate = "/SubmitScore",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        CommonResponse SubmitScoreData(ScoreFrame InputData);

        [OperationContract]
        [WebInvoke(UriTemplate = "/PushNotifyAvailability",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           Method = "POST")]
        PushNotifyAvailabilityResponse PushNotifyAvailability(PushNotifyAvailability InputData);

        [OperationContract]
        [WebInvoke(UriTemplate = "/PushNotifyClear",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           Method = "POST")]
        PushNotifyClearResponse PushNotifyClear(PushNotifyClear InputData);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetRankData/{VinNumber}",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           Method = "GET")]
        RankFrame GetRankData(string VinNumber);
    }

}
