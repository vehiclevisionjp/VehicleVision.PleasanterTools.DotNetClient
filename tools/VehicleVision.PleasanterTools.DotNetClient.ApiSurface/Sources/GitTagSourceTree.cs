namespace VehicleVision.PleasanterTools.DotNetClient.ApiSurface.Sources;

/// <summary>作業リポジトリの、あるタグのツリー。</summary>
internal sealed class GitTagSourceTree : ISourceTree
{
    private readonly GitRepository _repository;
    private readonly Dictionary<string, string> _oids;

    public GitTagSourceTree(GitRepository repository, string tag, IReadOnlyCollection<string> roots)
    {
        _repository = repository;
        Label = tag;
        Revision = repository.ResolveCommit(tag);
        _oids = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var (path, oid) in repository.ListBlobs(tag, roots))
        {
            _oids[path] = oid;
        }
        Files = [.. _oids.Keys.Order(StringComparer.Ordinal)];
    }

    public string Label { get; }

    public string? Revision { get; }

    public IReadOnlyList<string> Files { get; }

    public string ContentKey(string path) => _oids[path];

    public string? Read(string path)
        => _oids.TryGetValue(path, out var oid) ? TextDecoding.Decode(_repository.ReadBlob(oid)) : null;
}
