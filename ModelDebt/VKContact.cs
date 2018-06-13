namespace DebtModel
{
    public class VkContact : Contact
    {
        public int Id { get; }

        public VkContact(string name, int id) : base(name)
        {
            Id = id;
        }
    }
}