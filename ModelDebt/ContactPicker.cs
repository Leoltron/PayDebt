using System.Collections.Generic;
using System.Linq;

namespace DebtModel
{
    public abstract class ContactPicker<TContact> where TContact : Contact
    {
        private readonly IContactProvider<TContact> provider;   
        private List<TContact> allContacts;
        private List<TContact> currentlyDisplayedContacts;

        public IReadOnlyList<TContact> DisplayedContacts => currentlyDisplayedContacts;
        public string[] Names => currentlyDisplayedContacts.Select(x => x.Name).ToArray();
        public TContact this[int x] => DisplayedContacts[x];

        protected ContactPicker(IContactProvider<TContact> provider)
        {
            this.provider = provider;
        }

        public bool UpdateContacts()
        {

            var result = provider.TryGetContacts(out var contacts);
            if (result)
                allContacts = currentlyDisplayedContacts = contacts.ToList();
            return result;
        }

        public void FilterContacts(string prefix)
        {
            currentlyDisplayedContacts = allContacts.Where(c => c != null && c.Name.StartsWith(prefix)).ToList();
        }
    }
}