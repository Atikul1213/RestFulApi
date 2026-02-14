using EmployeeAdminPortal.Models.EcommerceModel.DTO;

namespace EmployeeAdminPortal.Services.TestServices
{
    public interface IOrderService
    {
        Task<OrderResponseDTO?> CreateOrderAsync(OrderCreateDTO orderCreateDto);
        Task<OrderResponseDTO?> GetOrderByIdAsync(int id);
    }
}
