using RdpOctopus.ViewModels;
using System.Windows.Controls;

namespace RdpOctopus.Views
{
    /// <summary>
    /// Interaction logic for InputControl.xaml
    /// </summary>
    public partial class InputControl : UserControl
    {
        public InputControl()
        {
            InitializeComponent();
            DataContext = new InputViewModel();
        }
    }
}
