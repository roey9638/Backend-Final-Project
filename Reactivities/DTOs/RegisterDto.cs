using System.ComponentModel.DataAnnotations;

namespace Reactivities.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //The [RegularExpression] Means, That the [Password] has to Have. Continue Down VV
        //[1 UpperCaser] , [1 LowerCaser] , [1 Number] , [Its has to be As Long between {4,8} Characters]
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password Must Be Complex")]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
