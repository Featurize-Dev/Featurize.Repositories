namespace Featurize.Repositories.FileRepository;

public readonly struct Filename
{
    private readonly string _filename;

    private Filename(string filename)
    {
        _filename = filename;
    }

    public string Value => _filename;

    public static Filename Create(string filename)
    {
        return new Filename(filename);
    }

    public override string ToString()
    {
        return Value;
    }
}
