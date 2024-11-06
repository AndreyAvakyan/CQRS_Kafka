using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class RemoveCommentCommand: BaseCommand {
	public Guid CommentId { get; set; }
	public string Username { get; set; }
}