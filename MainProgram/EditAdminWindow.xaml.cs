using Main_Program.Data;
using Main_Program.Models;
using Main_Program.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main_Program
{
    /// <summary>
    /// Логика взаимодействия для EditAdminWindow.xaml
    /// </summary>
    public partial class EditAdminWindow : Window
    {
        private Program_V1Context db;
        public int IdReq;
        public EditAdminWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            tb1.IsEnabled = true;
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            tb1.IsEnabled = false;
        }
        private void CheckBox2_Checked(object sender, RoutedEventArgs e)
        {
            tb2.IsEnabled = true;
        }

        private void CheckBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            tb2.IsEnabled = false;
        }

        private void CheckBox3_Checked(object sender, RoutedEventArgs e)
        {
            tb3.IsEnabled = true;
        }

        private void CheckBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            tb3.IsEnabled = false;
        }

        private void CheckBox4_Checked(object sender, RoutedEventArgs e)
        {
            tb4.IsEnabled = true;
        }

        private void CheckBox4_Unchecked(object sender, RoutedEventArgs e)
        {
            tb4.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (db = new Program_V1Context())
            {
                db.RequestsProducts.RemoveRange(db.RequestsProducts.Where(u => u.IdRequest == IdReq));
                db.SaveChanges();
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.LoadRequestsAdd();
            }

            if (cb1.IsChecked == true)
            {
                if ((tb1.Text != "") && (int.Parse(tb1.Text) != 0))
                {
                    using (db = new Program_V1Context())
                    {
                        RequestsProducts newReqProduct1 = new RequestsProducts()
                        {
                            IdRequest = IdReq,
                            IdProduct = 1,
                            Quantity = int.Parse(tb1.Text)
                        };
                        db.RequestsProducts.Add(newReqProduct1);
                        db.SaveChanges();
                        MessageBox.Show($"Количество антигрипина отредактировано");
                    };
                }
                else MessageBox.Show("Первое поле не введено или равно 0");

            }

            //редактирвание 2 товара (если выбран)
            if (cb2.IsChecked == true)
            {
                if ((tb2.Text != "") && (int.Parse(tb2.Text) != 0))
                {
                    using (db = new Program_V1Context())
                    {
                        RequestsProducts newReqProduct2 = new RequestsProducts()
                        {
                            IdRequest = IdReq,
                            IdProduct = 2,
                            Quantity = int.Parse(tb2.Text)
                        };
                        db.RequestsProducts.Add(newReqProduct2);
                        db.SaveChanges();
                        MessageBox.Show($"Количество парацетомола отредактировано");
                    };
                }
                else MessageBox.Show("Второе поле не введено или равно 0");

            }

            //добавление 3 товара (если выбран)
            if (cb3.IsChecked == true)
            {
                if ((tb3.Text != "") && (int.Parse(tb3.Text) != 0))
                {
                    using (db = new Program_V1Context())
                    {
                        RequestsProducts newReqProduct3 = new RequestsProducts()
                        {
                            IdRequest = IdReq,
                            IdProduct = 3,
                            Quantity = int.Parse(tb3.Text)
                        };
                        db.RequestsProducts.Add(newReqProduct3);
                        db.SaveChanges();
                        MessageBox.Show($"Количество валерьянки отредактировано");
                    };
                }
                else MessageBox.Show("Третье поле не введено или равно 0");

            }

            //добавление 4 товара (если выбран)
            if (cb4.IsChecked == true)
            {
                if ((tb4.Text != "") && (int.Parse(tb4.Text) != 0))
                {
                    using (db = new Program_V1Context())
                    {
                        RequestsProducts newReqProduct4 = new RequestsProducts()
                        {
                            IdRequest = IdReq,
                            IdProduct = 4,
                            Quantity = int.Parse(tb4.Text)
                        };
                        db.RequestsProducts.Add(newReqProduct4);
                        db.SaveChanges();
                        MessageBox.Show($"Количество наркотиков отредактировано");
                    };
                }
                else MessageBox.Show("Четверное поле не введено или равно 0");
            }


        }
    }
}
