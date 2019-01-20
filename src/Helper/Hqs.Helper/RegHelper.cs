using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Hqs.Helper
{
    public static class RegHelper
    {
        #region String类

        /// <summary>
        /// 验证是否为正整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            //return Regex.IsMatch(str, @"^[0-9]*$");
            return Regex.IsMatch(str, @"^(-?)(\d+)$");
        }

        /// <summary>
        /// 验证是否为字母或者数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsWords(this string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 验证对象是否为货币格式
        /// </summary>
        /// <param name="obj">验证对象</param>
        /// <returns>货币格式返回true，否则返回false</returns>
        public static bool IsDecimal(object obj)
        {
            bool flag = true;
            if (obj.GetType() != decimal.Parse("10.11").GetType())
            {
                try
                {
                    decimal.Parse((string)obj);
                }
                catch
                {
                    flag = false;
                }
            }
            return flag;
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsUrl(this string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }


        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(this string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// 验证是否为手机号码
        /// </summary>
        /// <param name="str">手机号码</param>
        /// <returns></returns>
        public static bool IsTel(this string str)
        {
            if (str.Length != 11)
                return false;
            return Regex.IsMatch(str, @"^1[3|4|5|7|8][0-9]\d{4,8}$");
        }
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(this string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");
        }

        public static bool IsIpSect(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckCardId(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length != 18)
                return false;
            string number17 = str.Substring(0, 17);
            string number18 = str.Substring(17);
            string check = "10X98765432";
            int[] num = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            int sum = 0;
            for (int i = 0; i < number17.Length; i++)
            {
                sum += Convert.ToInt32(number17[i].ToString()) * num[i];
            }
            sum %= 11;
            if (number18.Equals(check[sum].ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
