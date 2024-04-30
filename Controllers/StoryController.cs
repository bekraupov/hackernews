using AutoMapper;
using hackernews.Core.Dto;
using hackernews.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace hackernews.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : Controller
    {
        private IStoryService _service;
        private IMapper _mapper;

        public StoryController(IStoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Return top stroes
        /// </summary>
        /// <param name="topX"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> GetTopBestStory(int topX)
        {
            if (topX <= 0)
            {
                return BadRequest();
            }

            var models = await _service.GetTopBestStories(topX);

            var x = models.Select(x => _mapper.Map<StoryDto>(x));
            return Ok(x);
            
        }
    }
}
