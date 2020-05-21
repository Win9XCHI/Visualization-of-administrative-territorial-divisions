
using System.Collections.Generic;

namespace Handler.Models.Search
{
    public class FormSearch
    {
        public int Year { get; set; }
        public string Name { get; set; }
        public IEnumerable<ModelOptions> Options { set; get; } = new List<ModelOptions>
        {
            new ModelOptions {Id = 1, OptionsName = "Year"},
            new ModelOptions {Id = 2, OptionsName = "Sourse"},
            new ModelOptions {Id = 3, OptionsName = "Chrono"}
        };
        public int SelectedOption { set; get; }
    }
}