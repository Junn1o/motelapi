using System.ComponentModel.DataAnnotations;

namespace motel.Models.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { set; get; }
    }
    public class LoginModel
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }

}
