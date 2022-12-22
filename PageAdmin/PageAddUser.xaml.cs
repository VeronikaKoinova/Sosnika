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

namespace WPFApplicationOptika.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAddUser.xaml
    /// </summary>
    public partial class PageAddUser : Page
    {
        private Users _users = new Users();
        bool StatusOfErrorLog = false;
        int IdUser;

        public PageAddUser()
        {
            InitializeComponent();
            GetRole();
        }

        public PageAddUser(Users users)
        {
            InitializeComponent();
            _users = users;
            DataContext = _users;
            IdUser = _users.IdUser;
            GetRole();
            TakeRole();
        }

        private void GetRole()
        {
            foreach (var roles in AppConnect.model0db.Roles.ToList())
            {
                AddUserRole.Items.Add(roles.Name);
            }
        }

        private void TakeRole()
        {
            var Role = AppConnect.model0db.Roles.FirstOrDefault(x => x.IdRole == _users.IdRole);
            AddUserRole.SelectedItem = Role.Name;
        }

        private void AddUserSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void AddUserName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void AddUserMiddleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void AddUserPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]") || AddUserPhoneNumber.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private bool DataTest()
        {
            bool SendMessage = false;
            AddUserSurname.BorderBrush = Brushes.Black;
            AddUserName.BorderBrush = Brushes.Black;
            AddUserMiddleName.BorderBrush = Brushes.Black;
            AddUserLogin.BorderBrush = Brushes.Black;
            AddUserDateOfBirth.BorderBrush = Brushes.Black;
            AddUserEmail.BorderBrush = Brushes.Black;
            AddUserPhoneNumber.BorderBrush = Brushes.Black;
            AddUserPassword.BorderBrush = Brushes.Black;
            AddUserRole.BorderBrush = Brushes.Black;

            StatusOfErrorLog = false;
            if (AddUserSurname.Text.Length < 1 || AddUserSurname.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddUserSurname.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваша фамилия не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (AddUserName.Text.Length < 1 || AddUserName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddUserName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше имя не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (AddUserMiddleName.Text.Length < 1 || AddUserMiddleName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddUserMiddleName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше отчество не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (AddUserLogin.Text.Length < 4 || AddUserLogin.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddUserLogin.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваш логин должен содержать от 4 до 50 символов!");
                SendMessage = true;
            }
            if (AddUserDateOfBirth.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                AddUserDateOfBirth.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Введите дату!");
                SendMessage = true;
            }
            string rowEmail = AddUserEmail.Text;
            if (!rowEmail.Contains("@") || !rowEmail.Contains("."))
            {
                StatusOfErrorLog = true;
                AddUserEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Некорректный формат почты!");
                SendMessage = true;
            }
            if (AddUserEmail.Text.Length < 5 || AddUserEmail.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                AddUserEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Почта должна содержать не меньше 5 и не больше 150 символов!");
                SendMessage = true;
            }
            if (AddUserPhoneNumber.Text.Length < 5 || AddUserPhoneNumber.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                AddUserPhoneNumber.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Телефона не соответсвует шаблону (8-ХХХ-ХХХ-ХХ-ХХ)");
                SendMessage = true;
            }

            int RegistrUp = 0, RegistrDown = 0;

            string passwordrow = AddUserPassword.Text;

            foreach (char s in passwordrow.Where(char.IsUpper))
                RegistrUp++;
            foreach (char s in passwordrow.Where(char.IsLower))
                RegistrDown++;
            if (RegistrUp <= 0 || RegistrDown <= 0 || AddUserPassword.Text.Length < 8 || AddUserPassword.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Пароль должен содержать от 8 до 50 символов (Хотя бы одну прописную и строчную латинскую букву)!");
                SendMessage = true;
                AddUserPassword.BorderBrush = Brushes.Red;
            }
            if (AddUserRole.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                AddUserRole.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Выберите роль!");
                SendMessage = true;
            }
            return StatusOfErrorLog;
        }

        private void AddUserPick_Click(object sender, RoutedEventArgs e)
        {
            if (!DataTest())
            {
                try
                {
                    if (_users != null && _users.IdUser != 0)
                    {
                        var EditRecord = AppConnect.model0db.Users.FirstOrDefault(x => x.IdUser == IdUser);
                        var idRole = AppConnect.model0db.Roles.FirstOrDefault(x => x.Name == AddUserRole.Text);
                        EditRecord.Login = AddUserLogin.Text;
                        EditRecord.Password = AddUserPassword.Text;
                        EditRecord.IdRole = idRole.IdRole;
                        EditRecord.Surname = AddUserSurname.Text;
                        EditRecord.Name = AddUserName.Text;
                        EditRecord.MiddleName = AddUserMiddleName.Text;
                        EditRecord.DateOfBirth = (DateTime)AddUserDateOfBirth.SelectedDate;
                        EditRecord.Email = AddUserEmail.Text;
                        EditRecord.NumberPhone = AddUserPhoneNumber.Text;
                        AppConnect.model0db.SaveChanges();
                    }
                    else
                    {
                        var idRole = AppConnect.model0db.Roles.FirstOrDefault(x => x.Name == AddUserRole.Text);
                        Users usersObj = new Users()
                        {
                            Login = AddUserLogin.Text,
                            Password = AddUserPassword.Text,
                            IdRole = idRole.IdRole,
                            Surname = AddUserSurname.Text,
                            Name = AddUserName.Text,
                            MiddleName = AddUserMiddleName.Text,
                            DateOfBirth = (DateTime)AddUserDateOfBirth.SelectedDate,
                            Email = AddUserEmail.Text,
                            NumberPhone = AddUserPhoneNumber.Text
                        };
                        AppConnect.model0db.Users.Add(usersObj);
                        AppConnect.model0db.SaveChanges();
                    }
                    if (_users != null && _users.IdUser != 0)
                        MessageBox.Show("Запись успешно обновлена!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Запись успешно добавлена!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.Navigate(new PageAdministrator());
                }
                catch (Exception Ex)
                {
                    if (_users != null && _users.IdUser != 0)
                        MessageBox.Show("Ошибка при обновлении записи! Ошибка:" + Ex.Message.ToString(), "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("Ошибка при добавлении записи! Ошибка:" + Ex.Message.ToString(), "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _users = null;
        }

        private void AddUserBack_Click(object sender, RoutedEventArgs e)
        {
            _users = null;
            AppFrame.frameMain.Navigate(new PageAdministrator());
        }
    }
}
