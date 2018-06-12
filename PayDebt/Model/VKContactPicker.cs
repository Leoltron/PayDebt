using DebtModel;

namespace PayDebt.Model
{
    public class VKContactPicker : ContactPicker<VKContact>
    {
        public VKContactPicker(IContactProvider<VKContact> provider) : base(provider)
        {
        }
    }
}