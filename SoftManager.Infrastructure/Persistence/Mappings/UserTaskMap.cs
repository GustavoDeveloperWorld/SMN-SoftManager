using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SoftManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Infrastructure.Persistence.Mappings
{
    public class UserTaskMap : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.ToTable("UserTasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.DueDate)
                .IsRequired();

            builder.HasOne(t => t.ApplicationUser)
                .WithMany()
                .HasForeignKey(t => t.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.IsCompleted)
                .HasDefaultValue(false);
        }
    }
    }
