using System;

namespace DebtModel
{
    [Serializable]
    public class Contact
    {
        public string Name { get; }

        public Contact(string name)
        {
            Name = name;
        }
    }
}