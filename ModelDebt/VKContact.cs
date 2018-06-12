namespace DebtModel
{
    public class VKContact : Contact
    {
        public string Id { get; }

        public VKContact(string name, string id) : base(name)
        {
            Id = id;
        }
    }
}