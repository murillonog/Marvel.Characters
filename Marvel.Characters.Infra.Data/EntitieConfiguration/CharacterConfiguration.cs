using Marvel.Characters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marvel.Characters.Infra.Data.EntitieConfiguration
{
    public class CharacterConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            //Attributes
                builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

            builder.Property(e => e.Modified)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            builder.Property(e => e.ResourceURI)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.Property(e => e.Favorite)
                    .HasColumnType("bit")
                    .HasDefaultValue(false);
        }
    }
}
