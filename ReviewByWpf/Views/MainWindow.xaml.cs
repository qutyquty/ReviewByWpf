using ReviewByWpf.ViewModles;
using System.Windows;

namespace ReviewByWpf.Views
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private String CurrentPosterUrl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PosterSelectButton_Click(object sender, RoutedEventArgs e)
        {
            TmdbSearchWindow searchWindow = new TmdbSearchWindow();
            searchWindow.Owner = this;
            searchWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (searchWindow.ShowDialog() == true)
            {
                var vm = DataContext as ReviewViewModel;
                if (vm != null)
                {
                    vm.PosterUrl = searchWindow.SelectedPosterUrl;
                    vm.FirstYear = searchWindow.SelectedFirstYear;
                    vm.TmdbId = searchWindow.SelectedTmdbId;
                }
            }
        }

        private void OpenInsertWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // MainWindow의 DataContext는 ReviewViewModel
            var mainVM = this.DataContext as ReviewViewModel;
            if (mainVM == null) return;
            
            // ReviewModel 안에 있는 repo 가져오기
            var repo = mainVM.Repository;
            var selectedCategoryId = mainVM.SelectedCategory.Id;

            var insertWindow = new InsertWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = new InsertViewModel(repo, selectedCategoryId)
            };

            if (insertWindow.ShowDialog() == true)
            {
                mainVM?.LoadReviewsByCategory(selectedCategoryId);
                mainVM.StatusMessage = $"저장 완료";
            }
        }
    }
}
