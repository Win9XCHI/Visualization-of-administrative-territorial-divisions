using Microsoft.AspNetCore.Http;
using System.IO;

namespace Handler.Models.SoursePanel
{
    public class Sourse
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string YearRelevance { get; set; }
        public string Type { get; set; }
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
}