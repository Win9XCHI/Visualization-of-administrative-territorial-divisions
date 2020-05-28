
using System.Collections.Generic;

namespace Handler.Models.Search
{
    public class FormSearch
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public IEnumerable<ModelOptions> Options { set; get; } = new List<ModelOptions>
        {
            new ModelOptions {Id = 1, OptionsName = "Пошук за роком"},
            new ModelOptions {Id = 2, OptionsName = "Пошук джерел вказаного об'єкта"},
            new ModelOptions {Id = 3, OptionsName = "Хронологічна інформація про об'єкт"}
        };
        public int SelectedOption { set; get; }
    }
}