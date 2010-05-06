using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace PTC.Utils
{
    public class RealRandom
    {
        private Random m_Random;
        private int m_Min;
        private int m_Max;

        private static Dictionary<string, RealRandom> m_Dict = new Dictionary<string, RealRandom>();

        private RealRandom(int min, int max)
        {
            m_Random = new Random(GetSeed());
            m_Min = min;
            m_Max = max;
        }

        public static RealRandom Create(int min, int max)
        {
            string key = Key(min, max);
            if (m_Dict.ContainsKey(key))
            {
                return m_Dict[key];
            }

            var r = new RealRandom(min, max);
            m_Dict.Add(key, r);
            return r;
        }

        private static string Key(int min, int max)
        {
            var sb = new StringBuilder();
            sb.Append(min);
            sb.Append(';');
            sb.Append(max);
            return sb.ToString();
        }

        public int Next()
        {
            return m_Random.Next(m_Min, m_Max);
        }

        private int GetSeed()
        {
            var bytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return bytes[0] * bytes[1] * bytes[2] * bytes[3];
        }
    }
}
