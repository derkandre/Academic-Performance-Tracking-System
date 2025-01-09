using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Adonis = AdonisUI;

namespace Academic_Performance_Tracking_System.Pages.Forms
{
    /// <summary>
    /// Interaction logic for AddSubjectsPage.xaml
    /// </summary>
    public partial class AddSubjectsPage : Page
    {
        public AddSubjectsPage()
        {
            InitializeComponent();

            UserControls.Forms.SubjectFormControl subjectFormControl = new UserControls.Forms.SubjectFormControl("add");
            
            FormContentControl.Content = subjectFormControl;
        }
    }
}
