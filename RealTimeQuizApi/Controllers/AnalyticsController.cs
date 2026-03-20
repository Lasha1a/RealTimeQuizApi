using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeQuiz.Application.Features.Analytics.Queries;

namespace RealTimeQuizApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly ISender _mediator;

    public AnalyticsController(ISender mediator) 
    {
        _mediator = mediator;
    }

    [HttpGet("{quizId}")]
    [Authorize]
    public async Task<IActionResult> Analytics( Guid quizId)
    {
        
        var query = new GetQuizAnalyticsQuery(quizId);
        var result = await _mediator.Send(query);
        return Ok(result);

    }
}
