using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace miniSmartCloudService
{
    public class AndroidNotificationStatus
    {
        public bool Successful { get; set; }
        public string Response { get; set; }
        public Exception Error { get; set; }
    }
}