using System;
using System.IO;
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
using AdonisUI.Controls;
using CustomMessageBox = AdonisUI.Controls.MessageBox;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using System.Data.SQLite;
using System.ComponentModel;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;

namespace Academic_Performance_Tracking_System.Pages.Core
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Windows.MainWindow window;
        public SignUpPage(Windows.MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
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

                    string query = "INSERT INTO Profiles(profile_name, password) VALUES (@profile_name, @password);";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@profile_name", ProfileNameField.Text);
                        cmd.Parameters.AddWithValue("@password", ProfilePasswordField.Password);

                        cmd.ExecuteNonQuery();
                    }

                    CustomMessageBox.Show(
                        $"Profile '{ProfileNameField.Text}' has been created successfully!\nYou may now access your profile using the credentials you have set.",
                        "SUCCESS: Profile Created",
                        AdonisUI.Controls.MessageBoxButton.OK,
                        AdonisUI.Controls.MessageBoxImage.Information
                    );
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
