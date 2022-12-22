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
    /// Логика взаимодействия для PageCreateOrder.xaml
    /// </summary>
    public partial class PageCreateOrder : Page
    {
        public PageCreateOrder()
        {
            InitializeComponent();
            SetProducts();
        }
        bool StatusOfErrorLog = false;

        private void CreateOrderBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void SetProducts()
        {
            foreach (var products in AppConnect.model0db.Products.ToList())
            {
                CreateOrderNameProduct.Items.Add(products.NameProduct);
            }
        }

        private bool DataTest()
        {
            bool SendMessage = false;
            CreateOrderSurname.BorderBrush = Brushes.Black;
            CreateOrderName.BorderBrush = Brushes.Black;
            CreateOrderMiddleName.BorderBrush = Brushes.Black;
            CreateOrderPhoneNumber.BorderBrush = Brushes.Black;
            CreateOrderEmail.BorderBrush = Brushes.Black;
            CreateOrderNameProduct.BorderBrush = Brushes.Black;
            CreateOrderDeliveryDate.BorderBrush = Brushes.Black;
            CreateOrderDeliveryAddress.BorderBrush = Brushes.Black;

            StatusOfErrorLog = false;
            if (CreateOrderSurname.Text.Length < 1 || CreateOrderSurname.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                CreateOrderSurname.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваша фамилия не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (CreateOrderName.Text.Length < 1 || CreateOrderName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                CreateOrderName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше имя не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (CreateOrderMiddleName.Text.Length < 1 || CreateOrderMiddleName.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                CreateOrderMiddleName.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше отчество не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (CreateOrderPhoneNumber.Text.Length != 11)
            {
                StatusOfErrorLog = true;
                CreateOrderPhoneNumber.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Телефона не соответсвует шаблону (8-ХХХ-ХХХ-ХХ-ХХ)");
                SendMessage = true;
            }
            string rowEmail = CreateOrderEmail.Text;
            if (!rowEmail.Contains("@") || !rowEmail.Contains("."))
            {
                StatusOfErrorLog = true;
                CreateOrderEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Некорректный формат почты!");
                SendMessage = true;
            }
            if (CreateOrderEmail.Text.Length < 5 || CreateOrderEmail.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                CreateOrderEmail.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Почта должна содержать не меньше 5 и не больше 150 символов!");
                SendMessage = true;
            }
            if (CreateOrderNameProduct.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                CreateOrderNameProduct.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Выберите товар!");
                SendMessage = true;
            }
            if (CreateOrderDeliveryDate.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                CreateOrderDeliveryDate.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Введите дату!");
                SendMessage = true;
            }
            if (CreateOrderDeliveryAddress.Text.Length < 5 || CreateOrderDeliveryAddress.Text.Length > 150)
            {
                StatusOfErrorLog = true;
                CreateOrderDeliveryAddress.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Адресс должен содержать не меньше 5 и не больше 150 символов!");
                SendMessage = true;
            }

            return StatusOfErrorLog;
        }

        private void CreateOrderAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!DataTest())
            {
                try
                {
                    Clients clientObj = new Clients()
                    {
                        Surname = CreateOrderSurname.Text,
                        Name = CreateOrderName.Text,
                        MiddleName = CreateOrderMiddleName.Text,
                        NumberPhone = CreateOrderPhoneNumber.Text,
                        Email = CreateOrderEmail.Text
                    };
                    AppConnect.model0db.Clients.Add(clientObj);
                    AppConnect.model0db.SaveChanges();
                    int idClient = clientObj.IdClients;

                    var idProduct = AppConnect.model0db.Products.FirstOrDefault(x => x.NameProduct == CreateOrderNameProduct.Text);
                    Orders orderObj = new Orders()
                    {
                        IdProduct = idProduct.IdProduct,
                        IdClients = idClient,
                        DeliveryDate = (DateTime)CreateOrderDeliveryDate.SelectedDate,
                        DeliveryAddress = CreateOrderDeliveryAddress.Text
                    };
                    AppConnect.model0db.Orders.Add(orderObj);
                    AppConnect.model0db.SaveChanges();
                    MessageBox.Show("Заказ оформлен, ожидайте звонка оператора!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Ошибка оформления заказа! Ошибка: " + Ex.Message.ToString(), "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                AppFrame.frameMain.GoBack();
            }
        }

        private void CreateOrderPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[0-9]") || CreateOrderPhoneNumber.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void CreateOrderSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void CreateOrderName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void CreateOrderMiddleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }
    }
}
