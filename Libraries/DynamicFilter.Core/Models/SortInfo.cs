using DynamicFilter.Core.Common.Enums;

namespace DynamicFilter.Core.Models
{
    public class SortInfo
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private Dictionary<string, SortTypeEnum> _fields = new Dictionary<string, SortTypeEnum>();

        public IReadOnlyDictionary<string, SortTypeEnum> Fields => _fields;

        public void AddField(string name, SortTypeEnum sortType)
        {
            if (!_fields.ContainsKey(name))
                _fields.Add(name, sortType);
        }

        public void AddParameter(string name, object value)
        {
            if (!_parameters.ContainsKey(name))
                _parameters.Add(name, value);
        }

        public bool ContainsParameter(string name)
        {
            return _parameters.ContainsKey(name);
        }

        public T GetParameter<T>(string name)
        {
            return (T)_parameters[name];
        }
    }
}
