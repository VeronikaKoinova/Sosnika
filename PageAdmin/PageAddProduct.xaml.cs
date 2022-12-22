using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для PageAddProduct.xaml
    /// </summary>
    public partial class PageAddProduct : Page
    {
        private Products _products = new Products();
        bool StatusOfErrorLog = false;
        int IdProduct;
        private string NameOfPicture = String.Empty;

        public PageAddProduct()
        {
            InitializeComponent();
            GetManufaturers();
        }

        public PageAddProduct(Products products)
        {
            InitializeComponent();
            _products = products;
            DataContext = _products;
            IdProduct = _products.IdProduct;
            NameOfPicture = _products.ImageProduct;
            GetManufaturers();
            TakeManufaturers();
        }

        private void GetManufaturers()
        {
            foreach (var manufaturers in AppConnect.model0db.Manufacturers.ToList())
            {
                AddProductManufacturer.Items.Add(manufaturers.NameManufacturer);
            }
        }

        private void TakeManufaturers()
        {
            var Manufaturer = AppConnect.model0db.Manufacturers.FirstOrDefault(x => x.IdManufacturer == _products.IdManufacturer);
            AddProductManufacturer.SelectedItem = Manufaturer.NameManufacturer;
        }

        private void AddProductColor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

        private void AddProductMaterial_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[а-яА-Я]"))
            {
                e.Handled = true;
            }
        }

      

        private bool DataTest()
        {
            bool SendMessage = false;
            AddProductNameProduct.BorderBrush = Brushes.Black;
            AddProductColor.BorderBrush = Brushes.Black;
            AddProductMaterial.BorderBrush = Brushes.Black;
            AddProductCost.BorderBrush = Brushes.Black;
            AddProductInStock.BorderBrush = Brushes.Black;
            AddProductManufacturer.BorderBrush = Brushes.Black;

            StatusOfErrorLog = false;
            if (AddProductNameProduct.Text.Length < 1 || AddProductNameProduct.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddProductNameProduct.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваше название не может содержать меньше 1 и больше 150 символов!");
                SendMessage = true;
            }
            if (AddProductColor.Text.Length < 1 || AddProductColor.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddProductColor.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваш цвет не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (AddProductMaterial.Text.Length < 1 || AddProductMaterial.Text.Length > 50)
            {
                StatusOfErrorLog = true;
                AddProductMaterial.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваш материал не может содержать меньше 1 и больше 50 символов!");
                SendMessage = true;
            }
            if (AddProductManufacturer.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                AddProductManufacturer.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Выберите производителя!");
                SendMessage = true;
            }
            int var = 0;
            try
            {
                var = 0;
                var = Int32.Parse(AddProductCost.Text);
            }
            catch
            {
                StatusOfErrorLog = true;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Некорректная цена!");
                SendMessage = true;
            }
            if (AddProductCost.Text.Length <= 0 || var == 0)
            {
                StatusOfErrorLog = true;
                AddProductCost.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Ваша цена равна нулю или пуста!");
                SendMessage = true;
            }
            if (AddProductInStock.Text.Length <= 0)
            {
                StatusOfErrorLog = true;
                AddProductInStock.BorderBrush = Brushes.Red;
                if (!SendMessage)
                    MessageBox.Show("Ошибка: Введите количество товаров в наличии!");
                SendMessage = true;
            }
            return StatusOfErrorLog;
        }

            private void AddProductPick_Click(object sender, RoutedEventArgs e)
        {
            if (!DataTest())
            {
                try
                {
                    if (_products != null && _products.IdProduct != 0)
                    {
                        var EditRecord = AppConnect.model0db.Products.FirstOrDefault(x => x.IdProduct == IdProduct);
                        var idManufaturer = AppConnect.model0db.Manufacturers.FirstOrDefault(x => x.NameManufacturer == AddProductManufacturer.Text);
                        EditRecord.NameProduct = AddProductNameProduct.Text;
                        EditRecord.Color = AddProductColor.Text;
                        EditRecord.Material = AddProductMaterial.Text;
                        EditRecord.Cost = Int32.Parse(AddProductCost.Text);
                        EditRecord.InStock = Int32.Parse(AddProductInStock.Text);
                        EditRecord.IdManufacturer = idManufaturer.IdManufacturer;
                        EditRecord.ImageProduct = NameOfPicture;
                        AppConnect.model0db.SaveChanges();
                    }
                    else
                    {
                        var idManufaturer = AppConnect.model0db.Manufacturers.FirstOrDefault(x => x.NameManufacturer == AddProductManufacturer.Text);
                        Products productObj = new Products()
                        {
                            NameProduct = AddProductNameProduct.Text,
                            Color = AddProductColor.Text,
                            Material = AddProductMaterial.Text,
                            Cost = Int32.Parse(AddProductCost.Text),
                            InStock = Int32.Parse(AddProductInStock.Text),
                            IdManufacturer = idManufaturer.IdManufacturer,
                            ImageProduct = NameOfPicture
                        };
                        AppConnect.model0db.Products.Add(productObj);
                        AppConnect.model0db.SaveChanges();
                    }
                    if (_products != null && _products.IdProduct != 0)
                        MessageBox.Show("Данные успешно обновлены!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Данные успешно добавлены!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                    AppFrame.frameMain.Navigate(new PageAdministrator());
                }
                catch (Exception Ex)
                {
                    if (_products != null && _products.IdProduct != 0)
                        MessageBox.Show("Ошибка при обновлении данных! Ошибка:" + Ex.Message.ToString(), "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("Ошибка при добавлении данных! Ошибка:" + Ex.Message.ToString(), "Уведомление",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _products = null;
        }

        private void AddProductBack_Click(object sender, RoutedEventArgs e)
        {
            _products = null;
            AppFrame.frameMain.Navigate(new PageAdministrator());
        }
    }
}
