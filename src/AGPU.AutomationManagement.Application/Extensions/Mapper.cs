using AGPU.AutomationManagement.Application.User;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Mapper
{
    public static ContractorDTO ToContractorDTO(this Domain.Entities.User user)
    {
        return new ContractorDTO(
            user.Id,
            user.FullName,
            user.Post);
    }
}