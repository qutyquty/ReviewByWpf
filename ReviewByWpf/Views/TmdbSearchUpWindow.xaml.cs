using ReviewByWpf.ViewModles;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ReviewByWpf.Views
{
    /// <summary>
    /// TmdbSearchUpWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TmdbSearchUpWindow : Window
    {
        public TmdbSearchUpWindow()
        {
            InitializeComponent();
        }

        private void TmdbSearchUpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = FindVisualChild<ScrollViewer>(WrapListBox);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (sv.VerticalOffset >= sv.ScrollableHeight)
            {
                if (DataContext is TmdbSearchUpViewModel vm)
                {
                    vm.LoadMoreCommand.Execute(null);
                }
            }
        }

        // Helper: VisualTree 탐색
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                    return tChild;
                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
