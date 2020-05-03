using System.Collections.Generic;
using System.Collections;

namespace Handler.Models
{
    public interface IDBRepository
    {
        void INSERT(string table_name, ArrayList listColumns, ArrayList listValue);
        void DELETE(string table_name, string definition = "");
        List<T> SELECT<T>(string column, string table, string definition = "", string limit = "", string Order_by = "");
        void UPDATE(string table, string value, string definition);
    }
}

