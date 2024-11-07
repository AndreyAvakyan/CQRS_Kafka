using CQRS_Core.Messages;

namespace CQRS_Core.Events;

public abstract class BaseEvent: Message {
	protected BaseEvent(string type) {
		this.Type = type;
	}

	public int Version { get; set; }
	public string Type { get; set; }
}