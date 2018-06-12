namespace DebtModel
{
    public class VKContact : Contact
    {
        public int Id { get; }

        public VKContact(string name, int id) : base(name)
        {
            Id = id;
        }
    }
}