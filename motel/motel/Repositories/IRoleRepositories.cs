using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public interface IRoleRepositories
    {
        List<RoleDTO> GetlAllRole();
        RolesNoIdDTO GetRoleById(int id);
        AddRoleDTO AddRole(AddRoleDTO addRoleDTO);
        RolesNoIdDTO UpdateRoleById(int id, RolesNoIdDTO RoleNoIdDTO);
        Role? DeleteRoleById(int id);
    }
}
