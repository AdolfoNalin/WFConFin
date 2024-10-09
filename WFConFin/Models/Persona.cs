using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace WFConFin.Models
{
    public class Persona
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O limite minimo de caracteris para nome é 3")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O número de celular é obrigátório")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "O limite minimo de caracteris para número de celular é 3")]
        public string NumberPhone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateBirth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Wage { get; set; }

        [StringLength(20,MinimumLength = 1, ErrorMessage = "O campo Gênero é até 20 caracteris")]
        public string Gender { get; set; }

        public Guid? IdCity{ get; set; }

        public City City { get; set; }

        public Persona()
        {
            Id = Guid.NewGuid();
        }
    }
}
