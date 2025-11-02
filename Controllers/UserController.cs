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
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] int pageNumber = 1, [FromQuery]  int pageSize = 5)
        {
            // su dung reposity pattern 
            var allUsers = _userRepositories.GetAllUser(filterOn, filterQuery,pageNumber, pageSize);
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
            if (!ValidateAddUser(addUserDTO))
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                var userAdd = _userRepositories.AddUser(addUserDTO);
                return Ok(userAdd);
            }
            return Ok(addUserDTO);
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
        [HttpPut("update-customer")]
        public IActionResult UpdateUserBasic(int id, [FromForm] UpdateCustomer updateCustomer)
        {
            var user = _userRepositories.updateCustomer(id, updateCustomer);
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
        #region Private methods
        private bool ValidateAddUser(AddUserDTO user)
        {
            if (user == null)
            {
                ModelState.AddModelError(nameof(user), "Vui lòng thêm thông tin.");
                return false;
            }

            if (string.IsNullOrEmpty(user.firstname))
            {
                ModelState.AddModelError(nameof(user.firstname), "Vui lòng không để trống Tên của bạn.");
            }
            if (string.IsNullOrEmpty(user.lastname))
            {
                ModelState.AddModelError(nameof(user.lastname), "Vui lòng không để trống Họ của bạn.");
            }
            if (user.birthday == null)
            {
                ModelState.AddModelError(nameof(user.birthday), "Vui lòng không để trống ngày sinh.");
            }
            if (user.tierId == null)
            {
                ModelState.AddModelError(nameof(user.tierId), "Vui lòng lựa chọn gói.");
            }
            //else
            //{
            //    // Thực hiện kiểm tra giá trị tierId có hợp lệ hay không
            //    // Ví dụ: nếu tierId không hợp lệ, thêm lỗi vào ModelState
            //    if (!IsValidTierId(user.tierId))
            //    {
            //        ModelState.AddModelError(nameof(user.tierId), "Gói không hợp lệ.");
            //    }
            //}
            if (user.roleId == null)
            {
                ModelState.AddModelError(nameof(user.roleId), "Vui lòng lựa chọn vai trò.");
            }
            //else
            //{
            //    // Thực hiện kiểm tra giá trị roleId có hợp lệ hay không
            //    // Ví dụ: nếu roleId không hợp lệ, thêm lỗi vào ModelState
            //    if (!IsValidRoleId(user.roleId))
            //    {
            //        ModelState.AddModelError(nameof(user.roleId), "Vai trò không hợp lệ.");
            //    }
            //}

            if (user.phone == null)
            {
                ModelState.AddModelError(nameof(user.phone), "Vui lòng không để trống số điện thoại.");
            }
            else if (_dbcontext.User.Any(u => u.phone == user.phone))
            {
                ModelState.AddModelError(nameof(user.phone), "Số điện thoại này đã được đăng ký bởi người dùng khác.");
            }
            if (user.phone == null)
            {
                ModelState.AddModelError(nameof(user.phone), " Số điện thoại đã được sử dụng cho một tài khoản nào đó vui lòng nhập số khác");
            }

            if (user.birthday == null)
            {
                ModelState.AddModelError(nameof(user.birthday), "Vui lòng không để trống ngày sinh.");
            }
            if (string.IsNullOrEmpty(user.address))
            {
                ModelState.AddModelError(nameof(user.address), "Vui lòng không để trống Địa chỉ.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        
        #endregion



    }
}
