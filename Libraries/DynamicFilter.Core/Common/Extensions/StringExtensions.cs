using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicFilter.Core.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNothing(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
    }
}
