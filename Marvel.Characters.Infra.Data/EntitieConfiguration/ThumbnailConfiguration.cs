using Marvel.Characters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Characters.Infra.Data.EntitieConfiguration
{
    public class ThumbnailConfiguration : IEntityTypeConfiguration<Thumbnail>
    {
        public void Configure(EntityTypeBuilder<Thumbnail> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Path)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.Property(e => e.Extension)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            builder.HasOne(x => x.Character)
                    .WithOne(x => x.Thumbnail)
                    .HasForeignKey<Thumbnail>(x => x.CharacterId);
        }
    }
}
