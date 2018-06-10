using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DebtModel
{
    public class Debts : IEnumerable<Debt>
    {
        private readonly Dictionary<int, Debt> debts;
        public Debt this[int id] => debts[id];
        private int nextId = 0;

        public Debts(Dictionary<int, Debt> debts)
        {
            this.debts = debts;
            if (debts.Any())
                nextId = this.debts.Keys.Max() + 1;
        }

        public int GetNextId()
        {
            return nextId++;
        }

        public bool TryGetDebt(int id, out Debt debt)
        {
            return debts.TryGetValue(id, out debt);
        }

        public void Add(Debt debt, IDebtsStorageAccess storage)
        {
            debts[debt.Id] = debt;
            storage.SaveDebt(debt);
        }

        public void SaveTo(IDebtsStorageAccess storage)
        {
            foreach (var debt in debts.Values)
                storage.SaveDebt(debt);
        }

        public IEnumerator<Debt> GetEnumerator()
        {
            return debts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Debts LoadFrom(IDebtsStorageAccess storage)
        {
            return new Debts(storage.LoadDebts().ToDictionary(d => d.Id));
        }
    }
}