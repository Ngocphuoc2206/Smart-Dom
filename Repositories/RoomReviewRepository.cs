using Microsoft.EntityFrameworkCore;
using Smart_Dom.DTOs.RoomRevew;
using Smart_Dom.Interfaces;
using Smart_Dom.Models;
using System;
using System.Net.WebSockets;

namespace Smart_Dom.Repositories
{
    public class RoomReviewRepository : IRoomReviewRepository
    {
        private readonly AppDBContext _context;

        public RoomReviewRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomReviewModel>> GetAllAsync()
        {
            return await _context.RoomReviews.ToListAsync();
        }

        public async Task<RoomReviewModel?> GetByIdAsync(int id)
        {
            return await _context.RoomReviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(RoomReviewModel review)
        {
            await _context.RoomReviews.AddAsync(review);
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.RoomReviews.FindAsync(id);
            if (review != null)
            {
                _context.RoomReviews.Remove(review);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RoomReviewViewModel>> GetAllInfoAsync()
        {
            var roomReviewInfo = from roomReview in _context.RoomReviews
                                 join rb in _context.RoomBookings on roomReview.UserID equals rb.UserId
                                 join u in _context.Users on rb.UserId equals u.ID
                                 join r in _context.Rooms on rb.RoomId equals r.ID
                                 select new RoomReviewViewModel()
                                 {
                                     ID = roomReview.Id,
                                     FullName = u.FullName,
                                     RoomNumber = r.RoomNumber,
                                     OverallRating = roomReview.OverallRating,
                                     AmenitiesRating = roomReview.AmenitiesRating,
                                     CleanlinessRating = roomReview.CleanlinessRating,
                                     ExperienceComment = roomReview.ExperienceComment,
                                     LocationRating = roomReview.LocationRating,
                                     HostAttitudeRating = roomReview.HostAttitudeRating,
                                     ValueForMoneyRating = roomReview.ValueForMoneyRating,
                                     IsAnonymous = roomReview.IsAnonymous,
                                     CreateAt = roomReview.CreatedAt,
                                     ResponseFromOwner = roomReview.ResponseFromOwner,
                                     ResponseDate = roomReview.ResponseDate,
                                 };
            return await roomReviewInfo.ToListAsync();
        }

        public async Task UpdateAsync(RoomReviewModel review)
        {
            _context.RoomReviews.Update(review);
            await SaveChangesAsync();
        }
    }

}
