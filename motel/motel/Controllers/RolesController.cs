using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IRoleRepositories _roleRepository;

        public RolesController(AppDbContext dbContext, IRoleRepositories roleRepository)
        {
            _dbContext = dbContext;
            _roleRepository = roleRepository;
        }
        [HttpGet("get-all-role")]
        public IActionResult GetAllRole()
        {
            var allRoles = _roleRepository.GetlAllRole();
            return Ok(allRoles);
        }
        [HttpGet("get-role-id")]
        public IActionResult GetRoleById(int id)
        {
            var RoleWithId = _roleRepository.GetRoleById(id);
            return Ok(RoleWithId);
        }
        [HttpPost("add-role")]
        public IActionResult AddAuthors([FromForm] AddRoleDTO roleaddDTO)
        {
            var roleAdd = _roleRepository.AddRole(roleaddDTO);
            return Ok();
        }
        [HttpPut("update-role-id")]
        public IActionResult UpdateRoleById(int id, [FromForm] RolesNoIdDTO
       RoleDTO)
        {
            var roleUpdate = _roleRepository.UpdateRoleById(id, RoleDTO);
            return Ok(roleUpdate);
        }
        [HttpDelete("delete-role-id")]
        public IActionResult DeleteBookById(int id)
        {
            var roleDelete = _roleRepository.DeleteRoleById(id);
            return Ok();
        }
    }
}
