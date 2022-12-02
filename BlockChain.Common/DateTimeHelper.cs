using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Common
{
  public  class DateTimeHelper
    {

        public static readonly System.DateTime UnixBaseTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); 

        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";


        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertInt2DateTime(long d)
        {
            //TimeZoneInfo.Local.ToLocalTime(new System.DateTime(1970, 1, 1));
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            System.DateTime time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTime2Int(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000 / 1000;            //除10000调整为13位
            return t;
        }

        public static string Seconds2DateTimeStr(int seconds)
        {
            string result = string.Empty;
            TimeSpan ts = new TimeSpan(0, 0, (int)seconds);
            if (ts.TotalDays > 1) {
                result = result + ((int)ts.TotalDays).ToString() + "days";
            }

            result = result +  ts.Hours + " Hours  " + ts.Minutes + " Minutes  " + ts.Seconds + " Seconds";
            return result;
        }

    }

}
