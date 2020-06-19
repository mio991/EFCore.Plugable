# EFCore.Plugable

***This Project is work in Progress!***

The goal of this Library is to add Plugin functionality to EntityFrameworkCore.

## Feature List:

- [x] semi-dynamic Model
- [x] migrations
- [ ] `dotnet-ef` Code Generation

And probably more...

## Working with Migrations

`dotnet ef migrations add --project efcore.plugable.test InitialCreation --verbose -o test/simple.plugin/migrations`

Creates the initial Migration, has anybody an Idea how to test this in Code?

## Exsamples

``` C#
class SimplePlugin : IPluginConfig
{
    public ICollection<TypeInfo> CollectMigrations()
    {
        return Assembly.GetExecutingAssembly().DefinedTypes.Where(
            t => t.IsSubclassOf(typeof(Migration))
        ).ToList();
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
}

class Comment
{
    public long Id { get; set; }
    public long ArticleId { get; set; }

    public string Text { get; set; }

    public DateTimeOffset PublishingDate { get; set; }

    public virtual Article Article { get; set; }
}
```

``` C#
public static async Task Main(){
    var db = new PlugableContext(
            new List<IPluginDefinition>{
                new SimplePlugin()
            },
            services => services.AddEntityFrameworkInMemoryDatabase(), 
            new DbContextOptionsBuilder()
                .UseInMemoryDatabase("Test")
                .Options
        );

    var countBefore = await db.Articles().CountAsync();

    db.Articles().Add(new Article
        {
            Id = 1,
            Title = "Test Article 1",
            Text = "Lorem Ipsum...",
            PublishingDate = DateTimeOffset.Now
        });

    await db.SaveChangesAsync();

    Assert.Equal(countBefore + 1, await db.Articles().CountAsync());
}
```