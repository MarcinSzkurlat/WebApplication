using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Dtos;
using WebApplicationAPI.Interfaces.Services;

namespace WebApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebApplicationController : ControllerBase
    {
        private readonly IStackOverflowApiService _stackOverflowApiService;
        private readonly ITagService _tagService;
        private readonly IConfiguration _configuration;

        public WebApplicationController(IStackOverflowApiService stackOverflowApiService, ITagService tagService, IConfiguration configuration)
        {
            _stackOverflowApiService = stackOverflowApiService;
            _tagService = tagService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTagsAsync()
        {
            var tags = await _tagService.GetTagsAsync();

            return Ok(tags);
        }

        [HttpPost]
        public async Task<ActionResult> RefreshTagsAsync()
        {
            int.TryParse(_configuration["PagesToFetch"], out int pagesToFetch);

            await _tagService.DeleteTagsAsync();
            await _stackOverflowApiService.GetTagsAsync(pagesToFetch);

            return Ok();
        }
    }
}
