using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PTC.Utils
{
    [Serializable]
    public class HighscoreList
    {
        private const int Number = 10;
        private List<HighScore> m_Scores = new List<HighScore>(Number);

        private const string File = @"PTCHighScoreDDDF4D75A5A946a7992AB0BFCD290836.bin";

        private string m_LatestHighscoreHolder;
        public string LatestHighscoreHolder
        {
            get { return m_LatestHighscoreHolder; }
        }

        private string m_Country = Environment.NoCountryName;
        public string Country
        {
            get { return m_Country; }
            set { m_Country = value; }
        }

        public void Add(HighScore highScore)
        {
            m_LatestHighscoreHolder = highScore.Name;
            m_Scores.Add(highScore);
            m_Scores.Sort(new Comparison<HighScore>((score1, score2) => score2.Score.CompareTo(score1.Score)));
            if (m_Scores.Count > Number)
            {
                m_Scores.RemoveAt(Number);
            }
        }

        public bool IsNewHighscore(int score)
        {
            return m_Scores.Count < Number || score > m_Scores[Number-1].Score;
        }

        public void Save()
        {
            using (var stream = new IsolatedStorageFileStream(File, FileMode.Truncate))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Flush();
            }
        }

        public static HighscoreList Load()
        {
            try
            {
                using (var stream = new IsolatedStorageFileStream(File, FileMode.OpenOrCreate))
                {
                    var formatter = new BinaryFormatter();
                    return formatter.Deserialize(stream) as HighscoreList;
                }
            }
            catch (SerializationException)
            {
                //The file probably does not exist yet
                var highScores = new HighscoreList();
                highScores.Save();
                return highScores;
            }
        }

        public IEnumerable<string> GetScores()
        {
            int i = 0;
            foreach (var score in m_Scores)
            {
                i++;
                yield return i.ToString() + ". " + score.Name + "   " + score.Score.ToString() + " Points";
            }
        }
    }
}
