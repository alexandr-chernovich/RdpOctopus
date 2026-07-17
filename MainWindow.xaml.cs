using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RdpOctopus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl?.SelectedContent == null) return;

            var contentPresenter = tabControl.Template.FindName("PART_SelectedContentHost", tabControl) as ContentPresenter;
            if (contentPresenter == null) return;

            if (contentPresenter.RenderTransform == null || contentPresenter.RenderTransform is not TranslateTransform)
            {
                contentPresenter.RenderTransform = new TranslateTransform(10, 0);
                contentPresenter.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            var slideAnimation = new DoubleAnimation
            {
                From = 10,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            var storyboard = new Storyboard();
            Storyboard.SetTarget(storyboard, contentPresenter);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            storyboard.Children.Add(fadeAnimation);
            storyboard.Children.Add(slideAnimation);
            storyboard.Begin();
        }

    }
}
