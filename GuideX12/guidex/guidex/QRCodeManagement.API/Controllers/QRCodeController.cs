using Microsoft.AspNetCore.Mvc;
using QRCodeManagement.API.DTOs;
using QRCodeManagement.API.Models;
using QRCodeManagement.API.Services.Interfaces;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<QRCodeDetailsDTO>>> GetAllQRCodes([FromQuery] PaginationParameters paginationParams)
        {
            var result = await _qrCodeService.GetQRCodesAsync(paginationParams);
            return Ok(result);
        }

        [HttpGet("{qrCodeId}/{userId}")]
        public async Task<ActionResult<QRCodeDetailsDTO>> GetQRCodeDetails(int qrCodeId, int userId)
        {
            var result = await _qrCodeService.GetQRCodeDetailsAsync(qrCodeId, userId);
            
            if (result == null)
                return NotFound("QR Code not found");
            if (result.Item == null)
                return Ok(new { Message = "QR Code exists but no information is available" });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateQRCode([FromBody] QRCodeCreateDTO qrCodeCreateDTO)
        {
            var qrCodeId = await _qrCodeService.CreateQRCodeAsync(qrCodeCreateDTO.QRCode, qrCodeCreateDTO.UserId);
            
            if (qrCodeId == 0)
                return BadRequest("Failed to create QR Code");
                
            return CreatedAtAction(nameof(GetQRCodeDetails), new { qrCodeId = qrCodeId, userId = qrCodeCreateDTO.UserId }, qrCodeId);
        }

        [HttpPut("{qrCodeId}/{userId}")]
        public async Task<IActionResult> UpdateItemInfo(int qrCodeId, int userId, [FromBody] Item updatedItem)
        {
            var result = await _qrCodeService.UpdateItemInfoAsync(qrCodeId, userId, updatedItem);
            
            if (!result)
                return NotFound("QR Code not found or unauthorized");
            return Ok("Item information updated successfully");
        }

        [HttpDelete("{qrCodeId}/{userId}")]
        public async Task<IActionResult> DeleteItemInfo(int qrCodeId, int userId)
        {
            var result = await _qrCodeService.DeleteItemInfoAsync(qrCodeId, userId);
            
            if (!result)
                return NotFound("QR Code not found or unauthorized");
            return Ok("Item information deleted successfully");
        }
    }
}