using System;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace OceanicAirlines
{
    public static class SessionExtensions
    {
        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }

        public static void SetInt32(this ISession session, string key, Int32 value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static Int32 GetInt32(this ISession session, string key)
        {
            byte[] value;
            if (session.TryGetValue(key, out value))
                return BitConverter.ToInt32(value);
            return 0;
        }
    }
}
