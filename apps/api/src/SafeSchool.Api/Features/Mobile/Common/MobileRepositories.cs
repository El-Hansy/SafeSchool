namespace SafeSchool.Api.Features.Mobile;

public interface IMobileTenantOwnedRecord
{
    string TenantId { get; }
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset UpdatedAt { get; }
}

public sealed class MobileInMemoryRepository<T> where T : class
{
    private readonly List<T> _items = [];
    public IReadOnlyList<T> Items => _items;
    public void Add(T item) => _items.Add(item);
}
