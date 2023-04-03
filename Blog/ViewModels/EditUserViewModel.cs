using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class EditUserViewModel : CreateUserViewModel
    {
        [Required(ErrorMessage = "O id do usuário é obrigatório")]
        public int Id { get; set; }
    }
}