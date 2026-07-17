using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RdpOctopus.Services;
using System;
using System.Windows;

namespace RdpOctopus.ViewModels
{
    internal class RdpGetFileViewModel : ObservableObject
    {
        private string _serializedFileText = string.Empty;
        public string SerializedFileText
        {
            get => _serializedFileText;
            set => SetProperty(ref _serializedFileText, value);
        }

        public RelayCommand PasteFromClipboardCommand => new RelayCommand(PasteFromClipboard);

        public RelayCommand SaveFileCommand => new RelayCommand(SaveFile);

        private void PasteFromClipboard()
        {
            SerializedFileText = Clipboard.GetText();
        }

        private void SaveFile()
        {
            if (string.IsNullOrWhiteSpace(SerializedFileText))
            {
                MessageBox.Show("Сохранять нечего", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            var dialog = new SaveFileDialog()
            {
                Title = "Сохранение файла",
                Filter = "Все файлы (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
                return;


            try
            {
                var fileCompressor = new FileCompressor();
                var filePath = dialog.FileName;
                fileCompressor.DecompressStringToFile(SerializedFileText, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить файл:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
