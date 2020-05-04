using Microsoft.AspNetCore.Http;

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

    }
}
