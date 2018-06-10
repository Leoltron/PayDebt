using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt
{
    internal class DebtInfoAdapter : BaseAdapter<Debt>
    {
        private readonly Context context;
        private readonly LayoutInflater lInflater;

        public List<Debt> ShowingDebts { get; private set; }
        private readonly IEnumerable<Debt> debts;

        private readonly Func<Debt, bool> filter;

        public override int Count => ShowingDebts.Count;
        public override Debt this[int position] => ShowingDebts[position];

        public DebtInfoAdapter(Context context, IEnumerable<Debt> debtList, Func<Debt, bool> filter = null)
        {
            this.context = context;
            debts = debtList;
            lInflater = (LayoutInflater) this.context.GetSystemService(Context.LayoutInflaterService);
            this.filter = filter ?? (d => true);
            UpdateList();
        }

        public void UpdateList()
        {
            ShowingDebts = debts.Where(filter).ToList();
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? lInflater.Inflate(Resource.Layout.DebtListItem, parent, false);
            FillViewWithDebtInfo(view, this[position]);
            return view;
        }

        private void FillViewWithDebtInfo(View view, Debt d)
        {
            view.SetNormalTextForTextView(Resource.Id.nameTextView, d.AssosiatedContact.Name);
            view.SetNormalTextForTextView(Resource.Id.commentTextView, d.Comment);

            var paymentDateTextView = view.FindViewById<TextView>(Resource.Id.paymentDateTextView);
            paymentDateTextView.SetText(d.HasPaymentDate ? d.PaymentDateString : "", TextView.BufferType.Normal);

            var amountTextView = view.FindViewById<TextView>(Resource.Id.amountTextView);
            amountTextView.SetTextColor(GetColorForDebt(d));
            amountTextView.SetText(d.Money.ToString(), TextView.BufferType.Normal);
        }

        private Color GetColorForDebt(Debt debt)
        {
            if (debt.IsPaid)
                return Color.LightGray;
            if (debt.IsMyDebt)
                return GetColorFromResource(Resource.Color.myDebtAmountColor);
            if (debt.IsTheirDebt)
                return GetColorFromResource(Resource.Color.theirDebtAmountColor);
            return GetColorFromResource(Resource.Color.primary_text_default_material_light);
        }

        private Color GetColorFromResource(int resourceId)
        {
            return new Color(ContextCompat.GetColor(context, resourceId));
        }
    }
}