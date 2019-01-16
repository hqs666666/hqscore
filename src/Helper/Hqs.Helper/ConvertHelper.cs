using System;
using System.Collections.Generic;
using System.IO;
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
    }
}
