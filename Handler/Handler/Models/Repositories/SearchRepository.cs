using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Search;
using System.Collections.Generic;

namespace Handler.Models.Repositories {
    public class SearchRepository : DBRepository, ISearchRepository
    {
        public SearchRepository(string conn) : base(conn) { }

        private List<ResponseSearch> GetGeneralInformation(string Name)
        {
            return SELECT<ResponseSearch>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name LIKE '%" + Name + "%' OR Local_point.Name = '%" + Name + "' OR Local_point.Name = '" + Name + "%' OR Local_point.Name = '" + Name + "' " +
                "UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '%" + Name + "%'  OR Region.Name = '%" + Name + "' OR Region.Name = '" + Name + "%' OR Region.Name = '" + Name + "' " +
                "UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '%" + Name + "%' OR Administrative_unit.Name = '%" + Name + "' OR Administrative_unit.Name = '" + Name + "%' OR Administrative_unit.Name = '" + Name + "' " +
                "UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id) WHERE Country.Name = '%" + Name + "%' OR Country.Name = '%" + Name + "' OR Country.Name = '" + Name + "%' OR Country.Name = '" + Name + "'");

        }

        private List<Reference> GetReference(string Type, string TypeTwo, int id, string tableMidle = "")
        {
            if (tableMidle.Length == 0)
            {
                return SELECT<Reference>(Type + ".Name, " + Type + ".id AS ID",
                                    Type + " JOIN " + TypeTwo + " ON " + TypeTwo + "." + Type + "_id = " + Type + ".id JOIN Midle ON (Midle.id =  " + TypeTwo + ".Midle_id)",
                                    "Midle.id = " + id);
            }
            return SELECT<Reference>(Type + ".Name, " + Type + ".id AS ID",
                                Type + " JOIN " + tableMidle + " ON " + tableMidle + "." + Type + "_id = " + Type + ".id " +
                                "JOIN " + TypeTwo + " ON " + TypeTwo + ".id = " + tableMidle + "." + TypeTwo + "_id JOIN Midle ON(Midle.id = " + TypeTwo + ".Midle_id)",
                                "Midle.id = " + id);
        }

        private List<RecordTableSearch> GetRecords(string columns, string table_name, string where, string limit, string order)
        {
            return SELECT<RecordTableSearch>(columns, table_name, where, limit, order);
        }

        private List<ResponseSearch> Search(string Name, string columns, string table_name, string where, string limit = "", string order = "")
        {
            List<ResponseSearch> GeneralInfo = GetGeneralInformation(Name);
            if (GeneralInfo.Count == 0)
            {
                throw new System.Exception("Not Found");
            }

            for (int i = 0; i < GeneralInfo.Count; i++)
            {
                switch (GeneralInfo[i].Type)
                {
                    case "Local_point":
                        {
                            GeneralInfo[i].ReferenceOut = GetReference("Region", "Local_point", GeneralInfo[0].id, "Region_LP");
                            break;
                        }
                    case "Region":
                        {
                            GeneralInfo[i].ReferenceIn = GetReference("Local_point", "Region", GeneralInfo[0].id, "Region_LP");

                            GeneralInfo[i].ReferenceOut = GetReference("Administrative_unit", "Region", GeneralInfo[0].id, "Region_AU");
                            break;
                        }
                    case "Administrative unit":
                        {
                            GeneralInfo[i].ReferenceIn = GetReference("Region", "Administrative_unit", GeneralInfo[0].id, "Region_AU");

                            GeneralInfo[i].ReferenceOut = GetReference("Country", "Administrative_unit", GeneralInfo[0].id);

                            break;
                        }
                    case "Country":
                        {
                            GeneralInfo[i].ReferenceIn = GetReference("Administrative_unit", "Country", GeneralInfo[0].id);
                            break;
                        }
                }

                GeneralInfo[i].ListRecords = GetRecords(columns, table_name, where + GeneralInfo[i].id, limit, order);
            }

            return GeneralInfo;
        }

        public List<ResponseSearch> SearchYearView(FormSearch search)
        {
            return Search(search.Name, "Part.Information, DetailsInformation.Year",
                "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "YEAR(DetailsInformation.Year) = '" + search.Year + "' AND Midle.id = ");
        }

        public List<ResponseSearch> SearchChronologyView(FormSearch search)
        {
            return Search(search.Name, "Part.Information, DetailsInformation.Year",
                "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "Midle.id = ", "", "DetailsInformation.Year");
        }

        public List<ResponseSearch> SearchSourseView(FormSearch search)
        {
            return Search(search.Name, "Sourse.Name AS Information",
                "Sourse JOIN Part ON (Part.Sourse_idSourse = Sourse.idSourse) JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "YEAR(DetailsInformation.Year) = '" + search.Year + "' AND Midle.id = ");
        }
    }
}
