using Handler.Models.Search;
using System.Collections.Generic;

namespace Handler.Models.Repositories.Interfaces
{
    public interface ISearchRepository
    {
        List<ResponseSearch> SearchYearView(FormSearch search);
        List<ResponseSearch> SearchChronologyView(FormSearch search);
        List<ResponseSearch> SearchSourseView(FormSearch search);
    }
}

