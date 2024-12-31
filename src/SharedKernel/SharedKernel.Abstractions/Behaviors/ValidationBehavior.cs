using FluentValidation;
using MediatR;
using SharedKernel.Abstractions.CQRS;

namespace SharedKernel.Abstractions.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse> {
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
        //Preprocessor

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();

        if (failures.Any()) {
            throw new ValidationException(failures);
        }

        return await next();

        //Postprocessor
    }
}
