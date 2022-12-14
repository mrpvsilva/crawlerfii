using Crawler.Abtsracts;
using Crawler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FiisController : ControllerBase
    {
        private readonly IFiiService _fiiService;

        public FiisController(IFiiService fiiService)
        {
            _fiiService = fiiService;
        }

        [HttpGet]
        public Task<IEnumerable<Fii>> Get([FromQuery] GetFiiQuery? query, CancellationToken cancellationToken)
        {
            query.Fiis ??= new[]
            {
                "ARCT11",
                "BARI11",
                "BRCO11",
                "BRCR11",
                "BTAL11",
                "BTLG11",
                "CPTS11",
                "CVBI11",
                "DEVA11",
                "KNSC11",
                "RBRR11",
                "TRXF11",
                "VISC11",
                "XPML11"
            };

            return _fiiService.GetFiiAsync(query?.Fiis, cancellationToken);
        }
    }
}
