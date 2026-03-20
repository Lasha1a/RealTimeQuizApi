using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RealTimeQuiz.Application.DTOs.User;
using RealTimeQuiz.Application.Features.Users.Commands;
using RealTimeQuiz.Application.Features.Users.Queries;
using System.Security.Claims;

namespace RealTimeQuizApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [EnableRateLimiting("auth")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var command = new RegisterUserCommand(
            request.Username,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [EnableRateLimiting("auth")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    //needs token
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request)
    {
        //get user from jwt token claims
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new ChangePasswordCommand(
            userId,
            request.CurrentPassword,
            request.NewPassword);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize] 
    public async Task<IActionResult> GetCurrentUser()
    {
        // Get userId from JWT token claims
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
