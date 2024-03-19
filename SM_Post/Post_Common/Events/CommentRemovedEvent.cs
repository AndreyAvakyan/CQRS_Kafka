using CQRS_Core.Events;

namespace Post_Common.Events;

public class CommentRemovedEvent : BaseEvent {
    public CommentRemovedEvent() : base(nameof(CommentRemovedEvent)) { }

    public Guid CommentId { get; set; }
}