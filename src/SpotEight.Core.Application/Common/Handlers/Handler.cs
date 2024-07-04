using MediatR;
using System.Diagnostics.CodeAnalysis;
using SpotEight.Core.Domain.Dtos.Response;

namespace SpotEight.Core.Application.Common.Handlers;

[ExcludeFromCodeCoverage]
public abstract class Handler<TRequest, TResponse, T>
    where TRequest : IRequest<TResponse>
    where TResponse : ResponseBase<T>
    where T : class
{
    protected Handler<TRequest, TResponse, T>? _successor;

    public void SetSuccessor(Handler<TRequest, TResponse, T> successor)
    {
        _successor = successor;
    }

    public abstract Task Process(TRequest request, TResponse response);
}