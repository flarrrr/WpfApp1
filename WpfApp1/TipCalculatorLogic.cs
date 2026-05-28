using System;

namespace WpfApp1
{
    public class TipCalculatorLogic
    {
        /// <summary>
        /// Расчет суммы чаевых
        /// </summary>
        public decimal CalculateTipAmount(decimal bill, int tipPercent)
        {
            if (bill < 0)
                throw new ArgumentException("Сумма счета не может быть отрицательной", nameof(bill));

            if (tipPercent < 0 || tipPercent > 100)
                throw new ArgumentException("Процент чаевых должен быть от 0 до 100", nameof(tipPercent));

            return bill * tipPercent / 100;
        }

        /// <summary>
        /// Расчет общей суммы с чаевыми
        /// </summary>
        public decimal CalculateTotal(decimal bill, decimal tipAmount)
        {
            if (bill < 0)
                throw new ArgumentException("Сумма счета не может быть отрицательной", nameof(bill));

            if (tipAmount < 0)
                throw new ArgumentException("Сумма чаевых не может быть отрицательной", nameof(tipAmount));

            return bill + tipAmount;
        }

        /// <summary>
        /// Разделение суммы на количество гостей
        /// </summary>
        public decimal SplitPerPerson(decimal amount, int guests)
        {
            if (guests <= 0)
                throw new ArgumentException("Количество гостей должно быть больше 0", nameof(guests));

            return amount / guests;
        }

        /// <summary>
        /// Полный расчет (удобный метод)
        /// </summary>
        public CalculationResult CalculateFull(decimal bill, int tipPercent, int guests)
        {
            if (bill <= 0)
                throw new ArgumentException("Сумма счета должна быть положительной", nameof(bill));

            if (tipPercent != 0 && tipPercent != 5 && tipPercent != 10 && tipPercent != 15)
                throw new ArgumentException("Поддерживаются только проценты: 0, 5, 10, 15", nameof(tipPercent));

            if (guests <= 0)
                throw new ArgumentException("Количество гостей должно быть больше 0", nameof(guests));

            decimal tipAmount = CalculateTipAmount(bill, tipPercent);
            decimal total = CalculateTotal(bill, tipAmount);
            decimal perPersonTotal = SplitPerPerson(total, guests);
            decimal perPersonBill = SplitPerPerson(bill, guests);
            decimal perPersonTip = SplitPerPerson(tipAmount, guests);

            return new CalculationResult
            {
                BillAmount = bill,
                TipPercent = tipPercent,
                TipAmount = tipAmount,
                TotalAmount = total,
                Guests = guests,
                PerPersonBill = perPersonBill,
                PerPersonTip = perPersonTip,
                PerPersonTotal = perPersonTotal
            };
        }
    }

    public class CalculationResult
    {
        public decimal BillAmount { get; set; }
        public int TipPercent { get; set; }
        public decimal TipAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int Guests { get; set; }
        public decimal PerPersonBill { get; set; }
        public decimal PerPersonTip { get; set; }
        public decimal PerPersonTotal { get; set; }
    }
}