# Featurize.Repositories

Featurized Repository Pattern

## Usage

```csharp

var builder = WebApplication.CreateBuilder(args);

builder.Features()
    .AddRepositories(x => {
        // register your providers
        x.AddProvider(new InMemoryRepositoryProvider());
        x.AddMongo("mongodb://username:password@localhost:27017");

        // register your entities
        x.AddRepository<OverzichtItem, Guid>(x =>
        {
            x.Provider(MongoRepositoryProvider.DefaultName);
            x.Database("Sample");
            x.CollectionName("overzicht");
        });
        x.AddRepository<Dossier, Guid>(x =>
        {
            x.Provider(MongoRepositoryProvider.DefaultName);
            x.Database("Sample");
            x.CollectionName("dossiers");
        });
        // register your aggregates
        x.AddAggregate<MongoAggregate, Guid>(x =>
        {
            x.Provider(MongoRepositoryProvider.DefaultName);
            x.Database("Test");
            x.CollectionName("Test");
        });
        x.AddAggregate<MemmoryAggregate, Guid>(x =>
        {
            x.Provider(InMemoryRepositoryProvider.DefaultName);
        });
    });

var app = builder.BuildWithFeatures();

app.Run();

```

## Use Repositories

You can access the repositories via the following interfaces

- IRepository<TEntity, TId> => (FindById, Save, Delete)

```csharp
public class CreateDossierHandler : IHandleRequest<CreateDossierRequest>
{
    private readonly IRepository<Dossier, Guid> _repository;

    public CreateDossierHandler(IRepository<Dossier, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IResult> Handle(HttpContext context, CreateDossierRequest request)
    {
        var dossier = Dossier.Create();

        await _repository.SaveAsync(dossier);
        
        return Results.Created("/api/dossier/{id}", dossier.Id);
    }
}
```

- IEntityRepository<TEntity, TId> => (FindById, Save, Delete, Query)

```csharp

public class GetDossierHandler : IHandleRequest<GetDossierRequest>
{
    private readonly IEntityRepository<Dossier, Guid> _repository;

    public GetDossierHandler(IEntityRepository<Dossier, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IResult> Handle(HttpContext context, GetDossierRequest request)
    {
        var ids = await _repository.Query.Select(x => x.Id).ToListAsync();
        var item = await _repository.FindByIdAsync(request.Id);

        if(item != null)
        {
            return Results.Ok(item);
        }

        return Results.NotFound(request.Id);
    }
}

```