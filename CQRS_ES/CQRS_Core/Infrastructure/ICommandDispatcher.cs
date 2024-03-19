using CQRS_Core.Commands;

namespace CQRS_Core.Infrastructure;

public interface ICommandDispatcher {
	void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
	Task SendAsync(BaseCommand command);
}