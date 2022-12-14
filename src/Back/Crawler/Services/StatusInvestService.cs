using Crawler.Abtsracts;
using Crawler.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Web;

namespace Crawler.Services
{
    internal class StatusInvestService : IFiiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<StatusInvestService> _logger;

        public StatusInvestService(HttpClient httpClient, IMemoryCache cache, ILogger<StatusInvestService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Fii>> GetFiiAsync(IEnumerable<string>? fiis, CancellationToken cancellationToken = default)
        {
            var _data = await LoadData(fiis, cancellationToken);
            return _data;
        }


        private async Task<IEnumerable<Fii>> LoadData(IEnumerable<string>? fiis, CancellationToken cancellationToken = default)
        {
            try
            {
                var CACHE_KEY = DateTime.UtcNow.ToShortDateString();

                if (_cache.TryGetValue(CACHE_KEY, out IEnumerable<Fii> fiis_cached))
                {
                    return fiis_cached;
                }

                var response = await _httpClient.GetAsync("fundos-imobiliarios/proventos/ifix", cancellationToken);
                var htmlContent = await response.Content.ReadAsStringAsync(cancellationToken);

                var document = new HtmlDocument();
                document.LoadHtml(htmlContent);

                var input = document.GetElementbyId("result");


                var value = input.GetAttributeValue<string>("value", "");

                value = HttpUtility.HtmlDecode(value);

                var json = JsonDocument.Parse(value);
                var root = json.RootElement;

                var datePayment = root.GetProperty("datePayment");

                var payments = datePayment.EnumerateArray();
                var list = new List<Fii>();

                while (payments.MoveNext())
                {
                    var payment = payments.Current;

                    list.Add(new Fii
                    {
                        Name = payment.GetProperty("code").ToString(),
                        Date = payment.GetProperty("paymentDividend").ToString(),
                        Value = payment.GetProperty("resultAbsoluteValue").ToString(),
                    });
                }

                list = (from a in fiis
                       join b in list on a equals b.Name into fii
                       from left in fii.DefaultIfEmpty()
                       select new Fii
                       {
                           Name = a,
                           Date = left?.Date,
                           Value = left?.Value
                       }).ToList();

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
