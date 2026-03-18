using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Response;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Responses.Queries;

public record GetResponsesByQuizQuery(Guid QuizId) : IRequest<List<ResponseDto>>;

public class GetResponsesByQuizQueryHandler : IRequestHandler<GetResponsesByQuizQuery, List<ResponseDto>>
{
    private readonly IGenericRepository<Response> _responseRepository;

    public GetResponsesByQuizQueryHandler(IGenericRepository<Response> responseRepository)
    {
        _responseRepository = responseRepository;
    }

    public async Task<List<ResponseDto>> Handle(GetResponsesByQuizQuery request, CancellationToken cancellationToken)
    {
        var responses = await _responseRepository
            .GetAll()
            .Where(r => r.QuizId == request.QuizId)
            .ToListAsync(cancellationToken);

        return responses.Select(r => new ResponseDto
        {
            Id = r.Id,
            QuizId = r.QuizId,
            UserId = r.UserId,
            SessionId = r.SessionId,
            StartedAt = r.StartedAt,
            CompletedAt = r.CompletedAt
        }).ToList();
    }
}
