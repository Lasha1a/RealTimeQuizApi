using RealTimeQuiz.Domain.Common;
using RealTimeQuiz.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class Quiz : BaseEntity
{
    public Guid CreatorId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public QuizType Type { get; private set; } = QuizType.Quiz;
    public QuizVisibility Visibility { get; private set; } = QuizVisibility.Public;
    public string AccessCode { get; private set; } = string.Empty;
    public bool IsAnonymousAllowed { get; private set; } = false;           
    public DateTime StartsAt { get; private set; }
    public DateTime EndsAt { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;


    public User Creator { get; private set; } = null!;
    public IReadOnlyCollection<Question> Questions { get; private set; } = new List<Question>();
    public IReadOnlyCollection<Response> Responses { get; private set; } = new List<Response>();
    public QuizAnalytics? Analytics { get; private set; }

    private Quiz() { }

    public static Quiz Create(
        Guid creatorId,
        string title,
        string description,
        string accessCode,
        bool isAnonymousAllowed,
        DateTime startsAt,
        DateTime endsAt,
        bool isActive)
    {
        return new Quiz
        {
            CreatorId = creatorId,
            Title = title,
            Description = description,
            Type = QuizType.Quiz,
            Visibility = QuizVisibility.Public,
            AccessCode = accessCode,
            IsAnonymousAllowed = isAnonymousAllowed,
            StartsAt = startsAt,
            EndsAt = endsAt,
            IsActive = isActive
        };
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title;
        Description = description;
    }
    
    public void UpdateSchedule(DateTime startsAt, DateTime endsAt)
    {
        StartsAt = startsAt;
        EndsAt = endsAt;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void MakePublic() => Visibility = QuizVisibility.Public;
    public void MakePrivate() => Visibility = QuizVisibility.Private;

    public void AllowAnonymous() => IsAnonymousAllowed = true;
    public void DisallowAnonymous() => IsAnonymousAllowed = false;
}
