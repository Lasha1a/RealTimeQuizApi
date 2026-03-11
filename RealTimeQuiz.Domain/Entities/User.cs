using RealTimeQuiz.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation properties
    public IReadOnlyCollection<Quiz> Quizzes { get; private set; } = new List<Quiz>();
    public IReadOnlyCollection<Response> Responses { get; private set; } = new List<Response>();

    private User() { }

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
    }

    public void UpdateEmail(string email) => Email = email;
    public void UpdatePassword(string passwordHash) => PasswordHash = passwordHash;
}
