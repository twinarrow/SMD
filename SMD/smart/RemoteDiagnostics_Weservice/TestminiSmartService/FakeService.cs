using miniSmartCloudService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestminiSmartService
{
    internal class FakeService : IService1
    {
        public CommonResponse FreezeFrameData(FreezeFrame InputData)
        {
            throw new NotImplementedException();
        }

        public RankFrame GetRankData(string VinNumber)
        {
            throw new NotImplementedException();
        }

        public PushNotifyAvailabilityResponse PushNotifyAvailability(PushNotifyAvailability InputData)
        {
            throw new NotImplementedException();
        }

        public PushNotifyClearResponse PushNotifyClear(PushNotifyClear InputData)
        {
            throw new NotImplementedException();
        }

        public CommonResponse SubmitScoreData(ScoreFrame InputData)
        {
            throw new NotImplementedException();
        }
    }
}
    


