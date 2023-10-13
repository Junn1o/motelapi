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
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserRepositories _userRepository;

        public LoginController(IConfiguration configuration, IUserRepositories userRepository,AppDbContext dbContext )
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var authenticatedUser = _userRepository.Authenticate(model.Phone, model.Password);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(authenticatedUser);

            // Trích xuất thông tin từ token
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);

            // Lấy thông tin từ claims
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userFirstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value;
            var userLastName = claimsPrincipal.FindFirst(ClaimTypes.Surname)?.Value;
            var userGender = claimsPrincipal.FindFirst(ClaimTypes.Gender)?.Value;
            var userRoleId = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;
            var userTiersId = claimsPrincipal.FindFirst(ClaimTypes.UserData)?.Value;
            var userAddress = claimsPrincipal.FindFirst(ClaimTypes.StreetAddress)?.Value;
            var userBirthday = claimsPrincipal.FindFirst(ClaimTypes.DateOfBirth)?.Value;
            var userAvatar = claimsPrincipal.FindFirst("profilePicture")?.Value;

            // Sử dụng thông tin như cần
            // ...

            return Ok(new
            {
                Token = token,
                UserId = userId,
                FirstName = userFirstName,
                LastName = userLastName,
                Gender = userGender,
                RoleId = userRoleId,
                TiersId = userTiersId,
                Address = userAddress,
                Birthday = userBirthday,
                Avatar = userAvatar,
            });
        }

        private string GenerateJwtToken(User user)
        {
            var userTier = _dbContext.Tier_User
                            .Include(tu => tu.tiers) // Đảm bảo bạn đã sử dụng Include để nạp thông tin tiers
                            .Where(tu => tu.userId == user.Id)
                            .FirstOrDefault();
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user.firstname), // Sử dụng GivenName thay vì Name cho tên
            new Claim(ClaimTypes.Surname, user.lastname),    // Sử dụng Surname thay vì Name cho họ
            new Claim(ClaimTypes.Gender, user.gender ? "Nam" : "Nữ"),
            new Claim(ClaimTypes.Role, user.roleId.ToString()),
            new Claim(ClaimTypes.UserData, userTier.tierId.ToString()    ?? string.Empty),
            new Claim(ClaimTypes.StreetAddress, user.address), // Sử dụng StreetAddress cho địa chỉ
            new Claim(ClaimTypes.DateOfBirth, user.birthday.ToString("yyyy-MM-dd")), // Sử dụng DateOfBirth cho ngày sinh, và định dạng ISO 8601
            new Claim("profilePicture", user.actualFile ?? string.Empty),
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
        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return new ClaimsPrincipal(tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out validatedToken));
        }
    }

}

