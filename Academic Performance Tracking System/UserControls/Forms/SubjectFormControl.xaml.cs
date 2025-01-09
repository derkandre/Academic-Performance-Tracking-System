using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Adonis = AdonisUI;
using System.Data.SQLite;
using Academic_Performance_Tracking_System.Utilities;
using System.Collections.Generic;
using CustomMessageBox = AdonisUI.Controls.MessageBox;
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;
using CustomMessageResult = AdonisUI.Controls.MessageBoxResult;

namespace Academic_Performance_Tracking_System.UserControls.Forms
{
    /// <summary>
    /// Interaction logic for SubjectFormControl.xaml
    /// </summary>
    public partial class SubjectFormControl : UserControl
    {
        private TextBox? startHourTextBox;
        private TextBox? startMinTextBox;
        private TextBox? startSecTextBox;
        private TextBox? endHourTextBox;
        private TextBox? endMinTextBox;
        private TextBox? endSecTextBox;
        private TextBox? lastFocusedTextBox;

        private Button? lastFocusedBtn;

        private string action;

        public SubjectFormControl(string action)
        {
            InitializeComponent();

            this.action = action;
            this.Loaded += UserControl_Loaded;

            SemesterFieldData_Loaded();

            // Refer to comment in Ln. 330 - 335.
            //foreach (Button button in DayBtnsGrid.Children)
            //{
            //    button.MouseEnter += DaysBtn_MouseEnter;
            //    button.MouseLeave += DaysBtn_MouseLeave;
            //    button.PreviewMouseDown += DaysBtn_PreviewMouseDown;
            //    button.Tag = button.Content; 
            //    lastFocusedBtn = button;
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            startHourTextBox = StartTimeField.Template.FindName("PART_HourTextBox", StartTimeField) as TextBox;
            startMinTextBox = StartTimeField.Template.FindName("PART_MinuteTextBox", StartTimeField) as TextBox;
            startSecTextBox = StartTimeField.Template.FindName("PART_SecondTextBox", StartTimeField) as TextBox;

            endHourTextBox = EndTimeField.Template.FindName("PART_HourTextBox", EndTimeField) as TextBox;
            endMinTextBox = EndTimeField.Template.FindName("PART_MinuteTextBox", EndTimeField) as TextBox;
            endSecTextBox = EndTimeField.Template.FindName("PART_SecondTextBox", EndTimeField) as TextBox;

            if (startHourTextBox != null && endHourTextBox != null)
            {
                // Making the timepicker not read-only has the unintended consequence of being able to input > 24                

                //startHourTextBox.IsReadOnly = false;
                //endHourTextBox.IsReadOnly = false;

                TextBox_GotFocus(startHourTextBox, e);

                startHourTextBox.TextChanged -= TextBox_TextChanged;
                startHourTextBox.TextChanged += TextBox_TextChanged;
                startHourTextBox.GotFocus -= TextBox_GotFocus;
                startHourTextBox.GotFocus += TextBox_GotFocus;

                endHourTextBox.TextChanged -= TextBox_TextChanged;
                endHourTextBox.TextChanged += TextBox_TextChanged;
                endHourTextBox.GotFocus -= TextBox_GotFocus;
                endHourTextBox.GotFocus += TextBox_GotFocus;
            }
            if (startMinTextBox != null && endMinTextBox != null)
            {
                //startMinTextBox.IsReadOnly = false;
                //endMinTextBox.IsReadOnly = false;

                startMinTextBox.TextChanged -= TextBox_TextChanged;
                startMinTextBox.TextChanged += TextBox_TextChanged;
                startMinTextBox.GotFocus -= TextBox_GotFocus;
                startMinTextBox.GotFocus += TextBox_GotFocus;

                endMinTextBox.TextChanged -= TextBox_TextChanged;
                endMinTextBox.TextChanged += TextBox_TextChanged;
                endMinTextBox.GotFocus -= TextBox_GotFocus;
                endMinTextBox.GotFocus += TextBox_GotFocus;
            }
            if (startSecTextBox != null && endSecTextBox != null)
            {
                //startSecTextBox.IsReadOnly = false;
                //endSecTextBox.IsReadOnly = false;

                startSecTextBox.TextChanged -= TextBox_TextChanged;
                startSecTextBox.TextChanged += TextBox_TextChanged;
                startSecTextBox.GotFocus -= TextBox_GotFocus;
                startSecTextBox.GotFocus += TextBox_GotFocus;

                endSecTextBox.TextChanged -= TextBox_TextChanged;
                endSecTextBox.TextChanged += TextBox_TextChanged;
                endSecTextBox.GotFocus -= TextBox_GotFocus;
                endSecTextBox.GotFocus += TextBox_GotFocus;
            }
        }

        private void SemesterFieldData_Loaded()
        {
            SemesterField.Items.Clear();
            SemesterField.Items.Add("Select a semester");
            SemesterField.Foreground = new SolidColorBrush(Colors.Gray);
            SemesterField.SelectedIndex = 0; 

            SemesterField.DropDownOpened += SemesterField_DropDownOpened;

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
                                SemesterField.Items.Add(reader["semester_name"].ToString());
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

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.MainWindow.Frame.Navigate(new Pages.Core.SubjectsPage());
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (action == "add")
                Action_AddSubject(sender, e);
            else
                Action_EditSubject();
        }

        private void Action_AddSubject(object sender, RoutedEventArgs e)
        {
            try
            {
                string semester = "{Null}";

                if (SemesterField.SelectedItem != null)
                {
                    semester = SemesterField.SelectedItem.ToString();
                }

                    int profileIDNum = Utilities.SessionManager.GetCurrentSessionID();

                var selectedDays = new List<string>();

                if (SundayBtn.IsChecked == true) selectedDays.Add("S");
                if (MondayBtn.IsChecked == true) selectedDays.Add("M");
                if (TuesdayBtn.IsChecked == true) selectedDays.Add("T");
                if (WednesdayBtn.IsChecked == true) selectedDays.Add("W");
                if (ThursdayBtn.IsChecked == true) selectedDays.Add("Th");
                if (FridayBtn.IsChecked == true) selectedDays.Add("F");
                if (SaturdayBtn.IsChecked == true) selectedDays.Add("S");

                string selectedClassDays = string.Join("", selectedDays);
                string startTime = StartTimeField.SelectedTime.ToString();
                string endTime = EndTimeField.SelectedTime.ToString();

                using (var connection = new SQLiteConnection(Utilities.DatabaseConnection.ConnectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Subjects (subject_title, subject_code, class_days, start_time, end_time, room, instructor, profile_id, semester_id) " +
                                   "VALUES (@subject_title, @subject_code, @class_days, @start_time, @end_time, @room, @instructor, @profile_id, @semester_id)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@subject_title", SubjectTitleField.Text);
                        command.Parameters.AddWithValue("@subject_code", SubjectCodeField.Text);
                        command.Parameters.AddWithValue("@class_days", selectedClassDays);
                        command.Parameters.AddWithValue("@start_time", startTime);
                        command.Parameters.AddWithValue("@end_time", endTime);
                        command.Parameters.AddWithValue("@room", RoomField.Text);
                        command.Parameters.AddWithValue("@instructor", InstructorField.Text);
                        command.Parameters.AddWithValue("@profile_id", Utilities.SessionManager.GetCurrentSessionID());
                        command.Parameters.AddWithValue("@semester_id", GetSemesterID(semester));

                        var result = CustomMessageBox.Show($"Save subject \'{SubjectCodeField.Text} {SubjectTitleField.Text}\'\nfor Semester: \'{semester}' ?", "Confirm Add Subject", CustomMessageBoxButton.YesNoCancel, CustomMessageBoxImage.Information);

                        if (result == CustomMessageResult.Yes)
                        {
                            command.ExecuteNonQuery();
                            CustomMessageBox.Show($"The subject \'{SubjectCodeField.Text} {SubjectTitleField.Text}\' has been added successfully.", "Subject Added Successfully", CustomMessageBoxButton.OK, CustomMessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Utilities.ExceptionLogActions(ex).SaveDebugLog();
            }
        }

        private void Action_EditSubject()
        {

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (lastFocusedTextBox != null && lastFocusedTextBox != textBox)
                {
                    lastFocusedTextBox.Background = (Brush)Application.Current.Resources[Adonis.Brushes.Layer2BackgroundBrush];
                }

                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Background = (Brush)Application.Current.Resources[Adonis.Brushes.AccentBrush];
                }
                else
                {
                    textBox.Background = (Brush)Application.Current.Resources[Adonis.Brushes.Layer2BackgroundBrush];
                }

                lastFocusedTextBox = textBox;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (lastFocusedTextBox != null && lastFocusedTextBox != textBox)
                {
                    lastFocusedTextBox.Background = (Brush)Application.Current.Resources[Adonis.Brushes.Layer2BackgroundBrush];
                }

                textBox.Background = (Brush)Application.Current.Resources[Adonis.Brushes.AccentBrush];

                lastFocusedTextBox = textBox;
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in Utilities.VisualChildren.Get<TextBox>(DetailsFormGroupBox))
            {
                textBox.Clear();
            }
        }

        /* No way, did I really just waste my time on setting this custom behaviour??? (skull_emoji), I just realized that
           I need to allow multiple selections and this is where I realized that a fricking ToggleButton existss!! (crying_face_emoji)x5
           I guess I'll just comment this as a reminder xD. I'm just absolutely facepalming while writing this comment haist.
           
           Total Hours Wasted = 3 hrs, more or less. 2025-01-04.
        */

        //private void DaysBtn_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (sender is Button button)
        //    {
        //        if (button == SundayBtn || button == SaturdayBtn)
        //        {
        //            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
        //        }
        //        else
        //        {
        //            button.Foreground = (Brush)Application.Current.Resources[Adonis.Brushes.ForegroundBrush];
        //        }

        //        if (button == SaturdayBtn)
        //        {
        //            var stackPanel = new StackPanel();

        //            stackPanel.Children.Add(new TextBlock
        //            {
        //                TextAlignment = TextAlignment.Center,
        //                Foreground = button.Foreground,
        //                Text = "Sat",
        //                FontSize = 12
        //            });
        //            stackPanel.Children.Add(new TextBlock
        //            {
        //                TextAlignment = TextAlignment.Center,
        //                Foreground = button.Foreground,
        //                Text = "Sabbath",
        //                FontSize = 8
        //            });

        //            button.Content = stackPanel;
        //        }
        //    }
        //}

        //private void DaysBtn_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (sender is Button button)
        //    {
        //        button.Foreground = (Brush)Application.Current.Resources[Adonis.Brushes.ForegroundBrush];

        //        if (button.Tag is string originalContent)
        //        {
        //            button.Content = originalContent;
        //        }

        //        if (button == SundayBtn || button == SaturdayBtn)
        //        {
        //            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
        //        }
        //    }
        //}

        //private void DaysBtn_PreviewMouseDown(object sender, MouseEventArgs e)
        //{
        //    if (sender is Button button)
        //    {
        //        if (lastFocusedBtn is not null)
        //        {
        //            lastFocusedBtn.Foreground = (Brush)Application.Current.Resources[Adonis.Brushes.ForegroundBrush];
        //            lastFocusedBtn.Background = (Brush)Application.Current.Resources[Adonis.Brushes.Layer2BackgroundBrush];
        //        }

        //        if (button == SundayBtn || button == SaturdayBtn)
        //        {
        //            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
        //        }
        //        else
        //        {
        //            button.Background = (Brush)Application.Current.Resources[Adonis.Brushes.AccentBrush];
        //        }

        //        lastFocusedBtn = button;
        //    }
        //}
    }
}
