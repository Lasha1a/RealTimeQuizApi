using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .HasColumnName("id");

        builder.Property(q => q.QuizId)
            .HasColumnName("quiz_id")
            .IsRequired();

        builder.Property(q => q.QuestionText)
            .HasColumnName("question_text")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(q => q.QuestionType)
            .HasColumnName("question_type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(q => q.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        builder.Property(q => q.IsRequired)
            .HasColumnName("is_required")
            .HasDefaultValue(true);

        builder.Property(q => q.TimeLimitSeconds)
            .HasColumnName("time_limit_seconds")
            .HasDefaultValue(30);

        builder.Property(q => q.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        builder.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        // Relationships
        builder.HasMany(q => q.AnswerOptions)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.ResponseAnswers)
            .WithOne(r => r.Question)
            .HasForeignKey(r => r.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
