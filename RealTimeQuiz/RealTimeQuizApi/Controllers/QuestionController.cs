using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Features.Questions.Commands;
using RealTimeQuiz.Application.Features.Questions.Queries;
using System.Security.Claims;

namespace RealTimeQuizApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly ISender _mediator;

    public QuestionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateQuestion(Guid quizId, [FromBody] CreateQuestionRequestDto request)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new CreateQuestionCommand(
            quizId,
            creatorId,
            request.QuestionText,
            request.QuestionType,
            request.OrderIndex,
            request.IsRequired,
            request.TimeLimitSeconds,
            request.ImageUrl);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update{questionId}")]
    public async Task<IActionResult> UpdateQuestion(Guid quizId, Guid questionId, [FromBody] UpdateQuestionRequestDto request)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new UpdateQuestionCommand(
            questionId,
            creatorId,
            request.QuestionText,
            request.QuestionType,
            request.OrderIndex,
            request.IsRequired,
            request.TimeLimitSeconds,
            request.ImageUrl);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("delete{questionId}")]
    public async Task<IActionResult> DeleteQuestion(Guid quizId, Guid questionId)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new DeleteQuestionCommand(questionId, creatorId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("questions-by-quiz")]
    [AllowAnonymous]
    public async Task<IActionResult> GetQuestionsByQuiz(Guid quizId)
    {
        var query = new GetQuestionsByQuizQuery(quizId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
