using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
using motel.Repositories;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery]  int pageSize = 5)
        {
            // su dung reposity pattern 
            var allUsers = _userRepositories.GetAllUser(pageNumber,pageSize);
            return Ok(allUsers);
            
        }
        [HttpGet("get-role-users")]
        public IActionResult GetAllUserRole([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // su dung reposity pattern 
            var allUsers = _userRepositories.GetAllUserRole(pageNumber, pageSize);
            return Ok(allUsers);

        }
        [HttpPost("add-user")]
        public IActionResult AddUser([FromForm] AddUserDTO addUserDTO)
        {
            var userAdd = _userRepositories.AddUser(addUserDTO);
            return Ok();
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
        public IActionResult UpdateUser(int id,[FromForm] AddUserDTO updateUser)
        {
            var userUpdate = _userRepositories.UpdateUserById(id, updateUser);
            return Ok(userUpdate);
        }
        [HttpPut("update-userbasic")]
        public IActionResult UpdateUserBasic(int id, UpdateUserBasic updateUserBasic)
        {
            var user = _userRepositories.UpdateUser(id , updateUserBasic);
            if (user != null)
            {
                return Ok(user);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpPut("update-role-user")]
        public IActionResult EditUserRole(EditUserRoleDTO updateRole)
        {
            var user = _userRepositories.EditUserRole(updateRole);
            if (user != null)
            {
                return Ok(user);
            }
            else
                return NotFound("Data Empty");
        }
        [HttpDelete("delete-user-with-id")]
        public IActionResult DeleteUserwithId(int id)
        {
            var userDelete = _userRepositories.DeleteUserById(id);
            if (userDelete == null)
            {
                return Ok("User not delete or error");
            }
            else
            {
                return Ok("User deleted");
            }
        }
        

        

    }
}
