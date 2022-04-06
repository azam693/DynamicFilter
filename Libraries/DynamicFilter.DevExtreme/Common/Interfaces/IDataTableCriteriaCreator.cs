using DynamicFilter.Core.Models;

namespace DynamicFilter.DevExtreme.Common.Interfaces
{
    public interface IDataTableCriteriaCreator
    {
        CriteriaInfo Create(object criteria);
    }
}
