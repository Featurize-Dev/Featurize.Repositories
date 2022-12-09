namespace Featurize.Repositories.FileRepository;

/// <summary>
/// A Identifier for FileRepository.
/// </summary>
public readonly struct Filename
{
    private readonly string _filename;

    private Filename(string filename)
    {
        _filename = filename;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string Value => _filename;

    /// <summary>
    /// Creates the specified filename.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <returns></returns>
    public static Filename Create(string filename)
    {
        return new Filename(filename);
    }
    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return Value;
    }
}
