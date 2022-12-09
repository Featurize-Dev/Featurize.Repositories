using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace Featurize.Repositories.FileRepository;

/// <summary>
/// A IQuery for entities in directory
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <seealso cref="Featurize.Repositories.IQuery&lt;TEntity&gt;" />
internal class DirectoryQueryable<TEntity> : IQuery<TEntity>
{
    private readonly IFileSerializer _serializer;
    private readonly IEnumerable<string> _files;

    public DirectoryQueryable(IFileSerializer _serializer, IEnumerable<string> files)
    {
        this._serializer = _serializer;
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
            var content = await File.ReadAllTextAsync(file, cancellationToken);
            yield return _serializer.Deserialize<TEntity>(content);
        }
    }

    public IQuery<TEntity> Skip(int count)
    {
        var files = _files.Skip(count).AsEnumerable();
        return new DirectoryQueryable<TEntity>(_serializer, files);
    }

    public IQuery<TEntity> Take(int count)
    {
        var files = _files.Take(count).AsEnumerable();
        return new DirectoryQueryable<TEntity>(_serializer, files);
    }

    public IQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        List<string> _matches = new();
        foreach (var file in _files)
        {
            var content = File.ReadAllText(file);
            var entity = _serializer.Deserialize<TEntity>(content);
            if (predicate.Compile().Invoke(entity))
            {
                _matches.Add(file);
            }
        }

        return new DirectoryQueryable<TEntity>(_serializer, _matches.AsEnumerable());
    }
}
