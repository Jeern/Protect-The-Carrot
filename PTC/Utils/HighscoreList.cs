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
    public class HighscoreList
    {
        private const int Number = 10;
        private List<HighScore> m_Scores = new List<HighScore>(Number);

        private const string File = @"PTCHighScoreD8C310B266204dd4B69EF0A65FCDF715.bin";

        private string m_LatestHighscoreHolder;
        public string LatestHighscoreHolder
        {
            get { return m_LatestHighscoreHolder; }
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
                formatter.Serialize(stream, m_Scores);
                stream.Flush();
            }
        }

        public void Load()
        {
            try
            {
                using (var stream = new IsolatedStorageFileStream(File, FileMode.OpenOrCreate))
                {
                    var formatter = new BinaryFormatter();
                    m_Scores = formatter.Deserialize(stream) as List<HighScore>;
                }
            }
            catch (SerializationException)
            {
                Save();
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
