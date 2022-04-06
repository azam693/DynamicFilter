using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using DynamicFilter.Core.Common.Extensions;

namespace DynamicFilter.Core.Helpers
{
    public class TreeBuilderHelper
    {
        public static bool TryConvertValue(ref object value, Type type)
        {
            try
            {
                value = ConvertValue(value, type);
            }
            catch 
            {
                return false;
            }

            return true;
        }

        private static object ConvertValue(object value, Type type)
        {
            value = UnwrapNewtonsoftValue(value);

            if (value == null || type == null)
                return value;

            if (type.IsAssignableFrom(value.GetType()))
                return value;

            type = type.GetTypeUnderNullable();
            var valueType = value.GetType();
            // If value type equals property (field) type then return
            if (valueType == type) return value;

            if (type == typeof(DateTime) && value is string)
                return DateTime.Parse((string)value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

            var converter = TypeDescriptor.GetConverter(type);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            if (type.GetTypeInfo().IsEnum)
                return Enum.Parse(type, Convert.ToString(value), true);

            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        public static object UnwrapNewtonsoftValue(object value)
        {
            if (value is JValue jValue)
                return jValue.Value;

            return value;
        }
    }
}
