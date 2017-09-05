using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace miniSmartCloudService
{
    public static class CommonUtil
    {
        public static bool IsValidDate(object Expression)
        {
            DateTime RetDate;
            bool isDate = DateTime.TryParse(Convert.ToString(Expression), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out RetDate);
            return isDate;
        }
    }
}