﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PayDebt
{
    [Activity]
    public class DebtHistoryActivity : Activity
    {
        private DebtInfoAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DebtHistoryLayout);
            SetResult(Result.Canceled);

            FindViewById<Button>(Resource.Id.backButton).Click += (sender, args) => Finish();

            var listView = FindViewById<ListView>(Resource.Id.debtHistoryListView);
            adapter = new DebtInfoAdapter(this, MainActivity.Debts);

            listView.ItemLongClick += (sender, args) => OpenDebtDialog(adapter[args.Position]);
            listView.Adapter = adapter;
        }

        public void OpenDebtDialog(Debt debt)
        {
            var options = new[]
            {
                GetString(debt.IsPaid ? Resource.String.set_unpaid : Resource.String.set_paid)
            };

            var builder = new AlertDialog.Builder(this);
            builder.SetItems(options, (sender, args) =>
            {
                switch (args.Which)
                {
                    case 0:
                        debt.IsPaid = !debt.IsPaid;
                        MainActivity.Debts.Add(debt, MainActivity.Storage);
                        adapter.UpdateList();
                        adapter.NotifyDataSetInvalidated();
                        SetResult(Result.Ok);
                        break;
                }
            });
            builder.Show();
        }
    }
}