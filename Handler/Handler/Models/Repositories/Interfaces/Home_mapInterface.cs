using Handler.Models.Map;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Handler.Models.Repositories.Interfaces
{
    public interface IHome_mapRepository
    {
        List<InfoMaps> GetInformation(string Name, string Year);

        List<SqlGeography> GetCoordinates(string Name, string Year);
    }
}

