using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace WFConFin.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O limite minimo é 3 caracteris")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O limite minimo é 3 caracteris")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O limite minimo é 3 caracteris")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Função é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O limite minimo é 3 caracteris")]
        public string Function { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }
    }
}
