using Main_Program.Data;
using MaterialDesignThemes.Wpf;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.UI.Xaml.Grid.Converter;
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

namespace Main_Program
{
    /// <summary>
    /// Логика взаимодействия для Departments3Window.xaml
    /// </summary>
    public partial class Departments3Window : Window
    {
        private Program_V1Context db;

        public Departments3Window()
        {
            InitializeComponent();

            InitializeComponent();

            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            DarkModeToggleButton.IsChecked = theme.GetBaseTheme() == BaseTheme.Dark;

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e)
                    => DarkModeToggleButton.IsChecked = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
            }

            proguctsGrid2.Visibility = Visibility.Visible;
            LoadRequestsDepart();
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

        public async void LoadRequestsDepart()
        {
            using (db = new Program_V1Context())
            {
                var databaseProcedures = new Program_V1ContextProcedures(db);
                var result = await databaseProcedures.QuantityProdDepartment3Async();
                proguctsGrid2.ItemsSource = result.ToList();
            }


        }

        public async void LoadRequestsStore()
        {
            using (db = new Program_V1Context())
            {
                var databaseProcedures = new Program_V1ContextProcedures(db);
                var result = await databaseProcedures.QuantityProdGlavMedAsync();
                proguctsGrid1.ItemsSource = result.ToList();
            }


        }

        public void LoadRequestsAdd()
        {
            this.proguctsGrid3.SearchHelper.ClearSearch();
            using (db = new Program_V1Context())
            {
                var RequestsQuery = from s in db.Requests
                                    join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                    join o in db.Products on g.IdProduct equals o.IdProduct
                                    join r in db.Stores on s.StoreSource equals r.IdStore
                                    join e in db.Departments on r.IdDepartment equals e.IdDepartment
                                    where s.SroreАppointment == 5 && s.StoreSource == 2
                                    select new ClassHelperRequest()
                                    {
                                        IdRequest = s.IdRequest,
                                        StoreSource = e.TypeDepartment,
                                        Date = s.Date,
                                        Name = o.Name,
                                        Quantity = (int)g.Quantity

                                    };
                proguctsGrid3.ItemsSource = RequestsQuery.ToList();
            }
        }

        public void LoadRequestsRemove()
        {
            this.proguctsGrid4.SearchHelper.ClearSearch();
            using (db = new Program_V1Context())
            {
                var RequestsQuery = from s in db.Requests
                                    join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                    join o in db.Products on g.IdProduct equals o.IdProduct
                                    join r in db.Stores on s.SroreАppointment equals r.IdStore
                                    join e in db.Departments on r.IdDepartment equals e.IdDepartment
                                    where s.StoreSource == 5
                                    select new ClassHelperRequest()
                                    {
                                        IdRequest = s.IdRequest,
                                        SroreАppointment = e.TypeDepartment,
                                        Date = s.Date,
                                        Name = o.Name,
                                        Quantity = (int)g.Quantity

                                    };
                proguctsGrid4.ItemsSource = RequestsQuery.ToList();
            }
        }

        private void Kolich_Click3(object sender, RoutedEventArgs e)
        {
            proguctsGrid1.Visibility = Visibility.Hidden;
            proguctsGrid2.Visibility = Visibility.Hidden;
            proguctsGrid3.Visibility = Visibility.Visible;
            proguctsGrid4.Visibility = Visibility.Hidden;
            LoadRequestsAdd();
        }

        private void Kolich_Click4(object sender, RoutedEventArgs e)
        {
            proguctsGrid1.Visibility = Visibility.Hidden;
            proguctsGrid2.Visibility = Visibility.Hidden;
            proguctsGrid3.Visibility = Visibility.Hidden;
            proguctsGrid4.Visibility = Visibility.Visible;
            LoadRequestsRemove();
        }

        private void Kolich_Click1(object sender, RoutedEventArgs e)
        {
            proguctsGrid1.Visibility = Visibility.Visible;
            proguctsGrid2.Visibility = Visibility.Hidden;
            proguctsGrid3.Visibility = Visibility.Hidden;
            proguctsGrid4.Visibility = Visibility.Hidden;
            LoadRequestsStore();
        }

        private void Kolich_Click2(object sender, RoutedEventArgs e)
        {
            proguctsGrid1.Visibility = Visibility.Hidden;
            proguctsGrid2.Visibility = Visibility.Visible;
            proguctsGrid3.Visibility = Visibility.Hidden;
            proguctsGrid4.Visibility = Visibility.Hidden;
            LoadRequestsDepart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddDepartments3Window addDepartments3Window = new AddDepartments3Window();
            addDepartments3Window.ShowDialog();
        }

        private void PopupBox_OnClosed(object sender, RoutedEventArgs e)
        {

        }

        private void PopupBox_OnOpened(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Del(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid3.IsVisible == true)
            {
                if (proguctsGrid3.SelectedItem != null)
                {
                    var b = (ClassHelperRequest)proguctsGrid3.SelectedItem;

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
            else if (proguctsGrid4.IsVisible == true)
            {
                if (proguctsGrid4.SelectedItem != null)
                {
                    var b = (ClassHelperRequest)proguctsGrid4.SelectedItem;

                    using (db = new Program_V1Context())
                    {
                        db.RequestsProducts.RemoveRange(db.RequestsProducts.Where(u => u.IdRequest == b.IdRequest));
                        db.Requests.RemoveRange(db.Requests.Where(u => u.IdRequest == b.IdRequest));
                        db.SaveChanges();
                        LoadRequestsRemove();
                        MessageBox.Show("Удаление успешно выполнено");
                    }
                }
                else MessageBox.Show("Заказ не выбран");
            }
            else MessageBox.Show("Выберите таблицу");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid3.IsVisible == true)
            {
                if (proguctsGrid3.SelectedItem != null)
                {
                    EditDepartments3Window editDepartments3Window = new EditDepartments3Window();
                    editDepartments3Window.Show();

                    var b = (ClassHelperRequest)proguctsGrid3.SelectedItem;

                    editDepartments3Window.IdReq = b.IdRequest;

                    using (db = new Program_V1Context())
                    {
                        var editColection = db.RequestsProducts.Where(u => u.IdRequest == b.IdRequest);
                        foreach (var item in editColection)
                        {
                            if (item.IdProduct == 1)
                            {
                                editDepartments3Window.cb1.IsChecked = true;
                                editDepartments3Window.tb1.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 2)
                            {
                                editDepartments3Window.cb2.IsChecked = true;
                                editDepartments3Window.tb2.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 3)
                            {
                                editDepartments3Window.cb3.IsChecked = true;
                                editDepartments3Window.tb3.Text = item.Quantity.ToString();
                            }
                            else if (item.IdProduct == 4)
                            {
                                editDepartments3Window.cb4.IsChecked = true;
                                editDepartments3Window.tb4.Text = item.Quantity.ToString();

                            }
                        }
                    }
                }
                else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
            }
            else MessageBox.Show("Выбирите таблицу \"Добавленные\" и запись в ней");
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid3.IsVisible == true)
            {
                this.proguctsGrid3.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid3.SearchHelper.Search(RowId.Text);
            }
            else if (proguctsGrid4.IsVisible == true)
            {
                this.proguctsGrid4.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid4.SearchHelper.Search(RowId.Text);
            }
        }

        private void RowId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (proguctsGrid3.IsVisible == true)
            {
                this.proguctsGrid3.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid3.SearchHelper.Search(RowId.Text);
            }
            else if (proguctsGrid4.IsVisible == true)
            {
                this.proguctsGrid4.SearchHelper.SearchBrush = (Brush)(new BrushConverter().ConvertFrom("#00bfff"));
                this.proguctsGrid4.SearchHelper.Search(RowId.Text);
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            UsedDepartments3Window usedDepartments3Window = new UsedDepartments3Window();
            usedDepartments3Window.ShowDialog();    
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid1.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid1.ExportToExcel(proguctsGrid1.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("department3DG1.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл department3DG1.xlsx");
            }
            else if (proguctsGrid2.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid2.ExportToExcel(proguctsGrid2.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("department3DG2.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл department3DG2.xlsx");
            }
            else if (proguctsGrid3.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid3.ExportToExcel(proguctsGrid3.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("department3DG3.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл department3DG3.xlsx");
            }
            else if (proguctsGrid4.IsVisible == true)
            {
                var options = new ExcelExportingOptions();
                options.ExportMode = ExportMode.Value;
                var excelEngine = proguctsGrid4.ExportToExcel(proguctsGrid4.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                workBook.SaveAs("department3DG4.xlsx");
                MessageBox.Show("Программа успешно экспортировала данные в файл. \nЗайдите в папку с программой и найдите файл department3DG4.xlsx");
            }
            else MessageBox.Show("Выберите таблицу");
        }

        private void PechatZaprosa(object sender, RoutedEventArgs e)
        {
            if (proguctsGrid3.IsVisible == true)
            {
                if (proguctsGrid3.SelectedItem != null)
                {

                    var b = (ClassHelperRequest)proguctsGrid3.SelectedItem;

                    using (db = new Program_V1Context())
                    {

                        var RequestsQuery = from s in db.Requests
                                            join g in db.RequestsProducts on s.IdRequest equals g.IdRequest
                                            join o in db.Products on g.IdProduct equals o.IdProduct
                                            join r in db.Stores on s.SroreАppointment equals r.IdStore
                                            join y in db.Departments on r.IdDepartment equals y.IdDepartment
                                            where s.SroreАppointment == 5 && s.StoreSource == 2 && g.IdRequest == b.IdRequest
                                            select new ClassHelperRequest()
                                            {
                                                IdRequest = s.IdRequest,
                                                SroreАppointment = y.TypeDepartment,
                                                Date = s.Date.ToString(),
                                                Name = o.Name,
                                                Quantity = (int)g.Quantity

                                            };
                        var a = RequestsQuery.ToList();

                        var document = new WordDocument("ShablonDlyaZakazaDep3.docx", FormatType.Docx);

                        string[] kol = { "kol1", "kol2", "kol3", "kol4" };
                        string[] date = { "date1", "date2", "date3", "date4" };
                        string[] prod = { "prod1", "prod2", "prod3", "prod4" };


                        var bookmarkHelper = new BookmarkHelper(document);

                        bookmarkHelper.MyMethod("zakaz", b.IdRequest.ToString());
                        bookmarkHelper.MyMethod("date", DateTime.Today.ToString("dd.MM.yyyy"));
                        bookmarkHelper.MyMethod("name", "Ilya Kushner");

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
                var result = await databaseProcedures.QuantityProdDepartment3Async();
                var a = result.ToList();

                var document = new WordDocument("ShablonKolDep3.docx", FormatType.Docx);

                string[] kol = { "kol1", "kol2", "kol3", "kol4" };
                string[] prod = { "prod1", "prod2", "prod3", "prod4" };


                var bookmarkHelper = new BookmarkHelper(document);

                bookmarkHelper.MyMethod("date", DateTime.Today.ToString("dd.MM.yyyy"));
                bookmarkHelper.MyMethod("name", "Ilya Kushner");

                for (int i = 0; i < a.Count; i++)
                {
                    bookmarkHelper.MyMethod($"{prod[i]}", $"{a[i].ProductName}");
                    bookmarkHelper.MyMethod($"{kol[i]}", $"{a[i].Quantity}");
                }

                document.Save($"kolDep3.docx", FormatType.Docx);
                document.Close();
                MessageBox.Show("Печать завершена");
            }
        }
    }
}
