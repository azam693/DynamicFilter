
using DynamicFilter.Core.Models;

namespace ServiceCalculator.Web.Models
{
    public class DataRecordFilter
    {
        public CriteriaInfo Criteria { get; protected set; }
        public SortInfo Sort { get; protected set; }
        public int SkipRows { get; protected set; }
        public int TakeRows { get; protected set; }

        public DataRecordFilter(
            CriteriaInfo criteria = null,
            SortInfo sort = null,
            int skipRows = 0, 
            int takeRows = 40)
        {
            Criteria = criteria;
            Sort = sort;
            SkipRows = skipRows;
            TakeRows = takeRows;
        }
    }
}
