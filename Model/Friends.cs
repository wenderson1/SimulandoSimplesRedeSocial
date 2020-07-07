using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimulandoRedeSocial.Model
{
    public class Friends
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Id Inválido")]
        public int IdUserSent { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Id Inválido")]
        public int IdUserReceived { get; set; }
        public bool? Accept { get; set; }

        public Friends(int id, int idUserSent, int idUserReceived, bool? accept)
        {
            Id = id;
            IdUserSent = idUserSent;
            IdUserReceived = idUserReceived;
            Accept = accept;
        }
    }
}
