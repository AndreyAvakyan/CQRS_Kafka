using CQRS_Core.Events;

namespace Post_Common.Events;

public class PostLikedEvent: BaseEvent {
	public PostLikedEvent(): base(nameof(PostLikedEvent)) { }
}