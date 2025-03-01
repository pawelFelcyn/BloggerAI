using BloggerAI.Core.Dtos;
using BloggerAI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerAI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IBloggerAIAuthenticationService _service;

    public AuthenticationController(IBloggerAIAuthenticationService bloggerAIAuthenticationService)
    {
        _service = bloggerAIAuthenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
    {
        var jwtToken = await _service.GetJwtToken(loginDto);
        return Ok(jwtToken);
    }

    [HttpPatch("changePassword")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordDto dto)
    {
        await _service.ChangePassword(dto);
        return NoContent();
    }
}
