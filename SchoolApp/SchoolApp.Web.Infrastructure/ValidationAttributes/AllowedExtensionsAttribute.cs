using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SchoolApp.Web.Infrastructure.ValidationAttributes
{
	public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            if (extensions == null || extensions.Length == 0)
            {
                throw new ArgumentException("Трябва да подадете поне едно разрешено разширение", nameof(extensions));
            }

            _extensions = extensions.Select(e => e.ToLower().StartsWith(".") ? e.ToLower() : $".{e.ToLower()}").ToArray();
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"Позволените формати са: {string.Join(", ", _extensions)}");
                }
            }

            return ValidationResult.Success;
        }
    }
}