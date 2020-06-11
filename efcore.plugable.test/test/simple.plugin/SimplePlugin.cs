using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace EFCore.Plugable.Test
{
    class SimplePlugin : IPluginConfig
    {
        public ICollection<TypeInfo> CollectMigrations()
        {
            return new List<TypeInfo>
            {
                typeof(CreateSimplePluginDb).GetTypeInfo()
            };
        }

        public void OnModelCreation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(
                article =>
                {
                    article.HasKey(a => a.Id);

                    article.HasMany(a => a.Comments)
                        .WithOne(c => c.Article)
                        .OnDelete(DeleteBehavior.Cascade);
                }
            );

            modelBuilder.Entity<Comment>(
                comment =>
                {
                    comment.HasKey(c => c.Id);
                }
            );
        }
    }

    static class SimplePluginDbContextExtensions
    {
        public static DbSet<Article> Articles(this PlugableContext context)
        {
            return context.Set<Article>();
        }

        public static DbSet<Comment> Comments(this PlugableContext context)
        {
            return context.Set<Comment>();
        }
    }

    class Article
    {

        public long Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public DateTimeOffset PublishingDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public static Article Default()
        {
            return new Article
            {
                Id = 1,
                Title = "Test Article 1",
                Text = "Lorem Ipsum...",
                PublishingDate = DateTimeOffset.Now
            };
        }
    }

    class Comment
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishingDate { get; set; }

        public virtual Article Article { get; set; }
    }

    public class SimplePluginFixture
    {
        public SimplePluginFixture()
        {
            Plugins = new List<IPluginConfig>{
                new SimplePlugin()
            };

            Options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("Test")
                .Options;
        }

        public List<IPluginConfig> Plugins { get; private set; }
        public DbContextOptions Options { get; private set; }
    }

    [Migration("CreateSimplePluginDb")]
    class CreateSimplePluginDb : Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(article =>
            {
                article.HasKey(a => a.Id);
                article.HasMany(a => a.Comments)
                    .WithOne(c => c.Article);
            });

            modelBuilder.Entity<Comment>().HasKey(c => c.Id);
        }

        protected override void Up([NotNullAttribute] MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Article",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    PublishingDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_ArticleId", a => a.Id)
            );

            migrationBuilder.CreateTable(
                "Comment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    ArticleId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    PublishingDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentId", c => c.Id);
                    table.ForeignKey("FK_Comment_Article", c => c.ArticleId, "Articles", "Id");
                }
            );
        }
    }
}