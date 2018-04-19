using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Ficha
    {
        [Display(AutoGenerateField = false)]
        [Key]
        [ForeignKey("Jogador")]
        public int JogadorId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Valor máximo de 100 caracteres")]        
        [Display(AutoGenerateField = false, Name = "Posição")]
        public string Posicao { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Valor máximo de 100 caracteres")]
        [Display(AutoGenerateField = false, Name = "Naturalidade")]
        public string Naturalidade { get; set; }

        [Required]
        [Display(AutoGenerateField = false, Name = "Altura")]
        //[RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Altura inválida")]
        [Range(0.30, 3.00, ErrorMessage = "Altura inválida")]
        public decimal Altura { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "Número da camisa inválida")]
        [Display(AutoGenerateField = false, Name = "Nr. Camisa")]
        public int Camisa { get; set; }

        public virtual Jogador Jogador { get; set; }
    }
}
