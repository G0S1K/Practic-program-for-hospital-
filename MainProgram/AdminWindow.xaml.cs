using Main_Program.Data;
using MaterialDesignThemes.Wpf;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Syncfusion.UI.Xaml.Grid.Converter;

namespace Main_Program
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private Program_V1Context db;
        private List<ClassHelperRequest> a;

        public AdminWindow()
        {
            InitializeComponent();

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            DarkModeToggleButton.IsChecked = theme.GetBaseTheme() == BaseTheme.Dark;

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e)
                    => DarkModeToggleButton.IsChecked = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
            }

            proguctsGrid1.Visibility = Visibility.Visible;
            LoadRequests();
            
        }

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
            => ModifyTheme(DarkModeToggleButton.IsChecked == true);

        private static void ModifyTheme(bool isDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        public async void LoadRequests()
        {
            using (db = new Program_V1Context())
            {
                var databaseProcedures = new Program_V1ContextProcedures(db);
                var result = await databaseProcedures.QuantityProdAsync();
                proguctsGrid1.ItemsSource = result.ToList();
            }


        }

        public void LoadRequestsAdd()
        {
            this.proguctsGrid2.SearchHelper.ClearSearch();
            using (db = new Program_V1Context())
            {
                var RequestsQuery = from s in db.Requests
                                    join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                    join o in db.Products on g.IdProduct equals o.IdProduct
                                    join r in db.Stores on s.SroreАppointment equals r.IdStore
                                    join e in db.Departments on r.IdDepartment equals e.IdDepartment
                                    where s.SroreАppointment == 1 && s.StoreSource == null
                                    select new ClassHelperRequest()
                                    {
                                        IdRequest = s.IdRequest,
                                        SroreАppointment = e.TypeDepartment,
                                        Date = s.Date,
                                        Name = o.Name,
                                        Quantity = (int)g.Quantity

                                    };
                proguctsGrid2.ItemsSource = RequestsQuery.ToList();
                a = RequestsQuery.ToList();
            }
        }

        public void LoadRequestsRemove()
        {
            this.proguctsGrid3.SearchHelper.ClearSearch();
            using (db = new Program_V1Context())
            {
                var RequestsQuery = from s in db.Requests
                                    join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                    join o in db.Products on g.IdProduct equals o.IdProduct
                                    join r in db.Stores on s.SroreАppointment equals r.IdStore
                                    join e in db.Departments on r.IdDepartment equals e.IdDepartment
                                    where s.StoreSource == 1
                                    select new ClassHelperRequest()
                                    {
                                        IdRequest = s.IdRequest,
                                        SroreАppointment = e.TypeDepartment,
                                        Date = s.Date,
                                        Name = o.Name,
                                        Quantity = (int)g.Quantity

                                    };
                proguctsGrid3.ItemsSource = RequestsQuery.ToList();
            }
        }

        private void PopupBox_OnClosed(object sender, RoutedEventArgs e)
        {

        }

        private void PopupBox_OnOpened(object sender, RoutedEventArgs e)
        {

        }

        private void Kolich_Click(object sender, RoutedEventArgs e)
        {
            proguctsGrid2.Visibility = Visibility.Hidden;
            proguctsGrid3.Visibility = Visibility.Hidden;
            proguctsGrid1.Visibility = Visibility.Visible;
            LoadRequests();
        }

        private void Kolich_Click2(object sender, RoutedEventArgs e)
        {
            proguctsGrid2.Visibility = Visibility.Visible;
            proguctsGrid3.Visibility = Visibility.Hidden;
            proguctsGrid1.Visibility = Visibility.Hidden;
            LoadRequestsAdd();
        }

        private void Kolich_Click3(object sender, RoutedEventArgs e)
        {
            proguctsGrid3.Visibility = Visibility.Visible;
            proguctsGrid1.Visibility = Visibility.Hidden;
            proguctsGrid2.Visibility = Visibility.Hidden;
            LoadRequestsRemove();
        }

        private void Add_Button(object sender, RoutedEventArgs e)
        {
            AddAdminWindow addWindow = new AddAdminWindow();
            addWindow.ShowDialog();
        }



        private void Button_Del(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid2.IsVisible == true)
            {
                if (proguctsGrid2.SelectedItem != null)
                {
                    var b = (ClassHelperRequest)proguctsGrid2.SelectedItem;

                    using (db = new Program_V1Context())
                    {
                        db.RequestsProducts.RemoveRange(db.RequestsProducts.Where(u => u.IdRequest == b.IdRequest));
                        db.Requests.RemoveRange(db.Requests.Where(u => u.IdRequest == b.IdRequest));
                        db.SaveChanges();
                        LoadRequestsAdd();
                        MessageBox.Show("Удаление успешно выполнено");
                    }
                }
                else MessageBox.Show("Заказ не выбран");
            }
            else MessageBox.Show("Выберите таблицу \"Добавленные\"");

        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid2.IsVisible == true)
            {
                if (proguctsGrid2.SelectedItem != null)
                {
                    EditAdminWindow editAdminWindow = new EditAdminWindow();
                    editAdminWindow.Show();

                    var b = (ClassHelperRequest)proguctsGrid2.SelectedItem;

                    editAdminWindow.IdReq = b.IdRequest;

                    using (db = new Program_V1Context())
                    {
                        var editColection = db.RequestsProducts.Where(u => u.IdRequest == b.IdRequest);
                        foreach (var item in editColection)
                        {
                            if (item.IdProduct == 1)
                            {
                                editAdminWindow.cb1.IsChecked = true;
                                editAdminWindow.tb1.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 2)
                            {
                                editAdminWindow.cb2.IsChecked = true;
                                editAdminWindow.tb2.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 3)
                            {
                                editAdminWindow.cb3.IsChecked = true;
                                editAdminWindow.tb3.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 4)
                            {
                                editAdminWindow.cb4.IsChecked = true;
                                editAdminWindow.tb4.Text = item.Quantity.ToString();

                            }
                        }
                    }
                }
                else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
            }
            else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
        }



        private void RowId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (proguctsGrid2.IsVisible == true)
            {
                this.proguctsGrid2.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid2.SearchHelper.Search(RowId.Text);
            }
            else if (proguctsGrid3.IsVisible == true)
            {
                this.proguctsGrid3.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid3.SearchHelper.Search(RowId.Text);
            }
        }

        private void Excel(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid1.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid1.ExportToExcel(proguctsGrid1.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("adminDG1.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл adminDG1.xlsx");
            }
            else if (proguctsGrid2.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid2.ExportToExcel(proguctsGrid2.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("adminDG2.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл adminDG2.xlsx");
            }
            else if (proguctsGrid3.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid3.ExportToExcel(proguctsGrid3.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("adminDG3.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл adminDG3.xlsx");
            }
            else MessageBox.Show("Выберите таблицу");

        }

        private void PechatZaprosa(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid2.IsVisible == true)
            {
                if (proguctsGrid2.SelectedItem != null)
                {

                    var b = (ClassHelperRequest)proguctsGrid2.SelectedItem;

                    using (db = new Program_V1Context())
                    {
                        
                        var RequestsQuery = from s in db.Requests
                                            join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                            join o in db.Products on g.IdProduct equals o.IdProduct
                                            join r in db.Stores on s.SroreАppointment equals r.IdStore
                                            join y in db.Departments on r.IdDepartment equals y.IdDepartment
                                            where s.SroreАppointment == 1 && s.StoreSource == null && g.IdRequest == b.IdRequest
                                            select new ClassHelperRequest()
                                            {
                                                IdRequest = s.IdRequest,
                                                SroreАppointment = y.TypeDepartment,
                                                Date = s.Date.ToString(),
                                                Name = o.Name,
                                                Quantity = (int)g.Quantity

                                            };
                        var a = RequestsQuery.ToList();

                        var document = new WordDocument("ShablonDlyaZakazaAdmin.docx", FormatType.Docx);

                        string[] kol = { "kol1", "kol2", "kol3", "kol4" };
                        string[] date = { "date1", "date2", "date3", "date4" };
                        string[] prod = { "prod1", "prod2", "prod3", "prod4" };


                        var bookmarkHelper = new BookmarkHelper(document);

                        bookmarkHelper.MyMethod("zakaz", b.IdRequest.ToString());
                        bookmarkHelper.MyMethod("date", DateTime.Today.ToString("dd.MM.yyyy"));
                        bookmarkHelper.MyMethod("name", "Gleb korotkeivh");

                        for (int i = 0; i < a.Count; i++)
                        {
                            bookmarkHelper.MyMethod($"{date[i]}", $"{a[i].Date}");
                            bookmarkHelper.MyMethod($"{prod[i]}", $"{a[i].Name}");
                            bookmarkHelper.MyMethod($"{kol[i]}", $"{a[i].Quantity}");
                        }

                        document.Save($"Zapros_{b.IdRequest}.docx", FormatType.Docx);
                        document.Close();
                        MessageBox.Show("Печать завершена");

                    }
                }
                else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
            }
            else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
        }

        private async void PechatKol(object sender, RoutedEventArgs e)
        {
            using (db = new Program_V1Context())
            {
                var databaseProcedures = new Program_V1ContextProcedures(db);
                var result = await databaseProcedures.QuantityProdAsync();
                var a = result.ToList();

                var document = new WordDocument("ShablonKolAdmin.docx", FormatType.Docx);

                string[] kol = { "kol1", "kol2", "kol3", "kol4" };
                string[] prod = { "prod1", "prod2", "prod3", "prod4" };


                var bookmarkHelper = new BookmarkHelper(document);

                bookmarkHelper.MyMethod("date", DateTime.Today.ToString("dd.MM.yyyy"));
                bookmarkHelper.MyMethod("name", "Gleb korotkeivh");

                for (int i = 0; i < a.Count; i++)
                {
                    bookmarkHelper.MyMethod($"{prod[i]}", $"{a[i].ProductName}");
                    bookmarkHelper.MyMethod($"{kol[i]}", $"{a[i].Quantity}");
                }

                document.Save($"kolNaAdmin.docx", FormatType.Docx);
                document.Close();
                MessageBox.Show("Печать завершена");
            }
        }
    }
}
