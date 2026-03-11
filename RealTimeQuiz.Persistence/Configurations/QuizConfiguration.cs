using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("quizzes");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .HasColumnName("id");

        builder.Property(q => q.CreatorId)
            .HasColumnName("creator_id")
            .IsRequired();

        builder.Property(q => q.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(q => q.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(q => q.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(q => q.Visibility)
            .HasColumnName("visibility")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(q => q.AccessCode)
            .HasColumnName("access_code")
            .HasMaxLength(20);

        builder.Property(q => q.IsAnonymousAllowed)
            .HasColumnName("is_anonymous_allowed")
            .HasDefaultValue(false);

        builder.Property(q => q.StartsAt)
            .HasColumnName("starts_at");

        builder.Property(q => q.EndsAt)
            .HasColumnName("ends_at");

        builder.Property(q => q.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        // Relationships
        builder.HasOne(q => q.Creator)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.Responses)
            .WithOne(r => r.Quiz)
            .HasForeignKey(r => r.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Analytics)
            .WithOne(a => a.Quiz)
            .HasForeignKey<QuizAnalytics>(a => a.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
