namespace Featurize.Repositories.EntityFramework.Tests;

public class TestEntity : IIdentifiable<TestEntity, Guid>
{
    public Guid Id { get; set; }

    public static Guid Identify(TestEntity entity)
    {
        return entity.Id;
    }
}
