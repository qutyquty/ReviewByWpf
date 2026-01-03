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

        public ICommand SaveCommand { get; }

        public InsertViewModel(IReviewRepository repository, int selectedCategoryId)
        {
            _repository = repository;
            _selectedCategoryId = selectedCategoryId;

            SaveCommand = new RelayCommand(Save, CanSave);
        }

        private void Save(object parameters)
        {
            _repository.AddReview(Title, Content, PosterUrl, _selectedCategoryId);
            Saved?.Invoke(); // 저장 성공 알림
        }

        private bool CanSave(object parameters)
        {
            return !string.IsNullOrWhiteSpace(Content) && !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(PosterUrl);
        }
    }
}
