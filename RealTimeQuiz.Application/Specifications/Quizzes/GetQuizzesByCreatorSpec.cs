using RealTimeQuiz.Application.Specifications.Base;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Specifications.Quizzes
{
    public class GetQuizzesByCreatorSpec : BaseSpecification<Quiz>
    {
        public GetQuizzesByCreatorSpec(Guid creatorId, int pageNumber, int pageSize)
        : base(q => q.CreatorId == creatorId)
        {
            AddInclude(q => q.Creator);
            AddOrderByDescending(q => q.CreatedAt);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
