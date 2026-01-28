using ReviewByWpf.Helpers;
using ReviewByWpf.Models;
using ReviewByWpf.Services;
using ReviewByWpf.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace ReviewByWpf.ViewModles
{
    public class TmdbSearchUpViewModel : BaseViewModel
    {
        private string _query;
        public string Query
        {
            get => _query;
            set => SetProperty(ref _query, value);
        }

        private MTPoster _selectedMTPoster;
        public MTPoster SelectedMTPoster
        {
            get => _selectedMTPoster;
            set => SetProperty(ref _selectedMTPoster, value);
        }

        public ObservableCollection<MTPoster> Posters { get; } = new();

        public ICommand SearchCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand ConfirmCommand { get; }

        private int _currentPage = 1;
        private bool _isLoading = false;

        public TmdbSearchUpViewModel()
        {
            SearchCommand = new AsyncRelayCommand(Search, _ => CanSearch());
            LoadMoreCommand = new AsyncRelayCommand(LoadMore);
            ConfirmCommand = new RelayCommand(_ => Confirm());
        }

        private bool CanSearch() => !string.IsNullOrWhiteSpace(Query);

        private async Task Search(object parameter)
        {
            Posters.Clear();
            _currentPage = 1;
            await LoadPage(_currentPage);
        }

        private async Task LoadMore(object parameter)
        {
            if (_isLoading) return;
            _currentPage++;
            await LoadPage(_currentPage);
        }

        private async Task LoadPage(int page)
        {
            _isLoading = true;

            var config = new ConfigService();
            string _apiKey = config.GetTmdbApiKey();

            using var client = new HttpClient();
            var url = $"https://api.themoviedb.org/3/search/multi?api_key={_apiKey}&query={Query}&page={page}";
            var response = await client.GetStringAsync(url);

            var json = JsonDocument.Parse(response);
            var results = json.RootElement.GetProperty("results");

            foreach (var item in results.EnumerateArray())
            {
                if (item.TryGetProperty("poster_path", out var posterPath))
                {                    
                    Posters.Add(new MTPoster
                    {
                        TmdbId = item.GetProperty("id").GetInt32(),
                        PosterUrl = string.IsNullOrEmpty(posterPath.GetString())
                            ? "https://placehold.co/300x450.png?text=No+Image"
                            : "https://image.tmdb.org/t/p/w500" + posterPath.GetString(),
                        FirstYear = item.TryGetProperty("release_date", out var releaseDate) ? releaseDate.GetString().Substring(0, 4) :
                                    item.TryGetProperty("first_air_date", out var firstAirDate) ? firstAirDate.GetString().Substring(0, 4) :
                                    string.Empty
                    });
                }
            }

            _isLoading = false;
        }

        private void Confirm()
        {
            var window = Application.Current.Windows.OfType<TmdbSearchUpWindow>().FirstOrDefault();
            if (window != null)
            {
                window.DialogResult = true;
            }
        }
    }
}
