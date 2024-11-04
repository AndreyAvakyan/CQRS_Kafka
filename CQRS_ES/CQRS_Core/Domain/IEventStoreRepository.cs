using CQRS_Core.Events;

namespace CQRS_Core.Domain;

public interface IEventStoreRepository {
	Task SaveAsync(EventModel @event);
	Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId);
}