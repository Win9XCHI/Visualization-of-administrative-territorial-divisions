using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Map;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Handler.Models.Repositories {
    public class VisualizationRepository : DBRepository, IVisualizationRepository
    {
        public VisualizationRepository(string conn) : base(conn) { }

        private string Exceptions(string Name, string[] ListExceptions)
        {
            string Exceptions = "";

            if (ListExceptions.Length != 0)
            {
                Exceptions += " AND (";

                for (int i = 0; i < ListExceptions.Length; i++)
                {
                    Exceptions += Name + ".Name NOT LIKE '%" + ListExceptions[i] + "%' AND " + Name + ".Name NOT LIKE '" + ListExceptions[i] + "%' AND " + Name + ".Name NOT LIKE '%" + ListExceptions[i] + "'";

                    if (i != ListExceptions.Length - 1) {

                        Exceptions += " AND ";
                    }
                }
                Exceptions += ")";
            }

            return Exceptions;
        }
        public List<InfoMaps> GetInformation(string Name, string Year, string[] ListExceptions)
        {
            

            return SELECT<InfoMaps>("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, " +
                            Name + ".Name, " + Name + ".Information, DetailsInformation.Year, Сoordinates.Counter, Сoordinates.СoordinatesPoint.Lat AS Lat, Сoordinates.СoordinatesPoint.Long AS Long",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN DetailsInformation ON DetailsInformation.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.DetailsInformation_id = DetailsInformation.id)",
                            "DetailsInformation.Year = " + Year + Exceptions(Name, ListExceptions));

        }

        /*public List<SqlGeography> GetCoordinates(string Name, string Year, string[] ListExceptions)
        {

            return SELECT<SqlGeography>("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, " +
                            "Сoordinates.СoordinatesPoint",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN DetailsInformation ON DetailsInformation.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.DetailsInformation_id = DetailsInformation.id)",
                            "DetailsInformation.Year = " + Year + Exceptions(Name, ListExceptions));

        }*/
    }
}
