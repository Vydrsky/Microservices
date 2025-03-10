﻿using MediatR;

namespace SharedKernel.Abstractions.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : ICommand<Unit>;
