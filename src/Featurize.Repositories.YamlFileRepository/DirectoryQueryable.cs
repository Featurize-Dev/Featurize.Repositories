using Featurize.Repositories;
using System.Linq.Expressions;
using YamlDotNet.Serialization;

namespace Featurize.Repositories.YamlFileRepository;

public class DirectoryQueryable<TEntity> : IQuery<TEntity>
{
    private readonly IDeserializer _deserializer;
    private readonly IEnumerable<string> _files;

    public DirectoryQueryable(IDeserializer _deserializer, IEnumerable<string> files)
    {
        this._deserializer = _deserializer;
        _files = files;
    }

    public ValueTask<bool> AnyAsync()
    {
        var value = _files.Any();
        return ValueTask.FromResult(value);
    }

    public ValueTask<int> CountAsync()
    {
        var value = _files.Count();
        return ValueTask.FromResult(value);
    }

    public async IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        foreach (var file in _files)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var content = File.ReadAllText(file);
            yield return _deserializer.Deserialize<TEntity>(content);
        }
    }

    public IQuery<TEntity> Skip(int count)
    {
        var files = _files.Skip(count).AsEnumerable();
        return new DirectoryQueryable<TEntity>(_deserializer, files);
    }

    public IQuery<TEntity> Take(int count)
    {
        var files = _files.Take(count).AsEnumerable();
        return new DirectoryQueryable<TEntity>(_deserializer, files);
    }

    public IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        List<string> _matches = new List<string>();
        foreach (var file in _files)
        {
            var content = File.ReadAllText(file);
            var entity = _deserializer.Deserialize<TEntity>(content);
            if (predicate.Compile().Invoke(entity))
            {
                _matches.Add(file);
            }
        }

        return new DirectoryQueryable<TEntity>(_deserializer, _matches.AsEnumerable());
    }
}
