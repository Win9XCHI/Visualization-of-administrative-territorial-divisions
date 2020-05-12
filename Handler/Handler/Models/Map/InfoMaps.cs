using Microsoft.SqlServer.Types;

namespace Handler.Models.Map
{
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
}