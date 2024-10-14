namespace AGPU.AutomationManagement.Application.Common;

public interface IUseCase<TOut, in TIn>
{
    Task<Result<TOut>> ExecuteAsync(TIn parameter, CancellationToken cancellationToken);
}

public interface IUseCase<in TIn>
{
    Task<Result> ExecuteAsync(TIn parameter, CancellationToken cancellationToken);
}