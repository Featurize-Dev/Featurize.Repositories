namespace Featurize.Repositories.YamlFileRepository;

public readonly struct YamlFilename
{
    private readonly string _filenameWithoutExtension;

    private YamlFilename(string filenameWithoutExtension)
    {
        _filenameWithoutExtension = filenameWithoutExtension;
    }

    public string Filename => $"{_filenameWithoutExtension}.yaml";

    public static YamlFilename Create(string filenameWithoutExtension)
    {
        return new YamlFilename(filenameWithoutExtension);
    }

    public override string ToString()
    {
        return Filename;
    }
}
