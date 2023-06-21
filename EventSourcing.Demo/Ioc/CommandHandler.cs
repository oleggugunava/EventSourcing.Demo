namespace EventSourcing.Demo.Ioc;

public interface ICommandHandler<T>
{
    Task HandleAsync(T command);
}
