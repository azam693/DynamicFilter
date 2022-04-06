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
    public class TreeSortBuilder_Test
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
        public void SortWithoutExpressionParameterAsc_Test1()
        {
            var sort = new SortInfo();
            sort.AddField(nameof(FilterClass1.Age), SortTypeEnum.Asc);
            var sortBuilder = new TreeSortBuilder(_filterClass1Type);
            var expression = (Expression)sortBuilder.Build(sort);

            Assert.AreEqual("data.OrderBy(entity => entity.Age)", expression.ToString());
        }

        [TestMethod]
        public void SortWithoutExpressionParameterDesc_Test1()
        {
            var sort = new SortInfo();
            sort.AddField(nameof(FilterClass1.Age), SortTypeEnum.Desc);
            var sortBuilder = new TreeSortBuilder(_filterClass1Type);
            var expression = (Expression)sortBuilder.Build(sort);

            Assert.AreEqual("data.OrderByDescending(entity => entity.Age)", expression.ToString());
        }

        [TestMethod]
        public void SortWithoutExpressionParameterMany_Test1()
        {
            var sort = new SortInfo();
            sort.AddField(nameof(FilterClass1.Age), SortTypeEnum.Asc);
            sort.AddField(nameof(FilterClass1.Name), SortTypeEnum.Desc);
            var sortBuilder = new TreeSortBuilder(_filterClass1Type);
            var expression = (Expression)sortBuilder.Build(sort);

            Assert.AreEqual("data.OrderBy(entity => entity.Age).ThenByDescending(entity => entity.Name)", expression.ToString());
        }
        
        [TestMethod]
        public void ImplSortWithExpressionParameterAsc_Test1()
        {
            var sort = new SortInfo();
            sort.AddField(nameof(FilterClass1.Age), SortTypeEnum.Asc);
            var sortBuilder = new TreeSortBuilder(_filterClass1Type, _filterRecords1.AsQueryable().Expression);
            var expression = (Expression)sortBuilder.Build(sort);

            var values = _filterRecords1
                .AsQueryable()
                .Provider
                .CreateQuery<FilterClass1>(expression)
                .ToList();

            Assert.AreEqual(values[0].Age, 38);
            Assert.AreEqual(values[1].Age, 45);
            Assert.AreEqual(values[2].Age, 56);
        }
    }
}
