using System.Text.Json;
using Confluent.Kafka;
using CQRS_Core.Events;
using CQRS_Core.Producers;
using Microsoft.Extensions.Options;

namespace Post_Cmd.Infrastructure.Producers;

public class EventProducer: IEventProducer {
	private readonly ProducerConfig _config;

	public EventProducer(IOptions<ProducerConfig> config) {
		this._config = config.Value;
	}

	public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent {
		using var producer = new ProducerBuilder<string, string>(this._config)
			.SetKeySerializer(Serializers.Utf8)
			.SetValueSerializer(Serializers.Utf8).Build();

		var eventMessage = new Message<string, string> {
			Key = Guid.NewGuid().ToString(),
			Value = JsonSerializer.Serialize(@event, @event.GetType())
		};

		var deliveryResults = await producer.ProduceAsync(topic, eventMessage);

		if (deliveryResults.Status == PersistenceStatus.NotPersisted){
			throw new Exception(
				$"Could not produce {@event.GetType().Name} message to topic - {topic} due to the following reason: {deliveryResults.Message}. ");
		}
	}
}