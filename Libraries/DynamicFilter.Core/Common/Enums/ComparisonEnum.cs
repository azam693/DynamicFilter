namespace DynamicFilter.Core.Common.Enums;

/// <summary>
/// Type of compare for Criteria
/// </summary>
public enum ComparisonEnum
{
    NOT,
    AND,
    OR,
    IN,
    Equal,
    NotEqual,
    Contains,
    NotContains,
    StartsWith,
    EndsWith,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual,
    IsNull,
    IsNotNull
}