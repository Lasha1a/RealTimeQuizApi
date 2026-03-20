using MediatR;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Features.AnswerOptions.Commands;

public record UpdateAnswerOptionCommand(
    Guid AnswerOptionId,
    Guid CreatorId,
    string OptionText,
    int OrderIndex,
    bool IsCorrect) : IRequest<AnswerOptionResponseDto>;

public class UpdateAnswerOptionCommandHandler : IRequestHandler<UpdateAnswerOptionCommand, AnswerOptionResponseDto>
{
    private readonly IGenericRepository<AnswerOption> _answerOptionRepository;
    private readonly IGenericRepository<Question> _questionRepository;
    private readonly IGenericRepository<Quiz> _quizRepository;

    public UpdateAnswerOptionCommandHandler(
        IGenericRepository<AnswerOption> answerOptionRepository,
        IGenericRepository<Question> questionRepository,
        IGenericRepository<Quiz> quizRepository)
    {
        _answerOptionRepository = answerOptionRepository;
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<AnswerOptionResponseDto> Handle(UpdateAnswerOptionCommand request, CancellationToken cancellationToken)
    {
        var answerOption = await _answerOptionRepository.GetByIdAsync(request.AnswerOptionId);

        if (answerOption == null)
            throw new KeyNotFoundException("Answer option not found");

        // Check question exists
        var question = await _questionRepository.GetByIdAsync(answerOption.QuestionId);

        // Check only creator can update options
        var quiz = await _quizRepository.GetByIdAsync(question!.QuizId);

        if (quiz!.CreatorId != request.CreatorId)
            throw new UnauthorizedAccessException("Unauthorized to update this option");

        answerOption.UpdateDetails(request.OptionText, request.IsCorrect);
        answerOption.UpdateOrder(request.OrderIndex);

        _answerOptionRepository.Update(answerOption);
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
