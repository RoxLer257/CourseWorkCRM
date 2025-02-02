using CourseWork25.BD;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using CourseWork25.Classes;
using System;
using System.Windows.Input;

namespace CourseWork25.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для SelectedClient.xaml
    /// </summary>
    public partial class SelectedClient : Page
    {
        private MainPage _mainPage;
        private Interactions _interactions;
        public SelectedClient(MainPage mainPage, Interactions interactions)
        {
            InitializeComponent();
            DataContext = this;
            this._mainPage = mainPage;
            this._interactions = interactions;

            CmbCatClient.ItemsSource = CourseWork25Entities.GetContext().ClientCategories.ToList();
            CmbCatClient.SelectedValuePath = "CategoryID";
            CmbCatClient.DisplayMemberPath = "CategoryName";

            CmbTypeInter.ItemsSource = CourseWork25Entities.GetContext().InteractionType.ToList();
            CmbTypeInter.SelectedValuePath = "TypeID";
            CmbTypeInter.DisplayMemberPath = "TypeName";

            if (_interactions != null)
            {
                Clients clients = CourseWork25Entities.GetContext().Clients.FirstOrDefault(c => c.ClientID == _interactions.ClientID);
                if (clients != null)
                {
                    TxtFirstName.Text = clients.FirstName;
                    TxtLastName.Text = clients.LastName;
                    TxtAddress.Text = clients.Address;
                    TxtEmail.Text = clients.Email;
                    TxtPhoneNumber.Text = clients.Phone;
                    CmbCatClient.SelectedValue = clients.CategoryID;
                }

                Interactions interaction = CourseWork25Entities.GetContext().Interactions.FirstOrDefault(i => i.InteractionID == _interactions.InteractionID);
                if (interaction != null)
                {
                    DateInter.SelectedDate = interaction.InteractionDate;
                    CmbTypeInter.SelectedValue = interaction.InteractionType?.TypeID;
                    Note.Text = interaction.Notes;
                }

                mainPage.DtgClientsView.ItemsSource = CourseWork25Entities.GetContext().Interactions.ToList();
            }
        }

        private void ExtAcc_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.Logon.Authorization());
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var context = CourseWork25Entities.GetContext();

                if (string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                    string.IsNullOrWhiteSpace(TxtEmail.Text) || string.IsNullOrWhiteSpace(TxtPhoneNumber.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Clients client;

                if (_interactions == null)
                {
                    client = new Clients
                    {
                        FirstName = TxtFirstName.Text,
                        LastName = TxtLastName.Text,
                        Address = TxtAddress.Text,
                        Email = TxtEmail.Text,
                        Phone = TxtPhoneNumber.Text,
                        CategoryID = CmbCatClient.SelectedValue != null ? (int)CmbCatClient.SelectedValue : 0
                    };

                    context.Clients.Add(client);
                    context.SaveChanges();
                }
                else
                {
                    client = context.Clients.FirstOrDefault(c => c.ClientID == _interactions.ClientID);

                    if (client != null)
                    {
                        client.FirstName = TxtFirstName.Text;
                        client.LastName = TxtLastName.Text;
                        client.Address = TxtAddress.Text;
                        client.Email = TxtEmail.Text;
                        client.Phone = TxtPhoneNumber.Text;
                        client.CategoryID = CmbCatClient.SelectedValue != null ? (int)CmbCatClient.SelectedValue : 0;
                    }
                }

                Interactions interaction;

                if (_interactions == null)
                {
                    interaction = new Interactions
                    {
                        ClientID = client.ClientID,
                        InteractionDate = DateInter.SelectedDate ?? DateTime.Now,
                        Notes = Note.Text
                    };

                    if (CmbTypeInter.SelectedValue != null)
                    {
                        int selectedTypeId = (int)CmbTypeInter.SelectedValue;
                        interaction.InteractionType = context.InteractionType.FirstOrDefault(t => t.TypeID == selectedTypeId);
                    }

                    context.Interactions.Add(interaction);
                }
                else
                {
                    interaction = context.Interactions.FirstOrDefault(i => i.InteractionID == _interactions.InteractionID);

                    if (interaction != null)
                    {
                        interaction.InteractionDate = DateInter.SelectedDate ?? DateTime.Now;
                        interaction.Notes = Note.Text;

                        if (CmbTypeInter.SelectedValue != null)
                        {
                            int selectedTypeId = (int)CmbTypeInter.SelectedValue;
                            interaction.InteractionType = context.InteractionType.FirstOrDefault(t => t.TypeID == selectedTypeId);
                        }
                    }
                }


                context.SaveChanges();

                _mainPage.DtgClientsView.ItemsSource = null;
                context.ChangeTracker.Entries().ToList().ForEach(entry => entry.Reload());
                _mainPage.DtgClientsView.ItemsSource = context.Interactions.ToList();

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.GoBack();
        }
        private void TxtEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z0-9@._-]+$");
        }
        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            string email = TxtEmail.Text.Trim();

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Некорректный формат почты.");
                TxtEmail.Focus();
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }


        private void TxtPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }
        private void TxtPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            string phone = TxtPhoneNumber.Text.Trim();

            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Номер телефона должен содержать ровно 11 цифр.");
                TxtPhoneNumber.Focus();
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return phone.Length == 11 && phone.All(char.IsDigit);
        }

    }
}
