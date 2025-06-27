using Smart_Dom.Models;

namespace Smart_Dom.DTOs.RoomRevew
{
    public static class RoomReviewMapper
    {
        public static RoomReviewModel ToEntity(CreateRoomReviewDto dto, List<string> imageUrls)
        {
            return new RoomReviewModel
            {
                UserID = dto.UserID,
                OverallRating = dto.OverallRating,
                CleanlinessRating = dto.CleanlinessRating,
                AmenitiesRating = dto.AmenitiesRating,
                LocationRating = dto.LocationRating,
                ValueForMoneyRating = dto.ValueForMoneyRating,
                HostAttitudeRating = dto.HostAttitudeRating,
                ExperienceComment = dto.ExperienceComment,
                IsAnonymous = dto.IsAnonymous,
                Images = imageUrls.Select(url => new ReviewImageModel { ImageUrl = url }).ToList()
            };
        }
    }

}
