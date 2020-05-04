using Microsoft.SqlServer.Types;

namespace Handler.Models
{
    public class FormMap
    {
        public int Year { get; set; }
        public int Level { get; set; }
        public string Exeptions { get; set; }
    }

    public class InfoMaps
    {
        public int NumberRecord { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public int Year_first { get; set; }
        public int Year_second { get; set; }
        public SqlGeography СoordinatesPoint { get; set; }
        public int Counter { get; set; }
    }

    public class Input
    {
        public int Code { get; set; }
        public string PIB { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Rights { get; set; }
    }
}