using MediatR;

namespace InventoryAndSalesManagement.Features.Accounts.Commands
{
    public record LoginCommand(LoginViewModel userViewModel) : IRequest<bool>;
}
