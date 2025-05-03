using MediatR;

namespace TalanLunch.Application.Queries.Menu
{
    // La Query encapsule l’ID du menu qu’on veut récupérer
    public class GetMenuByIdQuery : IRequest<Domain.Entities.Menu>
    {
        public int MenuId { get; }
        public GetMenuByIdQuery(int menuId) => MenuId = menuId;
    }
}
