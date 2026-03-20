using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Response;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record StartResponseCommand(
    Guid QuizId,
    Guid? UserId,
    string SessionId,
    string IpAddressHash) : IRequest<ResponseDto>;

public class StartResponseCommandHandler : IRequestHandler<StartResponseCommand, ResponseDto>
{
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IQuizHubService _quizHubService;

    public StartResponseCommandHandler(
        IGenericRepository<Response> responseRepository,
        IGenericRepository<Quiz> quizRepository,
        IQuizHubService quizHubService)
    {
        _responseRepository = responseRepository;
        _quizRepository = quizRepository;
        _quizHubService = quizHubService;
    }

    public async Task<ResponseDto> Handle(StartResponseCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if(quiz == null)
        {
            throw new KeyNotFoundException("Quiz not found");
        }

        // Check if quiz is active
        if (!quiz.IsActive)
            throw new InvalidOperationException("Quiz is not active");

        //check if anonnymous is allowed
        if(!quiz.IsAnonymousAllowed && request.UserId == null)
        {
            throw new UnauthorizedAccessException("This quiz requires authentication");
        }

        //ip duplicate prevention
        var existingResponse = await _responseRepository
            .GetAll()
            .FirstOrDefaultAsync(r =>
               r.QuizId == request.QuizId &&
               r.IpAddressHash == request.IpAddressHash,
               cancellationToken);

        if (existingResponse != null)
            throw new InvalidOperationException("You have already responded to this quiz");

        var response = Response.Create(
            request.QuizId,
            request.SessionId,
            request.IpAddressHash,
            request.UserId);

        await _responseRepository.AddAsync(response);
        await _responseRepository.SaveChangesAsync();

        //get toal particiapnts for this quiz
        var totalParticiapnts = await _responseRepository
            .GetAll()
            .CountAsync(r => r.QuizId == request.QuizId, cancellationToken);

        //notify with signal
        await _quizHubService.NotifyParticipantJoined(request.QuizId, totalParticiapnts);

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

