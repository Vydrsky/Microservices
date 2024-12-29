using MediatR;

namespace SharedKernel.Abstractions.CQRS;

public interface ICommand<TResponse> : IRequest<TResponse>;

public interface ICommand : ICommand<Unit>;
