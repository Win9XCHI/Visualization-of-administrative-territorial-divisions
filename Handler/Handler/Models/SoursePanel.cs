using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace Handler.Models
{
    public class Sourse
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string YearRelevance { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public byte[] DOC { get; set; }
        public IFormFile DOCF { get; set; }

        public void ConvertToByte()
        {
            if (DOCF != null)
            {
                byte[] Data = null;
                using (var binaryReader = new BinaryReader(DOCF.OpenReadStream()))
                {
                    Data = binaryReader.ReadBytes((int)DOCF.Length);
                }
                DOC = Data;
            }
        }
    }

    public class ReturnOb
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int id { get; set; }
    }

    public class ClassGeographicFeature
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }
        public string Information { get; set; }
        public List<string> Reference { get; set; }
        public List<string> Coordinates { get; set; }
        public string Text { get; set; }

        public bool CheckCompleteness()
        {
            if (Name.Length == 0 || Type.Length == 0 || Year.Length == 0 || (Coordinates.Count == 0 && Text.Length == 0))
            {
                return false;
            }

            return true;
        }

        public void SetGeneral(string[] words)
        {
            Name = words[0];
            Type = words[1];
            Year = words[2];
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