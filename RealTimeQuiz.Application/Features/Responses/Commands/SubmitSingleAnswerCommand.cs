using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record SubmitSingleAnswerCommand(
    Guid ResponseId,
    Guid QuestionId,
    List<Guid> SelectedOptionIds,
    string? TextAnswer,
    int? RatingValue) : IRequest<bool>;

public class SubmitSingleAnswerCommandHandler : IRequestHandler<SubmitSingleAnswerCommand, bool>
{
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<ResponseAnswer> _responseAnswerRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IQuizHubService _quizHubService;

    public SubmitSingleAnswerCommandHandler(
        IGenericRepository<Response> responseRepository,
        IGenericRepository<ResponseAnswer> responseAnswerRepository,
        IGenericRepository<Question> questionRepository,
        IQuizHubService quizHubService)
    {
        _responseRepository = responseRepository;
        _responseAnswerRepository = responseAnswerRepository;
        _questionRepository = questionRepository;
        _quizHubService = quizHubService;
    }

    public async Task<bool> Handle(SubmitSingleAnswerCommand request, CancellationToken cancellationToken)
    {
        var response = await _responseRepository.GetByIdAsync(request.ResponseId);

        if (response == null)
            throw new KeyNotFoundException("Response session not found");

        // Check response is not already completed
        if (response.CompletedAt != null)
            throw new InvalidOperationException("This response session is already completed");

        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        // Check if already answered this question
        var existingAnswer = await _responseAnswerRepository
            .GetAll()
            .FirstOrDefaultAsync(ra =>
                ra.ResponseId == request.ResponseId &&
                ra.QuestionId == request.QuestionId,
                cancellationToken);

        if (existingAnswer != null)
            throw new InvalidOperationException("This question has already been answered");

        var responseAnswer = ResponseAnswer.Create(
            request.ResponseId,
            request.QuestionId,
            request.SelectedOptionIds,
            request.TextAnswer,
            request.RatingValue);

        await _responseAnswerRepository.AddAsync(responseAnswer);
        await _responseAnswerRepository.SaveChangesAsync();

        var totalAnswers = await _responseAnswerRepository
             .GetAll()
             .CountAsync(ra => ra.QuestionId == request.QuestionId, cancellationToken);

        // Notify creator via SignalR with live stats
        await _quizHubService.NotifyAnswerSubmitted(response.QuizId, new
        {
            QuestionId = request.QuestionId,
            TotalAnswers = totalAnswers
        });

        return true;
    }
}
