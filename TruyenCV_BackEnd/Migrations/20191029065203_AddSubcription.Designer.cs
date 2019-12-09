﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TruyenCV_BackEnd.DataAccess;

namespace TruyenCV_BackEnd.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20191029065203_AddSubcription")]
    partial class AddSubcription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.AttachmentFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<byte[]>("FileData");

                    b.Property<string>("FileExtension");

                    b.Property<string>("FileName");

                    b.Property<int>("FileSize");

                    b.Property<string>("FileType");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<Guid>("ParentId");

                    b.Property<string>("Remarks");

                    b.Property<bool>("StatusId");

                    b.HasKey("Id");

                    b.ToTable("AttachmentFile");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.AuditTrail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<Guid>("ItemId");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<bool>("StatusId");

                    b.Property<string>("TableName");

                    b.Property<string>("TrackChange");

                    b.Property<string>("TransactionId");

                    b.HasKey("Id");

                    b.ToTable("AuditTrail");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Link");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("StatusId");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Bookmark", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChapterId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<bool>("StatusId");

                    b.Property<Guid>("StoryId");

                    b.HasKey("Id");

                    b.ToTable("Bookmark");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("StatusId");

                    b.Property<Guid?>("StoryId");

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Link");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<int?>("NumberChapter");

                    b.Property<bool>("StatusId");

                    b.Property<Guid>("StoryId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.HasIndex("StoryId");

                    b.ToTable("Chapter");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Reading", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChapterId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<bool>("StatusId");

                    b.Property<Guid>("StoryId");

                    b.HasKey("Id");

                    b.ToTable("Reading");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Story", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AttachmentFileId");

                    b.Property<Guid>("AuthorId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Link");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<string>("ProgressStatus")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Source");

                    b.Property<bool>("StatusId");

                    b.Property<int>("TotalChapter");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Story");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ChapterId");

                    b.Property<Guid>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool>("IsNewest");

                    b.Property<Guid>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<bool>("StatusId");

                    b.Property<Guid>("StoryId");

                    b.HasKey("Id");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Category", b =>
                {
                    b.HasOne("TruyenCV_BackEnd.DataAccess.Models.Story")
                        .WithMany("Categories")
                        .HasForeignKey("StoryId");
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Chapter", b =>
                {
                    b.HasOne("TruyenCV_BackEnd.DataAccess.Models.Story", "Story")
                        .WithMany("Chapters")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TruyenCV_BackEnd.DataAccess.Models.Story", b =>
                {
                    b.HasOne("TruyenCV_BackEnd.DataAccess.Models.Author", "Author")
                        .WithMany("Stories")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
