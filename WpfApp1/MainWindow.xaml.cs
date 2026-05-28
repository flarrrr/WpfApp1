using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Валидация ввода: только цифры, точка и запятая
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9.,]+$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        // Обработчик кнопки "Рассчитать"
        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Получаем и проверяем сумму счета
                if (string.IsNullOrWhiteSpace(txtBillAmount.Text))
                {
                    MessageBox.Show("Введите сумму счета!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBillAmount.Focus();
                    return;
                }

                if (!decimal.TryParse(txtBillAmount.Text.Replace('.', ','), out decimal billAmount))
                {
                    MessageBox.Show("Введите корректную сумму счета (например: 1250.50)", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtBillAmount.Text = "";
                    txtBillAmount.Focus();
                    return;
                }

                if (billAmount <= 0)
                {
                    MessageBox.Show("Сумма счета должна быть больше 0!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtBillAmount.Text = "";
                    txtBillAmount.Focus();
                    return;
                }

                // 2. Получаем и проверяем количество гостей
                if (string.IsNullOrWhiteSpace(txtGuests.Text))
                {
                    MessageBox.Show("Введите количество гостей!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtGuests.Focus();
                    return;
                }

                if (!int.TryParse(txtGuests.Text, out int guests))
                {
                    MessageBox.Show("Введите корректное количество гостей (целое число)", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    txtGuests.Text = "1";
                    txtGuests.Focus();
                    return;
                }

                if (guests < 1)
                {
                    MessageBox.Show("Количество гостей должно быть не менее 1!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtGuests.Text = "1";
                    txtGuests.Focus();
                    return;
                }

                // 3. Определяем процент чаевых
                int tipPercent = 0;
                if (rbNoTip.IsChecked == true) tipPercent = 0;
                else if (rbTip5.IsChecked == true) tipPercent = 5;
                else if (rbTip10.IsChecked == true) tipPercent = 10;
                else if (rbTip15.IsChecked == true) tipPercent = 15;

                // 4. Выполняем расчеты
                decimal tipAmount = billAmount * tipPercent / 100;
                decimal totalWithTip = billAmount + tipAmount;

                // 5. Формируем результат
                string resultText;

                if (guests == 1)
                {
                    resultText = $"Сумма счета: {billAmount:F2} ₽\n" +
                                $"💵 Чаевые ({tipPercent}%): {tipAmount:F2} ₽\n" +
                                $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                $"Итого к оплате: {totalWithTip:F2} ₽";
                }
                else
                {
                    decimal perPersonBill = billAmount / guests;
                    decimal perPersonTip = tipAmount / guests;
                    decimal perPersonTotal = totalWithTip / guests;

                    resultText = $"Сумма счета: {billAmount:F2} ₽\n" +
                                $"Чаевые ({tipPercent}%): {tipAmount:F2} ₽\n" +
                                $"Итого к оплате: {totalWithTip:F2} ₽\n\n" +
                                $"\n" +
                                $"НА {guests} ГОСТЯ(ЕЙ):\n" +
                                $"   • С каждого за еду: {perPersonBill:F2} ₽\n" +
                                $"   • Чаевые с каждого: {perPersonTip:F2} ₽\n" +
                                $"   • Каждый платит: {perPersonTotal:F2} ₽";
                }

                lblResult.Text = resultText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}