using DevExtreme.AspNet.Data;
using DynamicFilter.DevExtreme.Common.Interfaces;
using ServiceCalculator.Web.Models;

namespace DynamicFilter.DevExtreme.Creators
{
    public class DxDataRecordFilterCreator : IDataRecordFilterCreator
    {
        private IDataTableCriteriaCreator _dataTableCriteriaCreator;
        private IDataTableSortCreator _dataTableSortCreator;

        public DxDataRecordFilterCreator(
            IDataTableCriteriaCreator dataTableCriteriaCreator,
            IDataTableSortCreator dataTableSortCreator)
        {
            _dataTableCriteriaCreator = dataTableCriteriaCreator;
            _dataTableSortCreator = dataTableSortCreator;
        }
        
        public DataRecordFilter Create(object filter)
        {
            var loadOptions = (DataSourceLoadOptionsBase)filter;
            return new DataRecordFilter(
                criteria: loadOptions.Filter != null && loadOptions.Filter.Count > 0
                    ? _dataTableCriteriaCreator.Create(loadOptions.Filter) : null,
                sort: loadOptions.Sort != null && loadOptions.Sort.Length > 0 
                    ? _dataTableSortCreator.Create(loadOptions.Sort) : null,
                skipRows: loadOptions.Skip,
                takeRows: loadOptions.Take);
        }
    }
}
