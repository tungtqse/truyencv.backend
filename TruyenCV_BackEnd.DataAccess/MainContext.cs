using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using TruyenCV_BackEnd.DataAccess.Models;

namespace TruyenCV_BackEnd.DataAccess
{
    public class ApplicationDbContextOptions
    {
        public readonly DbContextOptions<MainContext> Options;

        public ApplicationDbContextOptions(DbContextOptions<MainContext> options)
        {
            Options = options;
        }
    }

    public partial class MainContext : CoreContext
    {
        public MainContext(ApplicationDbContextOptions options) : base(options.Options) { }

        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.Relational().TableName = entityType.DisplayName();
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new StoryConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
        }
    }
}
