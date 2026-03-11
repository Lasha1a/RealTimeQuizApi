using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Configurations;

 class ResponseConfiguration : IEntityTypeConfiguration<Response>
{
    public void Configure(EntityTypeBuilder<Response> builder)
    {
        builder.ToTable("responses");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id");

        builder.Property(r => r.QuizId)
            .HasColumnName("quiz_id")
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasColumnName("user_id");

        builder.Property(r => r.SessionId)
            .HasColumnName("session_id")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.IpAddressHash)
            .HasColumnName("ip_address_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(r => r.StartedAt)
            .HasColumnName("started_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(r => r.CompletedAt)
            .HasColumnName("completed_at");

        // Relationships
        builder.HasOne(r => r.User)
            .WithMany(u => u.Responses)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(r => r.ResponseAnswers)
            .WithOne(ra => ra.Response)
            .HasForeignKey(ra => ra.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
