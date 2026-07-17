using RdpOctopus.ViewModels;
using System.Windows.Controls;

namespace RdpOctopus.Views
{
    /// <summary>
    /// Interaction logic for RdpGetFileControl.xaml
    /// </summary>
    public partial class RdpGetFileControl : UserControl
    {
        public RdpGetFileControl()
        {
            InitializeComponent();
            var viewModel = new RdpGetFileViewModel();
            DataContext = viewModel;
        }
    }
}
