using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;

namespace RealTimeQuiz.Persistence.Configurations;

public class QuizAnalyticsConfiguration : IEntityTypeConfiguration<QuizAnalytics>
{
    public void Configure(EntityTypeBuilder<QuizAnalytics> builder)
    {
        builder.ToTable("quiz_analytics");

        builder.HasKey(qa => qa.Id);

        builder.Property(qa => qa.Id)
            .HasColumnName("id");

        builder.Property(qa => qa.QuizId)
            .HasColumnName("quiz_id")
            .IsRequired();

        builder.HasIndex(qa => qa.QuizId)
            .IsUnique();

        builder.Property(qa => qa.TotalResponses)
            .HasColumnName("total_responses")
            .HasDefaultValue(0);

        builder.Property(qa => qa.AverageCompletionTime)
            .HasColumnName("average_completion_time")
            .HasDefaultValue(0);

        builder.Property(qa => qa.CompletionRate)
            .HasColumnName("completion_rate")
            .HasDefaultValue(0);

        builder.Property(qa => qa.LastCalculatedAt)
            .HasColumnName("last_calculated_at")
            .HasDefaultValueSql("NOW()");
    }
}
