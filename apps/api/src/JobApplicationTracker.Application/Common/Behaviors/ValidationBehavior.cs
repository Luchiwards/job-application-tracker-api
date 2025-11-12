using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace JobApplicationTracker.Application.Common.Behaviors;

/// <summary>
/// Executes all registered <see cref="IValidator{T}"/> instances for a request before it reaches the handler.
/// </summary>
/// <typeparam name="TRequest">Type of the request being handled.</typeparam>
/// <typeparam name="TResponse">Type of the response returned by the handler.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Validators to be executed for the given request.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Validates the incoming request and either throws a <see cref="ValidationException"/> or forwards the request to the next handler.
    /// </summary>
    /// <param name="request">The request being processed.</param>
    /// <param name="next">Delegate that invokes the next element in the pipeline.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The response produced by the next handler in the pipeline.</returns>
    /// <exception cref="ValidationException">Thrown when any validator detects failures.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next().ConfigureAwait(false);
        }

        var validationTasks = _validators
            .Select(validator => validator.ValidateAsync(request, cancellationToken))
            .ToArray();

        var results = await Task.WhenAll(validationTasks).ConfigureAwait(false);
        var failures = results
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count != 0)
            {
            throw new ValidationException(failures);
        }

        return await next().ConfigureAwait(false);
    }
}

