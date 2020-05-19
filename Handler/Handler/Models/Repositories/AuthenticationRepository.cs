using Handler.Models.Repositories.GenericRepository;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Authentication;
using System.Collections;

namespace Handler.Models.Repositories {
    public class AuthenticationRepository : DBRepository, IAuthenticationRepository
    {
        public AuthenticationRepository(string conn) : base(conn) { }

        public LogIn GetUser(LogIn User)
        {
            return SELECT<LogIn>("Code, PIB, Rights", "Input", "Login = '" + User.Login + "' AND Password = '" + User.Password + "'")[0];
        }

        public void AddUser(Register User)
        {
            ArrayList Columns = new ArrayList { "PIB", "Login", "Password", "Phone", "Birthday" };
            ArrayList Value = new ArrayList { "'" + User.PIB + "'", "'" + User.Login + "'", "'" + User.Password + "'", "'" + User.Phone + "'", "'" + User.Birthday + "'" };

            if (User.DiplomaF != null)
            {
                Columns.Add("Diploma");
                User.ConvertToByte();
                Value.Add(User.Diploma);
            }

            INSERT("Input", Columns, Value);
        }
    }
}
