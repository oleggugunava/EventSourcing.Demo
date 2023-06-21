using EventSourcing.Demo.Models;
using Trainline.EventSourcing.EventPublishing;
using Trainline.EventSourcing.Mapping;
using Trainline.EventSourcing.Model;
using Trainline.EventSourcing.Repository;

namespace EventSourcing.Demo.Repositories;

public interface ILedgerAggregateRepository : IAggregateRepository<Ledger>
{
}

public class LedgerAggregateRepository : ILedgerAggregateRepository
{
    private readonly AggregateRepository<Ledger> _aggregateRepository;

    public LedgerAggregateRepository(
        IEventRepository eventRepository,
        IEventMapper eventMapper,
        IEventPublisher eventPublisher)
    {
        _aggregateRepository = new AggregateRepository<Ledger>(
            eventRepository ?? throw new ArgumentNullException(nameof(eventRepository)),
            eventMapper ?? throw new ArgumentNullException(nameof(eventMapper)),
            eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher)));

    }

    public async Task<Ledger> GetAggregateAsync(string aggregateId, FetchOptions fetchOptions = null)
    {
        return await _aggregateRepository.GetAggregateAsync(aggregateId);
    }

    public async Task<IOrderedEnumerable<SerialisedEvent>> GetOrderedAggregateEventsAsync(string aggregateId)
    {
        return await _aggregateRepository.GetOrderedAggregateEventsAsync(aggregateId);
    }

    public async Task SaveAggregateAsync(Ledger aggregate, string conversationId)
    {
        await _aggregateRepository.SaveAggregateAsync(aggregate, conversationId);
    }
}
