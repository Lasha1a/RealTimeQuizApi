using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Response;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Interfaces.Hubs;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.Responses.Commands;

public record SubmitAllAnswersCommand(
    Guid ResponseId,
    List<SubmitSingleAnswerRequestDto> Answers) : IRequest<ResponseDto>;

public class SubmitAllAnswersCommandHandler : IRequestHandler<SubmitAllAnswersCommand, ResponseDto>
{
    private readonly IGenericRepository<Response> _responseRepository;
    private readonly IGenericRepository<ResponseAnswer> _responseAnswerRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IQuizHubService _quizHubService;

    public SubmitAllAnswersCommandHandler(
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

    public async Task<ResponseDto> Handle(SubmitAllAnswersCommand request, CancellationToken cancellationToken)
    {
        var response = await _responseRepository.GetByIdAsync(request.ResponseId);

        if(response == null)
        {
            throw new KeyNotFoundException("response session not found");
        }

        //check not alrdy completed
        if(response.CompletedAt != null)
        {
            throw new InvalidOperationException("response alrdy completed");
        }

        //save each answer
        foreach(var answer in request.Answers)
        {
            //check question exists
            var question = await _questionRepository.GetByIdAsync(answer.QuestionId);

            if (question == null)
                throw new KeyNotFoundException($"Question {answer.QuestionId} not found");

            //check not alrdy answered
            var existingAnswer = await _responseAnswerRepository
                .GetAll()
                .FirstOrDefaultAsync(ra => 
                   ra.ResponseId == request.ResponseId &&
                   ra.QuestionId == answer.QuestionId,
                   cancellationToken);

            if (existingAnswer != null)
                continue; // skip already answered questions

            var responseAnswer = ResponseAnswer.Create(
                request.ResponseId,
                answer.QuestionId,
                answer.SelectedOptionIds,
                answer.TextAnswer,
                answer.RatingValue);

            await _responseAnswerRepository.AddAsync(responseAnswer);
        }

        //save all answers at once
        await _responseAnswerRepository.SaveChangesAsync();

        // Mark response as completed
        response.Complete();
        _responseRepository.Update(response);
        await _responseRepository.SaveChangesAsync();

        // Notify creator via SignalR
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


