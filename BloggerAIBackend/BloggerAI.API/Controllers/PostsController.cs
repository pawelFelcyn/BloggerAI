using BloggerAI.Core.Dtos;
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
        private readonly IPostsService _service;

        public PostsController(IPostsService postsService)
        {
            _service = postsService;
        }

        [HttpPost("requestCreation")]
        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> RequestPostCreation(IFormFile file)
        {
            await _service.RequestCreation(file.OpenReadStream(), file.FileName);
            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = "GetAllPostsPolicy")]
        public async Task<ActionResult<PagedResult<PostDto>>> GetAll([FromQuery] PostsFilters filters)
        {
            var result = await _service.GetAll(filters);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "GetPostByIdPolicy")]
        public async Task<ActionResult<PostDetailsDto>> GetById([FromRoute]Guid id)
        {
            var post = await _service.GetById(id);
            return Ok(post);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "DeletePostByIdPolicy")]
        public async Task<ActionResult<PostDetailsDto>> DeleteById([FromRoute] Guid id)
        {
            await _service.DeleteById(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UpdatePostByIdPolicy")]
        public async Task<ActionResult<PostDetailsDto>> UpdateById([FromRoute] Guid id, UpdatePostDto dto)
        {
            await _service.UpdateById(id, dto);
            return NoContent();
        }
    }
}
