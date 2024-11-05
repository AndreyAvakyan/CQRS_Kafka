using CQRS_Core.Events;

namespace CQRS_Core.Infrastructure;

public interface IEventStore {
	Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
	Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
}