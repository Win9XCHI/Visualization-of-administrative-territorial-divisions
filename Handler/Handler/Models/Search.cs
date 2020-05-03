using System.Collections.Generic;

public class FormSearch
{
    public int Year { get; set; }
    public string Name { get; set; }
}

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

public class Reference
{
    public int ID { get; set; }
    public string Name { get; set; }
}

public class RecordTableSearch
{
    public int Year { get; set; }
    public string InformationYear { get; set; }
}