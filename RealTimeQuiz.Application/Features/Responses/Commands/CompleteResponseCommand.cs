using MediatR;
using RealTimeQuiz.Application.DTOs.Response;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record CompleteResponseCommand(Guid ResponseId) : IRequest<ResponseDto>;

public class CompleteResponseCommandHandler : IRequestHandler<CompleteResponseCommand, ResponseDto>
{
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IQuizHubService _quizHubService;

    public CompleteResponseCommandHandler(
        IGenericRepository<Response> responseRepository,
        IGenericRepository<Question> questionRepository,
        IQuizHubService quizHubService)
    {
        _responseRepository = responseRepository;
        _questionRepository = questionRepository;
        _quizHubService = quizHubService;
    }

    public async Task<ResponseDto> Handle(CompleteResponseCommand request, CancellationToken cancellationToken)
    {
        var response = await _responseRepository.GetByIdAsync(request.ResponseId);

        if (response == null)
            throw new KeyNotFoundException("Response session not found");

        // Check not already completed
        if (response.CompletedAt != null)
            throw new InvalidOperationException("Response already completed");

        // Mark as completed
        response.Complete();

        _responseRepository.Update(response);
        await _responseRepository.SaveChangesAsync();

        // Notify creator via SignalR that someone completed the quiz
        await _quizHubService.NotifyQuizCompleted(response.QuizId);

        return new ResponseDto
        {
            Id = response.Id,
            QuizId = response.QuizId,
            UserId = response.UserId,
            SessionId = response.SessionId,
            StartedAt = response.StartedAt,
            CompletedAt = response.CompletedAt
        };
    }
}
