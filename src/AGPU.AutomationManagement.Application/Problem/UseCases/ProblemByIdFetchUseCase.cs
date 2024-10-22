using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem.Queries;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemByIdFetchUseCase : IUseCase<ProblemDTO, ProblemByIdFetchQuery>
{
    public Task<Result<ProblemDTO>> ExecuteAsync(ProblemByIdFetchQuery parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Отдавать только по логике.
        
        throw new NotImplementedException();
    }
}