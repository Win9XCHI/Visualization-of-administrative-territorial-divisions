using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections;

namespace Handler.Models {

    public class DBRepository : IDBRepository
    {
        string connectionString = null;

        public DBRepository(string conn)
        {
            connectionString = conn;
        }
        public void INSERT(string table_name, ArrayList listColumns, ArrayList listValue)
        {
            if (table_name.Length != 0 && listColumns.Count != 0 && listColumns.Count == listValue.Count)
            {
                string Columns = "";
                string Values = "";

                for (int i = 0; i < listColumns.Count; i++)
                {
                    Columns += listColumns[i].ToString();

                    if (i != listColumns.Count - 1)
                    {
                        Columns += ", ";
                    }
                }

                for (int i = 0; i < listValue.Count; i++)
                {
                    Values += listValue[i].ToString();

                    if (i != listValue.Count - 1)
                    {
                        Values += ", ";
                    }
                }

                InsertP(table_name, Columns, Values);
            }
        }
        private void InsertP(string table_name, string listColumns, string listValue)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("INSERT INTO " + table_name + " (" + listColumns + ") VALUES (" + listValue + ");");
            }
        }
        public void UPDATE(string table_name, string value, string definition = "")
        {
            if (table_name.Length != 0 && value.Length != 0)
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string str = "UPDATE " + table_name + " SET " + value;
                    if (definition != "")
                    {
                        str += " WHERE " + definition;
                    }
                    str += ";";
                    db.Execute(str);
                }
            }
        }
        public List<T> SELECT<T>(string columns, string table_name, string definition = "", string Group_by = "", string Order_by = "", string Having = "")
        {
            if (table_name.Length == 0 || columns.Length == 0)
            {
                throw new System.Exception("Null table_name or columns ");
            }

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //
                string str = "SELECT " + columns + " FROM " + table_name;

                if (definition.Length != 0) {
                    str += " WHERE " + definition;
                }

                if (Group_by.Length != 0)
                {
                    str += " GROUP BY " + Group_by;
                }

                if (Order_by.Length != 0)
                {
                    str += " ORDER BY " + Order_by;
                }

                if (Having.Length != 0)
                {
                    str += " HAVING " + Having;
                }

                str += ";";
                return db.Query<T>(str).ToList();
            }
        }
        public void DELETE(string table_name, string definition = "")
        {
            if (table_name.Length != 0)
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string str = "DELETE FROM " + table_name;
                    if (definition.Length != 0)
                    {
                        str += " WHERE " + definition;
                    }
                    str += ";";
                    db.Execute(str);
                }
            }
        }
    }
}
