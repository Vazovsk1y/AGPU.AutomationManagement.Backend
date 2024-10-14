using AGPU.AutomationManagement.Application.Extensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AGPU.AutomationManagement.Application.Common;

internal class ValidationUseCaseDecorator<TOut, TIn>(
    IServiceScopeFactory serviceScopeFactory,
    IUseCase<TOut, TIn> decorated) : IUseCase<TOut, TIn>, IDecorator
{
    public virtual async Task<Result<TOut>> ExecuteAsync(TIn parameter, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var validations = scope
            .ServiceProvider
            .GetServices<IValidator<TIn>>()
            .Select(e => e.ValidateAsync(parameter, cancellationToken));

        var results = await Task.WhenAll(validations);
        foreach (var result in results)
        {
            if (!result.IsValid)
            {
                return result.ToFailure<TOut>();
            }
        }

        return await decorated.ExecuteAsync(parameter, cancellationToken);
    }
}

internal class ValidationUseCaseDecorator<TIn>(
    IServiceScopeFactory serviceScopeFactory,
    IUseCase<TIn> decorated) : IUseCase<TIn>, IDecorator
{
    public virtual async Task<Result> ExecuteAsync(TIn parameter, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var validations = scope
            .ServiceProvider
            .GetServices<IValidator<TIn>>()
            .Select(e => e.ValidateAsync(parameter, cancellationToken));

        var results = await Task.WhenAll(validations);
        foreach (var result in results)
        {
            if (!result.IsValid)
            {
                return result.ToFailure();
            }
        }

        return await decorated.ExecuteAsync(parameter, cancellationToken);
    }
}