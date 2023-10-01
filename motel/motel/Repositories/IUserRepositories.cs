using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IUserRepositories
    {
        UserListResult GetAllUser(int pageNumber = 1, int pageSize = 1000);
        UserNoIdDTO GetUserById(int id);
        AddUserDTO AddUser(AddUserDTO addUser);
        AddUserDTO? UpdateUserById(int id, AddUserDTO user);
        User? DeleteUserById(int id);
        User? Authenticate(string phone,string password);
        UpdateUserBasic UpdateUser(int id, UpdateUserBasic updateUserBasic);
    }
}
