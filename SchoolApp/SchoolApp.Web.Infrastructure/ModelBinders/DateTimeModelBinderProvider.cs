using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SchoolApp.Web.Infrastructure.ModelBinders
{
    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        private readonly string _dateFormat;

        public DateTimeModelBinderProvider(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DateTime) ||
                context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeModelBinder(_dateFormat);
            }

            return null;
        }
    }
}