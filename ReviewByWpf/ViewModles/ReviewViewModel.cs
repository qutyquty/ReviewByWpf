using ReviewByWpf.Helpers;
using ReviewByWpf.Models;
using ReviewByWpf.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ReviewByWpf.ViewModles
{
    public class ReviewViewModel : BaseViewModel
    {
        private readonly MySqlBackupService _backupService;

        private readonly IReviewRepository _repository;

        public PosterSelectorViewModel PosterSelectorVM { get; } = new PosterSelectorViewModel();

        // 읽기 전용 속성
        public IReviewRepository Repository => _repository;

        public ObservableCollection<Review> Reviews { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private Review _selectedReview;
        public Review SelectedReview
        {
            get => _selectedReview;
            set
            {
                if (SetProperty(ref _selectedReview, value))
                {
                    Title = _selectedReview?.Title;
                    Content = _selectedReview?.Content;
                    PosterUrl = string.IsNullOrEmpty(_selectedReview?.PosterPath)
                        ? "https://placehold.co/300x450.png?text=No+Image"
                        : $"https://image.tmdb.org/t/p/w200{_selectedReview?.PosterPath}";
                    FirstYear = _selectedReview?.FirstYear;
                    TmdbId = _selectedReview?.TmdbId;
                    StatusMessage = "";
                }
            }
        }

        private string _searchKeyword;

        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    LoadReviewsByCategory(_selectedCategory?.Id);
                }
            }                
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private string _posterUrl;
        public string PosterUrl
        {
            get => _posterUrl;
            set => SetProperty(ref _posterUrl, value);
        }

        private string _firstYear;
        public string FirstYear
        {
            get => _firstYear;
            set => SetProperty(ref _firstYear, value);
        }

        private int? _tmdbId;
        public int? TmdbId
        {
            get => _tmdbId;
            set => SetProperty(ref _tmdbId, value);
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public ICommand SearchCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand BackupCommand { get; }

        public ReviewViewModel(IReviewRepository repository)
        {
            _backupService = new MySqlBackupService();

            _repository = repository;

            Reviews = new ObservableCollection<Review>();

            // 카테고리 초기화
            Categories = new ObservableCollection<Category>
            {
                new Category { Id = 1, Name = "영화"},
                new Category { Id = 2, Name = "드라마"},
                new Category { Id = 3, Name = "애니"},
                new Category { Id = 4, Name = "도서"}
            };

            // 기본 카테고리 설정
            SelectedCategory = Categories[0];

            SearchCommand = new RelayCommand(Search, CanSearch);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            BackupCommand = new RelayCommand(BackupDatabase, CanBackupDatabase);
        }

        private void Search(object parameter)
        {
            var results = _repository.GetReviews()
                .Where(r => r.Title.Contains(SearchKeyword) || r.Content.Contains(SearchKeyword));
            Reviews.Clear();
            foreach (var r in results)
                Reviews.Add(r);
        }

        private bool CanSearch(object parameter)
        {
            return !string.IsNullOrWhiteSpace(SearchKeyword);
        }

        private void Update(object parameter)
        {
            _repository.UpdateReview(SelectedReview.Id, Content, PosterUrl, Title, FirstYear, TmdbId);

            SelectedReview.Content = Content;
            SelectedReview.PosterPath = PosterUrl;
            SelectedReview.Title = Title;
            SelectedReview.FirstYear = FirstYear;
            SelectedReview.TmdbId = TmdbId;

            StatusMessage = $"수정 완료";
        }

        private bool CanUpdate(object parameter)
        {
            // 제목, 내용은 필수 입력
            return !string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(Title);
        }

        private void Delete(object parameter)
        {
            if (SelectedReview != null)
            {
                var result = MessageBox.Show("정말 삭제하시겠습니까?", "삭제 확인",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // DB 삭제
                    _repository.DeleteReview(SelectedReview.Id);
                    // 목록 갱신
                    LoadReviewsByCategory(SelectedCategory.Id);
                    // 하단 컨트롤 데이터 제거
                    SelectedReview = null;

                    StatusMessage = $"삭제 완료";
                }
            }
        }

        private bool CanDelete(object parameter)
        {
            return SelectedReview != null;
        }

        private void BackupDatabase(object parameter)
        {
            string file = _backupService.BackupDatabase("db_review_user", "1234", "db_review");
            StatusMessage = $"백업 완료: {file}";
        }

        private bool CanBackupDatabase(object parameter)
        {
            return true;
        }

        public void LoadReviewsByCategory(int? categoryId)
        {
            var results = _repository.GetReviews()
                .Where(r => !categoryId.HasValue || r.CategoryId == categoryId.Value);

            Reviews.Clear();
            foreach (var r in results)
                Reviews.Add(r);
        }
    }
}
