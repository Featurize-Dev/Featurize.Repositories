namespace Featurize.Repositories.FileRepository.Tests;

public class TestEntity : IIdentifiable<TestEntity, Filename>
{
    public Guid Id { get; set; }
    public static Filename Identify(TestEntity entity)
    {
        return Filename.Create($"{entity.Id}.yaml");
    }
}
