using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Analytics;

public class QuestionDistributionDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int TotalAnswers { get; set; }
    public List<OptionDistributionDto> OptionDistributions { get; set; } = new();

}
