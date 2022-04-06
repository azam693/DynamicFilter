using DynamicFilter.Core.Builders;
using DynamicFilter.Core.Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicFilter.Core.Tests.Builder
{
    [TestClass]
    public class CriteriaBuilder_Test
    {
        [TestMethod]
        public void BuildAnd_Test1()
        {
            var criteria = CriteriaBuilder.Aggregate.And(
                CriteriaBuilder.Binary
                    .Equal("Name", "Test1")
                    .Contains("LastName", "ov"));

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.AND);

            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Name, "Name");
            Assert.AreEqual(criteria.Criterias[0].Value.ToString(), "Test1");

            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.Contains);
            Assert.AreEqual(criteria.Criterias[1].Name, "LastName");
            Assert.AreEqual(criteria.Criterias[1].Value.ToString(), "ov");
        }

        [TestMethod]
        public void BuildOr_Test1()
        {
            var criteria = CriteriaBuilder.Aggregate.Or(
                CriteriaBuilder.Binary
                    .Equal("Name", "Test1")
                    .IsNotNull("LastName"));

            Assert.AreEqual(criteria.Comparison, ComparisonEnum.OR);

            Assert.AreEqual(criteria.Criterias[0].Comparison, ComparisonEnum.Equal);
            Assert.AreEqual(criteria.Criterias[0].Name, "Name");
            Assert.AreEqual(criteria.Criterias[0].Value.ToString(), "Test1");

            Assert.AreEqual(criteria.Criterias[1].Comparison, ComparisonEnum.IsNotNull);
            Assert.AreEqual(criteria.Criterias[1].Name, "LastName");
            Assert.AreEqual(criteria.Criterias[1].Value, null);
        }
    }
}
