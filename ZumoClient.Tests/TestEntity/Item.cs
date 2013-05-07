namespace ZumoClient.Tests.TestEntity
{
    // Test class used during unit and integration tests
    public class Item
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public bool? Complete { get; set; }
        public override bool Equals(object obj)
        {
            var temp = obj as Item;
            return Id.Value == temp.Id && string.Compare(Text, temp.Text, true) == 0;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode() + Text.GetHashCode();
            }
        }
    }
}
