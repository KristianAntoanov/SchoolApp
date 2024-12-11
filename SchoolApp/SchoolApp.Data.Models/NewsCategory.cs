using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Data.Models;

public enum NewsCategory
{
    [Display(Name = "Новина")]
    News,

    [Display(Name = "Постижение")]
    Achievement
}