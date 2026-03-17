
using MediatR;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.AnswerOptions.Commands;

public record CreateAnswerOptionCommand(
    Guid QuestionId,
    Guid CreatorId,
    string OptionText,
    int OrderIndex,
    bool IsCorrect) : IRequest<AnswerOptionResponseDto>;

public class CreateAnswerOptionCommandHandler : IRequestHandler<CreateAnswerOptionCommand, AnswerOptionResponseDto>
{
    private readonly IGenericRepository<AnswerOption> _answerOptionRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public CreateAnswerOptionCommandHandler(
        IGenericRepository<AnswerOption> answerOptionRepository,
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<AnswerOptionResponseDto> Handle(CreateAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        // Check question exists
        var question = await _questionRepository.GetByIdAsync(request.QuestionId);

        if (question == null)
            throw new KeyNotFoundException("Question not found");

        // Check only creator can add options
        var quiz = await _quizRepository.GetByIdAsync(question.QuizId);

        if (quiz!.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to add options to this question");

        var answerOption = AnswerOption.Create(
            request.QuestionId,
            request.OptionText,
            request.OrderIndex,
            request.IsCorrect);

        await _answerOptionRepository.AddAsync(answerOption);
        await _answerOptionRepository.SaveChangesAsync();

        return new AnswerOptionResponseDto
        {
            Id = answerOption.Id,
            OptionText = answerOption.OptionText,
            OrderIndex = answerOption.OrderIndex,
            IsCorrect = answerOption.IsCorrect
        };
    }
}
