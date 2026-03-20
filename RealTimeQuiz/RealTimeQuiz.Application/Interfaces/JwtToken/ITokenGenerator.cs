using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Application.Interfaces.JwtToken
{
    public interface ITokenGenerator
    {
        string GenerateToken(Guid userId, string email, string name);
    }
}
