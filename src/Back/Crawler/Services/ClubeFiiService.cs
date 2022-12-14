using Crawler.Abtsracts;
using Crawler.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Crawler.Services
{
    internal class ClubeFiiService : IFiiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ClubeFiiService> _logger;

        public ClubeFiiService(HttpClient httpClient, IMemoryCache cache, ILogger<ClubeFiiService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Fii>> GetFiiAsync(IEnumerable<string>? fiis, CancellationToken cancellationToken = default)
        {
            var _data = await LoadData(cancellationToken);
            return _data.Where(d => fiis?.Any() != true || fiis.Select(f => f.ToUpper()).Contains(d.Name.ToUpper())).OrderBy(d => d.Name);
        }


        private async Task<IEnumerable<Fii>> LoadData(CancellationToken cancellationToken = default)
        {
            try
            {
                var CACHE_KEY = DateTime.UtcNow.ToShortDateString();

                if (_cache.TryGetValue(CACHE_KEY, out IEnumerable<Fii> fiis))
                {
                    return fiis;
                }

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("dat_ref_ini", DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy")),
                    new KeyValuePair<string, string>("somente_fiis_seguidos", "false"),
                    new KeyValuePair<string, string>("somente_cod_isi_padrao", "false")
                };

                var content = new FormUrlEncodedContent(nvc);

                var response = await _httpClient.PostAsync("proventos-rendimento-distribuicoes-amortizacoes_ajx?verifica_ultimo_provento=false", content, cancellationToken);

                var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"[ERROR]: {htmlContent}");
                    response.EnsureSuccessStatusCode();
                }

                var document = new HtmlDocument();
                document.LoadHtml(htmlContent);

                var table = document.GetElementbyId("table_data_fis");
                var rows = table.SelectNodes("tr");

                var list = new List<Fii>();

                foreach (var row in rows)
                {
                    var name = row.SelectSingleNode("td[1]/a")?.InnerText?.Trim();
                    var value = row.SelectSingleNode("td[4]/a")?.InnerText?.Trim();
                    var date = row.SelectSingleNode("td[11]/a")?.InnerText?.Trim();

                    list.Add(new Fii
                    {
                        Name = name,
                        Value = value,
                        Date = date
                    });
                }

                _cache.Set(CACHE_KEY, list);

                return list;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"[ERROR][{ex.Message}]: {ex.StackTrace}");
                throw;
            }
        }
    }
}
