using SchoolApp.Web.ViewModels.Home;

namespace SchoolApp.Services.Data.Contrancts;

public interface IContactService
{
    Task<bool> SubmitContactFormAsync(ContactFormModel model);
}