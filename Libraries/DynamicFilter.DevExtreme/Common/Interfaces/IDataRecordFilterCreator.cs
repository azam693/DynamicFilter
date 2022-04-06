using ServiceCalculator.Web.Models;

namespace DynamicFilter.DevExtreme.Common.Interfaces
{
    public interface IDataRecordFilterCreator
    {
        DataRecordFilter Create(object filter);
    }
}
