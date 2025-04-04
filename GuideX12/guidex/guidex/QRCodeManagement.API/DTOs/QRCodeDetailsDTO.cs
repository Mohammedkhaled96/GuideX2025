namespace QRCodeManagement.API.DTOs
{
    public class QRCodeDetailsDTO
    {
        public Models.QRCode QRCode { get; set; } = null!;
        public Models.Item Item { get; set; } = null!;
        public Models.User User { get; set; } = null!;
    }
}
