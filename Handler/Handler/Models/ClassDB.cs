using System;
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
        private void InsertP(string table_name, string listColumns, string listValue)
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                if (table_name != "" && listValue != "" && listColumns != "")
                {
                    db.Execute("INSERT INTO " + table_name + " (" + listColumns + ") VALUES (" + listValue + ");");
                }
            }
        }

        public void UPDATE(string table, string value, string definition = "")
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                if (table != "" && value != "")
                {

                    if (definition != "")
                    {
                        db.Execute("UPDATE " + table + " SET " + value + " WHERE " + definition + ";");
                    }
                    else
                    {
                        db.Execute("UPDATE " + table + " SET " + value + ";");
                    }
                }
            }
        }

        public List<T> SELECT<T>(string column, string table, string definition = "", string limit = "", string Order_by = "")
        {

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                if (table != "")
                {

                    if (limit == "")
                    {

                        if (Order_by == "")
                        {
                            if (definition == "")
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " ;").ToList();
                            }
                            else
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " WHERE " + definition + " ;").ToList();
                            }
                        }
                        else
                        {
                            if (definition == "")
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " ORDER BY " + Order_by + ";").ToList();
                            }
                            else
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " WHERE " + definition + " ORDER BY " + Order_by + ";").ToList();
                            }
                        }
                    }
                    else
                    {
                        if (Order_by == "")
                        {
                            if (definition == "")
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " LIMIT " + limit + " ;").ToList();
                            }
                            else
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " WHERE " + definition + " LIMIT " + limit + ";").ToList();
                            }
                        }
                        else
                        {
                            if (definition == "")
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " LIMIT " + limit + " ORDER BY " + Order_by + ";").ToList();
                            }
                            else
                            {
                                return db.Query<T>("SELECT " + column + " FROM " + table + " WHERE " + definition + " LIMIT " + limit + " ORDER BY " + Order_by + ";").ToList();
                            }
                        }
                    }
                }
                List<T> NullList = new List<T>();
                return NullList;
            }
        }

        public void DELETE(string table_name, string definition = "")
        {
            if (table_name.Length > 0)
            {

                using (IDbConnection db = new SqlConnection(connectionString))
                {

                    if (definition.Length > 0)
                    {

                        db.Execute("DELETE FROM " + table_name + ";");
                    }
                    else
                    {
                        db.Execute("DELETE FROM " + table_name + " WHERE " + definition + ";");
                    }
                }
            }
        }
    }
}
