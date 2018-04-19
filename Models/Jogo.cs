using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Jogo
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Time 1")]
        public int Time1 { get; set; }

        [DisplayName("Time 2")]
        public int Time2 { get; set; }

        [DisplayName("Data do jogo")]
        public DateTime Data { get; set; }

        [ForeignKey("Time1")]
        public virtual Time Time_1 { get; set; }

        [ForeignKey("Time2")]
        public virtual Time Time_2 { get; set; }

        public virtual ICollection<Gol> Gols { get; set; }
    }
}
