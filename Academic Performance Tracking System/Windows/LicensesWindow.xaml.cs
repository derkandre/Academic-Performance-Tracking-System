using System;
using System.Collections.Generic;
using System.IO;
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
using AdonisUI.Controls;

namespace Academic_Performance_Tracking_System.Windows
{
    /// <summary>
    /// Interaction logic for LicensesWindow.xaml
    /// </summary>
    public partial class LicensesWindow : AdonisWindow
    {
        public LicensesWindow()
        {
            InitializeComponent();
        }

        private void MyLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/LICENSE.txt";
            ShowLicense(filePath);
        }

        private void AdonisLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/AdonisUI_LICENSE.txt";
            ShowLicense(filePath);
        }

        private void WpfExConLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/WpfExtendedControls_LICENSE.txt";
            ShowLicense(filePath);
        }

        private void MetroIconPacksLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/MahApps.Metro.IconPacks_LICENSE.txt";
            ShowLicense(filePath);
        }

        private void MaterialIconsLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/MaterialIcons_LICENSE.txt";
            ShowLicense(filePath);
        }

        private void DotNetRunTimeLicenseBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "Licenses/dotNet_LICENSE.txt";
            ShowLicense(filePath);
        }

        private void ShowLicense(string filePath)
        {
            try
            {
                string licenseText = File.ReadAllText(filePath);
                FlowDocument flowDocument = new FlowDocument();

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run(licenseText));

                flowDocument.Blocks.Add(paragraph);

                LicenseTextBox.Document = flowDocument;
            }
            catch (Exception ex)
            {
                new Utilities.ExceptionLogActions(ex).SaveDebugLog();
            }
        }
    }
}
