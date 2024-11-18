using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.Infrastructure.ValidationAttributes
{
    public class IsDateValidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue.Date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult("Невалиден формат!");
            }
            return ValidationResult.Success;
        }
    }
}