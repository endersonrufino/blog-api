using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail do usuário é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido")]
        public string Email { get; set; }
    }
}