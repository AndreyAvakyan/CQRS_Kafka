using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class AddCommentCommand: BaseCommand{
	public string Comment{ get; set; }
	public string Username{ get; set; }
}