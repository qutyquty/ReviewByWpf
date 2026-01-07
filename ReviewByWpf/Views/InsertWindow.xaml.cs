using ReviewByWpf.ViewModles;
using System.Windows;

namespace ReviewByWpf.Views
{
    /// <summary>
    /// InsertWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertWindow : Window
    {
        public InsertWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (DataContext is InsertViewModel vm)
                {
                    vm.Saved += () =>
                    {
                        // MainWindow에서 ShowDialog() == true로 확인 가능
                        DialogResult = true;
                        Close();
                    };
                }
            };
        }

        private void PosterSelectButton_Click(object sender, RoutedEventArgs e)
        {
            TmdbSearchWindow searchWindow = new TmdbSearchWindow();
            searchWindow.Owner = this;
            searchWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (searchWindow.ShowDialog() == true)
            {
                var vm = DataContext as InsertViewModel;
                if (vm != null)
                {
                    vm.PosterUrl = searchWindow.SelectedPosterUrl;
                    vm.FirstYear = searchWindow.SelectedFirstYear;
                    vm.TmdbId = searchWindow.SelectedTmdbId;
                }
            }
        }
    }
}
