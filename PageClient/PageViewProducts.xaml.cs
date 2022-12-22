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
    /// Логика взаимодействия для PageViewProducts.xaml
    /// </summary>
    public partial class PageViewProducts : Page
    {
        public PageViewProducts()
        {
            InitializeComponent();
            ReloadData();
            SortListView();
            FilterListView();
        }

        private void SortListView()
        {
            MenuClientSort.Items.Add("-Без сортировки-");
            MenuClientSort.Items.Add("Стоимость по возрастанию");
            MenuClientSort.Items.Add("Стоимость по убыванию");

            MenuClientSort.SelectedIndex = 0;
        }

        private void FilterListView()
        {
            MenuClientFilter.Items.Add("-Все производители-");
            foreach (var manufacturer in AppConnect.model0db.Manufacturers)
            {
                MenuClientFilter.Items.Add(manufacturer.NameManufacturer);
            }

            MenuClientFilter.SelectedIndex = 0;
        }

        Products[] SortFilterProducts()
        {
            List<Products> products = AppConnect.model0db.Products.ToList();
            var CounterData = products;
            if (MenuClientSearch.Text != null)
            {
                products = products.Where(x => x.NameProduct.ToLower().Contains(MenuClientSearch.Text.ToLower())).ToList();

                switch (MenuClientSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.Cost).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }


            return products.ToArray();
        }

        private void ReloadData()
        {
            ClientProducts.ItemsSource = OptikaDBEntities1.GetContext().Products.ToList();
        }

        private void MenuClientUpdate_Click(object sender, RoutedEventArgs e)
        {
            ReloadData();
        }

        private void MenuClientBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.GoBack();
        }

        private void MenuClientSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ClientProducts.ItemsSource = SortFilterProducts();
        }

        private void MenuClientSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientProducts.ItemsSource = SortFilterProducts();
        }

        private void MenuClientFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientProducts.ItemsSource = SortFilterProducts();
        }
    }
}
