namespace BuildingBlocks.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResult> : IRequest<Result<TResult>>
{
}