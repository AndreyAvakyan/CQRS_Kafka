using CQRS_Core.Domain;
using CQRS_Core.Handlers;
using CQRS_Core.Infrastructure;
using Post_Cmd.Domain.Aggregates;

namespace Post_Cmd.Infrastructure.Handlers;

public class EventSourcingHandler: IEventSourcingHandler<PostAggregate> {
	private readonly IEventStore _eventStore;

	public EventSourcingHandler(IEventStore eventStore) {
		this._eventStore = eventStore;
	}

	public async Task SaveAsync(AggregateRoot aggregate) {
		await this._eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
		aggregate.MarkChangesAsCommitted();
	}

	public async Task<PostAggregate> GetByIdAsync(Guid aggregateId) {
		var aggregate = new PostAggregate();
		var events = await this._eventStore.GetEventsAsync(aggregateId);

		if (events == null || !events.Any()){
			return aggregate;
		}

		aggregate.ReplayEvents(events);

		aggregate.Version = events.Select(e => e.Version).Max();

		return aggregate;
	}
}