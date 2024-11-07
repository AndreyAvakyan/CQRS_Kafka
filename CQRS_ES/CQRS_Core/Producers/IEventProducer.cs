using CQRS_Core.Events;

namespace CQRS_Core.Producers;

public interface IEventProducer {
	Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent;
}