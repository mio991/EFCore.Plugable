using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EFCore.Plugable.Test
{
    public class PlugableContextModelCreation : IClassFixture<SimplePluginFixture>
    {
        SimplePluginFixture SimplePluginFixture { get; }

        public PlugableContextModelCreation(SimplePluginFixture fixture)
        {
            SimplePluginFixture = fixture;
        }

        [Fact]
        public void ModelContainsArticlsAndComments()
        {
            var db = new PlugableContext(SimplePluginFixture.Plugins,
                services => services.AddEntityFrameworkInMemoryDatabase(), SimplePluginFixture.Options);

            Assert.Collection(db.Model.GetEntityTypes(),
                article => Assert.Contains(nameof(Article), article.Name),
                comment => Assert.Contains(nameof(Comment), comment.Name)
                );
        }

        [Fact]
        public async Task CanAddArticle()
        {
            var db = new PlugableContext(SimplePluginFixture.Plugins,
                services => services.AddEntityFrameworkInMemoryDatabase(), SimplePluginFixture.Options);

            var countBefore = await db.Articles().CountAsync();

            db.Articles().Add(Article.Default());

            await db.SaveChangesAsync();

            Assert.Equal(countBefore + 1, await db.Articles().CountAsync());
        }
    }
}
