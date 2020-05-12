
namespace Handler.Models.UserPanel
{
    public class User
    {
        public int Code { get; set; }
        public string Rights { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PIB { get; set; }
        public string Phone { get; set; }
        public string Birthday { get; set; }
        public byte[] Diploma { get; set; }
    }
}
