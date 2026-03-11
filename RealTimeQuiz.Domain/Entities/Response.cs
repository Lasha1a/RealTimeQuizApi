using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class Response : BaseEntity
{
    public Guid QuizId { get; private set; }
    public Guid? UserId { get; private set; } // nullable for anonymous
    public string SessionId { get; private set; } = string.Empty;
    public string IpAddressHash { get; private set; } = string.Empty;
    public DateTime StartedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; private set; } // nullable, not completed yet

 
    public Quiz Quiz { get; private set; } = null!;
    public User? User { get; private set; } // nullable for anonymous
    public IReadOnlyCollection<ResponseAnswer> ResponseAnswers { get; private set; } = new List<ResponseAnswer>();

    private Response() { }

    public static Response Create(
        Guid quizId,
        string sessionId,
        string ipAddressHash,
        Guid? userId = null)
    {
        return new Response
        {
            QuizId = quizId,
            UserId = userId,
            SessionId = sessionId,
            IpAddressHash = ipAddressHash
        };
    }

    public void Complete() => CompletedAt = DateTime.UtcNow;
}
