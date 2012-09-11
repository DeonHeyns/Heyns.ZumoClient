using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Heyns.ZumoClient;
using ZumoClient.Test.TestEntity;

namespace ZumoClient.Test.IntegrationTests
{
    
    public class ZumoClientIntegrationTests
    {
        [TestClass]
        public class TableTests
        {
            private static Item _item;
            private static MobileServicesClient _client;

            [TestInitialize]
            public void Setup()
            {
                if (_item == null)
                    _item = new Item {Text = "Just some random text"};
                if (_client == null)
                    _client = new MobileServicesClient(string.Empty /* Your endpoint */, string.Empty /* Your Api key */);
            }

            [TestMethod]
            public void Insert_Some_Random_Item_Into_Zumo()
            {
                // Arrange 

                // Act
                _item = _client.GetTable<Item>().Insert(_item);
                // Assert

            }

            [TestMethod]
            public void Get_All_Items_From_Zumo()
            {
                // Arrange

                // Act
                var items = _client.GetTable<Item>().Get();

                // Assert
                Assert.IsNotNull(items);
            }

            [TestMethod]
            public void Get_Some_Random_Item_From_Zumo()
            {
                // Arrange

                // Act
                var item = _client.GetTable<Item>().Get(_item.Id);

                // Assert
                Assert.IsNotNull(item);
            }

            [TestMethod]
            public void Update_Some_Random_Item_Into_Zumo()
            {
                // Arrange
                _item.Text = "Just something else";
                // This just doesn't seem right but you need to set the Id to null
                // or ignore it when serializing to Json
                var id = _item.Id;
                _item.Id = null;
                // Act
                var actual = _client.GetTable<Item>().Update(id, _item);

                // Assert
                Assert.AreEqual(_item, actual);
                // Reset the Id back so we can delete
                _item.Id = id;
            }

            [TestMethod]
            public void Delete_Some_Random_Item_Into_Zumo()
            {
                // Arrange

                // Act
                _client.GetTable<Item>().Delete(_item.Id);

                // Assert
            }
        }

        [TestClass]
        public class TableQueryTests
        {
            private static Item _item;
            private static MobileServicesClient _client;

            [TestInitialize]
            public void Setup()
            {
                if (_item == null)
                    _item = new Item { Text = "Just some random text" };
                if (_client == null)
                    _client = new MobileServicesClient(string.Empty /* Your endpoint */, string.Empty /* Your Api key */);
                _item = _client.GetTable<Item>().Insert(_item);
            }

            [TestCleanup]
            public void CleanUp()
            {
                _client.GetTable<Item>().Delete(_item.Id);
            }

            [TestMethod]
            public void Filter_Brings_Back_Only_Item_With_Text_Filtered_On()
            {
                // Arrange
                var expected = _item;
                // Act
                var actual = _client.QueryTable<Item>().Filter(string.Format("text eq '{0}'", _item.Text)).ExecuteQuery().First();

                // Assert
                Assert.AreEqual(actual, expected);
            }

            [TestMethod]
            public void Top_10_Brings_Back_Only_10_Items_From_The_Service()
            {
                // Arrange

                // Act
                var actual = _client.QueryTable<Item>().Top(10).ExecuteQuery();

                // Assert
                Assert.AreEqual(actual.Count(), 10);
            }

            [TestMethod]
            public void Skip_10_Will_Bring_Back_Next_10_Records()
            {
                // Arrange
                var first = _client.QueryTable<Item>().Top(10).ExecuteQuery();
                
                // Act
                var second = _client.QueryTable<Item>().Skip(10).Top(10).ExecuteQuery();
                
                // Assert
                Assert.AreNotEqual(first, second);
                Assert.AreEqual(first.Count(), 10);
                Assert.AreEqual(second.Count(), 10);
            }

            [TestMethod]
            public void OrderBy_Will_Order_Items_Accordingly()
            {
                // Arrange

                // Act
                var items = _client.QueryTable<Item>().Top(2).OrderBy("id").ExecuteQuery();
                var firstItem = items.First();
                var secondItem = items.Skip(1).First();
                // Assert
                Assert.IsTrue(firstItem.Id < secondItem.Id);
            }

            [TestMethod]
            public void Select_Will_Project_Return_On_Projected_Values()
            {
                // Arrange

                // Act
                var actual = _client.QueryTable<Item>().Top(1).Select("text").ExecuteQuery().First();

                // Assert
                Assert.IsNull(actual.Id);
                Assert.IsNotNull(actual.Text);
            }
        }
    }
}