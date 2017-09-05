using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace miniSmartCloudService
{
    public class NotificationUtil
    {
        public AndroidNotificationStatus SendNotification(List<string> DeviceID, string Message)
        {
            AndroidNotificationStatus result = new AndroidNotificationStatus();
            try
            {
                var ServerApiKey = ConfigurationManager.AppSettings["APNS_FCM_SERVER_API_KEY"];
                var SenderID = ConfigurationManager.AppSettings["APNS_FCM_SENDER_ID"];

                result.Successful = false;
                result.Error = null;
                var value = Message;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";

                tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerApiKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));
                tRequest.ContentType = "application/json";

                var data = new
                {
                    registration_ids = DeviceID,
                    notification = new
                    {
                        body = Message,
                        title = "miniSMART Critical Alert"
                        //icon = "http://demo.hns.in/images/ba_apns_icon.png"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                                dynamic JsonVal = JsonConvert.DeserializeObject(result.Response);
                                foreach (var val in JsonVal)
                                {   
                                    if(val.Name == "success")
                                    {
                                        string ResponseVal = (string)val.Value;
                                        if (ResponseVal == "1") { result.Successful = true; break; }
                                        else result.Successful = false;
                                    }                                   
                                }
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }
            return result;
        }
    }
}