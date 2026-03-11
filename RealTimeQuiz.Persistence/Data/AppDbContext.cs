using Microsoft.EntityFrameworkCore;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<Quiz> Quizzes { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<AnswerOption> AnswerOptions { get; set; }
    public virtual DbSet<QuizAnalytics> QuizAnalytics { get; set; }
    public virtual DbSet<Response> Responses { get; set; }
    public virtual DbSet<ResponseAnswer> ResponseAnswers { get; set; }
    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
