using RdpOctopus.Services;
using System.Windows;

namespace RdpOctopus.ViewModels
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        protected InputWindow(string text, IKeyboardSimulator keyboardSimulator)
        {
            InitializeComponent();
            var viewModel = new InputWindowViewModel(text, keyboardSimulator);
            DataContext = viewModel;

            // Подписка на событие закрытия окна при завершении работы ViewModel
            viewModel.RequestClose += (s, e) =>
            {
                DialogResult = viewModel.IsCompleted;
                Close();
            };

            viewModel.Start();
        }

        public static void ShowInputWindow(string text, IKeyboardSimulator keyboardSimulator)
        {
            var window = new InputWindow(text, keyboardSimulator);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }
    }
}
