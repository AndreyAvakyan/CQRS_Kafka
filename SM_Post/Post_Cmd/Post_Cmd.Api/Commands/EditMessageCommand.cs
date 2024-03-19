using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class EditMessageCommand : BaseCommand{
	public string Message{ get; set; }
}