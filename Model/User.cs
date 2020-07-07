using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimulandoRedeSocial.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 a 60 caracteres")]
        public string Password { get; set; }

#nullable enable
        public List<string>? UsersFriend = new List<string>();
#nullable enable
        public List<Friends>? Request = new List<Friends>();
#nullable enable
        public List<User>? Friend = new List<User>();
    }
}
