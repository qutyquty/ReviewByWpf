using System.Windows;
using ReviewByWpf.Helpers;
using ReviewByWpf.Views;
using System.Windows.Input;
using ReviewByWpf.Services;

namespace ReviewByWpf.ViewModles
{
    public class InsertViewModel : BaseViewModel
    {
        private readonly IReviewRepository _repository;
        private readonly int _selectedCategoryId;

        public event Action Saved;

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

        private int _tmdbId;
        public int TmdbId
        {
            get => _tmdbId;
            set => SetProperty(ref _tmdbId, value);
        }

        public ICommand SaveCommand { get; }

        public InsertViewModel(IReviewRepository repository, int selectedCategoryId)
        {
            _repository = repository;
            _selectedCategoryId = selectedCategoryId;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        private void Save(object parameters)
        {
            _repository.AddReview(Title, Content, PosterUrl, _selectedCategoryId, FirstYear, TmdbId);
            Saved?.Invoke(); // 저장 성공 알림
        }

        private bool CanSave(object parameters)
        {
            // 제목, 내용은 필수 입력
            return !string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(Title);
        }
    }
}
