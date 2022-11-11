using System.ComponentModel.DataAnnotations;

namespace JanuszPOL.JanuszPOLBets.Repository.Account.Dto
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Username{ get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
