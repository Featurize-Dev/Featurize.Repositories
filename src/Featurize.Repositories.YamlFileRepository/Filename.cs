namespace Featurize.Repositories.FileRepository;

public readonly struct Filename
{
    private readonly string _filename;
    private readonly string _extension;

    private Filename(string filename, string extension)
    {
        _filename = filename;
    }

    public string Value => $"{_filename}.{_extension}";

    public static Filename Create(string filenameWithoutExtension, string extension)
    {
        return new Filename(filenameWithoutExtension, extension);
    }

    public override string ToString()
    {
        return Value;
    }
}
