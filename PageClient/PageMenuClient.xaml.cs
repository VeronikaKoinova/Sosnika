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

namespace WPFApplicationOptika.PageClient
{
    /// <summary>
    /// Логика взаимодействия для PageMenuClient.xaml
    /// </summary>
    public partial class PageMenuClient : Page
    {
        public PageMenuClient()
        {
            InitializeComponent();
        }

        private void CheckProducts_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageViewProducts());
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            string text = SelectedElement.ReturnStatusOfUserLabel();
            if (text != "Добро пожаловать, Гость!")
                AppFrame.frameMain.Navigate(new PageCreateOrder());
            else
                MessageBox.Show("Для оформления заказа вам необходимо авторизоваться!", "Доступ ограничен!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void MenuClientBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
            SelectedElement.TakeStatusOfUserLabel(false, "Гость");
            SelectedElement.TakeStatusOfUserButton(false);
        }
    }
}
