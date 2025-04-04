using QRCodeManagement.API.Models;

namespace QRCodeManagement.API.DTOs
{
    public class QRCodeCreateDTO
    {
        public QRCode QRCode { get; set; }
        public int UserId { get; set; }
    }
}