using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;

namespace Academic_Performance_Tracking_System.Pages.Core
{
    /// <summary>
    /// Interaction logic for SubjectsPage.xaml
    /// </summary>
    public partial class SubjectsPage : Page
    {
        public SubjectsPage()
        {
            InitializeComponent();

            SemesterFieldData_Loaded();
        }

        private void SemesterFieldData_Loaded()
        {
            SemesterField.DropDownOpened += SemesterField_DropDownOpened;

            SemesterField.Items.Insert(0, "Select a semester".Trim());
            SemesterField.Foreground = new SolidColorBrush(Colors.Gray);
            SemesterField.SelectedIndex = 0;

            try
            {
                using (var connection = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT semester_id, semester_name, profile_id FROM Semesters WHERE profile_id = @profile_id;";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@profile_id", Utilities.SessionManager.GetCurrentSessionID());

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SemesterField.Items.Add(reader[1].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"An error occured while loading semesters: {ex.Message}\n\n{ex.StackTrace}", "Failed to Save Semester", CustomMessageBoxButton.OK, CustomMessageBoxImage.Error);
            }
        }

        private void SemesterField_DropDownOpened(object? sender, EventArgs e)
        {
            if (SemesterField.Items.Count > 0 && !SemesterField.Items.Contains("No semesters yet. Add one first."))
            {
                SemesterField.Items.Remove("Select a semester");
                SemesterField.Foreground = (Brush)Application.Current.Resources[AdonisUI.Brushes.ForegroundBrush];

                if (SemesterField.Items.Count == 0)
                    SemesterField.Items.Add("No semesters yet. Add one first.");
            }
            else
            {
                if (!SemesterField.Items.Contains("No semesters yet. Add one first."))
                    SemesterField.Items.Add("No semesters yet. Add one first.");
            }
        }

        private void ViewSubjectsBtn_Click(object sender, RoutedEventArgs e)
        {
            string semester = "{Null}";

            if (SemesterField.SelectedItem != null)
            {
                semester = SemesterField.SelectedItem.ToString();
            }

            try
            {
                using (var connection = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT subject_title, subject_code, class_days || ' ' || strftime('%I:%M %p', start_time) || ' - ' || strftime('%I:%M %p', end_time) AS schedule, " +
                        "room,     substr(instructor, length(instructor) - instr(reverse(instructor), ' ') + 2) || ', ' || substr(instructor, 1, 1) || '.' AS instructorf " +
                        "FROM Subjects WHERE semester_id = @semester_id;";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@semester_id", GetSemesterID(semester));

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();

                            adapter.Fill(dataTable);

                            SubjectsDataGrid.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utilities.ExceptionLogActions(ex).SaveDebugLog();
            }
        }

        private uint GetSemesterID(string? selectedSemester)
        {
            uint semesterID = 0;

            try
            {
                using (var connection = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Semesters where semester_name = @semester_name";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@semester_name", selectedSemester);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                semesterID = Convert.ToUInt16(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utilities.ExceptionLogActions(ex).SaveDebugLog();
            }

            return semesterID;
        }

        private void AddSemesterBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddSubjectsBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.MainWindow.Frame.Navigate(new Forms.AddSubjectsPage());
        }

        private void AddSemestersBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.MainWindow.Frame.Navigate(new Forms.AddSemestersPage());
        }
    }
}
