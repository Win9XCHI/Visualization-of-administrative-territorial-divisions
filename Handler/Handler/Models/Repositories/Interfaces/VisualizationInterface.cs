using Handler.Models.Map;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace Handler.Models.Repositories.Interfaces
{
    public interface IVisualizationRepository
    {
        List<InfoMaps> GetInformation(string Name, string Year, string[] ListExceptions);

        //List<SqlGeography> GetCoordinates(string Name, string Year, string[] ListExceptions);
    }
}

