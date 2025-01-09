using AdonisUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Academic_Performance_Tracking_System.Pages.Core
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private bool isDark;
        public SettingsPage()
        {
            InitializeComponent();

            AppVersionTextBox.Text = "v" + Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }

        private void ChangeThemeColor_Switch(object sender, RoutedEventArgs e)
        {
            ResourceLocator.SetColorScheme(Application.Current.Resources, isDark ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);

            isDark = !isDark;
        }

        private void LicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            new Windows.LicensesWindow().ShowDialog();
        }
    }
}
