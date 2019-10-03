using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TruyenCV_BackEnd.DataAccess.Models;

namespace TruyenCV_BackEnd.DataAccess
{
    public partial class MainContext
    {
        public class CategoryConfiguration : IEntityTypeConfiguration<Category>
        {
            public void Configure(EntityTypeBuilder<Category> builder)
            {
                builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
            }
        }

        public class AuthorConfiguration : IEntityTypeConfiguration<Author>
        {
            public void Configure(EntityTypeBuilder<Author> builder)
            {
                builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
            }           
        }

        public class StoryConfiguration : IEntityTypeConfiguration<Story>
        {
            public void Configure(EntityTypeBuilder<Story> builder)
            {
                builder.Property(p => p.Name).HasMaxLength(500).IsRequired();
                builder.Property(p => p.ProgressStatus).HasMaxLength(100).IsRequired();
            }         
        }

        public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
        {
            public void Configure(EntityTypeBuilder<Chapter> builder)
            {
                builder.Property(p => p.Title).HasMaxLength(500).IsRequired();
            }
        }
    }
}
