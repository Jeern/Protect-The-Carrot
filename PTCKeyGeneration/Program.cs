using System.IO;
using System.Security.Cryptography;

namespace PTCKeyGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var enc = new RSACryptoServiceProvider(512);
            string xml = enc.ToXmlString(true);
            using(var fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Key.xml"), FileMode.OpenOrCreate))
            using (var sw = new StreamWriter(fs))
            {
                sw.Write(xml);
            }
        }
    }
}
