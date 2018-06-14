using DebtModel;

namespace PayDebt.Model
{
    public interface IContactPicker<out TContact> : IBaseContactPicker<TContact>, IAuthorizable
        where TContact : Contact
    {
        string Name { get; }
        int RequestCode { get; }
    }
}