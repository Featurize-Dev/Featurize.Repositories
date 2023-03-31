using Featurize.Repositories.InMemory;
using Featurize.Repositories.MongoDB;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.Repositories.Aggregate.Tests;
public class AddAggregate_With_InMemory
{
    [Test]
    public async Task Should_register_EventRepsitory()
    {
        var services = new ServiceCollection();
        var features = new FeatureCollection();

        features.AddRepositories(x => {
            x.AddProvider(new InMemoryRepositoryProvider());
            x.AddAggregate<TestAggregate, Guid>(x =>
            {
                x.Provider(InMemoryRepositoryProvider.DefaultName);
            });
        });

        foreach (var f in features.OfType<IServiceCollectionFeature>())
        {
            f.Configure(services);
        }

        var provider = services.BuildServiceProvider();

        var repo = provider.GetRequiredService<IRepository<TestAggregate, Guid>>();

        var aggregateId = Guid.NewGuid();

        var a = TestAggregate.Create(aggregateId);

        a.ChangeName("Test");
        a.ChangeName("Test1");
        a.ChangeName("Test2");

        await repo.SaveAsync(a);

        var b = await repo.FindByIdAsync(aggregateId);

        b.Should().NotBeNull();
        b?.Events.Version.Should().Be(4);
        b?.Name.Should().Be("Test2");
        b?.Id.Should().Be(aggregateId);
    }
}
