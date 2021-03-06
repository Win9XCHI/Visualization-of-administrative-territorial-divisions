﻿using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.SoursePanel;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Handler.Models.Repositories {
    public class SoursePanelRepository : DBRepository, ISoursePanelRepository
    {
        public SoursePanelRepository(string conn) : base(conn) { }

        public List<Sourse> GetSources(Sourse SourseOb)
        {
            string where = "";

            if (SourseOb.Name != null)
            {
                where += "Name = '" + SourseOb.Name + "'";
            }

            if (SourseOb.Author != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Author = '" + SourseOb.Author + "'";
            }

            if (SourseOb.Type != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Type = '" + SourseOb.Type + "'";
            }

            if (SourseOb.Year != 0)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "YearCreate = '" + SourseOb.Year + "'";
            }

            if (SourseOb.YearRelevance != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "CAST(SUBSTRING('YearRelevance', 1, 5) AS INT) < " + SourseOb.YearRelevance + " AND CAST(SUBSTRING('YearRelevance', 7, 11) AS INT) > " + SourseOb.YearRelevance;
            }

            return SELECT<Sourse>("idSourse AS id, Name, Type, Author, YearCreate AS Year, YearRelevance", "Sourse", where);
        }

        public void DeleteSourse(int id)
        {
            List<int> M_IDs = SELECT<int>("Local_point.Midle_id",
                "Local_point JOIN Sourse_LocalPoint ON (Local_point.id = Sourse_LocalPoint.Local_point_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_LocalPoint.Sourse_idSourse) WHERE Sourse.idSourse = " + id +
"UNION SELECT Region.Midle_id FROM Region JOIN Sourse_Region ON(Region.id = Sourse_Region.Region_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Region.Sourse_idSourse) WHERE Sourse.idSourse = " + id +
"UNION SELECT Administrative_unit.Midle_id FROM Administrative_unit JOIN Sourse_Administrative_unit ON(Administrative_unit.id = Sourse_Administrative_unit.Administrative_unit_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Administrative_unit.Sourse_idSourse) WHERE Sourse.idSourse = " + id +
"UNION SELECT Country.Midle_id FROM Country JOIN Sourse_Country ON(Country.id = Sourse_Country.Country_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Country.Sourse_idSourse)",
                "Sourse.idSourse = " + id); 

            for (int i = 0; i < M_IDs.Count; i++)
            {
                DELETE("Region", "Midle_id = " + M_IDs[i]);
                DELETE("Local_point", "Midle_id = " + M_IDs[i]);
                DELETE("Administrative_unit", "Midle_id = " + M_IDs[i]);
                DELETE("Midle", "id = " + M_IDs[i]);
            }

            DELETE("Sourse", "idSourse = " + id);
        }

        public void UpdateSourse(Sourse ObS)
        {
            UPDATE("Sourse",
                "Name = '" + ObS.Name + "', Author = '" + ObS.Author + "', Type = '" + ObS.Type + "', YearCreate = '" + ObS.Year + "', YearRelevance = '" + ObS.YearRelevance + "'",
                "idSourse = " + ObS.id);
        }

        public int AddNewSourse(Sourse ObS)
        {
            ObS.YearRelevance = "All";
            INSERT("Sourse", new ArrayList { "Name", "Type", "YearCreate", "YearRelevance", "Author", "DOC" },
                new ArrayList { "'" + ObS.Name + "'", "'" + ObS.Type + "'", "'" + ObS.Year + "'", "'" + ObS.YearRelevance + "'", "'" + ObS.Author + "'", ObS.DOC });

            return SELECT<int>("idSourse", "Sourse", "Name = " + ObS.Name)[0];
        }

        public void AddNewError(string str, int id)
        {
            INSERT("ErrorSourse", new ArrayList { "ErrorString", "Time", "Sourse_idSourse" }, new ArrayList { str, DateTime.Now, id }); 
        }

        public bool CheckNameObject(string Name)
        {
            if (SELECT<ReturnOb>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + Name + "' " +
"UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + Name + "' " +
"UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + Name + "' " +
"UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                "Country.Name = '" + Name + "'").Count > 0)
            {
                return true;
            }
            return false;
        }

        public ReturnOb GetObject(string Name)
        {
            return SELECT<ReturnOb>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + Name + "' " +
"UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + Name + "' " +
"UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + Name + "' " +
"UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                "Country.Name = '" + Name + "'")[0];
        }

        private void OldDetailsInformation(int Year, ClassGeographicFeature GFeature, int Cid, int id)
        {
            List<int> idDI = SELECT<int>("id", "DetailsInformation",
                "DetailsInformation.Year = '" + Year + "' AND DetailsInformation.Midle_id = " + Cid);

            if (idDI.Count == 0)
            {
                INSERT("DetailsInformation", new ArrayList { "Year", "Midle_id" },
                    new ArrayList { Year, Cid });

                idDI = SELECT<int>("id", "DetailsInformation",
                    "DetailsInformation.Year = '" + Year + "' AND DetailsInformation.Midle_id = " + Cid);

                if (GFeature.Coordinates.Count != 0)
                {
                    InsertCoordinates(GFeature.Coordinates, idDI[0]);
                }
                else
                {
                    InsertText(GFeature.Text, idDI[0], id);
                }
            }
            else
            {
                if (GFeature.Coordinates.Count != 0)
                {
                    List<int> CheckCoo = SELECT<int>("Coordinates.Counter",
                    "Coordinates JOIN DetailsInformation ON (DetailsInformation.id = Coordinates.DetailsInformation_id)",
                    "DetailsInformation.Year = '" + Year + "' AND DetailsInformation.Midle_id = " + Cid);

                    if (CheckCoo.Count == 0)
                    {
                        InsertCoordinates(GFeature.Coordinates, idDI[0]);
                    }
                }
                else
                {
                    List<int> CheckText = SELECT<int>("id",
                    "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id)",
                    "DetailsInformation.Year = '" + Year + "' AND DetailsInformation.Midle_id = " + Cid + " AND Part.Information = '" + GFeature.Text + "'");

                    if (CheckText.Count == 0)
                    {
                        InsertText(GFeature.Text, idDI[0], id);
                    }
                }
            }
        }

        private void NewDetailsInformation(int Year, ClassGeographicFeature GFeature, int Midle_id, int id)
        {
            INSERT("DetailsInformation", new ArrayList { "Year", "Midle_id" }, new ArrayList { Year, Midle_id });

            int idDI = SELECT<int>("id", "DetailsInformation", "DetailsInformation.Year = '" + Year + "' AND DetailsInformation.Midle_id = " + Midle_id)[0];

            if (GFeature.Coordinates.Count != 0)
            {
                InsertCoordinates(GFeature.Coordinates, idDI);
            }
            else
            {
                InsertText(GFeature.Text, idDI, id);
            }
        }

        public void NewObject(ClassGeographicFeature GFeature, int number, int id)
        {
            INSERT("Midle", new ArrayList { "Type" }, new ArrayList { GFeature.Type });
            int Midle_id = SELECT<int>("MAX(id)", "Midle")[0];

            INSERT(GFeature.Type, new ArrayList { "Name", "Information", "Midle_id" },
                new ArrayList { GFeature.Name, GFeature.Information, Midle_id });

            INSERT("Sourse_" + GFeature.Type, new ArrayList { "Sourse_idSourse", GFeature.Type + "_id" },
                new ArrayList { id, SELECT<int>("id", GFeature.Type, "Midle_id = " + Midle_id)[0] });

            if (GFeature.Year.Count == 2)
            {
                for (int i = Int32.Parse(GFeature.Year[0]); i < Int32.Parse(GFeature.Year[1]); i++)
                {
                    NewDetailsInformation(i, GFeature, Midle_id, id);
                }
            }
            else
            {
                NewDetailsInformation(Int32.Parse(GFeature.Year[0]), GFeature, Midle_id, id);
            }

            InsertReference(GFeature, SELECT<int>("id", GFeature.Type, "Midle_id = " + Midle_id)[0], number, id);
        }
        public void OldObject(ClassGeographicFeature GFeature, ReturnOb CheckName, int number, int id)
        {
            GFeature.Type = CheckName.Type;

            INSERT("Sourse_" + GFeature.Type, new ArrayList { "Sourse_idSourse", GFeature.Type + "_id" },
                new ArrayList { id, CheckName.id });

            if (GFeature.Year.Count == 2)
            {
                for (int i = Int32.Parse(GFeature.Year[0]); i < Int32.Parse(GFeature.Year[1]); i++)
                {
                    OldDetailsInformation(i, GFeature, CheckName.id, id);
                }
            }
            else
            {
                OldDetailsInformation(Int32.Parse(GFeature.Year[0]), GFeature, CheckName.id, id);
            }

            InsertReference(GFeature, CheckName.id, number, id);
        }

        private void InsertText(string Text, int idDI, int id)
        {
            INSERT("Part", new ArrayList { "Information", "DetailsInformation_id", "Sourse_idSourse" },
                new ArrayList { Text, idDI, id });
        }
        private void InsertReference(ClassGeographicFeature GFeature, int idOB, int number, int idS)
        {
            if (GFeature.Reference.Count != 0)
            {
                for (int i = 0; i < GFeature.Reference.Count; i++)
                {
                    List<ReturnOb> Reference = SELECT<ReturnOb>("Local_point.Name, Local_point.id, Midle.Type",
                    "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Region.Name, Region.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Administrative_unit.Name, Administrative_unit.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Country.Name, Country.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                    "Country.Name = '" + GFeature.Reference[i] + "'");

                    if (Reference.Count > 0)
                    {
                        if (GFeature.Type == "Administrative_unit" && Reference[0].Type == "Region")
                        {
                            List<int> id = SELECT<int>("id", "Region_AU", "Administrative_unit_id = " + idOB + " AND Region_id = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                INSERT("Region_AU", new ArrayList { "Administrative_unit_id", "Region_id" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Region" && Reference[0].Type == "Administrative_unit")
                        {
                            List<int> id = SELECT<int>("id", "Region_AU", "Region_id = " + idOB + " AND Administrative_unit_id = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                INSERT("Region_AU", new ArrayList { "Region_id", "Administrative_unit_id" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Region" && Reference[0].Type == "Local_point")
                        {
                            List<int> id = SELECT<int>("id", "Local_point", "Region_id = " + idOB + " AND id = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                UPDATE("Local_point", "Region_id = " + idOB, "id = " + Reference[0].id);
                            }
                        }

                        if (GFeature.Type == "Local_point" && Reference[0].Type == "Region")
                        {
                            List<int> id = SELECT<int>("id", "Local_point", "Region_id = " + Reference[0].id + " AND id = " + idOB);

                            if (id.Count == 0)
                            {
                                UPDATE("Local_point", "Region_id = " + Reference[0].id, "id = " + idOB);
                            }
                        }

                        if (GFeature.Type == "Administrative_unit" && Reference[0].Type == "Country")
                        {
                            List<int> id = SELECT<int>("id", "Administrative_unit", "id = " + idOB + " AND Country_id = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                UPDATE("Administrative_unit", "Country_id = " + Reference[0].id, "id = " + idOB);
                            }
                        }

                        if (GFeature.Type == "Country" && Reference[0].Type == "Administrative_unit")
                        {
                            List<int> id = SELECT<int>("id", "Administrative_unit", "id = " + Reference[0].id + " AND Country_id = " + idOB);

                            if (id.Count == 0)
                            {
                                UPDATE("Administrative_unit", "Country_id = " + idOB, "id = " + Reference[0].id);
                            }
                        }
                    }
                    else
                    {
                        AddNewError("Not reference for " + GFeature.Name + " and " + GFeature.Reference[i] + "; Line #" + number, idS);
                    }
                }
            }
        }
        private void InsertCoordinates(List<string> Coordinates, int idDI)
        {
            for (int i = 0; i < Coordinates.Count; i++)
            {
                INSERT("Coordinates", new ArrayList { "СoordinatesPoint", "Counter", "DetailsInformation_id" }, new ArrayList { "geography::Point(" + Coordinates[i] + ", 4326)", i + 1, idDI });
            }
        }
    }
}
