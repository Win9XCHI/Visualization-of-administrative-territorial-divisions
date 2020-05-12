using System.Collections.Generic;

namespace Handler.Models.Search
{
    public class ResponseSearch
    {
        public string Name { get; set; }
        public int id { get; set; }
        public string Information { get; set; }
        public string Type { get; set; }
        public List<Reference> ReferenceIn { get; set; }
        public List<Reference> ReferenceOut { get; set; }
        public List<RecordTableSearch> ListRecords { get; set; }
    }
}