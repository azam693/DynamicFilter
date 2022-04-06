using DevExtreme.AspNet.Data;
using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Models;
using DynamicFilter.DevExtreme.Common.Interfaces;

namespace DynamicFilter.DevExtreme.Creators
{
    public class DxDataTableSortCreator : IDataTableSortCreator
    {
        public SortInfo Create(object sort)
        {
            var sortResult = new SortInfo();
            var dxSorts = (IEnumerable<SortingInfo>)sort;
            foreach (var dxSort in dxSorts)
            {
                sortResult.AddField(
                    name: dxSort.Selector, 
                    sortType: dxSort.Desc 
                        ? SortTypeEnum.Desc 
                        : SortTypeEnum.Asc);
            }

            return sortResult;
        }
    }
}
