namespace AGPU.AutomationManagement.Application.Common;

public interface IUseCase<TOut, in TIn>
{
    Task<Result<TOut>> HandleAsync(TIn parameter, CancellationToken cancellationToken);
}

public interface IUseCase<in TIn>
{
    Task<Result> HandleAsync(TIn parameter, CancellationToken cancellationToken);
}