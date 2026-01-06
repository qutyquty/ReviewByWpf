using Newtonsoft.Json;
using ReviewByWpf.Services;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ReviewByWpf.Views
{
    /// <summary>
    /// TmdbSearchWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TmdbSearchWindow : Window
    {
        public string SelectedPosterUrl { get; private set; }
        public string SelectedFirstYear { get; private set; }
        public string SelectedTmdbId { get; set; }

        public TmdbSearchWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            string query = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(query)) return;

            var config = new ConfigService();
            string apiKey = config.GetTmdbApiKey();
            string url = $"https://api.themoviedb.org/3/search/multi?api_key={apiKey}&query={Uri.EscapeDataString(query)}&language=ko-KR";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                dynamic result = JsonConvert.DeserializeObject(response);

                PosterPanel.Children.Clear();

                foreach (var mt in result.results)
                {
                    // 개봉/첫방영년도 가져오기
                    string firstDate = "";
                    string firstYear = "";
                    string mediaType = mt.media_type;
                    if (mediaType == "movie")
                    {
                        firstDate = mt.release_date;
                    } else if (mediaType == "tv")
                    {
                        firstDate = mt.first_air_date;
                    }
                    if (!string.IsNullOrEmpty(firstDate))
                        firstYear = firstDate.Substring(0, 4);

                    // tmdb id 가져오기
                    string tmdbId = mt.id;

                    // 포스터 가져오기
                    string posterPath = mt.poster_path;
                    if (posterPath != null)
                    {
                        String posterUrl = $"https://image.tmdb.org/t/p/w200{posterPath}";
                        Image img = new Image
                        {
                            Width = 150,
                            Height = 220,
                            Margin = new Thickness(5),
                            Source = new BitmapImage(new Uri(posterUrl)),
                            //Tag = posterUrl,
                            //ToolTip = firstYear,                            
                            Tag = (posterUrl, firstYear, tmdbId)
                        };

                        Border border = new Border
                        {
                            BorderBrush = Brushes.Transparent,
                            BorderThickness = new Thickness(0),
                            Child = img,
                            Margin = new Thickness(5)
                        };

                        // 포스터 클릭 이벤트 연결
                        img.MouseLeftButtonUp += PosterImage_Click;

                        PosterPanel.Children.Add(border);
                    }
                }
            }

            Mouse.OverrideCursor = null;
        }

        private void PosterImage_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image img)
            {
                // 모든 포스터 테두리 초기화
                foreach (var child in PosterPanel.Children)
                {
                    if (child is Border border)
                    {
                        border.BorderBrush = Brushes.Transparent;
                        border.BorderThickness = new Thickness(0);
                    }
                }

                if (img.Tag is ValueTuple<string, string, string> data)
                {
                    SelectedPosterUrl = data.Item1; // posterUrl
                    SelectedFirstYear = data.Item2; // firstYear
                    SelectedTmdbId = data.Item3; // tmdbId
                }

                // SelectedPosterUrl = url;
                // SelectedFirstYear = img.ToolTip?.ToString();

                if (img.Parent is Border clickedBorder)
                {
                    clickedBorder.BorderBrush = Brushes.Red;
                    clickedBorder.BorderThickness = new Thickness(3);
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SelectedPosterUrl))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("포스터를 선택하세요.");
            }
        }
    }
}
