namespace BuildingBlocks.CQRS;

public interface IQuery<TResult> : IRequest<Result<TResult>> where TResult : notnull
{
}