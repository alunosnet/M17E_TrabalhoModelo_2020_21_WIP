using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace M17E_TrabalhoModelo_2020_21_WIP.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteID { get; set; }

        [Required(ErrorMessage ="Tem de indicar o nome do cliente")]
        [StringLength(80)]
        [MinLength(3,ErrorMessage ="Nome muito pequeno. Deve ter pelo menos 3 letras")]
        [UIHint("Insira o nome, pelo menos 3 letras")]
        public string Nome { get; set; }

        public string Morada { get; set; }
        public string CP { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        //TODO: lista das estadias

    }
}