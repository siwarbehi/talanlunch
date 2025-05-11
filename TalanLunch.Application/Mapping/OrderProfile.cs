using AutoMapper;
using TalanLunch.Application.Orders.Commands.PlaceOrder;
using TalanLunch.Application.Orders.Queries.GetAllOrders;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mappage de PlaceOrderCommand à Order
            CreateMap<PlaceOrderCommand, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore())  
                .ForMember(dest => dest.OrderDishes, opt => opt.MapFrom(src => new List<OrderDish>()))  
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())  
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow));  
                                                                                               
            CreateMap<Order, OrderDay>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture))
                .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.OrderDishes))
                .ForMember(dest => dest.Served, opt => opt.MapFrom(src => src.Served)) 
                .ForMember(dest => dest.OrderRemark, opt => opt.MapFrom(src => src.OrderRemark)); 

            // Mapping OrderDish -> DishOrderDto
            CreateMap<OrderDish, DishOrderQuantity>()
                .ForMember(dest => dest.DishName, opt => opt.MapFrom(src => src.Dish.DishName))
                .ForMember(dest => dest.DishId, opt => opt.MapFrom(src => src.Dish.DishId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            /*  // Mapping de l'entité Order vers le DTO GetAllOrdersQueryResult
              CreateMap<Order, GetAllOrdersQueryResult>()
                  .ForMember(dest => dest.FirstName,
                      opt => opt.MapFrom(src => src.User.FirstName))
                  .ForMember(dest => dest.LastName,
                      opt => opt.MapFrom(src => src.User.LastName))
                  .ForMember(dest => dest.ProfilePicture,
                      opt => opt.MapFrom(src => src.User.ProfilePicture))
                  .ForMember(dest => dest.OrderId,
                      opt => opt.MapFrom(src => src.OrderId))
                  .ForMember(dest => dest.OrderRemark,
                      opt => opt.MapFrom(src => src.OrderRemark))
                  .ForMember(dest => dest.TotalAmount,
                      opt => opt.MapFrom(src => src.TotalAmount))
                  .ForMember(dest => dest.Paid,
                      opt => opt.MapFrom(src => src.Paid))
                  .ForMember(dest => dest.Served,
                      opt => opt.MapFrom(src => src.Served))
                  .ForMember(dest => dest.OrderDate,
                      opt => opt.MapFrom(src => src.OrderDate))
                  .ForMember(dest => dest.Dishes,
                      opt => opt.MapFrom(src => src.OrderDishes));

              // Mapping de la relation OrderDish vers le DTO DishOrder
              CreateMap<OrderDish, DishOrder>()
                  .ForMember(dest => dest.DishId,
                      opt => opt.MapFrom(src => src.DishId))
                  .ForMember(dest => dest.DishName,
                      opt => opt.MapFrom(src => src.Dish.DishName))
                  .ForMember(dest => dest.Quantity,
                      opt => opt.MapFrom(src => src.Quantity));
          }*/
        }

    }
}
