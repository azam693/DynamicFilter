using DevExtreme.AspNet.Data;
using DynamicFilter.Core.Common.Enums;
using DynamicFilter.DevExtreme.Creators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicFilter.DevExtreme.Tests.Creators
{
    [TestClass]
    public class DxDataTableSortCreator_Test
    {
        [TestMethod]
        public void SingleSort()
        {
            var clientExpr = new[] {
                new SortingInfo { Selector = "Prop1" }
            };
            var sortCreator = new DxDataTableSortCreator();
            var sort = sortCreator.Create(clientExpr);

            Assert.AreEqual(sort.Fields["Prop1"], SortTypeEnum.Asc);
        }

        [TestMethod]
        public void SingleSortDesc()
        {
            var clientExpr = new[] {
                new SortingInfo {
                    Selector = "Prop2",
                    Desc = true
                },
            };
            var sortCreator = new DxDataTableSortCreator();
            var sort = sortCreator.Create(clientExpr);

            Assert.AreEqual(sort.Fields["Prop2"], SortTypeEnum.Desc);
        }

        [TestMethod]
        public void MultipleSort()
        {
            var clientExpr = new[] {
                new SortingInfo {
                    Selector="Prop1"
                },
                new SortingInfo {
                    Selector="Prop2",
                    Desc= true
                }
            };
            var sortCreator = new DxDataTableSortCreator();
            var sort = sortCreator.Create(clientExpr);

            Assert.AreEqual(sort.Fields["Prop1"], SortTypeEnum.Asc);
            Assert.AreEqual(sort.Fields["Prop2"], SortTypeEnum.Desc);
        }
    }
}
