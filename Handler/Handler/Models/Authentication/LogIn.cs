
namespace Handler.Models.Authentication
{
    public class LogIn
    {
        public int Code { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PIB { get; set; }
        public string Rights { get; set; }
    }
}