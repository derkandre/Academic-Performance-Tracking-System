using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
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
using CustomMessageBox = AdonisUI.Controls.MessageBox;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using Microsoft.Win32;
using Academic_Performance_Tracking_System.Utilities;

namespace Academic_Performance_Tracking_System.Pages.Core
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Windows.MainWindow main;
        private Windows.LoginWindow loginWindow;
        public LoginPage(Windows.MainWindow main, Windows.LoginWindow loginWindow)
        {
            InitializeComponent();
            this.main = main;
            this.loginWindow = loginWindow;
        }

        private void AccessBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProfileNameField.Text) || string.IsNullOrWhiteSpace(ProfilePasswordField.Password))
            {
                CustomMessageBox.Show(
                    $"All Fields are Required.\nPlease enter both username and password and try again.",
                    "ERROR: All Fields are Required",
                    CustomMessageBoxButton.OK,
                    CustomMessageBoxImage.Warning
                );

                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Profiles WHERE profile_name = @profile_name AND password = @password;";

                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@profile_name", ProfileNameField.Text);
                        command.Parameters.AddWithValue("@password", ProfilePasswordField.Password);

                        int result = Convert.ToInt32(command.ExecuteScalar());

                        if (result > 0)
                        {
                            main.MainWindowGrid.Effect = null;
                            main.MainWindowGrid.IsEnabled = true;

                            loginWindow.Close();

                            SetProfileIDToSession(ProfileNameField.Text, ProfilePasswordField.Password);
                        }
                        else
                        {
                            CustomMessageBox.Show(
                                $"The credentials you have entered is incorrect\nor does not exist.",
                                "ERROR: Access Denied",
                                CustomMessageBoxButton.OK,
                                CustomMessageBoxImage.Error
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = CustomMessageBox.Show(
                    $"{ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nClicking 'OK' will save this debug info. Kindly send the file to my messenger.",
                    $"ERROR: Something went wrong",
                    AdonisUI.Controls.MessageBoxButton.OKCancel,
                    AdonisUI.Controls.MessageBoxImage.Error
                );

                if (result is AdonisUI.Controls.MessageBoxResult.OK)
                {
                    try
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            FileName = $"APTs_Error_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                            Title = "Save Error Log",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string path = saveFileDialog.FileName;

                            File.WriteAllText(path, $"Exception Debug Report\n" +
                                "-------------- Start of Debug Info --------------\n" +
                                $"Date: {DateTime.Now}\n" +
                                "---------------------------------------------------\n" +
                                $"Message: {ex.Message}\n" +
                                "---------------------------------------------------\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                "---------------------------------------------------\n" +
                                $"Source: {ex.Source}\n" +
                                "-------------- End of Debug Info --------------\n" +
                                "Please send this error report via messenger. Much appreciated!");

                            CustomMessageBox.Show($"Error saved to:\n{path}", "Error Saved", CustomMessageBoxButton.OK, CustomMessageBoxImage.Information);
                        }
                    }
                    catch (Exception saveEx)
                    {
                        CustomMessageBox.Show($"Could not save error: {saveEx.Message}", "Save Error", CustomMessageBoxButton.OK, CustomMessageBoxImage.Error);
                    }
                }
            }
        }

        private void SetProfileIDToSession(string profile_name, string password)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM Profiles WHERE profile_name = @profile_name AND password = @password;";

                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@profile_name", profile_name);
                        command.Parameters.AddWithValue("@password", password);

                        object result = Convert.ToInt32(command.ExecuteScalar());

                        if (result is not null)
                        {
                            main.MainWindowGrid.Effect = null;
                            main.MainWindowGrid.IsEnabled = true;

                            loginWindow.Close();

                            UInt16 userID;

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    userID = Convert.ToUInt16(reader[0]);
                                }
                            }

                            UInt16 currentSessionID = Convert.ToUInt16(result);
                            SessionManager.SetCurrentSessionID(currentSessionID);
                        }
                        else
                        {
                            CustomMessageBox.Show(
                                $"The credentials you have entered is incorrect\nor does not exist.",
                                "ERROR: Access Denied",
                                CustomMessageBoxButton.OK,
                                CustomMessageBoxImage.Error
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var result = CustomMessageBox.Show(
                    $"{ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nClicking 'OK' will save this debug info. Kindly send the file to my messenger.",
                    $"ERROR: Something went wrong",
                    AdonisUI.Controls.MessageBoxButton.OKCancel,
                    AdonisUI.Controls.MessageBoxImage.Error
                );

                if (result is AdonisUI.Controls.MessageBoxResult.OK)
                {
                    try
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog
                        {
                            FileName = $"APTs_Error_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                            Title = "Save Error Log",
                            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                        };

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string path = saveFileDialog.FileName;

                            File.WriteAllText(path, $"Exception Debug Report\n" +
                                "-------------- Start of Debug Info --------------\n" +
                                $"Date: {DateTime.Now}\n" +
                                "---------------------------------------------------\n" +
                                $"Message: {ex.Message}\n" +
                                "---------------------------------------------------\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                "---------------------------------------------------\n" +
                                $"Source: {ex.Source}\n" +
                                "-------------- End of Debug Info --------------\n" +
                                "Please send this error report via messenger. Much appreciated!");

                            CustomMessageBox.Show($"Error saved to:\n{path}", "Error Saved", CustomMessageBoxButton.OK, CustomMessageBoxImage.Information);
                        }
                    }
                    catch (Exception saveEx)
                    {
                        CustomMessageBox.Show($"Could not save error: {saveEx.Message}", "Save Error", CustomMessageBoxButton.OK, CustomMessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
