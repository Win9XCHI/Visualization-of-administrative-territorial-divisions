using System.Collections.Generic;
using System.Collections;
using System.IO;
using Handler.Models.SoursePanel;
using Handler.Models.Repositories.Interfaces;

namespace Handler.Algorithms
{
    public class SourseAnalysis
    {
        private ISoursePanelRepository repo;
        private Sourse ObS;
        public int Status;
        public SourseAnalysis(ISoursePanelRepository r, Sourse Ob)
        {
            repo = r;
            ObS = Ob;
            Status = 0;
        }

        public void Start()
        {
            ObS.ConvertToByte();
            ObS.id = repo.AddNewSourse(ObS);

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
        private void ProcessMap(string str, int number)
        {
            ClassGeographicFeature GFeature = new ClassGeographicFeature();
            GFeature.SetForGeo(str);
            
            if (!GFeature.CheckCompleteness()) {
                repo.AddNewError("Not completeness data; Line #" + number, ObS.id);
                return;
            }

            if (repo.CheckNameObject(GFeature.Name))
            {
                repo.OldObject(GFeature, repo.GetObject(GFeature.Name), number, ObS.id);
            }
            else
            {
                repo.NewObject(GFeature, number, ObS.id);
            }
        }
        private void ProcessText(string str, int number)
        {
            ClassGeographicFeature GFeature = new ClassGeographicFeature();
            GFeature.SetForText(str);

            if (!GFeature.CheckCompleteness())
            {
                repo.AddNewError("Not completeness data; Line #" + number, ObS.id);
                return;
            }

            if (repo.CheckNameObject(GFeature.Name))
            {
                repo.OldObject(GFeature, repo.GetObject(GFeature.Name), number, ObS.id);
            }
            else
            {
                repo.NewObject(GFeature, number, ObS.id);
            }
        }
        
    }
}
