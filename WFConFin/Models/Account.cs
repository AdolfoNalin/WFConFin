﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WFConFin.Models
{
    public enum Situation { close, open }
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório!")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O limite minimo é de 3 caractéris")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo valor é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        public float Value { get; set; }

        [Required(ErrorMessage = "O campo Data Vencimento é obrigatório")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "O campo Situação é obrigatório")]
        [DataType(DataType.Date)]
        public Situation Situations { get; set; }

        [Required(ErrorMessage = "O Campo pessoa é obrigatório!")]
        public Guid PersonaId { get; set; }
        
        [JsonIgnore]
        public Persona? Persona { get; set; }

        public Account()
        {
            Id = Guid.NewGuid();
        }
    }
}
