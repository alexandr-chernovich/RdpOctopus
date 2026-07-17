using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RdpOctopus.Models;
using RdpOctopus.Services;
using System.Linq;
using System.Windows;

namespace RdpOctopus.ViewModels
{
    public class InputViewModel : ObservableObject
    {
        private readonly SettingsService<MainSettings> settingsService = new SettingsService<MainSettings>();

        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        public RelayCommand StartInputCommand => new RelayCommand(StartInput);

        public RelayCommand PasteFromClipboardCommand => new RelayCommand(PasteFromClipboard);

        private void PasteFromClipboard()
        {
            InputText = Clipboard.GetText();
        }

        private void StartInput()
        {
            if (string.IsNullOrWhiteSpace(InputText))
            {
                MessageBox.Show("Текст для ввода пуст! Делать нечего", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var settings = settingsService.Load() ?? new MainSettings();

            var keyboardLayoutService = new KeyboardLayoutService(settings.TypeSettings.KeyboardLayouts);
            var invalidCharacters = keyboardLayoutService.GetInvalidCharacters(InputText);

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

            InputWindow.ShowInputWindow(InputText, simulator);
        }
    }
}
