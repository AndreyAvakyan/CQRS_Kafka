using CQRS_Core.Events;

namespace CQRS_Core.Domain;

public abstract class AggregateRoot {
	private readonly List<BaseEvent> _changes = new();
	protected Guid _id;

	public Guid Id => this._id;

	public int Version { get; set; } = -1;

	public IEnumerable<BaseEvent> GetUncommittedChanges() {
		return this._changes;
	}

	public void MarkChangesAsCommitted() {
		this._changes.Clear();
	}

	private void ApplyChange(BaseEvent @event, bool isNew) {
		var method = this.GetType().GetMethod("Apply", new[] { @event.GetType() });

		if (method == null){
			throw new ArgumentNullException(nameof(method),
				$"The apply method was not found in the aggregate for {@event.GetType().Name}");
		}

		method.Invoke(this, new object[] { @event });

		if (isNew){
			this._changes.Add(@event);
		}
	}

	protected void RaiseEvent(BaseEvent @event) {
		this.ApplyChange(@event, true);
	}

	public void ReplayEvents(IEnumerable<BaseEvent> events) {
		foreach (var _event in events){
			this.ApplyChange(_event, false);
		}
	}

	//comment
}