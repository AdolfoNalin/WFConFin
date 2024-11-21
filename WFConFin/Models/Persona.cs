using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{
    public class Persona
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é necessária!")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "A quantidade minima de caracteris é de 3 e a quantidade máxima é 200")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O Telefone é obrigatório!")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "O minimo é de 10 caracteris e o máximo é 20 caracteris")]
        public string NumberPhone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A data de nas cimento é obrigatório!")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "O salário é obrigatório")]
        [Column(TypeName = "Decimal(18,2)")]
        public float Wage { get; set; }

        [Required(ErrorMessage = "O Genero é obrigatório")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "A quantidade minima é de 1 caracter e o máximo é é 20")]
        public string Gender { get; set; }

        public Guid CityId { get; set; }

        [JsonIgnore]
        private readonly DateTime Date;

        public Persona()
        {
            Id = Guid.NewGuid();
            BirthDate = Date.ToUniversalTime();
        }

        [JsonIgnore]
        public City? City { get; set; }
    }
}