using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PTC.Util;
using System.ServiceModel;
using PTC.Utils;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace PTC.HighScoreProxy
{
    public class HighScoreServiceClient : ClientBase<IHighScoreService>, IHighScoreService
    {
#if DEBUG
        public HighScoreServiceClient() : base(new BasicHttpBinding(BasicHttpSecurityMode.None), new EndpointAddress("http://localhost:1701/HighScoreService.svc")) {}
#else
        public HighScoreServiceClient() : base(new BasicHttpBinding(BasicHttpSecurityMode.None), new EndpointAddress("http://www.niedermann.dk/PTC/HighScoreService.svc")) { }
#endif

        public KeyInfo GetPublicKey()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return null;
            try
            {
                return Channel.GetPublicKey();
            }
            catch
            {
                return null;
            }
        }

        public void Submit(string highScore)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return;

            try
            {
                Channel.Submit(highScore);
            }
            catch
            {
                return;
            }
        }

        public List<HighScore> GetCurrentHighScores()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return null;

            try
            {
                return Channel.GetCurrentHighScores();
            }
            catch
            {
                return null;
            }
        }

        public void Save(HighScore score)
        {
            if (PublicKey == null)
                return;

            Submit(new Encrypter().Encrypt(score.ToString(), PublicKey.Key));
        }

        private KeyInfo m_PublicKey;

        private KeyInfo PublicKey
        {
            get
            {
                if (m_PublicKey == null)
                {
                    m_PublicKey = GetPublicKey();
                }
                return m_PublicKey;
            }
        }

        public IEnumerable<string> GetScores()
        {
            int i = 0;
            List<HighScore> scores = GetCurrentHighScores();
            if (scores == null)
            {
                yield return "NOT AVAILABLE";
            }
            else
            {
                foreach (HighScore score in scores)
                {
                    i++;
                    yield return i.ToString() + ". " + score.Name + ", " + score.Country + "   " + score.Score.ToString() + " Points";
                }
            }
        }
    }
}
