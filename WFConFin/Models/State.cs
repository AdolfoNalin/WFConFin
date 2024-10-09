using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models
{
    public class State
    {
        [Key]
        [StringLength( 2, MinimumLength = 2, ErrorMessage = "O campo sigla deve ter 2 caracteris")]
        public string Sigla { get; set; }

        [Required(ErrorMessage = "O campo nome obrigatório")]
        [StringLength(200,MinimumLength = 3, ErrorMessage = "O campo nome deve ter no minimo 3 caracteris")]
        public string Name { get; set; }
    }
}
