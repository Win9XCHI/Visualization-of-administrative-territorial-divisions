using System.Collections.Generic;

namespace Handler.Models.SoursePanel
{
    public class ClassGeographicFeature
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Year { get; set; }
        public string Information { get; set; }
        public List<string> Reference { get; set; }
        public List<string> Coordinates { get; set; }
        public string Text { get; set; }

        public bool CheckCompleteness()
        {
            if (Name.Length == 0 || Type.Length == 0 || Year.Count == 0 || (Coordinates.Count == 0 && Text.Length == 0))
            {
                return false;
            }

            return true;
        }

        public void SetGeneral(string[] words)
        {
            Name = words[0];
            Type = words[1];
            Year = new List<string>(words[2].Split("-"));
            Information = words[3];
            Reference = new List<string>(words[4].Split(";"));
        }
        public void SetForGeo(string str)
        {
            string[] words = str.Split("|");
            SetGeneral(words);
            Coordinates = new List<string>(words[5].Split(";"));
        }

        public void SetForText(string str)
        {
            string[] words = str.Split("|");
            SetGeneral(words);
            Text = words[5];
        }
    }
}