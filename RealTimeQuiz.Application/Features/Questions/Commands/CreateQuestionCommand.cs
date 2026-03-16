using MediatR;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using RealTimeQuiz.Domain.Enums;

namespace RealTimeQuiz.Application.Features.Questions.Commands;

public record CreateQuestionCommand(
    Guid QuizId,
    Guid CreatorId,
    string QuestionText,
    QuestionType QuestionType,
    int OrderIndex,
    bool IsRequired,
    int TimeLimitSeconds,
    string? ImageUrl) : IRequest<QuestionResponseDto>;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionResponseDto>
{
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public CreateQuestionCommandHandler(
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<QuestionResponseDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        //check if quiz exists
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId);

        if (quiz == null)
            throw new KeyNotFoundException("quiz not found");

        //only creator can create it
        if(quiz.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to add questions to this quiz");

        var question = Question.Create(
            request.QuizId,
            request.QuestionText,
            request.QuestionType,
            request.OrderIndex,
            request.IsRequired,
            request.TimeLimitSeconds,
            request.ImageUrl);

        await _questionRepository.AddAsync(question);
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
