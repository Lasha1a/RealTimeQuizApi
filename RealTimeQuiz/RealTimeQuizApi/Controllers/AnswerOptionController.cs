using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Application.DTOs.AnswerOption;
using RealTimeQuiz.Application.Features.AnswerOptions.Commands;
using RealTimeQuiz.Application.Features.AnswerOptions.Queries;
using System.Security.Claims;

namespace RealTimeQuizApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswerOptionController : ControllerBase
{
    private readonly ISender _mediator;

    public AnswerOptionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create {questionId}")]
    public async Task<IActionResult> CreateAnswerOption(Guid questionId, [FromBody] CreateAnswerOptionRequestDto request)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new CreateAnswerOptionCommand(
            questionId,
            creatorId,
            request.OptionText,
            request.OrderIndex,
            request.IsCorrect);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("update {answerOptionId}")]
    public async Task<IActionResult> UpdateAnswerOption(Guid answerOptionId, [FromBody] UpdateAnswerOptionRequestDto request)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new UpdateAnswerOptionCommand(
            answerOptionId,
            creatorId,
            request.OptionText,
            request.OrderIndex,
            request.IsCorrect);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("delete {answerOptionId}")]
    public async Task<IActionResult> DeleteAnswerOption(Guid answerOptionId)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new DeleteAnswerOptionCommand(answerOptionId, creatorId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{questionId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAnswerOptionsByQuestion(Guid questionId)
    {
        var query = new GetAnswerOptionsByQuestionQuery(questionId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
