using Smart_Dom.DTOs.RoomBooking;
using Smart_Dom.Models;

namespace Smart_Dom.Interfaces
{
    public interface IRoomBookingRepository
    {
        Task<IEnumerable<RoomBookingModel>> GetAllBookingsAsync();
        Task<RoomBookingModel?> GetBookingByIdAsync(int id);
        Task<IEnumerable<RoomBookingModel>> GetBookingsByUserIdAsync(int userId);
        Task<IEnumerable<RoomBookingModel>> GetBookingsByRoomIdAsync(int roomId);
        Task<IEnumerable<RoomBookingViewModel>> GetRoomAllInformation();
        Task AddBookingAsync(RoomBookingModel booking);
        Task UpdateBooking(RoomBookingModel booking);
        Task DeleteBookingAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
