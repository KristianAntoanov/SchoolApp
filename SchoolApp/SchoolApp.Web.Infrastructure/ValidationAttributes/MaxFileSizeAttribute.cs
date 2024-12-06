using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace SchoolApp.Web.Infrastructure.ValidationAttributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        IFormFile? file = value as IFormFile;
        if (file != null)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"Максималният размер на файла трябва да бъде {_maxFileSize / 1024 / 1024}MB");
            }
        }

        return ValidationResult.Success;
    }
}