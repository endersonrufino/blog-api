using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class EditCategoryViewModel : CreateCategoryViewModel
    {
        [Required(ErrorMessage = "O Id é obrigatório")]
        public int Id { get; set; }
    }
}