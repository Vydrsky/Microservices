using MediatR;

namespace SharedKernel.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
    where TResponse : notnull;
