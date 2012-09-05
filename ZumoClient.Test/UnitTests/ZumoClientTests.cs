using Microsoft.VisualStudio.TestTools.UnitTesting;

using Heyns.ZumoClient;
using Moq;
using ZumoClient.Test.TestEntity;

namespace ZumoClient.Test.UnitTests
{
    [TestClass]
    public class ZumoClientTests
    {
        private static readonly Mock<IMobileServicesTable<Item>> _mock = new Mock<IMobileServicesTable<Item>>();
        
        [TestMethod]
        public void Insert_Some_Random_Item_Into_Zumo()
        {
            // Arrange 
            var item = new Item {Id = 1, Text = "Random Text"};
            _mock.Setup(m => m.Insert(item)).Returns(new Item { Id = 1, Text = "Random Text" });

            // Act
            item = _mock.Object.Insert(new Item { Id = 1, Text = "Random Text" });

            // Assert
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void Get_All_Items_From_Zumo()
        {
            // Arrange
            var items = new[]{
                new Item { Id = 0, Text = "Random Text"}, 
                new Item() {Id = 1, Text = "Random Text"}};

            _mock.Setup(m => m.Get())
                .Returns(items);
            // Act
            var retrievedItems = _mock.Object.Get();

            // Assert
            Assert.IsNotNull(retrievedItems);
        }

        [TestMethod]
        public void Get_Some_Random_Item_From_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            _mock.Setup(m => m.Get(item.Id)).Returns(new Item { Id = 1, Text = "Random Text" });

            // Act
            item = _mock.Object.Get(item.Id);

            // Assert
            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void Update_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            // This just doesn't seem right but you need to set the Id to null
            // or ignore it when serializing to Json
            var id = item.Id;
            item.Id = null;
            _mock.Setup(m => m.Update(id, item)).Returns(new Item { Id = 1, Text = "Random Text" });
            // Act
            var actual = _mock.Object.Update(id,item);

            // Reset the Id back so we can delete
            item.Id = id;

            // Assert
            Assert.AreEqual(item.Id, actual.Id);
            Assert.AreEqual(item.Text, actual.Text);
        }

        [TestMethod]
        public void Delete_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            _mock.Setup(m => m.Delete(item)).Verifiable();

            // Act
            _mock.Object.Delete(item.Id);

            // Assert
            _mock.Verify(m => m.Delete(item.Id));
        }
    }
}