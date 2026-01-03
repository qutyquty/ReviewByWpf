using System.IO;
using System.Text.Json;

namespace ReviewByWpf.Services
{
    public class ConfigService
    {
        private readonly JsonDocument _config;

        public ConfigService()
        {
            var json = File.ReadAllText("appsettings.json");
            _config = JsonDocument.Parse(json);
        }

        public string GetTmdbApiKey()
        {
            return _config.RootElement.GetProperty("TMDB").GetProperty("ApiKey").GetString();
        }
    }
}
