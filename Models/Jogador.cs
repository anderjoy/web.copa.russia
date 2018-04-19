using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Jogador
    {
        [Key]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Display(AutoGenerateField = false)]
        [ForeignKey("Time")]
        public int TimeId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Valor máximo de 100 caracteres")]
        [Display(AutoGenerateField = true, Description = "Nome")]
        public string Nome { get; set; }

        public virtual Time Time { get; set; }

        public virtual Ficha Ficha { get; set; }
    }
}
