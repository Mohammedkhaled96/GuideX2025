using QRCodeManagement.API.DTOs;
using QRCodeManagement.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static QRCodeManagement.API.Services.QRCodeService;

namespace QRCodeManagement.API.Services.Interfaces
{
    public interface IQRCodeService
    {
        Task<PaginatedResult<QRCodeDetailsDTO>> GetQRCodesPagedAsync(int userId, int pageNumber, int pageSize);
        Task<List<QRCodeDetailsDTO>> GetAllQRCodesByUserIdAsync(int userId);
    }
}
