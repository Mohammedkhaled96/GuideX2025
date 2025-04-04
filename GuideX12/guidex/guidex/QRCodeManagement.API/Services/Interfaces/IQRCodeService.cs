using QRCodeManagement.API.DTOs;
using QRCodeManagement.API.Models;
using System.Threading.Tasks;

namespace QRCodeManagement.API.Services.Interfaces
{
    public interface IQRCodeService
    {
        Task<PaginatedResponse<QRCodeDetailsDTO>> GetQRCodesAsync(PaginationParameters paginationParams);
        Task<int> CreateQRCodeAsync(QRCode qrCode, int userId);
        Task<QRCodeDetailsDTO> GetQRCodeDetailsAsync(int qrCodeId, int userId);
        Task<bool> UpdateItemInfoAsync(int qrCodeId, int userId, Item updatedItem);
        Task<bool> DeleteItemInfoAsync(int qrCodeId, int userId);
    }
}