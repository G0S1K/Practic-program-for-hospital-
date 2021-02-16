using Main_Program.Data;
using Main_Program.Models;
using Main_Program.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Linq;

namespace Main_Program
{
    /// <summary>
    /// Логика взаимодействия для UsedDepartments3Window.xaml
    /// </summary>
    public partial class UsedDepartments3Window : Window
    {
        public Program_V1Context db;

        public UsedDepartments3Window()
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

        public bool Valid(string texb, int count)
        {
            if ((texb != "") && (int.Parse(texb) != 0) && (count >= int.Parse(texb)))
            {
                return true;
            }
            else return false;
        }

        public class SumProduct
        {
            public int SumP { get; set; }
        }
        // считает сумму товаров
        private int RequestsCount(int id)
        {
            int countProduct1 = 0;
            int countProduct2 = 0;
            using (db = new Program_V1Context())
            {
                var RequestsQuery1 = from s in db.Requests
                                     join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                     where s.SroreАppointment == 5 && g.IdProduct == id
                                     select new SumProduct()
                                     {
                                         SumP = (int)g.Quantity
                                     };

                foreach (var item in RequestsQuery1.ToList())
                {
                    countProduct1 += item.SumP;
                }

                var RequestsQuery2 = from s in db.Requests
                                     join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                     where s.StoreSource == 5 && g.IdProduct == id
                                     select new SumProduct()
                                     {
                                         SumP = (int)g.Quantity
                                     };

                foreach (var item in RequestsQuery2.ToList())
                {
                    countProduct2 += item.SumP;
                }
                return countProduct1 - countProduct2;
            }
        }

        // добавляет товары к запросу
        private void AddRequestsProducts(TextBox tb, int idProduct, CheckBox cb, int idAddRequest)
        {
            if (cb.IsChecked == true)
            {
                if (Valid(tb.Text, RequestsCount(idProduct)))
                {
                    using (db = new Program_V1Context())
                    {
                        RequestsProducts newReqProduct1 = new RequestsProducts()
                        {
                            IdRequest = idAddRequest,
                            IdProduct = idProduct,
                            Quantity = int.Parse(tb.Text)
                        };
                        db.RequestsProducts.Add(newReqProduct1);
                        db.SaveChanges();
                    };
                }
                else MessageBox.Show("Первое поле не введено или равно 0");

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int idAddRequest = 0;


            if (cb1.IsChecked == true || cb2.IsChecked == true || cb3.IsChecked == true || cb4.IsChecked == true)
            {
                if (Valid(tb1.Text, RequestsCount(1)) || Valid(tb2.Text, RequestsCount(2)) || Valid(tb3.Text, RequestsCount(3)) || Valid(tb4.Text, RequestsCount(4)))
                {
                    using (db = new Program_V1Context())
                    {
                        Requests newRequest = new Requests()
                        {
                            SroreАppointment = 7,
                            StoreSource = 5,
                            Date = DateTime.Today.ToString("dd.MM.yyyy")
                        };

                        db.Requests.Add(newRequest);
                        db.SaveChanges();

                        idAddRequest = newRequest.IdRequest;
                    }
                }

                if (cb1.IsChecked == true)
                {
                    if (Valid(tb1.Text, RequestsCount(1)))
                    {
                        AddRequestsProducts(tb1, 1, cb1, idAddRequest);
                        MessageBox.Show($"Израсходовано {tb1.Text} антигрипина");
                    }
                    else MessageBox.Show($"Поле некоректно или превышает исходное количество \nИх количество = {RequestsCount(1)}");
                }

                if (cb2.IsChecked == true)
                {
                    if (Valid(tb2.Text, RequestsCount(2)))
                    {
                        AddRequestsProducts(tb2, 2, cb2, idAddRequest);
                        MessageBox.Show($"Израсходовано {tb2.Text} парацетомола");
                    }
                    else MessageBox.Show($"Поле некоректно или превышает исходное количество \nИх количество = {RequestsCount(2)}");
                }

                if (cb3.IsChecked == true)
                {
                    if (Valid(tb3.Text, RequestsCount(3)))
                    {
                        AddRequestsProducts(tb3, 3, cb3, idAddRequest);
                        MessageBox.Show($"Израсходовано {tb3.Text} валерьянки");
                    }
                    else MessageBox.Show($"Поле некоректно или превышает исходное количество \nИх количество = {RequestsCount(3)}");
                }

                if (cb4.IsChecked == true)
                {
                    if (Valid(tb4.Text, RequestsCount(4)))
                    {
                        AddRequestsProducts(tb4, 4, cb4, idAddRequest);
                        MessageBox.Show($"Израсходовано {tb4.Text} наркотиков");
                    }
                    else MessageBox.Show($"Поле некоректно или превышает исходное количество \nИх количество = {RequestsCount(4)}");
                }
                this.Close();
            }
            else MessageBox.Show("Ничего не выбрано");
        }
    }
}
