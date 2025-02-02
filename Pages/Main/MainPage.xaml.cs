using CourseWork25.BD;
using CourseWork25.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Data.Entity;
using System.IO;

namespace CourseWork25.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DtgClientsView.ItemsSource = CourseWork25Entities.GetContext().Interactions
                .Include(c => c.Clients)
                .Include(c => c.Clients.ClientCategories)
                .Include(c => c.InteractionType)
                .ToList();
            CheckUserRole();
        }

        private void CheckUserRole()
        {
            if(ClassFrame.Role_Id == 1)
            {
                Statistika.Visibility = Visibility.Collapsed;
            }
            else
            {
                Statistika.Visibility = Visibility.Visible;
            }
        }

        private void ExtAcc_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.Logon.Authorization());
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = Search.Text;

            var filteredOrders = CourseWork25Entities.GetContext().Interactions
                .Where(o => o.Clients.LastName.Contains(search))
                .ToList();

            DtgClientsView.ItemsSource = filteredOrders;
        }

        private void AddContract_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new SelectedClient(this, null));
        }
        private void DelContract_Click(object sender, RoutedEventArgs e)
        {
            var interactionsForRemoving = DtgClientsView.SelectedItems.Cast<Interactions>().ToList();

            if (interactionsForRemoving.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите клиентов для удаления.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Удалить {interactionsForRemoving.Count} клиента(ов)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                try
                {
                    var context = CourseWork25Entities.GetContext();

                    var clientIdsToCheck = interactionsForRemoving.Select(i => i.ClientID).Distinct().ToList();

                    foreach (var interaction in interactionsForRemoving)
                    {
                        var interactionToRemove = context.Interactions.FirstOrDefault(i => i.InteractionID == interaction.InteractionID);
                        if (interactionToRemove != null)
                        {
                            context.Interactions.Remove(interactionToRemove);
                        }
                    }

                    context.SaveChanges();

                    foreach (var clientId in clientIdsToCheck)
                    {
                        bool hasOtherInteractions = context.Interactions.Any(i => i.ClientID == clientId);
                        if (!hasOtherInteractions)
                        {
                            var clientToRemove = context.Clients.FirstOrDefault(c => c.ClientID == clientId);
                            if (clientToRemove != null)
                            {
                                context.Clients.Remove(clientToRemove);
                            }
                        }
                    }

                    context.SaveChanges();

                    DtgClientsView.ItemsSource = CourseWork25Entities.GetContext().Interactions
                        .Include(c => c.Clients)
                        .Include(c => c.Clients.ClientCategories)
                        .Include(c => c.InteractionType)
                        .ToList();

                    MessageBox.Show("Клиенты удалены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Static_Click(object sender, RoutedEventArgs e)
        {
            GenerateClientReport();
        }
        private void GenerateClientReport()
        {
            try
            {
                var clients = CourseWork25Entities.GetContext().Clients
                    .Include(c => c.ClientCategories)
                    .Include(c => c.Interactions)
                    .Include(c => c.Users)
                    .Where(c => c.ClientID > 0) 
                    .ToList();

                if (!clients.Any())
                {
                    MessageBox.Show("Нет данных о клиентах.");
                    return;
                }

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Клиенты");

                    string[] headers = { "Имя", "Фамилия", "Телефон", "Email", "Адрес", "Категория", "Имя пользователя", "Дата взаимодействия", "Тип взаимодействия", "Примечания" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                    }

                    using (ExcelRange range = worksheet.Cells["A1:J1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(173, 216, 230));
                    }

                    int row = 2;
                    foreach (var client in clients)
                    {
                        worksheet.Cells[row, 1].Value = client.FirstName;
                        worksheet.Cells[row, 2].Value = client.LastName;
                        worksheet.Cells[row, 3].Value = client.Phone;
                        worksheet.Cells[row, 4].Value = client.Email;
                        worksheet.Cells[row, 5].Value = client.Address;
                        worksheet.Cells[row, 6].Value = client.ClientCategories.CategoryName;
                        worksheet.Cells[row, 7].Value = client.Users?.Username ?? "Нет пользователя";

                        foreach (var interaction in client.Interactions)
                        {
                            worksheet.Cells[row, 8].Value = interaction.InteractionDate.ToString("dd.MM.yyyy");
                            worksheet.Cells[row, 9].Value = interaction.InteractionType?.TypeName ?? "Не указан";
                            worksheet.Cells[row, 10].Value = interaction.Notes;
                            row++;
                        }
                    }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excel report");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, "Client_Report.xlsx");

                    excelPackage.SaveAs(new FileInfo(filePath));
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

                    MessageBox.Show("Отчет успешно экспортирован в Excel!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nДетали: {ex.InnerException?.Message}");
            }
        }


        private void EditClients_Click(object sender, RoutedEventArgs e)
        {
            if(DtgClientsView.SelectedItem is Interactions interactions)
            {
                NavigationService.Navigate(new SelectedClient(this, interactions));
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            StartDate.SelectedDate = null;
            EndDate.SelectedDate = null;
            DtgClientsView.ItemsSource = CourseWork25Entities.GetContext().Interactions.ToList();
            Search.Text = "";
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? startDate = StartDate.SelectedDate;
                DateTime? endDate = EndDate.SelectedDate;

                if (startDate > endDate)
                {
                    MessageBox.Show("Дата начала не может быть позже даты окончания!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var query = CourseWork25Entities.GetContext().Interactions
                    .Include(ra => ra.Clients)
                    .Include(r => r.Clients.ClientCategories)
                    .Include(ra => ra.InteractionType)
                    .AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(ra => ra.InteractionDate >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(ra => ra.InteractionDate <= endDate.Value);
                }

                DtgClientsView.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации по дате: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CategorClients_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var categories = CourseWork25Entities.GetContext().ClientCategories.ToList();

                if (!categories.Any())
                {
                    MessageBox.Show("Нет данных о категориях клиентов.");
                    return;
                }

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    foreach (var category in categories)
                    {
                        var clientsInCategory = CourseWork25Entities.GetContext().Clients
                            .Include(c => c.ClientCategories)
                            .Include(c => c.Interactions)
                            .Include(c => c.Users)
                            .Where(c => c.CategoryID == category.CategoryID)
                            .ToList();

                        if (clientsInCategory.Any())
                        {
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(category.CategoryName);

                            string[] headers = { "Имя", "Фамилия", "Телефон", "Email", "Адрес", "Категория", "Имя пользователя", "Дата взаимодействия", "Тип взаимодействия", "Примечания" };
                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = headers[i];
                            }

                            using (ExcelRange range = worksheet.Cells["A1:J1"])
                            {
                                range.Style.Font.Bold = true;
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(173, 216, 230));
                            }

                            int row = 2;
                            foreach (var client in clientsInCategory)
                            {
                                worksheet.Cells[row, 1].Value = client.FirstName;
                                worksheet.Cells[row, 2].Value = client.LastName;
                                worksheet.Cells[row, 3].Value = client.Phone;
                                worksheet.Cells[row, 4].Value = client.Email;
                                worksheet.Cells[row, 5].Value = client.Address;
                                worksheet.Cells[row, 6].Value = client.ClientCategories.CategoryName;
                                worksheet.Cells[row, 7].Value = client.Users?.Username ?? "Нет пользователя";

                                foreach (var interaction in client.Interactions)
                                {
                                    worksheet.Cells[row, 8].Value = interaction.InteractionDate.ToString("dd.MM.yyyy");
                                    worksheet.Cells[row, 9].Value = interaction.InteractionType?.TypeName ?? "Не указан";
                                    worksheet.Cells[row, 10].Value = interaction.Notes;
                                    row++;
                                }
                            }

                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                        }
                    }

                    string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excel report");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, "Client_Categories_Report.xlsx");

                    excelPackage.SaveAs(new FileInfo(filePath));
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

                    MessageBox.Show("Отчет успешно экспортирован в Excel!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nДетали: {ex.InnerException?.Message}");
            }
        }

    }
}
