using DynamicFilter.Core.Common.Enums;
using DynamicFilter.DevExtreme.Creators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections;

namespace DynamicFilter.DevExtreme.Tests.Creators
{
    [TestClass]
    public class DxDataTableCriteriaCreator_Test
    {
        class DataItem1
        {
            public int IntProp { get; set; }
            public string StringProp { get; set; }
            public int? NullableProp { get; set; }
            public DateTime Date { get; set; }
        }

        [TestMethod]
        public void ImplicitEquals()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new object[] { "IntProp", 123 });

            Assert.AreEqual(criteria.Name, "IntProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Value, 123);
        }

        [TestMethod]
        public void ExplicitEquals()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new object[] { "IntProp", "=", 1225 });

            Assert.AreEqual(criteria.Name, "IntProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Value, 1225);
        }

        [TestMethod]
        public void ComparisonOperations()
        {
            var filterCreator = new DxDataTableCriteriaCreator();

            var criteriaNotEqual = filterCreator.Create(new object[] { "IntProp", "<>", 1 });
            Assert.AreEqual(criteriaNotEqual.Name, "IntProp");
            Assert.AreEqual(criteriaNotEqual.Comparison, ComparisonEnum.NotEqual);
            Assert.AreEqual(criteriaNotEqual.Value, 1);

            var criteriaGreaterThan = filterCreator.Create(new object[] { "IntProp", ">", 1 });
            Assert.AreEqual(criteriaGreaterThan.Name, "IntProp");
            Assert.AreEqual(criteriaGreaterThan.Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteriaGreaterThan.Value, 1);

            var criteriaLessThan = filterCreator.Create(new object[] { "IntProp", "<", 1 });
            Assert.AreEqual(criteriaLessThan.Name, "IntProp");
            Assert.AreEqual(criteriaLessThan.Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteriaLessThan.Value, 1);

            var criteriaGreaterThanOrEqual = filterCreator.Create(new object[] { "IntProp", ">=", 1 });
            Assert.AreEqual(criteriaGreaterThanOrEqual.Name, "IntProp");
            Assert.AreEqual(criteriaGreaterThanOrEqual.Comparison, ComparisonEnum.GreaterThanOrEqual);
            Assert.AreEqual(criteriaGreaterThanOrEqual.Value, 1);

            var criteriaLessThanOrEqual = filterCreator.Create(new object[] { "IntProp", "<=", 1 });
            Assert.AreEqual(criteriaLessThanOrEqual.Name, "IntProp");
            Assert.AreEqual(criteriaLessThanOrEqual.Comparison, ComparisonEnum.LessThanOrEqual);
            Assert.AreEqual(criteriaLessThanOrEqual.Value, 1);
        }

        [TestMethod]
        public void StringContains()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new[] { "StringProp", "contains", "Abc" });

            Assert.AreEqual(criteria.Name, "StringProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.Contains);
            Assert.AreEqual(criteria.Value, "Abc");
        }

        [TestMethod]
        public void StringNotContains()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new[] { "StringProp", "notContains", "Abc" });

            Assert.AreEqual(criteria.Name, "StringProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.NotContains);
            Assert.AreEqual(criteria.Value, "Abc");
        }

        [TestMethod]
        public void StartsWith()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new[] { "StringProp", "startsWith", "Prefix" });

            Assert.AreEqual(criteria.Name, "StringProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.StartsWith);
            Assert.AreEqual(criteria.Value, "Prefix");
        }

        [TestMethod]
        public void EndsWith()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new[] { "StringProp", "endsWith", "Postfix" });

            Assert.AreEqual(criteria.Name, "StringProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.EndsWith);
            Assert.AreEqual(criteria.Value, "Postfix");
        }

        [TestMethod]
        public void StringFunctionOnNonStringData()
        {
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(new[] { "IntProp", "contains", "Abc" });

            Assert.AreEqual(criteria.Name, "IntProp");
            Assert.AreEqual(criteria.Comparison, ComparisonEnum.Contains);
            Assert.AreEqual(criteria.Value, "Abc");
        }

        [TestMethod]
        public void ImplicitAndOfTwo()
        {
            var crit = new[] {
                new object[] { "IntProp", ">", 1 },
                new object[] { "IntProp", "<", 10 }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteria.Criterias[1].Value, 10);
        }

        [TestMethod]
        public void ExplicitAndOfTwo()
        {
            var crit = new object[] {
                new object[] { "IntProp", ">", 1 },
                "and",
                new object[] { "IntProp", "<", 10 }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteria.Criterias[1].Value, 10);
        }

        [TestMethod]
        public void OrOfTwo()
        {
            var crit = new object[] {
                new object[] { "IntProp", 1 },
                "or",
                new object[] { "IntProp", 2 }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.OR);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[1].Value, 2);
        }

        [TestMethod]
        public void Not()
        {
            var crit = new object[] {
                "!",
                new object[] {
                    new object[] { "IntProp", ">", 1 },
                    "and",
                    new object[] { "IntProp", "<", 10 }
                }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.NOT);
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[0].Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Criterias[0].Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteria.Criterias[0].Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[0].Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Criterias[1].Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteria.Criterias[0].Criterias[1].Value, 10);
        }

        [TestMethod]
        public void GroupOfMany()
        {
            var crit = new object[] {
                new object[] { "IntProp", ">", 1 },
                new object[] { "IntProp", "<", 10 },
                "and",
                new[] { "StringProp", "<>", "abc" }

            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteria.Criterias[1].Value, 10);
            Assert.AreEqual(criteria.Criterias[2].Name, "StringProp");
            Assert.AreEqual(criteria.Criterias[2].Comparison, ComparisonEnum.NotEqual);
            Assert.AreEqual(criteria.Criterias[2].Value, "abc");
        }

        [TestMethod]
        public void NestedGroups()
        {
            var crit = new object[] {
                new object[] { "IntProp", 1 },
                "||",
                new[] {
                    new object[] { "IntProp", ">", 1 },
                    new object[] { "IntProp", "<", 10 }
                }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.OR);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[1].Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Criterias[0].Comparison, ComparisonEnum.GreaterThan);
            Assert.AreEqual(criteria.Criterias[1].Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Criterias[1].Comparison, ComparisonEnum.LessThan);
            Assert.AreEqual(criteria.Criterias[1].Criterias[1].Value, 10);
        }

        [TestMethod]
        public void MixedGroupOperatorsWithoutBrackets()
        {
            var crit = new object[] {
                new object[] { "IntProp", ">", 1 },
                new object[] { "IntProp", "<", 10 },
                "||",
                new object[] { "IntProp", "=", 100 },
            };
            var filterCreator = new DxDataTableCriteriaCreator();

            Assert.ThrowsException<ArgumentException>(() => filterCreator.Create(crit), "Mixing of and/or is not allowed inside a single group");
        }

        [TestMethod]
        public void MultipleOrRegression()
        {
            var crit = new object[] {
                new object[] { "IntProp", 1 },
                "or",
                new object[] { "IntProp", 2 },
                "or",
                new object[] { "IntProp", 3 }
            };
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.OR);
            Assert.AreEqual(criteria.Criterias[0].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Value, 1);
            Assert.AreEqual(criteria.Criterias[1].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[1].Value, 2);
            Assert.AreEqual(criteria.Criterias[2].Name, "IntProp");
            Assert.AreEqual(criteria.Criterias[2].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[2].Value, 3);
        }

        [TestMethod]
        public void JsonObjects()
        {
            var crit = (IList)JsonConvert.DeserializeObject(@"[ [ ""StringProp"", ""abc"" ], [ ""NullableProp"", null ] ]");
            var filterCreator = new DxDataTableCriteriaCreator();
            var criteria = filterCreator.Create(crit);

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.AND);
            Assert.AreEqual(criteria.Criterias[0].Name, "StringProp");
            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Value, "abc");
            Assert.AreEqual(criteria.Criterias[1].Name, "NullableProp");
            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[1].Value, null);
        }
    }
}
