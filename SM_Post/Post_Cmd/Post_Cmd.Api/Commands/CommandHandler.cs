using CQRS_Core.Handlers;
using Post_Cmd.Domain.Aggregates;

namespace Post_Cmd.Api.Commands;

public class CommandHandler: ICommandHandler {
	private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

	public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler) {
		this._eventSourcingHandler = eventSourcingHandler;
	}

	public async Task HandleAsync(NewPostCommand command) {
		var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(EditMessageCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.EditMessage(command.Message);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(LikePostCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.LikePost();
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(AddCommentCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.AddComment(command.Comment, command.Username);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(EditCommentCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.EditComment(command.CommentId, command.Comment, command.Username);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(RemoveCommentCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.RemoveComment(command.CommentId, command.Username);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}

	public async Task HandleAsync(DeletePostCommand command) {
		var aggregate = await this._eventSourcingHandler.GetByIdAsync(command.Id);
		aggregate.DeletePost(command.Username);
		await this._eventSourcingHandler.SaveAsync(aggregate);
	}
}