using CQRS_Core.Events;

namespace Post_Common.Events;

public class CommentUpdatedEvent : BaseEvent {
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent)) { }

    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public string UserName { get; set; }
    public DateTime EditDate { get; set; }
}