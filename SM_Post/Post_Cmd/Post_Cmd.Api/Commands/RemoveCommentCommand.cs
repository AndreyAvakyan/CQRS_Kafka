using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class RemoveCommentCommand: BaseCommand{
	public Guid CommentID{ get; set; }
	public string Username { get; set; }
}