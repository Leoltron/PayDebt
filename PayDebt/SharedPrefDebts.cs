using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;

namespace PayDebt
{
    public class SharedPrefDebts : IDebtsStorageAccess
    {
        private const string DebtIdsSetKey = "debtIds";

        private readonly ISharedPreferences sharedPreferences;

        public SharedPrefDebts(ISharedPreferences sharedPreferences)
        {
            this.sharedPreferences = sharedPreferences;
        }

        private static string GetDebtKey(int id) => "debt#" + id;

        public IEnumerable<Debt> LoadDebts()
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

        public void SaveDebt(Debt debt)
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

        public bool DeleteDebt(Debt debt)
        {
            var ids = new HashSet<string>(sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>()));
            var result = ids.Remove(debt.Id.ToString());
            var editor = sharedPreferences.Edit();
            editor.PutStringSet(DebtIdsSetKey, ids);
            editor.Apply();
            return result;
        }

        public bool DebtWithIdExisis(int id)
        {
            return sharedPreferences.GetStringSet(DebtIdsSetKey, new HashSet<string>()).Contains(id.ToString());
        }
    }
}