using Contracts.Abstractions.Messages;
using Grpc.Core;
using MassTransit;

namespace WebAPI.Abstractions;

public interface IRequest
{
    CancellationToken CancellationToken { get; }
    bool IsValid(out IDictionary<string, string[]> errors);
}

public interface ICommandRequest : IRequest
{
    ICommand Command { get; }
    IBus Bus { get; }
}

public interface IQueryRequest<out TClient> : IRequest
    where TClient : ClientBase
{
    TClient Client { get; }
}