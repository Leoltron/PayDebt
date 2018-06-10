using System.Collections.Generic;

namespace DebtModel
{
    public interface IDebtsStorageAccess
    {
        IEnumerable<Debt> LoadDebts();
        void SaveDebt(Debt debt);
        bool DeleteDebt(Debt debt);
        bool DebtWithIdExisis(int id);
    }
}