using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RdpOctopus.Models;
using RdpOctopus.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RdpOctopus.ViewModels
{
    public class InputWindowViewModel : ObservableObject
    {
        private SettingsService<MainSettings> _settingsService = new SettingsService<MainSettings>();

        private bool _isCompleted = false;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => SetProperty(ref _isCompleted, value);
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool CanStart => !_isRunning;

        public string _statusText = string.Empty;
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public int _maximum = 10;
        public int Maximum
        {
            get => _maximum;
            set => SetProperty(ref _maximum, value);
        }

        public int _progress = 0;
        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        private string _text = string.Empty;

        protected string Text
        {
            get => _text;
            set
            {
                Maximum = value.Length;
                SetProperty(ref _text, value);
            }
        }

        public InputWindowViewModel(string text, IKeyboardSimulator keyboardSimulator)
        {
            Text = text;
            KeyboardSimulator = keyboardSimulator ?? throw new ArgumentNullException(nameof(keyboardSimulator));
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public IKeyboardSimulator KeyboardSimulator { get; set; }

        public RelayCommand StartCommand => new RelayCommand(Start);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        public event EventHandler RequestClose;

        public async void Start()
        {
            try
            {
                IsRunning = true;
                var progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;

                var cancellationToken = _cancellationTokenSource.Token;

                var settings = _settingsService.Load();
                var startDelay = settings?.StartDelay ?? 4.5f;

                StatusText = $"Подготовка к вводу... Задержка {startDelay:F1} секунд";

                await Task.Delay(TimeSpan.FromSeconds(startDelay), cancellationToken);

                StatusText = "Начинаю ввод текста...";

                await KeyboardSimulator.Type(Text, progress, cancellationToken);

                StatusText = "Ввод текста завершен успешно!";

                IsCompleted = true;
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (OperationCanceledException)
            {
                StatusText = "Операция отменена";
            }
            catch (Exception ex)
            {
                StatusText = $"Ошибка: {ex.Message}";
            }
        }

        private void Progress_ProgressChanged(object sender, int e)
        {
            Progress = e;
            StatusText = $"Напечатано {Progress} символов из {Maximum}";
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
