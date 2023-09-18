using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IRoleRepositories
    {
        List<RoleDTO> GetlAllRole();
        RolesNoIdDTO GetRoleById(int id);
        AddRoleDTO AddRole(AddRoleDTO addRoleDTO);
        AddRoleDTO UpdateRoleById(int id, AddRoleDTO RoleNoIdDTO);
        Role? DeleteRoleById(int id);
    }
}
