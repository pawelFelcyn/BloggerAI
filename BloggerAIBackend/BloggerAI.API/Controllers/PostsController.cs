using BloggerAI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggerAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _postsService;

        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }

        [HttpPost("requestCreation")]
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> RequestPostCreation(IFormFile file)
        {
            await _postsService.RequestCreation(file.OpenReadStream(), file.FileName);
            return Ok();
        }
    }
}
