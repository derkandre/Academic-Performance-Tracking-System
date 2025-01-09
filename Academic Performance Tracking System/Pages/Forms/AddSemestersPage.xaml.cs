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

namespace Academic_Performance_Tracking_System.Pages.Forms
{
    /// <summary>
    /// Interaction logic for AddSemestersPage.xaml
    /// </summary>
    public partial class AddSemestersPage : Page
    {
        public AddSemestersPage()
        {
            InitializeComponent();

            UserControls.Forms.SemesterFormControl semesterFormControl = new UserControls.Forms.SemesterFormControl();

            FormContentControl.Content = semesterFormControl;
        }
    }
}
