using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class DeletePostCommand: BaseCommand{
	public string Username { get; set; }
}