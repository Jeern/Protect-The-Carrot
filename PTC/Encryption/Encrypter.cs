using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlpineSoft;

namespace PTC.HighScoreProxy
{
    public class Encrypter
    {
        public string Encrypt(string value, string publicKey)
        {
            var ezrsa = new EZRSA(512);
            ezrsa.FromXmlString(publicKey);
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            return Convert.ToBase64String(ezrsa.Encrypt(byteConverter.GetBytes(value), false));
        }
    }
}
