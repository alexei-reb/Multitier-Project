using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Sockets;

namespace MobileClient
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Presenter presenter = PhoneApplicationService.Current.State["presenter"] as Presenter;
            if (presenter.Connect(txtServerName.Text, int.Parse(txtPortNumber.Text)))
            {
                presenter.GetTable(string.Empty);
                MessageBox.Show("Connected!");
            }
            else
            {
                MessageBox.Show("Can not connect to the server.");
            }
        }

        private void GoBackButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}