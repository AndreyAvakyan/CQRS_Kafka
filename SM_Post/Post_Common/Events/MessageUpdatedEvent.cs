using CQRS_Core.Events;

namespace Post_Common.Events;

public class MessageUpdatedEvent: BaseEvent {
	public MessageUpdatedEvent(): base(nameof(MessageUpdatedEvent)) { }

	public string Message { get; set; }
}