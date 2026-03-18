
using MediatR;
using Microsoft.AspNetCore.SignalR;
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
    private readonly IHubContext<QuizHub, IQuizHub> _hubContext;
}

