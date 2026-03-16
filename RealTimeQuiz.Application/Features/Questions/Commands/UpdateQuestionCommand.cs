using MediatR;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using RealTimeQuiz.Domain.Enums;

namespace RealTimeQuiz.Application.Features.Questions.Commands;

public record UpdateQuestionCommand(
    Guid QuestionId,
    Guid CreatorId,
    string QuestionText,
    QuestionType QuestionType,
    int OrderIndex,
    bool IsRequired,
    int TimeLimitSeconds,
    string? ImageUrl) : IRequest<QuestionResponseDto>;


public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionResponseDto>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public UpdateQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<QuestionResponseDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        // Check quiz ownership
        var quiz = await _quizRepository.GetByIdAsync(question.QuizId);

        if (quiz!.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to update this question");

        question.UpdateDetails(
            request.QuestionText,
            request.QuestionType,
            request.TimeLimitSeconds,
            request.ImageUrl);

        question.UpdateOrder(request.OrderIndex);

        _questionRepository.Update(question);
        await _questionRepository.SaveChangesAsync();

        return new QuestionResponseDto
        {
            Id = question.Id,
            QuizId = question.QuizId,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType.ToString(),
            OrderIndex = question.OrderIndex,
            IsRequired = question.IsRequired,
            TimeLimitSeconds = question.TimeLimitSeconds,
            ImageUrl = question.ImageUrl,
            CreatedAt = question.CreatedAt,
            AnswerOptions = new List<AnswerOptionResponseDto>()
        };
    }
}