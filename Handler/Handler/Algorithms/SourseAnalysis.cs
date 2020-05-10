using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text;
using Handler.Models;

namespace Handler.Algorithms
{
    public class SourseAnalysis
    {
        private IDBRepository repo;
        private Sourse ObS;
        public int Status;
        public SourseAnalysis(IDBRepository r, Sourse Ob)
        {
            repo = r;
            ObS = Ob;
            Status = 0;
        }

        public void Start()
        {
            ObS.ConvertToByte();
            CreateNewSourse();

            using (var reader = new StreamReader(ObS.DOCF.OpenReadStream()))
            {
                int number = 1;
                while (reader.Peek() >= 0)
                {
                    if (ObS.Type == "Text")
                    {
                        ProcessText(reader.ReadLine(), number);
                    }
                    if (ObS.Type == "Map")
                    {
                        ProcessMap(reader.ReadLine(), number);
                    }
                    number++;
                }
            }
            Status = 1;
        }
        private void CreateNewSourse()
        {
            repo.INSERT("Sourse", new ArrayList { "Name", "Type", "YearCreate", "Author", "DOC" },
                new ArrayList { "'" + ObS.Name + "'", "'" + ObS.Type + "'", "'" + ObS.Year + "'", "'" + ObS.Author + "'", ObS.DOC });
            
            ObS.id = repo.SELECT<int>("MAX(idSourse)", "Sourse")[0];
        }
        private void NewError(string str)
        {//add time and pib
            repo.INSERT("ErrorSourse", new ArrayList { "ErrorString" }, new ArrayList { str });
        }
        private void ProcessMap(string str, int number)
        {
            ClassGeographicFeature GFeature = new ClassGeographicFeature();
            GFeature.SetForGeo(str);
            
            if (!GFeature.CheckCompleteness()) {
                NewError("Not completeness data; Line #" + number);
                return;
            }

            List<ReturnOb> CheckName = repo.SELECT<ReturnOb>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + GFeature.Name + "' " +
"UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + GFeature.Name + "' " +
"UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + GFeature.Name + "' " +
"UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                "Country.Name = '" + GFeature.Name + "'");

            if (CheckName.Count > 0)
            {
                OldObject(GFeature, CheckName[0], number);
            }
            else
            {
                NewObject(GFeature, number);
            }
        }
        private void ProcessText(string str, int number)
        {
            ClassGeographicFeature GFeature = new ClassGeographicFeature();
            GFeature.SetForText(str);

            if (!GFeature.CheckCompleteness())
            {
                NewError("Not completeness data; Line #" + number);
                return;
            }

            List<ReturnOb> CheckName = repo.SELECT<ReturnOb>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + GFeature.Name + "' " +
"UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + GFeature.Name + "' " +
"UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + GFeature.Name + "' " +
"UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                "Country.Name = '" + GFeature.Name + "'");

            if (CheckName.Count > 0)
            {
                OldObject(GFeature, CheckName[0], number);
            }
            else
            {
                NewObject(GFeature, number);
            }
        }
        private void NewObject(ClassGeographicFeature GFeature, int number)
        {
            repo.INSERT("Midle", new ArrayList { "Type" }, new ArrayList { GFeature.Type });
            int Midle_id = repo.SELECT<int>("MAX(id)", "Midle")[0];

            repo.INSERT(GFeature.Type, new ArrayList { "Name", "Information", "Midle_id" },
                new ArrayList { GFeature.Name, GFeature.Information, Midle_id });

            repo.INSERT("Sourse_" + GFeature.Type, new ArrayList { "Sourse_idSourse", GFeature.Type + "_id" }, 
                new ArrayList { ObS.id, repo.SELECT<int>("id", GFeature.Type, "Midle_id = " + Midle_id)[0] });

            repo.INSERT("DetailsInformation", new ArrayList { "Year", "Midle_id" }, new ArrayList { GFeature.Year, Midle_id });

            int idDI = repo.SELECT<int>("id", "DetailsInformation", "DetailsInformation.Year = '" + GFeature.Year + "' AND DetailsInformation.Midle_id = " + Midle_id)[0];

            if (GFeature.Coordinates.Count != 0)
            {
                InsertCoordinates(GFeature.Coordinates, idDI);
            }
            else
            {
                InsertText(GFeature.Text, idDI);
            }

            InsertReference(GFeature, repo.SELECT<int>("id", GFeature.Type, "Midle_id = " + Midle_id)[0], number);
        }
        private void OldObject(ClassGeographicFeature GFeature, ReturnOb CheckName, int number)
        {
            GFeature.Type = CheckName.Type;

            repo.INSERT("Sourse_" + GFeature.Type, new ArrayList { "Sourse_idSourse", GFeature.Type + "_id" }, 
                new ArrayList { ObS.id, CheckName.id});

            List<int> idDI = repo.SELECT<int>("id", "DetailsInformation", 
                "DetailsInformation.Year = '" + GFeature.Year + "' AND DetailsInformation.Midle_id = " + CheckName.id);

            if (idDI.Count == 0)
            {
                repo.INSERT("DetailsInformation", new ArrayList { "Year", "Midle_id" }, 
                    new ArrayList { GFeature.Year, CheckName.id });

                idDI = repo.SELECT<int>("id", "DetailsInformation", 
                    "DetailsInformation.Year = '" + GFeature.Year + "' AND DetailsInformation.Midle_id = " + CheckName.id);

                if (GFeature.Coordinates.Count != 0) {
                    InsertCoordinates(GFeature.Coordinates, idDI[0]);
                }
                else {
                    InsertText(GFeature.Text, idDI[0]);
                }
            }
            else
            {
                if (GFeature.Coordinates.Count != 0)
                {
                    List<int> CheckCoo = repo.SELECT<int>("Coordinates.Counter",
                    "Coordinates JOIN DetailsInformation ON (DetailsInformation.id = Coordinates.DetailsInformation_id)",
                    "DetailsInformation.Year = '" + GFeature.Year + "' AND DetailsInformation.Midle_id = " + CheckName.id);

                    if (CheckCoo.Count == 0)
                    {
                        InsertCoordinates(GFeature.Coordinates, idDI[0]);
                    }
                }
                else
                {
                    List<int> CheckText = repo.SELECT<int>("id",
                    "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id)",
                    "DetailsInformation.Year = '" + GFeature.Year + "' AND DetailsInformation.Midle_id = " + CheckName.id + " AND Part.Information = '" + GFeature.Text + "'");

                    if (CheckText.Count == 0)
                    {
                        InsertText(GFeature.Text, idDI[0]);
                    }
                }
            }

            InsertReference(GFeature, CheckName.id, number);
        }

        private void InsertText(string Text, int idDI)
        {
            repo.INSERT("Part", new ArrayList { "Information", "DetailsInformation_id", "Sourse_idSourse" },
                new ArrayList { Text, idDI, ObS.id});
        }
        private void InsertReference(ClassGeographicFeature GFeature, int idOB, int number)
        {
            if (GFeature.Reference.Count != 0)
            {
                for (int i = 0; i < GFeature.Reference.Count; i++)
                {
                    List<ReturnOb> Reference = repo.SELECT<ReturnOb>("Local_point.Name, Midle.id, Midle.Type",
                    "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '" + GFeature.Reference[i] + "' " +
    "UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id)",
                    "Country.Name = '" + GFeature.Reference[i] + "'");

                    if (Reference.Count > 0)
                    {
                        if (GFeature.Type == "Administrative_unit" && Reference[0].Type == "Region")
                        {
                            List<int> id = repo.SELECT<int>("id", "Region_AU", "Administrative_unit_idAU = " + idOB + " AND Region_idR = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                repo.INSERT("Region_AU", new ArrayList { "Administrative_unit_idAU", "Region_idR" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Region" && Reference[0].Type == "Administrative_unit")
                        {
                            List<int> id = repo.SELECT<int>("id", "Region_AU", "Region_idR = " + idOB + " AND Administrative_unit_idAU = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                repo.INSERT("Region_AU", new ArrayList { "Region_idR", "Administrative_unit_idAU" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Region" && Reference[0].Type == "Local_point")
                        {
                            List<int> id = repo.SELECT<int>("id", "Region_LP", "Region_idR = " + idOB + " AND Local_point_idLP = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                repo.INSERT("Region_LP", new ArrayList { "Region_idR", "Local_point_idLP" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Local_point" && Reference[0].Type == "Region")
                        {
                            List<int> id = repo.SELECT<int>("id", "Region_LP", "Local_point_idLP = " + idOB + " AND Region_idR = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                repo.INSERT("Region_LP", new ArrayList { "Local_point_idLP", "Region_idR" }, new ArrayList { idOB, Reference[0].id });
                            }
                        }

                        if (GFeature.Type == "Administrative_unit" && Reference[0].Type == "Country")
                        {
                            List<int> id = repo.SELECT<int>("id", "Administrative_unit", "id = " + idOB + " AND Country_idC = " + Reference[0].id);

                            if (id.Count == 0)
                            {
                                repo.UPDATE("Administrative_unit", "Country_idC = " + Reference[0].id, "id = " + idOB);
                            }
                        }
                    }
                    else
                    {
                        NewError("Not reference for " + GFeature.Name + " and " + GFeature.Reference[i] + "; Line #" + number);
                    }
                }
            }
        }
        private void InsertCoordinates(List<string> Coordinates, int idDI)
        {
            for (int i = 0; i < Coordinates.Count; i++)
            {
                repo.INSERT("Coordinates", new ArrayList { "СoordinatesPoint", "Counter", "DetailsInformation_id" }, new ArrayList { "geography::STGeomFromText('LINESTRING(" + Coordinates[i] + ")', 4326)", i + 1, idDI });
            }
        }
    }
}
