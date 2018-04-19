using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Gol
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Jogo")]
        public int JogoId { get; set; }

        [ForeignKey("Time")]
        public int TimeId { get; set; }

        [ForeignKey("Jogador")]
        public int JogadorId { get; set; }

        public System.TimeSpan Hora { get; set; }

        public virtual Jogo Jogo { get; set; }

        public virtual Time Time { get; set; }

        public virtual Jogador Jogador { get; set; }
    }
}
