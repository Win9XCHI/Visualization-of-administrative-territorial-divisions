using Handler.Models.Authentication;

namespace Handler.Models.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        LogIn GetUser(LogIn User);
        void AddUser(Register User);
    }
}

