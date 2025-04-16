using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalanLunch.Application.Dtos;
using TalanLunch.Domain.Entities;

namespace TalanLunch.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderRequestDto request);

    }
}
