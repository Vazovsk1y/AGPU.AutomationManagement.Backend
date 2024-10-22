using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem.UseCases;

internal sealed class ProblemAddUseCase(
    IWriteDbContext writeDbContext,
    TimeProvider timeProvider,
    ICurrentUserProvider currentUserProvider) : IUseCase<Guid, ProblemAddCommand>
{
    public async Task<Result<Guid>> ExecuteAsync(ProblemAddCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);

        var request = new Domain.Entities.Problem
        {
            Title = parameter.Title.Trim(),
            CreationDateTime = timeProvider.GetUtcNow(),
            CreatorId = currentUser.Id,
            Description = parameter.Description.Trim(),
            Audience = parameter.Audience.Trim(),
            Status = ProblemStatus.Pending,
            Type = parameter.Type,
        };

        writeDbContext.Problems.Add(request);
        await writeDbContext.SaveChangesAsync(cancellationToken);

        return request.Id;
    }
}