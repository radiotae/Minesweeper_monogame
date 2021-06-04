using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Minesweeper.System
{
    class HighScoreManager
    {
        private const int MAX_SCORES_SAVED = 5;

        private List<double> _scoreList;
        private string _fileName;

        public HighScoreManager()
        {
            this.Reset();
            _fileName = "Highscores.json";

        }

        public void Reset()
        {
            _scoreList = new List<double>();

            for (int i = 0; i < MAX_SCORES_SAVED; i++)
            {
                _scoreList.Add(999);
            }
        }

        public async Task Save(List<double> scoreList)
        {
            /*string jsonString = JsonSerializer.Serialize(_scoreList);
            File.WriteAllText(_fileName, jsonString);*/

            await using (FileStream createStream = File.Create(_fileName))
            {
                await JsonSerializer.SerializeAsync(createStream, scoreList);
            }


        }

        public List<double> Load()
        {
            if (!File.Exists(_fileName))
            {
                string jsonString = JsonSerializer.Serialize(_scoreList);
                File.WriteAllText(_fileName, jsonString);
            }

            string jsonStringLoad = File.ReadAllText(_fileName);
            _scoreList = JsonSerializer.Deserialize<List<double>>(jsonStringLoad);

            return _scoreList;
                
        }
    }
}

/*
 * await using (FileStream createStream = File.Create(fileName)) {
     await JsonSerializer.SerializeAsync(createStream, weatherForecast);
}
 * */