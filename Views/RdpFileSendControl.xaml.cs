using RdpOctopus.ViewModels;
using System.Windows.Controls;

namespace RdpOctopus.Views
{
    /// <summary>
    /// Interaction logic for RdpFileSend.xaml
    /// </summary>
    public partial class RdpFileSendControl : UserControl
    {
        public RdpFileSendControl()
        {
            InitializeComponent();
            var viewModel = new RdpFileSendViewModel();
            DataContext = viewModel;
        }
    }
}
