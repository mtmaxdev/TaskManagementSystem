using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Persistence.EntityConfigurations;

public class TaskConfig : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(x => x.Name).HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Status).IsRequired();
        builder.Ignore(x => x.DomainEvents);
    }
}
