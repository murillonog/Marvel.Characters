using Marvel.Characters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marvel.Characters.Infra.Data.EntitieConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.Property(e => e.ResourceURI)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.HasOne(x => x.Character)
                    .WithMany(x => x.Events)
                    .HasForeignKey(x => x.CharacterId)
                    .IsRequired();
        }
    }
}
