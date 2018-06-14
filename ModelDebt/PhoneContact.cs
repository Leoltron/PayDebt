using System;

namespace DebtModel
{
    [Serializable]
    public class PhoneContact : Contact
    {
        public string PhoneNumber { get; }

        public PhoneContact(string name, string phoneNumber) 
            : base(name)
        {
            PhoneNumber = phoneNumber;
        }
    }
}