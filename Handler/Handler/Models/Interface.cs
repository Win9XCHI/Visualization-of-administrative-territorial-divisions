using System.Collections.Generic;
using System.Collections;

namespace Handler.Models
{
    public interface IDBRepository
    {
        void INSERT(string table_name, ArrayList listColumns, ArrayList listValue);
        void DELETE(string table_name, string definition = "");
        List<T> SELECT<T>(string columns, string table_name, string definition = "", string Group_by = "", string Order_by = "", string Having = "");
        void UPDATE(string table, string value, string definition);
    }
}

