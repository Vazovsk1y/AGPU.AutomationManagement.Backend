using AGPU.AutomationManagement.Application.Extensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AGPU.AutomationManagement.Application.Common;

internal class ValidationUseCaseDecorator<TOut, TIn>(
    IServiceScopeFactory serviceScopeFactory,
    IUseCase<TOut, TIn> decorated) : IUseCase<TOut, TIn>
{
    public virtual async Task<Result<TOut>> HandleAsync(TIn parameter, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TIn>>();
        var validationResult = await validator.ValidateAsync(parameter, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToFailure<TOut>();
        }

        return await decorated.HandleAsync(parameter, cancellationToken);
    }
}

internal class ValidationUseCaseDecorator<TIn>(
    IServiceScopeFactory serviceScopeFactory,
    IUseCase<TIn> decorated) : IUseCase<TIn>
{
    public virtual async Task<Result> HandleAsync(TIn parameter, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TIn>>();
        var validationResult = await validator.ValidateAsync(parameter, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToFailure();
        }

        return await decorated.HandleAsync(parameter, cancellationToken);
    }
}