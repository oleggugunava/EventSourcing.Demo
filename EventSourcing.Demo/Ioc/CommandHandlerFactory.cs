namespace EventSourcing.Demo.Ioc;

public interface ICommandHandlerFactory
{
    ICommandHandler<T> Create<T>(T command);
}

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IServiceProvider _provider;

    public CommandHandlerFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ICommandHandler<T> Create<T>(T command)
    {
        return _provider.GetService<ICommandHandler<T>>();
    }
}
