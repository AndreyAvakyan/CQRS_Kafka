using CQRS_Core.Domain;

namespace CQRS_Core.Handlers;

public interface IEventSourcingHandler<T> {
	Task SaveAsync(AggregateRoot aggregate);
	Task<T> GetByIdAsync(Guid aggregateId);
}