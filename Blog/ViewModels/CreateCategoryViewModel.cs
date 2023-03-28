using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O slug da categoria é obrigatório")]
        public string Slug { get; set; }

    }
}