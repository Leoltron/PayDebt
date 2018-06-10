using System;

namespace DebtModel
{
    public class Debt
    {
        public readonly int Id;
        public Contact AssosiatedContact { get; }
        public Money Money { get; }
        public DateTime CreationDate { get; }

        public bool HasPaymentDate { get; }
        public DateTime PaymentDate { get; }

        public string PaymentDateString => PaymentDate.ToShortDateString();

        public string Comment { get; }

        public bool IsMyDebt => Money.Amount < 0;
        public bool IsTheirDebt => Money.Amount > 0;

        public bool IsPaid { get; set; }

        public Debt(int id, Contact assosiatedContact, Money money, string comment, DateTime creationDate)
        {
            if (money.Amount == 0)
                throw new ArgumentException("Zero amount! There's no debt!");
            AssosiatedContact = assosiatedContact;
            Money = money;
            Comment = comment;
            CreationDate = creationDate;
            Id = id;
            HasPaymentDate = false;
            PaymentDate = DateTime.MaxValue;
            IsPaid = false;
        }

        public Debt(int id, Contact assosiatedContact, Money money, string comment, DateTime creationDate,
            DateTime paymentDate) :
            this(id, assosiatedContact, money, comment, creationDate)
        {
            HasPaymentDate = true;
            PaymentDate = paymentDate;
        }
    }
}