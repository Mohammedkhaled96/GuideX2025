using Microsoft.AspNetCore.Mvc;
using QRCodeManagement.API.DTOs;
using QRCodeManagement.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static QRCodeManagement.API.Services.QRCodeService;

namespace QRCodeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly IQRCodeService _qrCodeService;

        public QRCodeController(IQRCodeService qrCodeService)
        {
            _qrCodeService = qrCodeService;
        }

        // Endpoint: Get Paginated QR Codes
        [HttpGet("user/{userId}/paginated")]
        public async Task<ActionResult<PaginatedResult<QRCodeDetailsDTO>>> GetQRCodes(
            int userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Invalid pagination parameters");

            var result = await _qrCodeService.GetQRCodesPagedAsync(userId, pageNumber, pageSize);
            return Ok(result);
        }

        // Endpoint: Get All QR Codes for a User
        [HttpGet("user/{userId}/all")]
        public async Task<ActionResult<List<QRCodeDetailsDTO>>> GetAllQRCodes(int userId)
        {
            var result = await _qrCodeService.GetAllQRCodesByUserIdAsync(userId);
            return Ok(result);
        }
    }
}
