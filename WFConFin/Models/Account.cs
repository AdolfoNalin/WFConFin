using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{
    public enum Situation { Aberta, Paga }
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório!")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O limite minimo é de 3 caractéris")]
        public string Descriprion { get; set; }

        [Required(ErrorMessage = "O campo valor é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Velue { get; set; }

        [Required(ErrorMessage = "O campo Data Vencimento é obrigatório")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        [DataType(DataType.Date)]
        public Situation Situations { get; set; }

        [Required(ErrorMessage = "O pessoa é obrigatória!")]
        public Guid IdPersona { get; set; }

        [JsonIgnore]
        public Persona Persona { get; set; }

        public Account()
        {
            Id = Guid.NewGuid();
        }
    }
}
