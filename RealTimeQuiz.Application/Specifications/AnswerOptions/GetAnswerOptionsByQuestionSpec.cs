using RealTimeQuiz.Application.Specifications.Base;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Specifications.AnswerOptions;

public class GetAnswerOptionsByQuestionSpec : BaseSpecification<AnswerOption>
{
    public GetAnswerOptionsByQuestionSpec(Guid questionId)
        : base(a => a.QuestionId == questionId)
    {
        AddOrderBy(a => a.OrderIndex);
    }
}
