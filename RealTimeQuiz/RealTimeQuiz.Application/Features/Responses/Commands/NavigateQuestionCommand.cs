using MediatR;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record NavigateQuestionCommand(
     Guid QuizId,
     Guid CreatorId,
     Guid QuestionId,
     int QuestionIndex) : IRequest<bool>;

public class NavigateQuestionCommandHandler : IRequestHandler<NavigateQuestionCommand, bool>
{
    private readonly IGenericRepository<Quiz> _quizRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IQuizHubService _quizHubService;

    public NavigateQuestionCommandHandler(
        IGenericRepository<Quiz> quizRepository,
        IGenericRepository<Question> questionRepository,
        IQuizHubService quizHubService)
    {
        _quizRepository = quizRepository;
        _questionRepository = questionRepository;
        _quizHubService = quizHubService;
    }

    public async Task<bool> Handle(NavigateQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if(quiz == null)
        {
            throw new KeyNotFoundException("quiz not found");
        }

        if (quiz.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to navigate questions");

        // Check quiz is active
        if (!quiz.IsActive)
            throw new InvalidOperationException("Quiz is not active");

        // Check question exists
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        // Notify all players via SignalR to move to this question
        await _quizHubService.NotifyQuestionNavigation(
            request.QuizId,
            request.QuestionId,
            request.QuestionIndex);

        return true;
    }
}
