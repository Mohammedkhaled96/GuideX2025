namespace QRCodeManagement.API.Models
{
    public class Scan
    {
        public int Id { get; set; }
        public int user_id{ get; set; }
        public int qr_code_id{ get; set; }
        public DateTime scan_time{ get; set; }
    }
}