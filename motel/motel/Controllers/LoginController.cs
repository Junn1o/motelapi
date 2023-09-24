using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using motel.Data;
using motel.Models.Domain;
using motel.Models.DTO;
using motel.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepositories _userRepository;

        public LoginController(IConfiguration configuration, IUserRepositories userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Kiểm tra thông tin đăng nhập
            var authenticatedUser = _userRepository.Authenticate(model.Phone, model.Password);

            if (authenticatedUser == null)
            {
                return Unauthorized(); // Trả về lỗi 401 Unauthorized nếu thông tin không hợp lệ.
            }

            // Tạo và phát sinh JWT token
            var token = GenerateJwtToken(authenticatedUser);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            // Tạo mã token ở đây sử dụng các thư viện phù hợp (ví dụ: System.IdentityModel.Tokens.Jwt)
            // Trong ví dụ này, chúng ta sẽ sử dụng System.IdentityModel.Tokens.Jwt để tạo token.

            // Cài đặt các thông tin cần thiết cho token payload (claims)
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.firstname),
            new Claim(ClaimTypes.Name, user.lastname),
            // Thêm các claims khác nếu cần
        };

            // Cấu hình và tạo token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

