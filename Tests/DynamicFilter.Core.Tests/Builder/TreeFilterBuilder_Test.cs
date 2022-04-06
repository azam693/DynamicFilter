using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DynamicFilter.Core.Builders;
using DynamicFilter.Core.Common.Enums;
using DynamicFilter.Core.Models;

namespace DynamicFilter.Core.Tests.Builder
{
    [TestClass]
    public class TreeFilterBuilder_Test
    {
        #region Models 
        public class FilterClass1
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        #endregion

        private Type _filterClass1Type;
        private List<FilterClass1> _filterRecords1;

        [TestInitialize]
        public void Init()
        {
            _filterClass1Type = typeof(FilterClass1);

            _filterRecords1 = new List<FilterClass1>
            {
                new FilterClass1 { Name = "Test1", Age = 45 },
                new FilterClass1 { Name = "Test2", Age = 56 },
                new FilterClass1 { Name = "Test3", Age = 38 },
            };
        }

        [TestMethod]
        public void Equals_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .Equal(nameof(FilterClass1.Name), "Test1"));

            Assert.AreEqual("entity => (entity.Name == \"Test1\")", expression.ToString());
        }

        [TestMethod]
        public void Equals_Test2()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .Equal(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age == 45)", expression.ToString());
        }

        [TestMethod]
        public void NotEqual_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .NotEqual(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age != 45)", expression.ToString());
        }

        [TestMethod]
        public void GreaterThan_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .GreaterThan(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age > 45)", expression.ToString());
        }

        [TestMethod]
        public void GreaterThanOrEqual_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .GreaterThanOrEqual(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age >= 45)", expression.ToString());
        }

        [TestMethod]
        public void LessThan_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .LessThan(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age < 45)", expression.ToString());
        }
        
        [TestMethod]
        public void LessThanOrEqual_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .LessThanOrEqual(nameof(FilterClass1.Age), 45));

            Assert.AreEqual("entity => (entity.Age <= 45)", expression.ToString());
        }

        [TestMethod]
        public void Contains_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .Contains(nameof(FilterClass1.Name), "es"));

            Assert.AreEqual("entity => entity.Name.Contains(\"es\")", expression.ToString());
        }

        [TestMethod]
        public void NotContains_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .NotContains(nameof(FilterClass1.Name), "es"));

            Assert.AreEqual("entity => Not(entity.Name.Contains(\"es\"))", expression.ToString());
        }
                                                   
        [TestMethod]
        public void StartsWith_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .StartsWith(nameof(FilterClass1.Name), "es"));

            Assert.AreEqual("entity => entity.Name.StartsWith(\"es\")", expression.ToString());
        }

        [TestMethod]
        public void EndsWith_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .EndsWith(nameof(FilterClass1.Name), "es"));

            Assert.AreEqual("entity => entity.Name.EndsWith(\"es\")", expression.ToString());
        }

        [TestMethod]
        public void IsNull_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .IsNull(nameof(FilterClass1.Name)));

            Assert.AreEqual("entity => (entity.Name == null)", expression.ToString());
        }

        [TestMethod]
        public void IsNotNull_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .IsNotNull(nameof(FilterClass1.Name)));

            Assert.AreEqual("entity => (entity.Name != null)", expression.ToString());
        }

        [TestMethod]
        public void In_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .In(nameof(FilterClass1.Age), new object[] { 45, 56 }));

            Assert.AreEqual("entity => ((entity.Age == 45) Or (entity.Age == 56))", expression.ToString());
        }

        [TestMethod]
        public void And_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Aggregate.And(
                    CriteriaBuilder.Binary
                        .Equal(nameof(FilterClass1.Name), "Test1")
                        .GreaterThanOrEqual(nameof(FilterClass1.Age), 45)));

            Assert.AreEqual("entity => ((entity.Name == \"Test1\") And (entity.Age >= 45))", expression.ToString());
        }

        [TestMethod]
        public void Or_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Aggregate.Or(
                    CriteriaBuilder.Binary
                        .Equal(nameof(FilterClass1.Name), "Test1")
                        .GreaterThanOrEqual(nameof(FilterClass1.Age), 45)));

            Assert.AreEqual("entity => ((entity.Name == \"Test1\") Or (entity.Age >= 45))", expression.ToString());
        }

        [TestMethod]
        public void Not_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Aggregate.Not(
                    CriteriaBuilder.Binary
                        .Equal(nameof(FilterClass1.Name), "Test1")));

            Assert.AreEqual("entity => Not((entity.Name == \"Test1\"))", expression.ToString());
        }

        [TestMethod]
        public void Not_Test2()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Aggregate.Not(
                    CriteriaBuilder.Binary
                        .Equal(nameof(FilterClass1.Name), "Test1")
                        .GreaterThan(nameof(FilterClass1.Age), 40)));

            Assert.AreEqual("entity => Not(((entity.Name == \"Test1\") And (entity.Age > 40)))", expression.ToString());
        }

        [TestMethod]
        public void Complex_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Aggregate.Or(
                    CriteriaBuilder.Aggregate.And(
                        CriteriaBuilder.Binary
                            .Equal(nameof(FilterClass1.Name), "Test1")
                            .GreaterThanOrEqual(nameof(FilterClass1.Age), 45)),
                    CriteriaBuilder.Aggregate.And(
                        CriteriaBuilder.Binary
                            .Equal(nameof(FilterClass1.Name), "Test2")
                            .GreaterThanOrEqual(nameof(FilterClass1.Age), 50))
                ));

            Assert.AreEqual(
                "entity => (((entity.Name == \"Test1\") And (entity.Age >= 45)) Or ((entity.Name == \"Test2\") And (entity.Age >= 50)))", 
                expression.ToString());
        }

        [TestMethod]
        public void ImplFilterOr_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var criteria = CriteriaBuilder.Aggregate.Or(
                CriteriaBuilder.Binary
                    .Equal(nameof(FilterClass1.Age), 56)
                    .Equal(nameof(FilterClass1.Age), 45));
            var expression = (Expression<Func<FilterClass1, bool>>)filterBuilder.Build(criteria);

            var values = _filterRecords1
                .AsQueryable()
                .Where(expression)
                .ToList();
        }

        [TestMethod]
        public void ConditionWithDifferentType_Test1()
        {
            var filterBuilder = new TreeFilterBuilder(_filterClass1Type);
            var expression = filterBuilder.Build(
                CriteriaBuilder.Binary
                    .Equal(nameof(FilterClass1.Age), (long)45));

            Assert.AreEqual("entity => (entity.Age == 45)", expression.ToString());
        }
    }
}
