using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Map;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Handler.Models.Repositories {
    public class Home_mapRepository : DBRepository, IHome_mapRepository
    {
        public Home_mapRepository(string conn) : base(conn) { }

        public List<InfoMaps> GetInformation(string Name, string Year)
        {
            return SELECT<InfoMaps>("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, " +
                            Name + ".Name, " + Name + ".Information, Years.Year_first, Years.Year_second, " +
                            "Сoordinates.Counter",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN Years ON Years.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.Years_id = Years.id)",
                            "Years.Year_first < " + Year + " AND (Years.Year_second > " + Year + " OR Years.Year_second IS NULL)");

        }

        public List<SqlGeography> GetCoordinates(string Name, string Year)
        {
            return SELECT<SqlGeography>
                            ("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, Сoordinates.СoordinatesPoint",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN Years ON Years.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.Years_id = Years.id)",
                            "Years.Year_first < " + Year + " AND (Years.Year_second > " + Year + " OR Years.Year_second IS NULL)");

        }
    }
}
