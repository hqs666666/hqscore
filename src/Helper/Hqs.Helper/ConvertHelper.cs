using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;

namespace Hqs.Helper
{
    public static class ConvertHelper
    {
        #region Stream

        public static string GetString(this Stream stream)
        {
            return new StreamReader(stream).ReadToEnd();
        }

        public static Stream ToStream(this string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            return stream;
        }

        #endregion

        #region Enum

        public static string ToDisplay(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var customAttribute = field.GetCustomAttribute<DisplayAttribute>(false);
            string name = customAttribute?.GetName();
            if (!string.IsNullOrEmpty(name))
                return name;
            return field.Name;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        #endregion

        #region String

        public static byte[] ToByte(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        #endregion
    }
}
