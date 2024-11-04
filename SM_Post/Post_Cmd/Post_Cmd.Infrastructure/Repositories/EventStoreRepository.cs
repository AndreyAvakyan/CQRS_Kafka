using CQRS_Core.Domain;
using CQRS_Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post_Cmd.Infrastructure.Config;

namespace Post_Cmd.Infrastructure.Repositories;

public class EventStoreRepository: IEventStoreRepository {
	private readonly IMongoCollection<EventModel> _eventStoreCollection;

	public EventStoreRepository(IOptions<MongoDbConfig> config) {
		var mongoClient = new MongoClient(config.Value.ConnectionString);
		var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);
		this._eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
	}

	public async Task SaveAsync(EventModel @event) {
		await this._eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
	}

	public async Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId) {
		return await this._eventStoreCollection.Find(x => x.AggregationIdentifier == aggregateId).ToListAsync()
			.ConfigureAwait(false);
	}
}