using Handler.Models.UserPanel;
using System.Collections.Generic;

namespace Handler.Models.Repositories.Interfaces
{
    public interface IUserPanelRepository
    {
        List<User> GetUsers(User U);
        public void DeleteUser(int code);
        void UpdateUser(User U);
    }
}

