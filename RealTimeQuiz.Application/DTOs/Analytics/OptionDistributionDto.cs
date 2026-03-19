using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.DTOs.Analytics;

public class OptionDistributionDto
{
    public Guid OptionId { get; set; }
    public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int SelectionCount { get; set; }
    public double Percentage { get; set; }
}
