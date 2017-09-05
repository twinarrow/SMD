using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace miniSmartCloudService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public RankFrame GetRankData(string VinNumber)
        {
            CommonResponse Response = new CommonResponse();
            var rankframe = new RankFrame();
            var IsSuccessful = false;
            var MessageCode = 400;
            var Message = string.Empty;
            using (miniSmartDataContext db = new miniSmartDataContext())
            {
                try
                {

                    var rank = (from d in db.tab_m_VIN_SCOREs
                             where d.Leader_TIMESTAMP >= DateTime.Now.AddDays(-30)
                             select new
                             {
                                 VinNumber = d.Leader_VIN_NUMBER,
                                 Rank = (from o in db.tab_m_VIN_SCOREs
                                         where o.Leader_SCORE > d.Leader_SCORE && o.Leader_ID_PK == d.Leader_ID_PK
                                         select o.Leader_SCORE).Distinct().Count() + 1
                             }).ToList().Last(a => a.VinNumber == VinNumber).Rank;

                    var top10 = (from d in db.tab_m_VIN_SCOREs
                                where d.Leader_TIMESTAMP >= DateTime.Now.AddDays(-30)
                                select new
                                {
                                    VinNumber = d.Leader_VIN_NUMBER,
                                    Score = d.Leader_SCORE,
                                    Name = d.Leader_Name,
                                    Rank = (from o in db.tab_m_VIN_SCOREs
                                            where o.Leader_SCORE > d.Leader_SCORE //&& o.Leader_ID_PK == d.Leader_ID_PK
                                            select o.Leader_SCORE).Distinct().Count() + 1
                                }).ToList().OrderBy(a => a.Rank).Take(10);
                    
                    rankframe.Rank = rank;
                    List<ScoreFrame> top10ranks = new List<ScoreFrame>();
                    foreach (var data in top10)
                    {
                        top10ranks.Add(new ScoreFrame()
                        {
                            Name = data.Name,
                            Rank = Convert.ToString(data.Rank),
                            VINNumber = data.VinNumber,
                            Score = data.Score
                        });
                    };

                    var top5 = (from d in db.tab_m_VIN_SCOREs
                                 where d.Leader_TIMESTAMP >= DateTime.Now.AddDays(-30) && d.Leader_VIN_NUMBER == VinNumber
                                 select new
                                 {
                                     VinNumber = d.Leader_VIN_NUMBER,
                                     Score = d.Leader_SCORE,
                                     Name = d.Leader_Name,
                                     Rank = (from o in db.tab_m_VIN_SCOREs
                                             where o.Leader_SCORE > d.Leader_SCORE //&& o.Leader_ID_PK == d.Leader_ID_PK
                                             select o.Leader_SCORE).Distinct().Count() + 1
                                 }).ToList().OrderBy(a => a.Rank).Take(5);
                     rankframe = new RankFrame();

                    List<ScoreFrame> top5ranks = new List<ScoreFrame>();
                    foreach (var data in top5)
                    {
                        top5ranks.Add(new ScoreFrame()
                        {
                            Name = data.Name,
                            Rank = Convert.ToString(data.Rank),
                            VINNumber = data.VinNumber,
                            Score = data.Score
                        });
                    };
                    rankframe.Top10ScoreOverall = top10ranks;
                    rankframe.Top5Score = top5ranks;
                   
                    MessageCode = 200;
                    Message = "success";
                    IsSuccessful = true;
                    return rankframe;
                    //NotificationUtil Notify = new NotificationUtil();

                    ////AndroidNotificationStatus STATUS = Notify.SendNotification(rankframe, "Please visit your nearest service center");



                    //var ranklist = from d in db.tab_m_VIN_SCOREs
                    //                               where d.Leader_TIMESTAMP >= DateTime.Now.AddDays(-30)
                    //                               select new { d.Leader_VIN_NUMBER, d.Leader_SCORE, d.Leader_TIMESTAMP };
                    //                ranklist.OrderByDescending(a => a.Leader_SCORE);
                    //                var item = 
                    //                ranklist.ToList().LastIndexOf(a=> a.Leader_VIN_NUMBER == VinNumber)
                    //where d.Leader_VIN_NUMBER == VinNumber
                    //        select new { d.crt_DTC_CODE, d.crt_DTC_THRESHOLD, d.crt_DTC_TYPE };

                }
                catch (Exception ex)
                {
                    MessageCode = 500;
                    Message = "Error occured in server " + ex.Message;
                    IsSuccessful = false;
                }
                finally
                {
                }
            }

            Response.IsSuccessful = IsSuccessful;
            Response.Message = Message;
            Response.MessageCode = MessageCode;
            return rankframe;
        }
        public CommonResponse FreezeFrameData(FreezeFrame InputData)
        {
            CommonResponse Response = new CommonResponse();
            var IsSuccessful = false;
            var MessageCode = 400;
            var Message = string.Empty;
            if (InputData != null)
            {
                var VINNumber = InputData.VINNumber;
                var MobileNumber = InputData.MobileNumber;
                var Latitude = InputData.Latitude;
                var Longitude = InputData.Longitude;
                var TimeStamp = InputData.TimeStamp;
                var DTCParameters = InputData.DTCParameters;

                var ECUName = InputData.ECUName;
                var VehicleName = InputData.VehicleName;
                var LoginID = InputData.LoginID;
                var LoginName = InputData.LoginName;
                var MobileMACAddress = InputData.MobileMACAddress;
                var VCIID = InputData.VCIID;
                var AppVersion = InputData.AppVersion;
                var DealerName = InputData.DealerName;
                var Area = InputData.Area;
                var Location = InputData.Location;
                var Source = InputData.Source;
                var ODOValue = InputData.ODOValue;

                var MasterID = 0;
                DateTime Date = DateTime.Now;

                using (miniSmartDataContext db = new miniSmartDataContext())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(TimeStamp))
                        {
                            if (TimeStamp.Contains('-'))
                            {
                                TimeStamp = TimeStamp.Replace('-', '/');
                                if (CommonUtil.IsValidDate(TimeStamp))
                                {
                                    Date = Convert.ToDateTime(TimeStamp);
                                }
                            }
                        }

                        tab_CA_EMS_MASTER_INFO emsmaster = new tab_CA_EMS_MASTER_INFO();
                        emsmaster.freeze_VIN_NUMBER = VINNumber;
                        emsmaster.freeze_MOBILE_NUMBER = MobileNumber;
                        emsmaster.freeze_LATITUDE = Latitude;
                        emsmaster.freeze_LONGITUDE = Longitude;
                        emsmaster.freeze_TIMESTAMP = Date;
                        emsmaster.freeze_UPDATEDON = DateTime.Now;

                        emsmaster.freeze_APP_VERSION = AppVersion;
                        emsmaster.freeze_AREA = Area;
                        emsmaster.freeze_DEALER_NAME = DealerName;
                        emsmaster.freeze_ECUNAME = ECUName;
                        emsmaster.freeze_LOCATION = Location;
                        emsmaster.freeze_LOGINID = LoginID;
                        emsmaster.freeze_LOGINNAME = LoginName;
                        emsmaster.freeze_MOBILE_MAC_ADDRESS = MobileMACAddress;
                        emsmaster.freeze_MOBILE_NUMBER = MobileNumber;
                        emsmaster.freeze_SOURCE = Source;
                        emsmaster.freeze_VCIID = VCIID;
                        emsmaster.freeze_VEHICLENAME = VehicleName;
                        emsmaster.freeze_ODOValue = ODOValue;
                        emsmaster.freeze_PUSH_NOTIFY = false; //Poomari Added this line on 14-Aug-2017
                        emsmaster.freeze_NOTIFY_COUNTER = 5; //Poomari Added this line on 14-Aug-2017
                        db.tab_CA_EMS_MASTER_INFOs.InsertOnSubmit(emsmaster);
                        db.SubmitChanges();

                        var master = from m in db.tab_CA_EMS_MASTER_INFOs
                                     orderby m.freeze_MASTER_ID descending
                                     select m;
                        if (master.Count() > 0)
                        {
                            foreach (var item in master)
                            {
                                MasterID = item.freeze_MASTER_ID;
                                break;
                            }
                        }

                        //DTC Parameters
                        if (DTCParameters.Count() > 0)
                        {
                            foreach (var item in DTCParameters)
                            {
                                var DTCID = 0;
                                var DTCCode = item.DTCCode;
                                var DTCDescription = item.DTCDescription;
                                var DTCStatus = item.DTCStatus;
                                var SignalParams = item.Signals;

                                tab_CA_EMS_DTC_INFO dtcinfo = new tab_CA_EMS_DTC_INFO();
                                dtcinfo.freeze_MASTER_ID = MasterID;
                                dtcinfo.freeze_DTC_CODE = DTCCode;
                                dtcinfo.freeze_DTC_DESCRIPTION = DTCDescription;
                                dtcinfo.freeze_DTC_STATUS = DTCStatus;                             
                                db.tab_CA_EMS_DTC_INFOs.InsertOnSubmit(dtcinfo);
                                db.SubmitChanges();

                                var dtc = from d in db.tab_CA_EMS_DTC_INFOs
                                          orderby d.dtc_ID descending
                                          select new { d.dtc_ID };
                                if (dtc.Count() > 0)
                                {
                                    foreach (var d in dtc)
                                    {
                                        DTCID = d.dtc_ID;
                                        break;
                                    }
                                }

                                if (DTCID > 0)
                                {
                                    if (SignalParams != null)
                                    {
                                        foreach (var signal in SignalParams)
                                        {
                                            var SignalName = signal.SignalName;
                                            var SignalValue = signal.SignalValue;

                                            tab_CA_EMS_FREEZEFRAME_DATA data = new tab_CA_EMS_FREEZEFRAME_DATA();
                                            data.master_ID = MasterID;
                                            data.dtc_INFO_ID = DTCID;
                                            data.freeze_SIGNAL = SignalName;
                                            data.freeze_VALUE = SignalValue;

                                            db.tab_CA_EMS_FREEZEFRAME_DATAs.InsertOnSubmit(data);
                                            db.SubmitChanges();
                                        }
                                    }
                                }
                            }
                        }
                        //Check the Critical DTC codes
                        //If any critical DTC met the threshold value, send a PUSH Notification message
                        #region Check Critical DTC 
                        //VINNumber
                        var dtclist = from d in db.tab_CA_CRITICAL_DTC_ADDs
                                      where d.crt_DTC_STATUS == true
                                      select new { d.crt_DTC_CODE, d.crt_DTC_THRESHOLD, d.crt_DTC_TYPE };
                        if (dtclist.Count() > 0)
                        {
                            foreach (var item in dtclist)
                            {
                                var DTCCODE = item.crt_DTC_CODE;
                                var ThresholdValue = item.crt_DTC_THRESHOLD;
                                var Type = item.crt_DTC_TYPE;
                                var ThresholdBreach = false;
                                //Get The list of data for the particular VIN Number
                                if (Type == "OCC_COUNT")
                                {
                                    var OCC_COUNT = Convert.ToInt32(ThresholdValue);
                                    var dtc = from dt in db.tab_CA_EMS_DTC_INFOs
                                              join m in db.tab_CA_EMS_MASTER_INFOs on dt.freeze_MASTER_ID equals m.freeze_MASTER_ID
                                              where dt.freeze_DTC_CODE == DTCCODE && m.freeze_VIN_NUMBER == VINNumber
                                              && m.freeze_PUSH_NOTIFY== false && m.freeze_NOTIFY_COUNTER != 0 //Poomari Added this line on 14-Aug-2017 
                                              // Notification will be send to vehicle until the  freeze_NOTIFY_COUNTER is 0.
                                              select dt;
                                    if (dtc.Count() >= OCC_COUNT)
                                    {
                                        ThresholdBreach = true;
                                    }
                                }
                                else // ODO_VALUE
                                {
                                    var ODO_VALUE = 0;
                                    var ODO_THRESHOLD = Convert.ToInt32(ThresholdValue);
                                    var odo = from m in db.tab_CA_EMS_MASTER_INFOs
                                              where m.freeze_VIN_NUMBER == VINNumber
                                              && m.freeze_PUSH_NOTIFY == false && m.freeze_NOTIFY_COUNTER != 0//Poomari Added this line on 14-Aug-2017
                                                // Notification will be send to vehicle until the  freeze_NOTIFY_COUNTER is 0.
                                              orderby m.freeze_MASTER_ID descending
                                              select new { m.freeze_ODOValue };
                                    if (odo.Count() > 0)
                                    {
                                        foreach (var i in odo)
                                        {
                                            ODO_VALUE = Convert.ToInt32(i.freeze_ODOValue);
                                            break;
                                        }
                                    }
                                    if (ODO_VALUE == ODO_THRESHOLD)
                                    {
                                        ThresholdBreach = true;
                                    }
                                }
                                if (ThresholdBreach)
                                {
                                    List<string> deviceList = new List<string>();
                                    var deviceTokens = from m in db.tab_m_DEVICE_VIN_MAPPINGs
                                                       where m.VIN_NUMBER == VINNumber
                                                       select new { m.device_TOKEN };
                                    if (deviceTokens.Count() > 0)
                                    {
                                        foreach (var dt in deviceTokens)
                                        {
                                            deviceList.Add(dt.device_TOKEN);
                                            break;
                                        }
                                    }
                                    NotificationUtil Notify = new NotificationUtil();
                                    AndroidNotificationStatus STATUS = Notify.SendNotification(deviceList, "Please visit your nearest service center");
                                    if (STATUS.Successful)
                                    {
                                        Message = "Push Notification Sent";

                                        //Poomari Added this line on 14-Aug-2017
                                        //If Push Notification sent to that vehicle, Change the Push Notification Status and Decrease the Notification Counter
                                        var UpdateNotify = from u in db.tab_CA_EMS_MASTER_INFOs
                                                           where u.freeze_VIN_NUMBER == VINNumber
                                                           select u;

                                        if(UpdateNotify.Count() > 0)
                                        {
                                            foreach(var m in UpdateNotify)
                                            {
                                                m.freeze_PUSH_NOTIFY = true;
                                                m.freeze_NOTIFY_COUNTER = Convert.ToInt32(m.freeze_NOTIFY_COUNTER) - 1;
                                                db.SubmitChanges();
                                                break;
                                            }
                                        }

                                    }
                                    else Message = "Push Notification Not Sent";
                                }
                            }
                        }
                        #endregion
                        /***********Commented by Poomari on  on 14-Aug-2017 ***********/
                        //List<string> deviceList = new List<string>();
                        //deviceList.Add("fw7aCJXafIE:APA91bE_S8Vu-S5E1q-uOWIf4v2HcJy_hLu3a1nLZZf3qJTO5o_-QFXhO7hDZiNn4uYFg93euPrmaHG1OqryAKb8bFHohAobFgLM1vVxRprGaSejwwoAezrnq8CH5nxF-u_Ix5-q53HL");
                        //NotificationUtil Notify = new NotificationUtil();
                        //AndroidNotificationStatus STATUS = Notify.SendNotification(deviceList, "Please visit your nearest service center");
                        //if (STATUS.Successful)
                        //{
                        //    Message = "Push Notification Sent";
                        //}
                        /***********Commented by Poomari on  on 14-Aug-2017 ***********/
                        IsSuccessful = true;

                        MessageCode = 200;
                    }
                    catch (Exception ex)
                    {
                        MessageCode = 500;
                        Message = "Error occured in server " + ex.Message;
                        IsSuccessful = false;
                    }
                    finally
                    {
                    }
                }
            }
            else
            {
                MessageCode = 400;
                Message = "Invalid data; please check your input data";
                IsSuccessful = false;
            }
            Response.IsSuccessful = IsSuccessful;
            Response.Message = Message;
            Response.MessageCode = MessageCode;
            return Response;
        }
        /*******Poomari Added the following Services on 14 Aug 2017*********/

        public CommonResponse SubmitScoreData(ScoreFrame InputData)
        {
            CommonResponse Response = new CommonResponse();
            var IsSuccessful = false;
            var MessageCode = 400;
            var Message = string.Empty;

            try { 
                if (InputData != null)
                {
                    var Name = InputData.Name;
                    var Score = InputData.Score;
                    var VinNumber = InputData.VINNumber;

                    DateTime Date = DateTime.Now;

                    using (miniSmartDataContext db = new miniSmartDataContext())
                    {
                   
                        tab_m_VIN_SCORE vinscore = new tab_m_VIN_SCORE();
                        vinscore.Leader_Name = Name;
                        vinscore.Leader_SCORE = Score;
                        vinscore.Leader_VIN_NUMBER = VinNumber;
                        vinscore.Leader_TIMESTAMP = Date;
                        db.tab_m_VIN_SCOREs.InsertOnSubmit(vinscore);
                        db.SubmitChanges();
                    }
                }
                IsSuccessful = true;

                MessageCode = 200;
            }
            catch (Exception ex)
            {
                MessageCode = 500;
                Message = "Error occured in server " + ex.Message;
                IsSuccessful = false;
            }
            finally
            {
            }
            Response.IsSuccessful = IsSuccessful;
                        Response.Message = Message;
                        Response.MessageCode = MessageCode;
                        return Response;
        }
        public PushNotifyAvailabilityResponse PushNotifyAvailability(PushNotifyAvailability InputData)
        {
            PushNotifyAvailabilityResponse Response = new PushNotifyAvailabilityResponse();
            var IsSuccessful = false;
            var MessageCode = 400;
            var Message = string.Empty;
            if (InputData != null)
            {
                var VINNumber = InputData.VINNumber;
                using (miniSmartDataContext db = new miniSmartDataContext())
                {
                    try
                    {
                        var CheckNotify = from u in db.tab_CA_EMS_MASTER_INFOs
                                          where u.freeze_VIN_NUMBER == VINNumber && u.freeze_PUSH_NOTIFY == true
                                          select u;

                        if (CheckNotify.Count() > 0)
                        {
                            foreach (var m in CheckNotify)
                            {
                                Message = "Push Notification is Available for this VINNumber";
                                IsSuccessful = true;
                                MessageCode = 200;
                                break;
                            }
                        }
                        else
                        {
                            Message = "Push Notification is Not Available for this VINNumber";
                            IsSuccessful = false;
                            MessageCode = 400;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageCode = 500;
                        Message = "Error occured in server " + ex.Message;
                        IsSuccessful = false;
                    }
                    finally { }
                }

            }
            else
            {
                MessageCode = 400;
                Message = "Invalid data; please check your input data";
                IsSuccessful = false;
            }
            Response.IsSuccessful = IsSuccessful;
            Response.Message = Message;
            Response.MessageCode = MessageCode;
            return Response;
        }

        public PushNotifyClearResponse PushNotifyClear(PushNotifyClear InputData)
        {
            PushNotifyClearResponse Response = new PushNotifyClearResponse();
            var IsSuccessful = false;
            var MessageCode = 400;
            var Message = string.Empty;
            if (InputData != null)
            {
                var VINNumber = InputData.VINNumber;
                using (miniSmartDataContext db = new miniSmartDataContext())
                {
                    try
                    {
                        var CheckNotifyClear = from u in db.tab_CA_EMS_MASTER_INFOs
                                          where u.freeze_VIN_NUMBER == VINNumber && u.freeze_PUSH_NOTIFY == true
                                          select u;

                        if (CheckNotifyClear.Count() > 0)
                        {
                            foreach (var m in CheckNotifyClear)
                            {
                                m.freeze_PUSH_NOTIFY = false;
                                m.freeze_NOTIFY_COUNTER = 5;
                                db.SubmitChanges();
                            }

                            Message = "Push Notification is Clear for this VINNumber";
                            IsSuccessful = true;
                            MessageCode = 200;

                        }
                        else
                        {
                            Message = "Push Notification is not Clear for this VINNumber";
                            IsSuccessful = false;
                            MessageCode = 400;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageCode = 500;
                        Message = "Error occured in server " + ex.Message;
                        IsSuccessful = false;
                    }
                    finally { }
                }
            }

            Response.IsSuccessful = IsSuccessful;
            Response.Message = Message;
            Response.MessageCode = MessageCode;
            return Response;
        }
    }         
}
