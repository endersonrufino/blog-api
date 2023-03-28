using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace blog.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<string>();

            foreach (var value in modelState.Values)
            {
                errors.AddRange(value.Errors.Select(x => x.ErrorMessage));                
            }

            return errors;
        }
    }
}