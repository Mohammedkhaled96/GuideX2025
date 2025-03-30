namespace QRCodeManagement.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int user_id{ get; set; }
        public string name_EN{ get; set; }
        public string description_EN{ get; set; }
        public string color_EN{ get; set; }
        public string name_AR{ get; set; }
        public string description_AR{ get; set; }
        public string color_AR{ get; set; }
    }
}