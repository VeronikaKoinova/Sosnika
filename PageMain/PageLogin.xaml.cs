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
using WPFApplicationOptika.PageAdmin;
using WPFApplicationOptika.PageClient;

namespace WPFApplicationOptika.PageMain
{
    /// <summary>
    /// Логика взаимодействия для PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void buttonRegistration_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageRegistration());
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = AppConnect.model0db.Users.FirstOrDefault(x => x.Login == textBoxLogin.Text && x.Password == PasswordBoxEnter.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя не существует!", "Ошибка авторизации!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    WindowCaptcha wincaptcha = new WindowCaptcha();
                    wincaptcha.ShowDialog();
                }
                else
                {
                    switch (userObj.IdRole)
                    {
                        case 1:
                            MessageBox.Show("Здравствуйте, Администратор " + userObj.Name + "!",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new PageAdministrator());
                            break;
                        case 2:
                            MessageBox.Show("Здравствуйте, Клиент " + userObj.Name + "!",
                         "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new PageMenuClient());
                            break;
                        case 3:
                            MessageBox.Show("Здравствуйте, Менеджер " + userObj.Name + "!",
                         "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                            AppFrame.frameMain.Navigate(new PageMenuClient());
                            break;
                        default:
                            MessageBox.Show("Данный пользователь не имеет роли!", "Уведомление", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                            break;
                    }
                    SelectedElement.TakeStatusOfUserLabel(true, userObj.Name);
                    SelectedElement.TakeStatusOfUserButton(true);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Ошибка" + Ex.Message.ToString() + "Критическая работа приложения!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
