using System.Linq;
using Heyns.ZumoClient;
using NUnit.Framework;
using ZumoClient.Tests.TestEntity;

namespace ZumoClient.IntegrationTests
{
    [TestFixture]
    public class TableQueryTests
    {
        private static Item _item;
        private static MobileServiceClient _client;

        [TestFixtureSetUp]
        public void Setup()
        {
            if (_item == null)
                _item = new Item {Text = "Just some text used to test querying"};
            if (_client == null)
                _client = new MobileServiceClient(string.Empty /* Your endpoint */, string.Empty /* Your Api key */);

        }


        [Test]
        public void Filter_Brings_Back_Only_Item_With_Text_Filtered_On()
        {
            // Arrange
            _item = _client.GetTable<Item>().Insert(_item);
            var expected = _item;
            // Act
            var actual = _client.QueryTable<Item>().Filter(string.Format("text eq '{0}'", _item.Text)).First();

            // Assert
            Assert.AreEqual(actual, expected);
            _client.GetTable<Item>().Delete(_item.Id);
        }

        [Test]
        public void Top_10_Brings_Back_Only_10_Items_From_The_Service()
        {
            // Arrange

            // Act
            var actual = _client.QueryTable<Item>().Top(10);

            // Assert
            Assert.AreEqual(actual.Count(), 10);
        }

        [Test]
        public void Skip_10_Will_Bring_Back_Next_10_Records()
        {
            // Arrange
            var first = _client.QueryTable<Item>().Top(10);

            // Act
            var second = _client.QueryTable<Item>().Skip(10).Top(10);

            // Assert
            Assert.AreNotEqual(first, second);
            Assert.AreEqual(first.Count(), 10);
            Assert.AreEqual(second.Count(), 10);

        }

        [Test]
        public void OrderBy_Will_Order_Items_Accordingly()
        {
            // Arrange

            // Act
            var items = _client.QueryTable<Item>().Top(2).OrderBy("id");
            var firstItem = items.First();
            var secondItem = items.Skip(1).First();
            // Assert
            Assert.IsTrue(firstItem.Id < secondItem.Id);
        }

        [Test]
        public void Select_Will_Project_Return_On_Projected_Values()
        {
            // Arrange

            // Act
            var actual = _client.QueryTable<Item>().Top(1).Select("text").First();

            // Assert
            Assert.IsNull(actual.Id);
            Assert.IsNotNull(actual.Text);
        }
    }
}