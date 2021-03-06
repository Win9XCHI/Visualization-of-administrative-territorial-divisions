﻿using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.UserPanel;
using System.Collections.Generic;

namespace Handler.Models.Repositories {
    public class UserPanelRepository : DBRepository, IUserPanelRepository
    {
        public UserPanelRepository(string conn) : base(conn) { }

        public List<User> GetUsers(User U)
        {
            string where = "";

            if (U.PIB != null)
            {
                where += "PIB = '" + U.PIB + "'";
            }

            if (U.Phone != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Phone = '" + U.Phone + "'";
            }

            if (U.Login != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Login = '" + U.Login + "'";
            }

            if (U.Birthday != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Birthday = '" + U.Birthday + "'";
            }

            if (U.Rights != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }
                where += "Rights = '" + U.Rights + "'";
            }

            return SELECT<User>("*", "Input", where);
        }

        public void DeleteUser(int code)
        {
            List<int> M_IDs = SELECT<int>("Local_point.Midle_id",
                "Local_point JOIN Sourse_LocalPoint ON (Local_point.id = Sourse_LocalPoint.Local_point_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_LocalPoint.Sourse_idSourse) WHERE Sourse.idSourse = " + code +
"UNION SELECT Region.Midle_id FROM Region JOIN Sourse_Region ON(Region.id = Sourse_Region.Region_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Region.Sourse_idSourse) WHERE Sourse.idSourse = " + code +
"UNION SELECT Administrative_unit.Midle_id FROM Administrative_unit JOIN Sourse_Administrative_unit ON(Administrative_unit.id = Sourse_Administrative_unit.Administrative_unit_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Administrative_unit.Sourse_idSourse) WHERE Sourse.idSourse = " + code +
"UNION SELECT Country.Midle_id FROM Country JOIN Sourse_Country ON(Country.id = Sourse_Country.Country_id) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Country.Sourse_idSourse)",
                "Sourse.Input_Code = " + code);

            for (int i = 0; i < M_IDs.Count; i++)
            {
                DELETE("Region", "Midle_id = " + M_IDs[i]);
                DELETE("Local_point", "Midle_id = " + M_IDs[i]);
                DELETE("Administrative_unit", "Midle_id = " + M_IDs[i]);
                DELETE("Midle", "id = " + M_IDs[i]);
            }

            DELETE("Input", "Code = " + code);
        }

        public void UpdateUser(User U)
        {
            UPDATE("Input",
                "PIB = '" + U.PIB + "', Phone = '" + U.Phone + "', Birthday = '" + U.Birthday + "', Login = '" + U.Login + "', Rights = '" + U.Rights + "'", 
                "Code = " + U.Code);
        }
    }
}
