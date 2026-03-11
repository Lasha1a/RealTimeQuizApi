using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Persistence.Configurations;

public class AnswerOptionConfiguration : IEntityTypeConfiguration<AnswerOption>
{
    public void Configure(EntityTypeBuilder<AnswerOption> builder)
    {
        builder.ToTable("answer_options");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.Property(a => a.QuestionId)
            .HasColumnName("question_id")
            .IsRequired();

        builder.Property(a => a.OptionText)
            .HasColumnName("option_text")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.OrderIndex)
            .HasColumnName("order_index")
            .IsRequired();

        builder.Property(a => a.IsCorrect)
            .HasColumnName("is_correct")
            .HasDefaultValue(false);
    }
}
