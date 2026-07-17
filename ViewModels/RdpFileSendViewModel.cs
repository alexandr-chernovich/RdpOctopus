using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using RdpOctopus.Models;
using RdpOctopus.Services;
using System;
using System.Linq;
using System.Windows;

namespace RdpOctopus.ViewModels
{
    public class RdpFileSendViewModel : ObservableObject
    {
        private readonly SettingsService<MainSettings> settingsService = new SettingsService<MainSettings>();

        private string _filePath = string.Empty;
        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        private string _serializedFileText = string.Empty;
        public string SerializedFileText
        {
            get => _serializedFileText;
            set => SetProperty(ref _serializedFileText, value);
        }

        public RelayCommand StartInputCommand => new RelayCommand(StartInput);

        public RelayCommand CopyToClipboardCommand => new RelayCommand(CopyToClipboard);

        public RelayCommand SelectFileCommand => new RelayCommand(SelectFile);

        private void SelectFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите файл для отправки",
                Filter = "Все файлы (*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
                return;


            try
            {
                var fileCompressor = new FileCompressor();
                FilePath = dialog.FileName;
                SerializedFileText = fileCompressor.CompressFileToString(FilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось прочитать файл:\n{ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CopyToClipboard()
        {
            Clipboard.SetText(SerializedFileText);
        }

        private void StartInput()
        {
            if (string.IsNullOrWhiteSpace(SerializedFileText))
            {
                MessageBox.Show("Текст для ввода пуст! Делать нечего", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var settings = settingsService.Load() ?? new MainSettings();

            var keyboardLayoutService = new KeyboardLayoutService(settings.TypeSettings.KeyboardLayouts);
            var invalidCharacters = keyboardLayoutService.GetInvalidCharacters(SerializedFileText);

            if (invalidCharacters.Any() && settings.InputMethodName == "LowLevel")
            {
                var invalid = string.Join("", invalidCharacters);
                var dialogResult = MessageBox.Show(
                    $"Текст содержит символы, которые невозможно ввести с клавиатуры '{invalid}'.\n\nПродолжить ввод?",
                    "Предупреждение",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning
                );

                if (dialogResult == MessageBoxResult.Cancel)
                    return;
            }

            IKeyboardSimulator simulator = new TextTyper(settings.TypeSettings);
            if (settings.InputMethodName == "LowLevel")
            {
                simulator = new LowLeverSimulator(settings.TypeSettings);
            }

            InputWindow.ShowInputWindow(SerializedFileText, simulator);
        }
    }
}
