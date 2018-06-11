using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace DebtModel
{
    public class Debts : IEnumerable<Debt>
    {
        private readonly Dictionary<int, Debt> debts;
        public Debt this[int id] => debts[id];
        private int nextId = 0;

        private Debts(Dictionary<int, Debt> debts)
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

        public void Add(Debt debt, IEntityStorageAccess<int, Debt> storage)
        {
            debts[debt.Id] = debt;
            storage.SaveEntity(debt);
        }

        public void SaveTo(IEntityStorageAccess<int, Debt> storage)
        {
            foreach (var debt in debts.Values)
                storage.SaveEntity(debt);
        }

        public IEnumerator<Debt> GetEnumerator()
        {
            return debts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Debts LoadFrom(IEntityStorageAccess<int, Debt> storage)
        {
            return new Debts(storage.LoadEntities().ToDictionary(d => d.Id));
        }
    }
}