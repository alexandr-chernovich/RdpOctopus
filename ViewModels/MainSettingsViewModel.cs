using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RdpOctopus.Models;
using RdpOctopus.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RdpOctopus.ViewModels
{
    public partial class MainSettingsViewModel : ObservableObject
    {
        private readonly SettingsService<MainSettings> _settingsService = new SettingsService<MainSettings>();

        private MainSettings _settings;
        public MainSettings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        private string _inputMethodName;
        public string InputMethodName
        {
            get => _inputMethodName;
            set => SetProperty(ref _inputMethodName, value);
        }

        private float _startDelay;
        public float StartDelay
        {
            get => _startDelay;
            set => SetProperty(ref _startDelay, value);
        }

        private float _intervalDelay;
        public float IntervalDelay
        {
            get => _intervalDelay;
            set => SetProperty(ref _intervalDelay, value);
        }

        private float _shiftDelay;
        public float ShiftDelay
        {
            get => _shiftDelay;
            set => SetProperty(ref _shiftDelay, value);
        }

        private float _switchDelay;
        public float SwitchDelay
        {
            get => _switchDelay;
            set => SetProperty(ref _switchDelay, value);
        }

        private string _initLayoutName;
        public string InitLayoutName
        {
            get => _initLayoutName;
            set => SetProperty(ref _initLayoutName, value);
        }

        private SwitchMethod _switchMethod;
        public SwitchMethod SwitchMethod
        {
            get => _switchMethod;
            set => SetProperty(ref _switchMethod, value);
        }

        private ObservableCollection<string> _availableLayouts;
        public ObservableCollection<string> AvailableLayouts
        {
            get => _availableLayouts;
            set => SetProperty(ref _availableLayouts, value);
        }

        private ObservableCollection<string> _inputMethodNames; // Изменено на свойство
        public ObservableCollection<string> InputMethodNames
        {
            get => _inputMethodNames;
            set => SetProperty(ref _inputMethodNames, value);
        }

        public IEnumerable<SwitchMethod> AvailableSwitchMethods { get; set; }

        public MainSettingsViewModel()
        {
            // Инициализация коллекций
            InputMethodNames = new ObservableCollection<string>()
            {
                "LowLevel",
                "TextTyper"
            };

            AvailableSwitchMethods = Enum.GetValues(typeof(SwitchMethod)).OfType<SwitchMethod>();

            LoadSettings();
        }

        public RelayCommand LoadSettingsCommand => new RelayCommand(LoadSettings);
        public RelayCommand SaveSettingsCommand => new RelayCommand(SaveSettings);

        private void LoadSettings()
        {
            Settings = _settingsService.Load() ?? new MainSettings
            {
                TypeSettings = new TypeSettings(),
                InputMethodName = "Default"
            };

            var typeSettings = Settings.TypeSettings;
            IntervalDelay = typeSettings.IntervalDelay;
            ShiftDelay = typeSettings.ShiftDelay;
            SwitchDelay = typeSettings.SwitchDelay;
            InitLayoutName = typeSettings.InitLayoutName;
            SwitchMethod = typeSettings.SwitchMethod;
            InputMethodName = Settings.InputMethodName;
            StartDelay = Settings.StartDelay;

            AvailableLayouts = new ObservableCollection<string>(
                typeSettings.KeyboardLayouts.Select(l => l.Name).ToList() ?? new List<string>());
        }

        private void SaveSettings()
        {
            Settings.TypeSettings.IntervalDelay = IntervalDelay;
            Settings.TypeSettings.ShiftDelay = ShiftDelay;
            Settings.TypeSettings.SwitchDelay = SwitchDelay;
            Settings.TypeSettings.InitLayoutName = InitLayoutName;
            Settings.TypeSettings.SwitchMethod = SwitchMethod;
            Settings.InputMethodName = InputMethodName;
            Settings.StartDelay = StartDelay;

            _settingsService.Save(Settings);
        }
    }
}