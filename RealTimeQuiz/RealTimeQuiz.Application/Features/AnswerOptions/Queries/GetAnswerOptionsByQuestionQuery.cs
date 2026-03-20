using MediatR;
using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Application.DTOs.Question;
using RealTimeQuiz.Application.Interfaces.GenericRepo;
using RealTimeQuiz.Application.Specifications.AnswerOptions;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Application.Features.AnswerOptions.Queries;

public record GetAnswerOptionsByQuestionQuery(Guid QuestionId) : IRequest<List<AnswerOptionResponseDto>>;

public class GetAnswerOptionsByQuestionQueryHandler : IRequestHandler<GetAnswerOptionsByQuestionQuery, List<AnswerOptionResponseDto>>
{
    private readonly IGenericRepository<AnswerOption> _answerOptionRepository;

    public GetAnswerOptionsByQuestionQueryHandler(IGenericRepository<AnswerOption> answerOptionRepository)
    {
        _answerOptionRepository = answerOptionRepository;
    }

    public async Task<List<AnswerOptionResponseDto>> Handle(GetAnswerOptionsByQuestionQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetAnswerOptionsByQuestionSpec(request.QuestionId);
        var options = await _answerOptionRepository
            .GetQueryWithSpec(spec)
            .ToListAsync(cancellationToken);

        return options.Select(a => new AnswerOptionResponseDto
        {
            Id = a.Id,
            OptionText = a.OptionText,
            OrderIndex = a.OrderIndex,
            IsCorrect = a.IsCorrect
        }).ToList();
    }
}
