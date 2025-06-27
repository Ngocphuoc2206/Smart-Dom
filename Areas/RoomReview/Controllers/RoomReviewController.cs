using Microsoft.AspNetCore.Mvc;
using Smart_Dom.DTOs.RoomRevew;
using Smart_Dom.Services;

namespace Smart_Dom.Areas.RoomReview.Controllers
{
    [Area("RoomReview")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomReviewController : Controller
    {
        private readonly IRoomReviewService _service;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<RoomReviewController> _logger;

        public RoomReviewController(IRoomReviewService service, IWebHostEnvironment env, ILogger<RoomReviewController> logger)
        {
            _service = service;
            _env = env;
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateReview([FromForm] CreateRoomReviewDto dto, int id)
        {
            _logger.LogInformation("Create review...");
            //Tạo thư mục Uploads trước khi tải về
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var imageUrls = new List<string>();

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var image in dto.Images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                        var path = Path.Combine(_env.WebRootPath, "uploads", fileName);

                        using var stream = new FileStream(path, FileMode.Create);
                        await image.CopyToAsync(stream);

                        imageUrls.Add($"/uploads/{fileName}");
                    }
                }
            }
            dto.UserID = id;
            var review = RoomReviewMapper.ToEntity(dto, imageUrls);
            await _service.AddReviewAsync(review);

            return Ok(new { message = "Đánh giá đã được gửi thành công!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResponseOwner([FromBody] CreateResposeRoomReview dto, int id)
        {
            _logger.LogInformation("Create review...");
            _logger.LogInformation($"response from owners: {dto.Response}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existReview = await _service.GetReviewByIdAsync(id);
            if (existReview == null) 
            {
                _logger.LogError("Cannot find by ID");
                return BadRequest("Cannot find by ID");
            }
            existReview.ResponseDate = DateTime.Now;
            existReview.ResponseFromOwner = dto.Response;
            await _service.UpdateReviewAsynx(existReview);
            return Ok(new { message = "Đánh giá đã được gửi thành công!" });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _service.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInfo()
        {
            var reviews = await _service.GetAllReviewsViewModelAsync();
            return Ok(reviews);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteReviewAsync(id);
            return NoContent();
        }
    }

}
