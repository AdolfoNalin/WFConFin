using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "O Campo login não pode ser vázio")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "O campo login deve ter no minimo 3 caracteris e no máximo 50")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O Campo senha não pode ser vázio")]
        [StringLength(maximumLength: 8, MinimumLength = 4, ErrorMessage = "O campo senha deve ter no minimo 4 caracteris e no máximo 8")]
        public string Password { get; set; }
    }
}
