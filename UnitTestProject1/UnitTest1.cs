using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WpfApp1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private TipCalculatorLogic _calculator;

        [TestInitialize]
        public void Setup()
        {
            _calculator = new TipCalculatorLogic();
        }

        #region Тесты расчета чаевых

        [TestMethod]
        public void CalculateTip_NoTip_ReturnsZero()
        {
            decimal bill = 1000;
            int tipPercent = 0;
            decimal tipAmount = _calculator.CalculateTipAmount(bill, tipPercent);
            Assert.AreEqual(0, tipAmount);
        }

        [TestMethod]
        public void CalculateTip_5Percent_CorrectAmount()
        {
            decimal bill = 1000;
            int tipPercent = 5;
            decimal tipAmount = _calculator.CalculateTipAmount(bill, tipPercent);
            Assert.AreEqual(50, tipAmount);
        }

        [TestMethod]
        public void CalculateTip_10Percent_CorrectAmount()
        {
            decimal bill = 1000;
            int tipPercent = 10;
            decimal tipAmount = _calculator.CalculateTipAmount(bill, tipPercent);
            Assert.AreEqual(100, tipAmount);
        }

        [TestMethod]
        public void CalculateTip_15Percent_CorrectAmount()
        {
            decimal bill = 1000;
            int tipPercent = 15;
            decimal tipAmount = _calculator.CalculateTipAmount(bill, tipPercent);
            Assert.AreEqual(150, tipAmount);
        }

        [TestMethod]
        public void CalculateTip_WithDecimalBill_ReturnsCorrectAmount()
        {
            decimal bill = 499.99m;
            int tipPercent = 10;
            decimal tipAmount = _calculator.CalculateTipAmount(bill, tipPercent);
            Assert.AreEqual(49.999m, tipAmount, 0.0001m);
        }

        #endregion

        #region Тесты расчета общей суммы

        [TestMethod]
        public void CalculateTotal_WithTip_ReturnsCorrectTotal()
        {
            decimal bill = 1000;
            decimal tipAmount = 100;
            decimal total = _calculator.CalculateTotal(bill, tipAmount);
            Assert.AreEqual(1100, total);
        }

        [TestMethod]
        public void CalculateTotal_ZeroTip_ReturnsSameBill()
        {
            decimal bill = 500;
            decimal tipAmount = 0;
            decimal total = _calculator.CalculateTotal(bill, tipAmount);
            Assert.AreEqual(500, total);
        }

        #endregion

        #region Тесты разделения счета

        [TestMethod]
        public void SplitPerPerson_SingleGuest_ReturnsSameAmount()
        {
            decimal amount = 1000;
            int guests = 1;
            decimal perPerson = _calculator.SplitPerPerson(amount, guests);
            Assert.AreEqual(1000, perPerson);
        }

        [TestMethod]
        public void SplitPerPerson_TwoGuests_ReturnsHalf()
        {
            decimal amount = 1000;
            int guests = 2;
            decimal perPerson = _calculator.SplitPerPerson(amount, guests);
            Assert.AreEqual(500, perPerson);
        }

        [TestMethod]
        public void SplitPerPerson_ThreeGuests_ReturnsOneThird()
        {
            decimal amount = 1000;
            int guests = 3;
            decimal perPerson = _calculator.SplitPerPerson(amount, guests);
            decimal expected = 1000m / 3m;
            Assert.AreEqual(expected, perPerson);
        }

        [TestMethod]
        public void SplitPerPerson_FourGuests_ReturnsQuarter()
        {
            decimal amount = 1000;
            int guests = 4;
            decimal perPerson = _calculator.SplitPerPerson(amount, guests);
            Assert.AreEqual(250, perPerson);
        }

        #endregion

        #region Комплексные тесты

        [TestMethod]
        public void FullCalculation_SingleGuest_NoTip()
        {
            decimal bill = 500;
            int tipPercent = 0;
            int guests = 1;
            var result = _calculator.CalculateFull(bill, tipPercent, guests);
            Assert.AreEqual(0, result.TipAmount);
            Assert.AreEqual(500, result.TotalAmount);
            Assert.AreEqual(500, result.PerPersonTotal);
        }

        [TestMethod]
        public void FullCalculation_TwoGuests_10PercentTip()
        {
            decimal bill = 1000;
            int tipPercent = 10;
            int guests = 2;
            var result = _calculator.CalculateFull(bill, tipPercent, guests);
            Assert.AreEqual(100, result.TipAmount);
            Assert.AreEqual(1100, result.TotalAmount);
            Assert.AreEqual(550, result.PerPersonTotal);
        }

        [TestMethod]
        public void FullCalculation_ThreeGuests_15PercentTip()
        {
            decimal bill = 1000;
            int tipPercent = 15;
            int guests = 3;
            var result = _calculator.CalculateFull(bill, tipPercent, guests);
            decimal expectedPerPerson = (1000m + 150m) / 3m;
            Assert.AreEqual(150, result.TipAmount);
            Assert.AreEqual(1150, result.TotalAmount);
            Assert.AreEqual(expectedPerPerson, result.PerPersonTotal);
        }

        [TestMethod]
        public void FullCalculation_FourGuests_15PercentTip()
        {
            decimal bill = 2000;
            int tipPercent = 15;
            int guests = 4;
            var result = _calculator.CalculateFull(bill, tipPercent, guests);
            Assert.AreEqual(300, result.TipAmount);
            Assert.AreEqual(2300, result.TotalAmount);
            Assert.AreEqual(575, result.PerPersonTotal);
        }

        #endregion

        #region Тесты на исключения

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionTest_NegativeBill_ThrowsException()
        {
            decimal bill = -100;
            int tipPercent = 10;
            _calculator.CalculateTipAmount(bill, tipPercent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionTest_ZeroGuests_ThrowsException()
        {
            decimal amount = 1000;
            int guests = 0;
            _calculator.SplitPerPerson(amount, guests);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionTest_NegativeGuests_ThrowsException()
        {
            decimal amount = 1000;
            int guests = -5;
            _calculator.SplitPerPerson(amount, guests);
        }

        // ЭТОТ ТЕСТ ТЕПЕРЬ БУДЕТ РАБОТАТЬ
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionTest_InvalidTipPercent_ThrowsException()
        {
            decimal bill = 1000;
            int tipPercent = 20;  // Неподдерживаемый процент
            _calculator.CalculateFull(bill, tipPercent, 1);  // Используем CalculateFull, где есть проверка
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionTest_ZeroBill_ThrowsException()
        {
            decimal bill = 0;
            int tipPercent = 10;
            int guests = 1;
            _calculator.CalculateFull(bill, tipPercent, guests);
        }

        #endregion
    }
}