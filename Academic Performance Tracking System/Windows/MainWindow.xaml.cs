using System.Text;
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
using MahApps.Metro.IconPacks;
using System.Windows.Media.Effects;
using System.Xml;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.Runtime.CompilerServices;
using System.Reflection;
using WpfExtendedControls;

namespace Academic_Performance_Tracking_System.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        private Button _activeButton;
        public Grid MainWindowGrid => MainGrid;
        public TextBlock message;
        public static Frame? Frame;
        public string WindowTitle { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            WindowTitle = $"Academic Performance Tracking System v{version?.ToString()}" ?? "{Null}";

            this.DataContext = this;

            MainWindowGrid.IsEnabled = false;

            BlurEffect blur = new BlurEffect()
            {
                Radius = 10
            };

            Frame = MainFrame;

            // I'll probably re-implement this at a later date.
            // Both for design and practical purpose so users can easily open again the LoginWindow.

            //message = new TextBlock()
            //{
            //    Text = "Please login first...",
            //    FontSize = 32,
            //    Foreground = Brushes.White,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center,
            //    TextAlignment = TextAlignment.Center
            //};

            //MainWindowGrid.Children.Add(message);

            MainWindowGrid.Effect = blur;

            _activeButton = DashboardPanelBtn;
            SetActiveButton(_activeButton);

            new LoginWindow(this).Show();
        }

        private void ChangeThemeColor(object sender, RoutedEventArgs e)
        {
            
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            CustomMessageBox.Show(MainFrame.ActualWidth.ToString() + " " + MainFrame.ActualHeight.ToString());
        }

        private void CalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Core.CalendarPage());

            SetActiveButton((Button)sender);
        }

        private void SubjectsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Core.SubjectsPage());

            SetActiveButton((Button)sender);
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Core.SettingsPage());

            SetActiveButton((Button)sender);
        }

        private void SetActiveButton(Button button)
        {
            ResetActiveButton(_activeButton);

            button.Background = (Brush)Application.Current.Resources[AdonisUI.Brushes.AccentBrush];

            switch ((string)button.Tag)
            {
                case "DashboardBtn":
                    DashboardBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "ViewDashboard");
                    break;
                case "CalendarBtn":
                    CalendarBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "CalendarToday");
                    break;
                case "SubjectsBtn":
                    SubjectsBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "BookEducation");
                    break;
                case "SettingsBtn":
                    SettingsBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "Cog");
                    break;
            }

            _activeButton = button;
        }

        private void ResetActiveButton(Button button)
        {
            button.Background = Brushes.Transparent;

            switch ((string)button.Tag)
            {
                case "DashboardBtn":
                    DashboardBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "ViewDashboardOutline");
                    break;
                case "CalendarBtn":
                    CalendarBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "CalendarTodayOutline");
                    break;
                case "SubjectsBtn":
                    SubjectsBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "BookEducationOutline");
                    break;
                case "SettingsBtn":
                    SettingsBtnIcon.Kind = (PackIconMaterialKind)Enum.Parse(typeof(PackIconMaterialKind), "CogOutline");
                    break;
            }
        }

        public void LicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            new LicensesWindow().ShowDialog();
        }
    }
}