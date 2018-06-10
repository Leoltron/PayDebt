using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using DebtModel;
using Infrastructure;

namespace PayDebt
{
    public class SharedPrefDebts : IEntityStorageAccess<int, Debt>
    {
        private const string DebtIdsSetKey = "debtIds";

        private readonly ISharedPreferences sharedPreferences;

        public SharedPrefDebts(ISharedPreferences sharedPreferences)
        {
            this.sharedPreferences = sharedPreferences;
        }

        private static string GetDebtKey(int id) => "debt#" + id;

        public IEnumerable<Debt> LoadEntities()
        {
            var ids = sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>());
            foreach (var idStr in ids)
                if (TryGetDebt(idStr, out var debt))
                    yield return debt;
        }

        private bool TryGetDebt(string idStr, out Debt debt)
        {
            try
            {
                debt = GetDebt(idStr);
                return true;
            }
            catch (Exception e)
            {
                Log.Warn(typeof(SharedPrefDebts).FullName, $"An error occured during debt getting (id = {idStr})", e);
                debt = null;
                return false;
            }
        }

        private Debt GetDebt(string idStr)
        {
            return sharedPreferences.GetDebt(GetDebtKey(int.Parse(idStr)));
        }

        public void SaveEntity(Debt debt)
        {
            var ids = new HashSet<string>(sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>()))
            {
                debt.Id.ToString()
            };

            var editor = sharedPreferences.Edit();
            editor.PutStringSet(DebtIdsSetKey, ids);
            editor.PutDebt(GetDebtKey(debt.Id), debt);
            editor.Apply();
        }

        public bool DeleteEntity(Debt debt)
        {
            var ids = new HashSet<string>(sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>()));
            var result = ids.Remove(debt.Id.ToString());
            var editor = sharedPreferences.Edit();
            editor.PutStringSet(DebtIdsSetKey, ids);
            editor.Apply();
            return result;
        }

        public bool EntityWithIdExisis(int id)
        {
            return sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>()).Contains(id.ToString());
        }
    }
}