namespace Delivery.Client.Models
{
    public sealed record LoginRequest(int UserId);
    public sealed record LoginResponse(int UserId, string Role);

    public sealed record RestaurantDto(int RestaurantId, string Name, string? Address, string? Phone);
    public sealed record MenuItemDto(int ItemId, int RestaurantId, string Name, decimal Price, string? Description, bool IsAvailable);
    public sealed record RestaurantUpdateRequest(string Name);
    public sealed record MenuItemUpsertRequest(string Name, decimal Price);

    public sealed record CartItemRequest(int ItemId, int Quantity);
    public sealed record CartItemDto(int ItemId, string Name, int Quantity, decimal Price);
    public sealed record CartSummaryDto(int CartId, int UserId, int RestaurantId, decimal Total, List<CartItemDto> Items);

    public sealed record CheckoutRequest(int UserId, int RestaurantId);
    public sealed record OrderDto(int OrderId, int UserId, int RestaurantId, int? RiderId, decimal TotalPrice, string Status);
    public sealed record OrderItemDto(int ItemId, string Name, int Quantity, decimal Price);
    public sealed record OrderDetailDto(int OrderId, int UserId, string CustomerName, int RestaurantId, decimal TotalPrice, string Status);
    public sealed record RiderCurrentOrderDto(int OrderId, int UserId, string CustomerName, int RestaurantId, string Status);

    public sealed record UpdateStatusRequest(string Status);
}
