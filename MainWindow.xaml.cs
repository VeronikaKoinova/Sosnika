using WPFApplicationOptika.ApplicationData;
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
using WPFApplicationOptika.PageMain;
using WPFApplicationOptika.PageClient;

namespace WPFApplicationOptika
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.model0db = new OptikaDBEntities1();
            AppFrame.frameMain = FrmMain;

            FrmMain.Navigate(new PageLogin());
        }

        private void EnterAsGuest_Click(object sender, RoutedEventArgs e)
        {
            if (EnterAsGuest.Content.ToString() == "Выйти")
            {
                AppFrame.frameMain.Navigate(new PageLogin());
                SelectedElement.TakeStatusOfUserLabel(false, "Гость");
                SelectedElement.TakeStatusOfUserButton(false);
            }
            else if (EnterAsGuest.Content.ToString() == "Войти как Гость")
            {
                MessageBox.Show("Добро пожаловать!",
                         "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                AppFrame.frameMain.Navigate(new PageMenuClient());
                SelectedElement.TakeStatusOfUserLabel(true, "Гость");
                SelectedElement.TakeStatusOfUserButton(true);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string FirstText = SelectedElement.ReturnStatusOfUserLabel();
            string SecondText = SelectedElement.ReturnStatusOfUserButton();
            StatusText.Content = FirstText;
            EnterAsGuest.Content = SecondText;
        }
    }
}
