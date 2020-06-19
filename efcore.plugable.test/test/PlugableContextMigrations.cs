using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EFCore.Plugable.Test
{
    public class PlugableContextMigrations : IClassFixture<SimplePluginFixture>
    {
        public PlugableContextMigrations(SimplePluginFixture fixture)
        {
            this.Fixture = fixture;
        }

        public SimplePluginFixture Fixture { get; }

        [Fact]
        public async Task CanMigrate()
        {
            string testDb = Path.GetTempFileName();

            var db = new PlugableContext(
                Fixture.Plugins,
                services => services.AddEntityFrameworkSqlite(),
                new DbContextOptionsBuilder().UseSqlite($"Data Source={testDb}").Options
            );

            await db.Database.MigrateAsync();

            db.Articles().Add(Article.Default());

            await db.SaveChangesAsync();

            Assert.Equal(1, await db.Articles().CountAsync());

            File.Delete(testDb);
        }
    }
}