using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem inválida")]
        public string Base64Image { get; set; }
    }
}