using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PTC.Utils;
using PTC.Util;
using PTCHighScoreService.Encryption;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Web;

namespace PTCHighScoreService
{
    public class HighScoreService : IHighScoreService
    {
        public KeyInfo GetPublicKey()
        {
            var info = new KeyInfo();
            var enc = new Encrypter();
            info.Key = enc.PublicKey;
            return info;
        }

        public void Submit(string highScore)
        {
            bool save = false;
            HighScore score = new HighScore();
            score.FromString(new Encrypter().Decrypt(highScore));
            List<HighScore> scores = GetCurrentHighScores();
            if (scores.Count < 10)
            {
                scores.Add(score);
                save = true;
            }
            else
            {
                if (score.Score > scores[9].Score)
                {
                    scores.Add(score);
                    save = true;
                }
            }
            if (save)
            {
                Save(scores);
            }
        }

        private static readonly string FILE = Path.Combine(HttpRuntime.AppDomainAppPath, "Score.bin");

        private void Save(List<HighScore> scores)
        {
            scores.Sort(new Comparison<HighScore>((score1, score2) => (score2.Score.CompareTo(score1.Score))));
            while (scores.Count > 10)
            {
                scores.RemoveAt(10);
            }

            using (var stream = new FileStream(FILE, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, scores);
                stream.Flush();
            }
        }

        public List<HighScore> GetCurrentHighScores()
        {
            if (!File.Exists(FILE))
                return new List<HighScore>();

            using (var stream = new FileStream(FILE, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as List<HighScore>;
            }
        }
    }
}
