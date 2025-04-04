namespace QRCodeManagement.API.Models
{
    public class QRCode
    {
        public int Id { get; set; }
        public int ItemId{ get; set; }
        public string Code{ get; set; }
        public DateTime createdat{ get; set; }
    }
}