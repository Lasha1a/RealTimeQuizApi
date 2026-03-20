using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Response;

public class SubmitSingleAnswerRequestDto
{
    public Guid QuestionId { get; set; }
    public List<Guid> SelectedOptionIds { get; set; } = new();
    public string? TextAnswer { get; set; }
    public int? RatingValue { get; set; }
}
