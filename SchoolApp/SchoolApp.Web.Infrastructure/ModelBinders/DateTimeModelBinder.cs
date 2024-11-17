using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SchoolApp.Web.Infrastructure.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        private readonly string _targetDateFormat;

        public DateTimeModelBinder(string targetDateFormat)
        {
            _targetDateFormat = targetDateFormat ?? throw new ArgumentNullException(nameof(targetDateFormat));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            var dateStr = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(dateStr))
            {
                return Task.CompletedTask;
            }

            // Първо парсваме входящата дата в ISO формат
            if (DateTime.TryParseExact(dateStr,
                "yyyy-MM-dd",  // Входящият формат
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                // След успешно парсване, форматираме датата в желания формат
                string formattedDate = parsedDate.ToString(_targetDateFormat);
                // Парсваме отново за да получим DateTime обект в правилния формат
                if (DateTime.TryParseExact(formattedDate,
                    _targetDateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime finalDate))
                {
                    bindingContext.Result = ModelBindingResult.Success(finalDate);
                }
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    "Невалиден формат на дата.");
            }

            return Task.CompletedTask;
        }
    }
}