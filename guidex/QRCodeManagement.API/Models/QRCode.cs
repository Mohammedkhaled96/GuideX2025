namespace QRCodeManagement.API.Models
{
    public class QRCode
    {
        public int Id { get; set; }
        public int user_id { get; set; }
        public int item_id{ get; set; }
        public string qr_Code { get; set; }
        public DateTime created_at{ get; set; }
    }
}