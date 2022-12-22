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
using WPFApplicationOptika.PageMain;

namespace WPFApplicationOptika.PageAdmin
{
    /// <summary>
    /// Логика взаимодействия для PageAdministrator.xaml
    /// </summary>  
    public partial class PageAdministrator : Page
    {
        public PageAdministrator()
        {
            InitializeComponent();
            ReloadData();
            SortProducts();
            FilterProducts();
            SortUsers();
            FilterUsers();
        }

        Users[] ReloadFilterAndSortingUsers()
        {
            List<Users> users = AppConnect.model0db.Users.ToList();
            var CounterData = users;
            if (MenuUsersSearch.Text != null)
            {
                users = users.Where(x => x.Surname.ToLower().Contains(MenuUsersSearch.Text.ToLower())).ToList();

                switch (MenuUsersSort.SelectedIndex)
                {
                    case 1:
                        users = users.OrderBy(x => x.Surname).ToList();
                        break;
                    case 2:
                        users = users.OrderByDescending(x => x.Surname).ToList();
                        break;
                }
            }

            if (MenuUsersFilter.SelectedIndex > 0)
            {
                users = users.Where(x => x.Roles.Name == MenuUsersFilter.SelectedItem.ToString()).ToList();
            }

            if (users.Count != 0)
            {
                UsersCounter.Content = "Показано пользователей: " + users.Count + " из " + CounterData.Count;
            }
            else
            {
                UsersCounter.Content = "Не найдено";
            }

            return users.ToArray();
        }

        Products[] ReloadFilterAndSortingProduts()
        {
            List<Products> products = AppConnect.model0db.Products.ToList();
            var CounterData = products;
            if (MenuProductsSearch.Text != null)
            {
                products = products.Where(x => x.NameProduct.ToLower().Contains(MenuProductsSearch.Text.ToLower())).ToList();

                switch (MenuProductsSort.SelectedIndex)
                {
                    case 1:
                        products = products.OrderBy(x => x.Cost).ToList();
                        break;
                    case 2:
                        products = products.OrderByDescending(x => x.Cost).ToList();
                        break;
                }
            }

            if (MenuProductsFilter.SelectedIndex > 0)
            {
                products = products.Where(x => x.Manufacturers.NameManufacturer == MenuProductsFilter.SelectedItem.ToString()).ToList();
            }

            if (products.Count != 0)
            {
                ProductsCounter.Content = "Показано товаров: " + products.Count + " из " + CounterData.Count;
            }
            else
            {
                ProductsCounter.Content = "Не найдено";
            }

            return products.ToArray();
        }

        private void ReloadData()
        {
            ListViewProducts.ItemsSource = OptikaDBEntities1.GetContext().Products.ToList();
            ListViewUsers.ItemsSource = OptikaDBEntities1.GetContext().Users.ToList();
        }

        private void SortProducts()
        {
            MenuProductsSort.Items.Add("-Без сортировки-");
            MenuProductsSort.Items.Add("Стоимость по возрастанию");
            MenuProductsSort.Items.Add("Стоимость по убыванию");

            MenuProductsSort.SelectedIndex = 0;
        }

        private void SortUsers()
        {
            MenuUsersSort.Items.Add("-Без сортировки-");
            MenuUsersSort.Items.Add("Фамилия по возрастанию");
            MenuUsersSort.Items.Add("Фамилия по убыванию");

            MenuUsersSort.SelectedIndex = 0;
        }

        private void FilterProducts()
        {
            MenuProductsFilter.Items.Add("-Все производители-");
            foreach (var manufacturers in AppConnect.model0db.Manufacturers)
            {
                MenuProductsFilter.Items.Add(manufacturers.NameManufacturer);
            }

            MenuProductsFilter.SelectedIndex = 0;
        }

        private void FilterUsers()
        {
            MenuUsersFilter.Items.Add("-Все роли-");
            foreach (var role in AppConnect.model0db.Roles)
            {
                MenuUsersFilter.Items.Add(role.Name);
            }
            MenuUsersFilter.SelectedIndex = 0;
        }

        private void MenuProductsSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ListViewProducts.ItemsSource = ReloadFilterAndSortingProduts();
        }

        private void MenuProductsSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewProducts.ItemsSource = ReloadFilterAndSortingProduts();
        }

        private void MenuProductsFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewProducts.ItemsSource = ReloadFilterAndSortingProduts();
        }

        private void ProductsUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListViewProducts.Items.Refresh();
        }

        private void ProductsAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddProduct());
        }

        private void ProductsEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productsObj = ListViewProducts.SelectedItems.Cast<Products>().ToList().ElementAt(0);
                Products products = new Products()
                {
                    IdProduct = productsObj.IdProduct,
                    NameProduct = productsObj.NameProduct,
                    Color = productsObj.Color,
                    Material = productsObj.Material,
                    Cost = productsObj.Cost,
                    InStock = productsObj.InStock,
                    IdManufacturer = productsObj.IdManufacturer,
                    ImageProduct = productsObj.ImageProduct
                };
                AppFrame.frameMain.Navigate(new PageAddProduct(products));
            }
            catch
            {
                MessageBox.Show("Выберите запись для редактирования!");
            }
        }

        private void ProductsDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var productsObj = ListViewProducts.SelectedItems.Cast<Products>().ToList().ElementAt(0);
                if (MessageBox.Show("Вы подтверждаете безвозвратное удаление записи?", "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var addvar = OptikaDBEntities1.GetContext().Products.Where(x => x.IdProduct == productsObj.IdProduct).FirstOrDefault();
                        OptikaDBEntities1.GetContext().Products.Remove(addvar);
                        OptikaDBEntities1.GetContext().SaveChanges();
                        MessageBox.Show("Запись успешна удалёна!");

                        ListViewProducts.ItemsSource = OptikaDBEntities1.GetContext().Products.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Выберите запись для удаления!");
            }
        }

        private void MenuUsersSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ListViewUsers.ItemsSource = ReloadFilterAndSortingUsers();
        }

        private void MenuUsersSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewUsers.ItemsSource = ReloadFilterAndSortingUsers();
        }

        private void MenuUsersFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewUsers.ItemsSource = ReloadFilterAndSortingUsers();
        }

        private void UsersUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListViewUsers.Items.Refresh();
        }

        private void UsersAdd_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageAddUser());
        }

        private void UsersEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var usersObj = ListViewUsers.SelectedItems.Cast<Users>().ToList().ElementAt(0);
                Users users = new Users()
                {
                    IdUser = usersObj.IdUser,
                    Login = usersObj.Login,
                    Password = usersObj.Password,
                    IdRole = usersObj.IdRole,
                    Surname = usersObj.Surname,
                    Name = usersObj.Name,
                    MiddleName = usersObj.MiddleName,
                    DateOfBirth = usersObj.DateOfBirth,
                    Email = usersObj.Email,
                    NumberPhone = usersObj.NumberPhone
                };
                AppFrame.frameMain.Navigate(new PageAddUser(users));
            }
            catch
            {
                MessageBox.Show("Выберите запись для редактирования!");
            }
        }

        private void UsersDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы подтверждаете безвозвратное удаление записи?", "Предупреждение!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var usersObj = ListViewUsers.SelectedItems.Cast<Users>().ToList().ElementAt(0);
                        var addvar = OptikaDBEntities1.GetContext().Users.Where(x => x.IdUser == usersObj.IdUser).FirstOrDefault();
                        OptikaDBEntities1.GetContext().Users.Remove(addvar);
                        OptikaDBEntities1.GetContext().SaveChanges();
                        MessageBox.Show("Запись успешна удалёна!");

                        ListViewUsers.ItemsSource = OptikaDBEntities1.GetContext().Users.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Выберите запись для удаления!");
            }
        }

        private void MenuAdminBack_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.frameMain.Navigate(new PageLogin());
            SelectedElement.TakeStatusOfUserLabel(false, "Незнакомец");
            SelectedElement.TakeStatusOfUserButton(false);
        }
    }
}
