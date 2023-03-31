//using Featurize.Repositories.Aggregates;

//using Microsoft.Extensions.DependencyInjection;
//using Featurize.Repositories.InMemory;
//using FluentAssertions;

//namespace Featurize.Repositories.Aggregate.Tests;
//internal class Projector_Registration
//{
//    [Test]
//    public async Task should_publish_events()
//    {
//        var services = new ServiceCollection();
//        var features = new FeatureCollection();

//        features.AddRepositories(x => {
//            x.AddProvider(new InMemoryRepositoryProvider());
//            x.AddAggregate<TestAggregate, Guid>(x =>
//            {
//                x.Provider(InMemoryRepositoryProvider.DefaultName);
//            });
//        });

//        foreach (var item in features.OfType<IServiceCollectionFeature>())
//        {
//            item.Configure(services);
//        }

//        var provider = services.BuildServiceProvider();

//        var repository = provider.GetRequiredService<IRepository<TestAggregate, Guid>>();
        
//        var aggregate = await repository.FindByIdAsync(Guid.NewGuid());

//        aggregate!.ChangeName("test");

//        await repository.SaveAsync(aggregate);

//        Projector.Called.Should().BeTrue();
//    }
    
//}

//public class Projector : IEventHandler<ChangeNameEvent>
//{
//    public static bool Called = false;
//    public ValueTask Handle(ChangeNameEvent e)
//    {
//        Called = true;
//        return ValueTask.CompletedTask;
//    }
//}



