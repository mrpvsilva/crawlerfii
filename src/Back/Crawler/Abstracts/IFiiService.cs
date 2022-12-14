using Crawler.Models;

namespace Crawler.Abtsracts
{
    public interface IFiiService
    {
        Task<IEnumerable<Fii>> GetFiiAsync(IEnumerable<string>? fiis, CancellationToken cancellationToken = default);
    }
}
