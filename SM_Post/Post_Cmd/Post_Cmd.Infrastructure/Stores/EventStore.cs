﻿using CQRS_Core.Domain;
using CQRS_Core.Events;
using CQRS_Core.Exceptions;
using CQRS_Core.Infrastructure;
using CQRS_Core.Producers;
using Post_Cmd.Domain.Aggregates;

namespace Post_Cmd.Infrastructure.Stores;

public class EventStore: IEventStore {
	private readonly IEventProducer _eventProducer;
	private readonly IEventStoreRepository _eventStoreRepository;

	public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer) {
		this._eventStoreRepository = eventStoreRepository;
		this._eventProducer = eventProducer;
	}

	//ADVISE: operations with MongoDb should be transactional (especially if we have a bunch of mongoDb replicas)
	public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion) {
		var eventStream = await this._eventStoreRepository.FindByAggregateIdAsync(aggregateId);
		if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion){
			throw new ConcurrencyException();
		}

		var version = expectedVersion;
		foreach (var @event in events){
			version++;
			@event.Version = version;
			var eventType = @event.GetType().Name;
			var eventModel = new EventModel {
				TimeStamp = DateTime.UtcNow,
				AggregationIdentifier = aggregateId,
				AggregateType = nameof(PostAggregate),
				Version = version,
				EventType = eventType,
				EventData = @event
			};

			await this._eventStoreRepository.SaveAsync(eventModel);

			//now save to kafka
			var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
			await this._eventProducer.ProduceAsync(topic, @event);
		}
	}

	public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId) {
		var eventStream = await this._eventStoreRepository.FindByAggregateIdAsync(aggregateId);
		if (eventStream == null || !eventStream.Any()){
			throw new AggregateNotFoundException("Incorrect post ID provided!");
		}

		return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
	}
}