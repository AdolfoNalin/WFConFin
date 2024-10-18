using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{
    public class City
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo nome obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo nome deve ter no minimo 3 caracteris")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo estado teve ter 2 caracteris")]
        public string StateSigla { get; set; }

        public City()
        {
            Id = Guid.NewGuid();
        }

        [JsonIgnore]
        public State? State { get; set; }
    }
}
