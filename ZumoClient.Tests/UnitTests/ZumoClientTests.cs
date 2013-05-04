using Heyns.ZumoClient;
using Moq;
using NUnit.Framework;
using ZumoClient.Tests.TestEntity;

namespace ZumoClient.Tests.UnitTests
{
    [TestFixture]
    public class ZumoClientTests
    {
        private static readonly Mock<IMobileServicesTable<Item>> Mock = new Mock<IMobileServicesTable<Item>>();
      
        [Test]
        public void Insert_Some_Random_Item_Into_Zumo()
        {
            // Arrange 
            var item = new Item {Id = 1, Text = "Random Text"};
            Mock.Setup(m => m.Insert(item)).Returns(new Item { Id = 1, Text = "Random Text" });

            // Act
            item = Mock.Object.Insert(new Item { Id = 1, Text = "Random Text" });
            
            // Assert
            Assert.IsNotNull(item);
        }

        [Test]
        public void Get_All_Items_From_Zumo()
        {
            // Arrange
            var items = new[]{
                new Item { Id = 0, Text = "Random Text"}, 
                new Item() {Id = 1, Text = "Random Text"}};

            Mock.Setup(m => m.Get())
                .Returns(items);
            // Act
            var retrievedItems = Mock.Object.Get();

            // Assert
            Assert.IsNotNull(retrievedItems);
        }

        [Test]
        public void Get_Some_Random_Item_From_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            Mock.Setup(m => m.Get(item.Id)).Returns(new Item { Id = 1, Text = "Random Text" });

            // Act
            item = Mock.Object.Get(item.Id);

            // Assert
            Assert.IsNotNull(item);
        }

        [Test]
        public void Update_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            // This just doesn't seem right but you need to set the Id to null
            // or ignore it when serializing to Json
            var id = item.Id;
            item.Id = null;
            Mock.Setup(m => m.Update(id, item)).Returns(new Item { Id = 1, Text = "Random Text" });
            // Act
            var actual = Mock.Object.Update(id,item);

            // Reset the Id back so we can delete
            item.Id = id;

            // Assert
            Assert.AreEqual(item.Id, actual.Id);
            Assert.AreEqual(item.Text, actual.Text);
        }

        [Test]
        public void Delete_Some_Random_Item_Into_Zumo()
        {
            // Arrange
            var item = new Item { Id = 1, Text = "Random Text" };
            Mock.Setup(m => m.Delete(item)).Verifiable();

            // Act
            Mock.Object.Delete(item.Id);

            // Assert
            Mock.Verify(m => m.Delete(item.Id));
        }
    }
}