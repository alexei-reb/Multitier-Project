using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace DesktopClient
{
    public partial class MainWindow : Window
    {
        private Presenter presenter;

        public MainWindow()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.Connect();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            presenter.Exit();
        }

        private void GetPersonTable_Click(object sender, RoutedEventArgs e)
        {
            presenter.GetTable("Person");
        }

        private void MainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (!(e.Row.Item as DataRowView).IsNew)
                {
                    presenter.UpdatePerson(GetCurrentRow(e));
                }
                else
                {
                    presenter.AddPerson(GetCurrentRow(e));
                }
            }
        }

        private List<object> GetCurrentRow(DataGridCellEditEndingEventArgs e)
        {
            List<object> values = new List<object>();
            foreach (object item in (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray)
            {
                values.Add(item);
            }

            string name = e.Column.Header.ToString();
            string value = (e.EditingElement as TextBox).Text;

            for (int i = 0; i < MainDataGrid.Columns.Count; i++)
            {
                if (MainDataGrid.Columns[i].Header.ToString() == name)
                {
                    values[i] = value;
                    break;
                }
            }
            return values;
        }

        private void MainDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var item = MainDataGrid.SelectedItem;
            var value = MainDataGrid.SelectedValue;
        }

        private void MainDataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Key.Delete == e.Key)
            {
                DataGrid dataGrid = sender as DataGrid;
                foreach (var row in dataGrid.SelectedItems)
                {
                    if (row is DataRowView)
                    {
                        object id = (row as DataRowView).Row.ItemArray.First();
                        presenter.DeletePerson(id);
                    }
                }
            }
        }

        private void LoadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = DesktopClient.Properties.Resources.ImageFilter;
            if (MainDataGrid.SelectedItem != null)
                if (dlg.ShowDialog() == true)
                {
                    if (MainDataGrid.SelectedItem is DataRowView)
                    {
                        object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray.First();
                        presenter.SendPhoto(dlg.FileName, id);
                    }
                }
        }
        private void SavePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = DesktopClient.Properties.Resources.ImageFilter;
            if (MainDataGrid.SelectedItem != null)
                if (sfd.ShowDialog() == true)
                {
                    if (MainDataGrid.SelectedItem is DataRowView)
                    {
                        object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray.First();
                        presenter.ReadPhoto(sfd.FileName, id);
                    }
                }
        }


        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = DesktopClient.Properties.Resources.FileFilter;
            if (MainDataGrid.SelectedItem != null)
                if (dlg.ShowDialog() == true)
                {
                    if (MainDataGrid.SelectedItem is DataRowView)
                    {
                        object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray.First();
                        presenter.SendFile(dlg.FileName, id);
                    }
                }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = DesktopClient.Properties.Resources.FileFilter;
            if (MainDataGrid.SelectedItem != null)
                if (sfd.ShowDialog() == true)
                {
                    if (MainDataGrid.SelectedItem is DataRowView)
                    {
                        object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray.First();
                        presenter.ReadFile(sfd.FileName, id);
                    }
                }
        }

        private void LoadPhotoLinkButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = DesktopClient.Properties.Resources.ImageFilter;
            if (MainDataGrid.SelectedItem != null)
                if (dlg.ShowDialog() == true)
                {
                    if (MainDataGrid.SelectedItem is DataRowView)
                    {
                        object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray.First();
                        presenter.SendPhotoLink(dlg.FileName, id);
                    }
                }
        }
        private void SavePhotoLinkButoon_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null)
                if (MainDataGrid.SelectedItem is DataRowView)
                {
                    object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray[0];
                    object FileName = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray[5];
                    if (FileName.ToString() != string.Empty)
                        presenter.ReadPhotoLink(FileName.ToString(), id);
                }
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null && MainDataGrid.SelectedItem is DataRowView)
            {
                object id = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray[0];
                object FileName = (MainDataGrid.SelectedItem as DataRowView).Row.ItemArray[5];
                if (FileName.ToString() != string.Empty)
                    presenter.ReadPhotoLink(FileName.ToString(), id);
                else
                    MainImage.Source = null;
            }
        }
    }
}
