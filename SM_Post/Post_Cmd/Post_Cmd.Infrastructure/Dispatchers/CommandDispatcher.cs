using CQRS_Core.Commands;
using CQRS_Core.Infrastructure;

namespace Post_Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher: ICommandDispatcher {
	private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

	public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand {
		if (this._handlers.ContainsKey(typeof(T))){
			throw new IndexOutOfRangeException("Handler is already registered!");
		}

		this._handlers.Add(typeof(T), x => handler((T)x));
	}

	public async Task SendAsync(BaseCommand command) {
		if (this._handlers.TryGetValue(command.GetType(), out var handler)){
			await handler(command);
		} else{
			throw new ArgumentNullException(nameof(handler), "No command handler was registered!");
		}
	}
}