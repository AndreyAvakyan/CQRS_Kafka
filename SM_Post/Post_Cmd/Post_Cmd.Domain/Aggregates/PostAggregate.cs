using CQRS_Core.Domain;
using Post_Common.Events;

namespace Post_Cmd.Domain.Aggregates;

public class PostAggregate: AggregateRoot {
	private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();
	private bool _active;
	private string _author;

	public PostAggregate() { }

	public PostAggregate(Guid id, string author, string message) {
		this.RaiseEvent(new PostCreatedEvent
			{ Id = id, Author = author, Message = message, DatePosted = DateTime.UtcNow });
	}

	// ReSharper disable once ConvertToAutoPropertyWhenPossible
	public bool Active {
		get => this._active;
		set => this._active = value;
	}


	public void Apply(PostCreatedEvent @event) {
		this._id = @event.Id;
		this._active = true;
		this._author = @event.Author;
	}

	public void EditMessage(string message) {
		if (!this._active){
			throw new InvalidOperationException("You can not edit the message of an inactive post!");
		}

		if (string.IsNullOrWhiteSpace(message)){
			throw new InvalidOperationException(
				$"The value of {nameof(message)} can not be null or empty. PLease provide a valid {nameof(message)}");
		}

		this.RaiseEvent(new MessageUpdatedEvent {
			Id = this._id,
			Message = message
		});
	}


	public void Apply(MessageUpdatedEvent @event) {
		this._id = @event.Id;
	}

	public void LikePost() {
		if (!this._active){
			throw new InvalidOperationException("You can not like an inactive post!");
		}

		this.RaiseEvent(new PostLikedEvent {
			Id = this._id
		});
	}

	public void Apply(PostLikedEvent @event) {
		this._id = @event.Id;
	}

	public void AddComment(string comment, string username) {
		if (!this._active){
			throw new InvalidOperationException("You can not add a comment to an inactive post!");
		}

		if (string.IsNullOrWhiteSpace(comment)){
			throw new InvalidOperationException(
				$"The value of {nameof(comment)} can not be null or empty. PLease provide a valid {nameof(comment)}");
		}

		this.RaiseEvent(new CommentAddedEvent {
			Id = this._id,
			CommentId = Guid.NewGuid(),
			Comment = comment,
			UserName = username,
			CommentDate = DateTime.UtcNow
		});
	}

	public void Apply(CommentAddedEvent @event) {
		this._id = @event.Id;
		this._comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
	}

	public void EditComment(Guid commentId, string comment, string username) {
		if (!this._active){
			throw new InvalidOperationException("You can not edit a comment of an inactive post!");
		}

		if (!this._comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase)){
			throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user!");
		}

		this.RaiseEvent(new CommentUpdatedEvent {
			Id = this._id,
			CommentId = commentId,
			Comment = comment,
			UserName = username,
			EditDate = DateTime.UtcNow
		});
	}

	public void Apply(CommentUpdatedEvent @event) {
		this._id = @event.Id;
		this._comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
	}

	public void RemoveComment(Guid commentId, string username) {
		if (!this._active){
			throw new InvalidOperationException("You can not remove a comment of an inactive post!");
		}

		if (!this._comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase)){
			throw new InvalidOperationException(
				"You are not allowed to remove a comment that was made by another user!");
		}

		this.RaiseEvent(new CommentRemovedEvent {
			Id = this._id,
			CommentId = commentId
		});
	}

	public void Apply(CommentRemovedEvent @event) {
		this._id = @event.Id;
		this._comments.Remove(@event.CommentId);
	}

	public void DeletePost(string username) {
		if (!this._active){
			throw new InvalidOperationException("The post has already been removed!");
		}

		if (!this._author.Equals(username, StringComparison.CurrentCultureIgnoreCase)){
			throw new InvalidOperationException(
				"You are not allowed to delete the post that was made by someone else!");
		}

		this.RaiseEvent(new PostRemovedEvent {
			Id = this._id
		});
	}

	public void Apply(PostRemovedEvent @event) {
		this._id = @event.Id;
		this._active = false;
	}
}