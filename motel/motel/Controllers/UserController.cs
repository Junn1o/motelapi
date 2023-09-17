using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using motel.Data;
using motel.Models.DTO;
using motel.Repositories;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly IUserRepositories _userRepositories;

        public UserController(AppDbContext context, IUserRepositories userRepositories)
        {
            _dbcontext = context;
            _userRepositories = userRepositories;
        }
        [HttpGet("get-all-users")]
        public IActionResult GetAll()
        {
            // su dung reposity pattern 
            var allUsers = _userRepositories.GetAllUser();
            return Ok(allUsers);
        }
        [HttpPost("add-user")]
        public IActionResult AddUser([FromForm] AddUserDTO addUser)
        {
            var userAdd = _userRepositories.AddUser(addUser);
            if (userAdd == null)
            {
                return null;
            }
            else
                return Ok(userAdd);
        }
        [HttpGet("get-user-with-id")]
        public IActionResult GetUserwithId(int id)
        {
            var user = _userRepositories.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpPut("update-user-with-id")]
        public IActionResult UpdateUser(int id, [FromForm] AddUserDTO updateUser)
        {
            var userUpdate = _userRepositories.UpdateUserById(id, updateUser);
            return Ok(userUpdate);
        }
        [HttpDelete("delete-user-with-id")]
        public IActionResult DeleteUserwithId(int id)
        {
            var userDelete = _userRepositories.DeleteUserById(id);
            if (userDelete == null)
            {
                return Ok("User already delete or error");
            }
            else
            {
                return Ok("User deleted");
            }
        }
    }
 }
