using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CustomMessageBox = AdonisUI.Controls.MessageBox;
using CustomMessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using CustomMessageBoxImage = AdonisUI.Controls.MessageBoxImage;
using CustomMessageResult = AdonisUI.Controls.MessageBoxResult;

namespace Academic_Performance_Tracking_System.Utilities
{
    class ExceptionLogActions
    {
        private Exception ex;

        public ExceptionLogActions(Exception ex)
        {
            this.ex = ex;
        }

        public void SaveDebugLog()
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
                        FileName = $"APTs_Error_{DateTime.Now:yyyyMMdd_HHmmss}.log",
                        Filter = "Log Files (*.log)|*.log|All Files (*.*)|*.*",
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
