using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Application.DTOs.Response;
using RealTimeQuiz.Application.Features.Responses.Commands;
using RealTimeQuiz.Application.Features.Responses.Queries;
using RealTimeQuiz.Domain.Entities;
using System.Security.Claims;

namespace RealTimeQuizApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResponseController : ControllerBase
{
    private readonly ISender _mediator;

    public ResponseController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartResponse([FromBody] StartResponseRequestDto request)
    {
        //get userId from jwt if authenticated, null if annonymous
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = userIdClaim != null ? Guid.Parse(userIdClaim) : null;

        //get ip address and hash it for duplicate preventions
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var ipHash = Convert.ToBase64String(
             System.Security.Cryptography.SHA256.HashData(
                 System.Text.Encoding.UTF8.GetBytes(ipAddress)));

        //generate session ID if not provided 
        var sessionId = request.SessionId ?? Guid.NewGuid().ToString();

        var command = new StartResponseCommand(
            request.QuizId,
            userId,
            sessionId,
            ipHash);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("submit-single")]
    public async Task<IActionResult> SubmitSingleAnswer([FromBody] SubmitSingleAnswerRequestDto request, [FromQuery] Guid responseId)
    {
        var command = new SubmitSingleAnswerCommand(
            responseId,
            request.QuestionId,
            request.SelectedOptionIds,
            request.TextAnswer,
            request.RatingValue);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("submit-all")]
    public async Task<IActionResult> SubmitAllAnswers([FromBody] SubmitAllAnswersRequestDto request, [FromQuery] Guid responseId)
    {
        var command = new SubmitAllAnswersCommand(
            responseId,
            request.Answers);

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteResponse([FromQuery] Guid responseId)
    {
        var command = new CompleteResponseCommand(responseId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{quizId}")]
    [Authorize] // only creator can see responses
    public async Task<IActionResult> GetResponsesByQuiz(Guid quizId)
    {
        var query = new GetResponsesByQuizQuery(quizId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/navigate")]
    public async Task<IActionResult> NavigateQuestion(Guid id, [FromQuery] Guid questionId, [FromQuery] int questionIndex)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new NavigateQuestionCommand(id, creatorId, questionId, questionIndex);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/announce-results")]
    public async Task<IActionResult> AnnounceResults(Guid id)
    {
        var creatorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new AnnounceResultsCommand(id, creatorId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
