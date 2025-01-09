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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomMessageBox = AdonisUI.Controls.MessageBox;
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;
using CustomMessageResult = AdonisUI.Controls.MessageBoxResult;

using System.Data.SQLite;

namespace Academic_Performance_Tracking_System.UserControls.Forms
{
    /// <summary>
    /// Interaction logic for SemesterFormControl.xaml
    /// </summary>
    public partial class SemesterFormControl : UserControl
    {
        public SemesterFormControl()
        {
            InitializeComponent();
            Semesters_Loaded();
            Years_Loaded();
        }

        private void Semesters_Loaded()
        {
            SemesterComboBox.Items.Insert(0, "Select a semester");
            SemesterComboBox.Foreground = new SolidColorBrush(Colors.Gray);
            SemesterComboBox.SelectedIndex = 0;            

            SemesterComboBox.DropDownOpened += SemesterComboBox_DropDownOpened;
        }

        private void SemesterComboBox_DropDownOpened(object sender, EventArgs e)
        {
            SemesterComboBox.Items.Remove("Select a semester".Trim());
            SemesterComboBox.Foreground = (Brush)Application.Current.Resources[AdonisUI.Brushes.ForegroundBrush];
        }

        private void Years_Loaded()
        {
            int startYear = 2000;
            int endYear = DateTime.Now.Year;

            for (int year = endYear; year >= startYear; year--)
            {
                AcadYearStart.Items.Add(year);
            }

            for (int year = endYear + 1; year >= startYear; year--)
            {
                AcadYearEnd.Items.Add(year);
            }

            AcadYearStart.Items.Insert(0, "Select a year");
            AcadYearEnd.Items.Insert(0, "Select a year");

            AcadYearStart.SelectedIndex = 0;
            AcadYearEnd.SelectedIndex = 0;

            AcadYearStart.Foreground = new SolidColorBrush(Colors.Gray);
            AcadYearEnd.Foreground = new SolidColorBrush(Colors.Gray);

            AcadYearStart.SelectionChanged += AcadYearComboBox_SelectionChanged;
        }

        private void AcadYearComboBox_SelectionChanged(object sender, EventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;

            if (comboBox == AcadYearStart) AcadYearStart.Items.Remove("Select a year");
            else AcadYearEnd.Items.Remove("Select a year");
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.MainWindow.Frame.Navigate(new Pages.Core.SubjectsPage());
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in Utilities.VisualChildren.Get<TextBox>(DetailsFormGroupBox))
            {
                textBox.Clear();
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string semester = (SemesterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "{Null}";
            string acadYearStart = AcadYearStart.SelectedItem?.ToString() ?? "{Null}";
            string acadYearEnd = AcadYearEnd.SelectedItem?.ToString() ?? "{Null}?";

            string semesterName = $"AY {acadYearStart}-{acadYearEnd} {semester}";

            try
            {
                using (var connection = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Semesters(semester_name, acad_year_start, acad_year_end, date_start, date_end, profile_id)" +
                        "VALUES(@semester_name, @acad_year_start, @acad_year_end, date(@date_start), date(@date_end), @profile_id);";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@semester_name", semesterName);
                        command.Parameters.AddWithValue("@acad_year_start", acadYearStart);
                        command.Parameters.AddWithValue("@acad_year_end", acadYearEnd);
                        command.Parameters.AddWithValue("@date_start", StartDateField.SelectedDate);
                        command.Parameters.AddWithValue("@date_end", EndDateField.SelectedDate);
                        command.Parameters.AddWithValue("@profile_id", Utilities.SessionManager.GetCurrentSessionID());

                        var result = CustomMessageBox.Show($"Save semester: {semesterName}\'?", "Confirm Add Semester", CustomMessageBoxButton.YesNoCancel, CustomMessageBoxImage.Information);

                        if (result == CustomMessageResult.Yes)
                        {
                            command.ExecuteNonQuery();
                            CustomMessageBox.Show($"The semester \'{semesterName}\' has been added successfully.", "Semester Added Successfully", CustomMessageBoxButton.OK, CustomMessageBoxImage.Information);
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show($"An error occured while saving the semester \'{semesterName}\':{ex.Message}\n\n{ex.StackTrace}", "Failed to Save Semester", CustomMessageBoxButton.OK, CustomMessageBoxImage.Error);
            }
        }
    }
}
