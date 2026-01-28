using ReviewByWpf.ViewModles;
using System.Windows;
using System.Windows.Controls;

namespace ReviewByWpf.Views.Controls
{
    /// <summary>
    /// PosterSelector.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PosterSelector : UserControl
    {
        public PosterSelector()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PosterUrlProperty =
            DependencyProperty.Register(
                nameof(PosterUrl),
                typeof(string),
                typeof(PosterSelector),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string PosterUrl
        {
            get => (string)GetValue(PosterUrlProperty);
            set => SetValue(PosterUrlProperty, value);
        }

        public static readonly DependencyProperty TmdbIdProperty =
            DependencyProperty.Register(
                nameof(TmdbId),
                typeof(int?),
                typeof(PosterSelector),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int? TmdbId
        {
            get => (int?)GetValue(TmdbIdProperty);
            set => SetValue(TmdbIdProperty, value);
        }

        public static readonly DependencyProperty FirstYearProperty =
            DependencyProperty.Register(
                nameof(FirstYear),
                typeof(string),
                typeof(PosterSelector),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string FirstYear
        {
            get => (string)GetValue(FirstYearProperty);
            set => SetValue(FirstYearProperty, value);
        }

        private void PosterSelectButton_Click(object sender, RoutedEventArgs e)
        {
            // TmdbSearchWindow 열기
            var searchWindow = new TmdbSearchUpWindow();
            searchWindow.Owner = Application.Current.MainWindow;
            searchWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (searchWindow.ShowDialog() == true)
            {
                var vm = searchWindow.DataContext as TmdbSearchUpViewModel;
                var selected = vm.SelectedMTPoster;

                if (selected != null)
                {
                    PosterUrl = selected.PosterUrl;
                    TmdbId = selected.TmdbId;
                    FirstYear = selected.FirstYear;
                }
            }
        }
    }
}
