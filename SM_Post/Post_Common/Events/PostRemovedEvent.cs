using CQRS_Core.Events;

namespace Post_Common.Events;

public class PostRemovedEvent: BaseEvent {
	public PostRemovedEvent(): base(nameof(PostRemovedEvent)) { }
}