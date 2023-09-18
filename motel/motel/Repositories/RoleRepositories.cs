using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;

namespace motel.Repositories
{
    public class RoleRepositories : IRoleRepositories
    {
        private readonly AppDbContext _dbContext;
        public RoleRepositories(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public AddRoleDTO AddRole(AddRoleDTO addRoleDTO)
        {
            var RoleDomainModel = new Role
            {
                rolename = addRoleDTO.rolename,
            };
            //Use Domain Model to create Author
            _dbContext.Role.Add(RoleDomainModel);
            _dbContext.SaveChanges();
            return addRoleDTO;
        }

        public Role? DeleteRoleById(int id)
        {
            var RoleDomain = _dbContext.Role.FirstOrDefault(n => n.Id == id);
            if (RoleDomain != null)
            {
                _dbContext.Role.Remove(RoleDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }

        public List<RoleDTO> GetlAllRole()
        {
            var allRoles = _dbContext.Role.Select(Roles => new RoleDTO()
            {
                Id = Roles.Id,
                rolename = Roles.rolename,
                Users = Roles.user.Select(n => n.firstname + " " + n.lastname).ToList()
            }).ToList();
            return allRoles;
        }

        public RolesNoIdDTO GetRoleById(int id)
        {
            var RoleWithDomain = _dbContext.Role.Where(n => n.Id == id);
            var RoleWithIdDTO = RoleWithDomain.Select(Role => new RolesNoIdDTO()
            {
                rolename = Role.rolename,
                Users = Role.user.Select(n => n.firstname + " " + n.lastname).ToList()
            }).FirstOrDefault();
            return RoleWithIdDTO;
        }

        public RolesNoIdDTO UpdateRoleById(int id, RolesNoIdDTO RoleNoIdDTO)
        {
            var RoleDomain = _dbContext.Role.FirstOrDefault(n => n.Id == id);
            if (RoleDomain != null)
            {
                RoleDomain.rolename = RoleNoIdDTO.rolename;
                _dbContext.SaveChanges();
            }
            return RoleNoIdDTO;
        }
    }
}
