using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.Infrastructure.Extensions
{
	public class EnumExtensions
	{
        public static SelectList ToSelectList<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.GetType()
                        .GetMember(e.ToString())
                        .First()
                        .GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault()?.Name ?? e.ToString(),
                    Value = e.ToString()
                })
                .ToList();

            return new SelectList(values, "Value", "Text");
        }
    }
}