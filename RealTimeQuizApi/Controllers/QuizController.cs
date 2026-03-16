using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Application.DTOs.Quiz;
using RealTimeQuiz.Application.Features.Quizzes.Commands;
using RealTimeQuiz.Application.Features.Quizzes.Queries;
using System.Security.Claims;

namespace RealTimeQuizApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly ISender _mediator;

    public QuizController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-quiz")]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequestDto request)
    {
        //get creatorid from jwt
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new CreateQuizCommand(
            creatorId,
            request.Title,
            request.Description,
            request.Type,
            request.Visibility,
            request.AccessCode,
            request.IsAnonymousAllowed,
            request.StartsAt,
            request.EndsAt);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update {id}")]
    public async Task<IActionResult> UpdateQuiz(Guid id, [FromBody] UpdateQuizRequestDto request)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new UpdateQuizCommand(
            id,
            creatorId,
            request.Title,
            request.Description,
            request.Visibility,
            request.AccessCode,
            request.IsAnonymousAllowed,
            request.StartsAt,
            request.EndsAt);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("delete {id}")]
    public async Task<IActionResult> DeleteQuiz(Guid id)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new DeleteQuizCommand(id, creatorId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // anyone can view a quiz
    public async Task<IActionResult> GetQuizById(Guid id)
    {
        var query = new GetQuizByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("get-all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllQuizzes([FromQuery] int pageNumber =1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllQuizzesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("my-quizzes")]
    public async Task<IActionResult> GetMyQuizzes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var query = new GetQuizzesByCreatorQuery(creatorId, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateQuiz(Guid id, [FromQuery] bool isActive)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new ActivateQuizCommand(id, creatorId, isActive);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
