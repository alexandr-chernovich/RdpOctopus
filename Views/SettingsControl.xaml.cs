using RdpOctopus.ViewModels;
using System.Windows.Controls;

namespace RdpOctopus.Views
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            DataContext = new MainSettingsViewModel();
        }
    }
}
