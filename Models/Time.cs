using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Time
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Valor máximo de 50 caracteres")]      
        [Display(AutoGenerateField = true, Name = "País")]
        public string Pais { get; set; }

        [Display(AutoGenerateField = true, Name = "Bandeira (.jpg|.png)")]
        [DataType(DataType.Upload)]
        public byte[] Bandeira { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Valor máximo de 100 caracteres")]
        [Display(AutoGenerateField = true, Name = "Técnico")]
        public string NMTecnico { get; set; }

        public virtual ICollection<Jogador> Jogadores { get; set; }

    }
}
