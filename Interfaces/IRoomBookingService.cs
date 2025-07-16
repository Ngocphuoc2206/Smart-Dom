using Smart_Dom.DTOs.RoomBooking;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomBookingService
    {
        Task<IEnumerable<RoomBookingModel>> GetAllBookingsAsync();
        Task<RoomBookingModel?> GetBookingByIdAsync(int id);
        Task<RoomBookingModel> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<RoomBookingModel>> GetBookingsByRoomIdAsync(int roomId);
        Task<IEnumerable<RoomBookingViewModel>> GetRoomAllInformation();
        Task AddBookingAsync(CreateRoomBookingDTO booking);
        Task AddBookingTenantAsync(RoomBookingViewModel booking);
        Task UpdateBookingAsync(RoomBookingModel booking);
        Task DeleteBookingAsync(int id);
    }
}
