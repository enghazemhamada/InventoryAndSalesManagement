using MediatR;
using Microsoft.AspNetCore.Identity;

namespace InventoryAndSalesManagement.Features.Accounts.Commands
{
    public record RegisterCommand(RegisterViewModel userViewModel) : IRequest<IdentityResult>;
}
