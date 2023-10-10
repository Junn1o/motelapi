using Microsoft.AspNetCore.Mvc;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IUserRepositories
    {
        UserListResult GetAllUser(int pageNumber = 1, int pageSize = 1000);
        UserNoIdDTO GetUserById(int id);
        UserRoleResult GetAllUserRole(int pageNumber = 1, int pageSize = 10);
        EditUserRoleDTO EditUserRole(EditUserRoleDTO role);
        AddUserDTO AddUser(AddUserDTO addUser);
        AddUserDTO? UpdateUserById(int id, AddUserDTO user);
        User? DeleteUserById(int id);
        User? Authenticate(string phone,string password);
        UpdateUserBasic UpdateUser(int id, UpdateUserBasic updateUserBasic);
    }
}
