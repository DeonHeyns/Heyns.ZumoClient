using System.Linq;
using Heyns.ZumoClient;
using NUnit.Framework;
using ZumoClient.Tests.TestEntity;

namespace ZumoClient.IntegrationTests
{
    [TestFixture]
    public class TableTests
    {
        private static Item _item;
        private static MobileServiceClient _client;

        [TestFixtureSetUp]
        public void Setup()
        {
            if (_item == null)
                _item = new Item {Text = "Just some random text"};
            if (_client == null)
                _client = new MobileServiceClient(string.Empty /* Your endpoint */, string.Empty /* Your Api key */);
        }

        [Test]
        public void Insert_Some_Random_Item_Into_Zumo()
        {
            // Arrange 

            // Act
            _item = _client.GetTable<Item>().Insert(_item);
            // Assert

        }

        [Test]
        public void Get_All_Items_From_Zumo()
        {
            // Arrange

            // Act
            var items = _client.GetTable<Item>().Get();

            // Assert
            Assert.IsNotNull(items);
        }

        [Test]
        public void Get_Some_Random_Item_From_Zumo()
        {
            // Arrange
            var randomItem = _client.GetTable<Item>().Get().First();

            // Act
            var item = _client.GetTable<Item>().Get(randomItem.Id);

            // Assert
            Assert.IsNotNull(item);
        }

        [Test]
        public void Update_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            _item.Text = "Just something else";
            _item = _client.GetTable<Item>().Insert(_item);
            // This just doesn't seem right but you need to set the Id to null
            // or ignore it when serializing to Json
            var id = _item.Id;
            // Act
            var actual = _client.GetTable<Item>().Update(id, _item);

            // Assert
            Assert.AreEqual(_item, actual);
            // Reset the Id back so we can delete
            _item.Id = id;
        }

        [Test]
        public void Delete_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            if (_item.Id == null)
            {
                _item.Text = "Just something else";
                _item = _client.GetTable<Item>().Insert(_item);
            }
            // Act
            _client.GetTable<Item>().Delete(_item.Id);

            // Assert
        }
    }
}