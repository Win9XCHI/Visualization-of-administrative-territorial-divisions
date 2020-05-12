using Handler.Models.SoursePanel;
using System.Collections.Generic;

namespace Handler.Models.Repositories.Interfaces
{
    public interface ISoursePanelRepository
    {
        List<Sourse> GetSources(Sourse SourseOb);
        void DeleteSourse(int id);
        void UpdateSourse(Sourse ObS);
        int AddNewSourse(Sourse ObS);
        void AddNewError(string str);
        bool CheckNameObject(string Name);
        ReturnOb GetObject(string Name);
        void NewObject(ClassGeographicFeature GFeature, int number, int id);
        void OldObject(ClassGeographicFeature GFeature, ReturnOb CheckName, int number, int id);
    }
}

