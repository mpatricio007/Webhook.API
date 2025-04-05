using Webhook.API.Model;

namespace Webhook.API.Repository;

internal sealed class InMemoryFineRepository
{
    private readonly List<Fine> _fines = [];
    public void Add(Fine fine)
    {
        _fines.Add(fine);
    }

    public IReadOnlyList<Fine> GetAll()
    {
        return _fines.AsReadOnly();
    }
}
