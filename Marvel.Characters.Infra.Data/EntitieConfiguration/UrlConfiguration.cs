using Marvel.Characters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvel.Characters.Infra.Data.EntitieConfiguration
{
    public class UrlConfiguration : IEntityTypeConfiguration<Url>
    {
        public void Configure(EntityTypeBuilder<Url> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.URL)
                    .HasMaxLength(200)
                    .IsUnicode(false);

            builder.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

            builder.HasOne(x => x.Character)
                    .WithMany(x => x.Urls)
                    .HasForeignKey(x => x.CharacterId)
                    .IsRequired();
        }
    }
}
