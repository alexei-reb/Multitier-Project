using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System;

namespace MobileClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        Presenter presenter;

        public MainPage()
        {
            InitializeComponent();
            presenter = new Presenter(this);
            PhoneApplicationService.Current.State["presenter"] = presenter;

        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new System.Uri("/SettingsPage.xaml", System.UriKind.Relative));
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            presenter.NextRow();
            SaveChangesButton.IsEnabled = false;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            presenter.GetTable(string.Empty);
            presenter.FillRow();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, CancelEventArgs e)
        {
            presenter.Exit();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.FillRow();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveChangesButton.IsEnabled = true;
        }

        private void SaveChanges_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<object> person = new List<object>();
            person.Add(int.Parse(tboxID.Text));
            person.Add(tboxFName.Text != string.Empty ? tboxFName.Text : null);
            person.Add(tboxLName.Text != string.Empty ? tboxLName.Text : null);
            person.Add(tboxBDate.Text != string.Empty ? tboxBDate.Text : null);
            person.Add(null);
            person.Add(null);
            person.Add(tboxEMail.Text != string.Empty ? tboxEMail.Text : null);
            person.Add(tboxPhone.Text != string.Empty ? tboxPhone.Text : null);
            person.Add(tboxDescription.Text != string.Empty ? tboxDescription.Text : null);
            person.Add(null);
            presenter.UpdatePerson(person);
        }
    }
}