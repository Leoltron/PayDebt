using DebtModel;
using Infrastructure;

namespace PayDebt.Model
{
    public class ContactPickers : StaticStorage<IContactPicker<Contact>, ContactPickers>
    {
    }
}