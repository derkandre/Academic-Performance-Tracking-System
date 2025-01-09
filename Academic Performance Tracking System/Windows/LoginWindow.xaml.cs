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
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Data.SQLite;
using AdonisUI.Controls;
using System.IO;
using System.Diagnostics;

namespace Academic_Performance_Tracking_System.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : AdonisWindow
    {
        private MainWindow main;
        public LoginWindow(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            PageAccessFrame.Navigate(new Pages.Core.LoginPage(main, this));
        }

        private void AccessBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Profiles";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {

                    }
                }
            }
            catch
            {

            }

            main.MainWindowGrid.Effect = null;

            //if (main.MainWindowGrid.Children.Contains(main.message))
            //{
            //    main.MainWindowGrid.Children.Remove(main.message);
            //}

            main.MainWindowGrid.IsEnabled = true;

            this.Close();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            PageNavigateBtn.Click -= CreateBtn_Click;
            PageNavigateBtn.Click += GoBackBtn_Click;
            PageAccessFrame.Navigate(new Pages.Core.SignUpPage(main));

            PageNavigateBtn.Content = "Access";
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            PageNavigateBtn.Click -= GoBackBtn_Click;
            PageNavigateBtn.Click += CreateBtn_Click;
            PageAccessFrame.Navigate(new Pages.Core.LoginPage(main, this));

            PageNavigateBtn.Content = "Create";
        }
    }
}
