using System;
using System.Collections.Generic;
using System.Text;

namespace Qorrect.Integration.Helper
{
    public static class ExtentionsService
    {
        // Convert Array to String
        public static string UTF8ByteArrayToString(byte[] ArrBytes)
        {
            return ArrBytes == null ? null : new UTF8Encoding().GetString(ArrBytes);
        }
        // Convert String to Array
        public static byte[] StringToUTF8ByteArray(string XmlString)
        { return string.IsNullOrEmpty(XmlString) ? null : new UTF8Encoding().GetBytes(XmlString); }

        public static string SerializeJson<T>(this T value)
        {
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(value);

                return json;


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T element in source) act(element);
            return source;
        }


    }
}
