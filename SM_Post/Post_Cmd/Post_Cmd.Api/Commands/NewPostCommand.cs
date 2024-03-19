using CQRS_Core.Commands;

namespace Post_Cmd.Api.Commands;

public class NewPostCommand : BaseCommand{
	public string Author{ get; set; }
	public string Message{ get; set; }
}