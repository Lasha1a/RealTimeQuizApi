using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Configurations;

public class ResponseAnswerConfiguration : IEntityTypeConfiguration<ResponseAnswer>
{
    public void Configure(EntityTypeBuilder<ResponseAnswer> builder)
    {
        builder.ToTable("response_answers");

        builder.HasKey(ra => ra.Id);

        builder.Property(ra => ra.Id)
            .HasColumnName("id");

        builder.Property(ra => ra.ResponseId)
            .HasColumnName("response_id")
            .IsRequired();

        builder.Property(ra => ra.QuestionId)
            .HasColumnName("question_id")
            .IsRequired();

        builder.Property(ra => ra.SelectedOptionIds)
            .HasColumnName("selected_option_ids")
            .HasColumnType("uuid[]"); // PostgreSQL array

        builder.Property(ra => ra.TextAnswer)
            .HasColumnName("text_answer")
            .HasMaxLength(2000);

        builder.Property(ra => ra.RatingValue)
            .HasColumnName("rating_value");

        builder.Property(ra => ra.AnsweredAt)
            .HasColumnName("answered_at")
            .HasDefaultValueSql("NOW()");
    }
}
