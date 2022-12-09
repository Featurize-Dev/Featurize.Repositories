namespace Featurize.Repositories.Tests;

public class TestEntity2 : IIdentifiable<TestEntity2, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(TestEntity2 entity)
    {
        return entity.Id;
    }
}