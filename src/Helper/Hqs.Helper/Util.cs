using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Hqs.Helper
{
    public class Util
    {
        public static Dictionary<Type, Type[]> GetClassName(string assemblyName)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    result.Add(item, interfaceType);
                }
                return result;
            }
            return new Dictionary<Type, Type[]>();
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrWhiteSpace(strContent))
            {
                if (strContent.IndexOf(strSplit, StringComparison.Ordinal) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 判断元素是否在列表中
        /// </summary>
        /// <param name="strin">判断字符</param>
        /// <param name="arraystring">使用,号隔开</param>
        /// <returns></returns>
        public static bool IsInArrayString(string strin, string arraystring)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(arraystring))
            {
                string[] srrids = arraystring.Split(new string[] { "," }, StringSplitOptions.None);
                List<string> kids = new List<string>();
                foreach (string s in srrids)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        kids.Add(s.Trim());//加入允许使用的商品列表
                    }
                }
                if (kids.FindIndex(s => s == strin) >= 0)
                    flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.UTF8.GetBytes(str).Length;
        }

        /// <summary>
        /// 获取文件名字，不带扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string fileName)
        {
            int length = fileName.Length - 1, dotPos = fileName.IndexOf(".", StringComparison.Ordinal);


            if (dotPos == -1)
                return fileName;

            return fileName.Substring(0, dotPos);
        }

        /// <summary>
        /// 创建一个短的GUID
        /// </summary>
        /// <returns>字符串类型</returns>
        public static string GetShortGuid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return $"{i - DateTime.Now.Ticks:x}";
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFilename(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".", StringComparison.Ordinal) == -1)
                return false;

            string extname = filename.Substring(filename.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        /// <summary>
        /// 获取HTML里面首张图片
        /// </summary>
        /// <param name="html">html内容</param>
        /// <returns>首张图片地址，为空则图片不存在</returns>
        public static string GetHtmlfirstImgSrc(string html)
        {
            string imgsrc = "";
            Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.IgnoreCase);
            MatchCollection m = reg.Matches(html);
            if (m.Count > 0)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    if (!string.IsNullOrEmpty(m[i].Groups[1].Value) && IsImgFilename(m[i].Groups[1].Value.ToString()))
                    {
                        imgsrc = m[i].Groups[1].Value.ToString();
                        break;
                    }
                }
            }
            else
            {
                imgsrc = "";
            }
            return imgsrc;

        }
        /// <summary>
        /// 获取所有图片
        /// </summary>
        /// <param name="pic">图片字符串</param>
        /// <returns>图片地址</returns>
        public static List<string> GetAllPic(string pic)
        {
            if (!string.IsNullOrEmpty(pic))
            {
                List<string> list = new List<string>();
                string[] arrpic = pic.Split(new string[] { "|||" }, StringSplitOptions.None);
                foreach (string picurl in arrpic)
                    list.Add(picurl);
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="strInput">字符串</param>
        /// <param name="intLen">截取长度</param>
        /// <returns></returns>
        public static string CutString(string strInput, int intLen)
        {
            if (string.IsNullOrEmpty(strInput))
                return "";
            strInput = strInput.Trim();
            byte[] myByte = Encoding.UTF8.GetBytes(strInput);
            //Response.Write("cutString Function is::" + myByte.Length.ToString());
            if (myByte.Length > intLen + 2)
            {
                //截取操作
                string resultStr = "";
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = Encoding.UTF8.GetBytes(resultStr);
                    if (tempByte.Length < intLen)
                    {

                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + "...";
            }
            else
            {
                return strInput;
            }
        }
        /// <summary>
        /// 判断是否是GUID
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsGuidByParse(string strSrc)
        {
            Guid g = Guid.Empty;
            return Guid.TryParse(strSrc, out g);
        }

        /// <summary>
        /// 时间戳转换成DateTime格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            //TimeZoneInfo.ConvertTimeFromUtc()

            DateTime dateTimeStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        ///一个处理危险HTML标签的方法
        /// </summary>
        /// <param name="M_Htmlstring">要处理的字符串</param>
        /// <returns></returns>
        public static string NoHtml(string M_Htmlstring) //去除HTML标记 
        {
            if (string.IsNullOrWhiteSpace(M_Htmlstring))
                return "";
            //删除脚本 
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"<script[^>]*?>.*? </script>", string.Empty, RegexOptions.IgnoreCase);
            //删除HTML 
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"<(.?[^>]*)>", string.Empty, RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);//空格换行
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"-->", string.Empty, RegexOptions.IgnoreCase);//注释
            M_Htmlstring = Regex.Replace(M_Htmlstring, @" <!--.*", string.Empty, RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(quot|34);", "\"", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(amp|38);", "&", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(lt|60);", " <", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(gt|62);", ">", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(nbsp|160);", " ", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(iexcl|161);", "\xa1", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(cent|162);", "\xa2", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(pound|163);", "\xa3", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"&(copy|169);", "\xa9", RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @" href *= *[\s\S]*script *:", string.Empty, RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @" on[\s\S]*=", string.Empty, RegexOptions.IgnoreCase);//过滤其它控件的on...事件
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"<iframe[\s\S]+</iframe *>", string.Empty, RegexOptions.IgnoreCase);//过滤iframe
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"<frameset[\s\S]+</frameset *>", string.Empty, RegexOptions.IgnoreCase);//过滤frameset
            M_Htmlstring = Regex.Replace(M_Htmlstring, @"<", string.Empty, RegexOptions.IgnoreCase);
            M_Htmlstring = Regex.Replace(M_Htmlstring, @">", string.Empty, RegexOptions.IgnoreCase);
            // M_Htmlstring.Replace("\r\n", string.Empty);
            M_Htmlstring = M_Htmlstring.Trim();
            return M_Htmlstring;
        }
        /// <summary>
        /// 普通过滤脚本、框架
        /// </summary>
        /// <param name="M_Html">要处理的字符串</param>
        /// <returns>去除脚本、框架、style样式标记</returns>
        public static string NoScriptAndIframe(string M_Html)
        {
            if (!string.IsNullOrEmpty(M_Html))
            {
                M_Html = Regex.Replace(M_Html, @"^<style\\s*[^>]*>([^>]|[~<])*<\\/style>$", string.Empty, RegexOptions.IgnoreCase);
                M_Html = Regex.Replace(M_Html, @"<script[^>]*?>.*? </script>", string.Empty, RegexOptions.IgnoreCase);
                M_Html = Regex.Replace(M_Html, @"<iframe[\s\S]+</iframe *>", string.Empty, RegexOptions.IgnoreCase);//过滤iframe
                M_Html = Regex.Replace(M_Html, @"<frameset[\s\S]+</frameset *>", string.Empty, RegexOptions.IgnoreCase);//过滤frameset
            }
            else
            {
                M_Html = "";
            }
            return M_Html.Trim();
        }
        /// <summary>    
        /// 过滤字符串中的html代码    
        /// </summary>    
        /// <param name="Str"></param>    
        /// <returns>返回过滤之后的字符串</returns>    
        public static string LostHtml(string Str)
        {
            if (string.IsNullOrWhiteSpace(Str))
                return "";

            string Re_Str = "";
            string Pattern = "<\\/*[^<>]*>";
            Re_Str = Regex.Replace(Str, Pattern, "");
            return (Re_Str.Replace("\\r\\n", "")).Replace("\\r", "");
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar, StringComparison.Ordinal) >= 0 && str.LastIndexOf(strchar, StringComparison.Ordinal) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar, StringComparison.Ordinal));
            }
            return str;
        }
    }
}
