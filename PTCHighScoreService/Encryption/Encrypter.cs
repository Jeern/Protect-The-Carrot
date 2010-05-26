using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpineSoft;
using System.Text;
using System.Reflection;
using System.IO;

namespace PTCHighScoreService.Encryption
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

        public string Decrypt(string encryptedValue)
        {
            var ezrsa = new EZRSA(512);
            ezrsa.FromXmlString(PrivateKey);
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            return byteConverter.GetString(ezrsa.Decrypt(Convert.FromBase64String(encryptedValue), false));
        }

        private string PrivateKey
        {
            get
            {
                using (Stream stream = KeyStream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                };
            }
        }

        public string PublicKey
        {
            get
            {
                var ezrsa = new EZRSA(512);
                ezrsa.FromXmlString(PrivateKey);
                return ezrsa.ToXmlString(false);
            }
        }

        private Stream KeyStream
        {
            get
            {
                Assembly assembly = Assembly.GetAssembly(GetType());
                return assembly.GetManifestResourceStream("PTCHighScoreService.Encryption.Key.xml");
            }
        }


    }
}
