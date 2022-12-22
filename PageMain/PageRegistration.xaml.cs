using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFApplicationOptika.ApplicationData;

namespace WPFApplicationOptika.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageRegistration.xaml
    /// </summary>
    public partial class PageRegistration : Page
    {
        public PageRegistration()
        {
            InitializeComponent();
        }
        bool StatusOfErrorLog = false;

        private void RegistrationGoBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void RegistrationPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (RegistrationPasswordSecond.Password != RegistrationPasswordFirst.Password)
            {
                RegistrationPasswordSecond.Background = Brushes.LightCoral;
                RegistrationPasswordSecond.BorderBrush = Brushes.Red;
                RegistrationCreateAccount.IsEnabled = false;
            }
            else
            {
                RegistrationPasswordSecond.Background = Brushes.LightGreen;
                RegistrationPasswordSecond.BorderBrush = Brushes.Green;
                RegistrationCreateAccount.IsEnabled = true;
            }
        }

        private bool DataTest()
        {
            bool SendMessage = false;
            RegistrationSurname.BorderBrush = Brushes.Black;
            RegistrationName.BorderBrush = Brushes.Black;
            RegistrationMiddleName.BorderBrush = Brushes.Black;
            RegistrationLogin.BorderBrush = Brushes.Black;
            RegistrationDateOfBirth.BorderBrush = Brushes.Black;
            RegistrationEmail.BorderBrush = Brushes.Black;
            RegistrationNumberPhone.BorderBrush = Brushes.Black;
            RegistrationPasswordFirst.BorderBrush = Brushes.Black;

            StatusOfErrorLog = false;
            if (RegistrationSurname.Text.Length < 1 || RegistrationSurname.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                RegistrationSurname.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваша фамилия не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (RegistrationName.Text.Length < 1 || RegistrationName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                RegistrationName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше имя не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (RegistrationMiddleName.Text.Length < 1 || RegistrationMiddleName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                RegistrationMiddleName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше отчество не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (RegistrationLogin.Text.Length < 4 || RegistrationLogin.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                RegistrationLogin.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваш логин должен содержать от 4 до 50 символов!");
                SendMessage = true;
            }
            if (RegistrationDateOfBirth.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                RegistrationDateOfBirth.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Введите дату!");
                SendMessage = true;
            }
            string rowEmail = RegistrationEmail.Text;
            if (!rowEmail.Contains("@") || !rowEmail.Contains("."))
            {
                StatusOfErrorLog = true;
                RegistrationEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Некорректный формат почты!");
                SendMessage = true;
            }
            if (RegistrationEmail.Text.Length < 5 || RegistrationEmail.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                RegistrationEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Почта должна содержать не меньше 5 и не больше 150 символов!");
                SendMessage = true;
            }
            if (RegistrationNumberPhone.Text.Length < 5 || RegistrationNumberPhone.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                RegistrationNumberPhone.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Телефона не соответсвует шаблону (8-ХХХ-ХХХ-ХХ-ХХ)");
                SendMessage = true;
            }

            int RegistrUp = 0, RegistrDown = 0;

            string passwordrow = RegistrationPasswordFirst.Password;

            foreach (char s in passwordrow.Where(char.IsUpper))
                RegistrUp++;
            foreach (char s in passwordrow.Where(char.IsLower))
                RegistrDown++;
            if (RegistrUp <= 0 || RegistrDown <= 0 || RegistrationPasswordFirst.Password.Length < 8 || RegistrationPasswordFirst.Password.Length > 50)
            {
                StatusOfErrorLog = true;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Пароль должен содержать от 8 до 50 символов (Хотя бы одну прописную и строчную латинскую букву)!");
                SendMessage = true;
                RegistrationPasswordFirst.BorderBrush = Brushes.Red;
            }
            return StatusOfErrorLog;
        }

        private void RegistrationCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (AppConnect.model0db.Users.Count(x => x.Login == RegistrationLogin.Text) > 0)
            {
                MessageBox.Show("Пользователь с таким логином есть!", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!DataTest())
            {
                try
                {
                    Users userObj = new Users()
                    {
                        Login = RegistrationLogin.Text,
                        Password = RegistrationPasswordFirst.Password,
                        Surname = RegistrationSurname.Text,
                        Name = RegistrationName.Text,
                        MiddleName = RegistrationMiddleName.Text,
                        DateOfBirth = (DateTime)RegistrationDateOfBirth.SelectedDate,
                        Email = RegistrationEmail.Text,
                        IdRole = 2,
                        NumberPhone = RegistrationNumberPhone.Text
                    };
                    AppConnect.model0db.Users.Add(userObj);
                    AppConnect.model0db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.GoBack();
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении данных!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RegistrationNumberPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]") || RegistrationNumberPhone.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void RegistrationSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void RegistrationName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void RegistrationMiddleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }
    }
}
