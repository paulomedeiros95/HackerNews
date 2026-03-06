using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Santander.HackerNews.Domain.Dtos;
using Santander.HackerNews.Domain.Interfaces;

namespace Santander.HackerNews.Api.Controllers;

[ApiController]
[Route("api/stories")]
public class StoriesController : ControllerBase
{
    #region Fields
    private readonly IStoryService _storyService;
    #endregion

    #region Constructor

    public StoriesController(IStoryService storyService)
    {
        _storyService = storyService;
    }
    #endregion

    #region Endpoints
    //[Authorize]
    [HttpGet("best")]
    [ProducesResponseType(typeof(IEnumerable<StoryDto>), 200)]
    public async Task<IActionResult> GetBestStories([FromQuery] int n, CancellationToken cancellationToken)
    {
        if (n <= 0) return BadRequest("The parameter 'n' must be greater than 0.");

        var stories = await _storyService.GetBestStoriesAsync(n, cancellationToken);

        return Ok(stories); // Returns array of the best n stories 
    }
    #endregion
}