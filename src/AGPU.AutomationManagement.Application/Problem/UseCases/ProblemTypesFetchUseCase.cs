using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemTypesFetchUseCase : IUseCase<IReadOnlyCollection<ProblemType>, ProblemTypesFetchQuery>
{
    public Task<Result<IReadOnlyCollection<ProblemType>>> ExecuteAsync(ProblemTypesFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(Result.Success<IReadOnlyCollection<ProblemType>>(Enum
            .GetValues<ProblemType>()
            .ToList()));
    }
}