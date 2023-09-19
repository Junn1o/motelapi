using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IUserRepositories
    {
        List<UserDTO> GetAllUser();
        UserNoIdDTO GetUserById(int id);
        AddUserDTO AddUser(AddUserDTO addUser);
        AddUserDTO? UpdateUserById(int id, AddUserDTO user);
        User? DeleteUserById(int id);
        User? Authenticate(string phone,string password);
    }
}
