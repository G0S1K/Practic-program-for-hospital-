using Main_Program.Data;
using Main_Program.ViewModels;
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

namespace Main_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Program_V1Context db;
        private int role;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text != "" || PasswordBox.Password != "")
            {
                using (db = new Program_V1Context())
                {
                    if (db.Users.Any(user => user.Login == NameTextBox.Text && user.Password == PasswordBox.Password))
                    {
                        role = (int)db.Users.Where(u => u.Login == NameTextBox.Text && u.Password == PasswordBox.Password).Select(u => u.IdRole).SingleOrDefault();

                    }
                }
                if (role == 1)
                {
                    AdminWindow adminWindow = new AdminWindow();
                    this.Hide();
                    adminWindow.ShowDialog();
                    this.Show();
                    role = 0;
                }
                else if (role == 2)
                {
                    GlavMedWindow mainUser = new GlavMedWindow();
                    this.Hide();
                    mainUser.ShowDialog();
                    this.Show();
                    role = 0;
                }
                else if (role == 3)
                {
                    Departments1Window departments1Window = new Departments1Window();
                    this.Hide();
                    departments1Window.ShowDialog();
                    this.Show();
                    role = 0;
                }
                else if (role == 4)
                {
                    Departments2Window departments2Window = new Departments2Window();
                    this.Hide();
                    departments2Window.ShowDialog();
                    this.Show();
                    role = 0;
                }
                else if (role == 5)
                {
                    Departments3Window departments3Window = new Departments3Window();
                    this.Hide();
                    departments3Window.ShowDialog();
                    this.Show();
                    role = 0;
                }
                else MessageBox.Show("Такого логина или пароля не существует");
            }
            else MessageBox.Show("Заполните все поля");
            
        }
    }
}
