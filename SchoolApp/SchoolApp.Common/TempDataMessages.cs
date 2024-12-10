namespace SchoolApp.Common;

public static class TempDataMessages
{
    public const string TempDataSuccess = "SuccessMessage";
    public const string TempDataError = "ErrorMessage";

    public static class Gallery
    {
        // ControllerMessages
        public const string AlbumNotFound = "Албумът не беше намерен.";
        public const string InvalidData = "Невалидни данни.";
        public const string InvalidImage = "Моля, изберете валидна снимка.";
        public const string InvalidImageError = "Невалидна снимка.";
        public const string InvalidAlbum = "Невалиден албум.";
        public const string InvalidLoadGallery = "Грешка при зареждането на галерията";

        // ServiceMessages
        public const string AlbumExistsError = "Албум с това заглавие вече съществува.";
        public const string AlbumCreateSuccess = "Албумът беше създаден успешно.";
        public const string AlbumCreateError = "Възникна грешка при създаването на албума.";
        public const string InvalidAlbumId = "Невалидно ID на албум!";
        public const string ImageSizeError = "Файлът трябва да е по-малък от 2MB";
        public const string AllowedFormatsMessage = "Позволените формати са само JPG, JPEG и PNG";
        public const string ImageUploadError = "Възникна грешка при качването на снимката.";
        public const string ImageUploadSuccess = "Успешно качихте снимкa.";
        public const string InvalidImageId = "Невалидно ID на снимка!";
        public const string ImageNotFound = "Снимката не беше намерена!";
        public const string ImageDeleteBlobError = "Възникна грешка при изтриването на снимката от хранилището!";
        public const string ImageDeleteSuccess = "Снимката беше изтрита успешно!";
        public const string ImagesDeleteBlobError = "Възникна грешка при изтриването на снимките от хранилището!";
        public const string AlbumDeleteSuccess = "Албумът беше изтрит успешно!";
    }

    public static class Students
    {
        public const string DeleteSuccessMessage = "Студентът беше успешно изтрит.";
        public const string DeleteErrorMessage = "Възникна проблем при изтриването на студента.";
        public const string EditSuccessMessage = "Студентът беше успешно редактиран.";
        public const string EditErrorMessage = "Възникна грешка при редактирането на студента.";
        public const string GradeDeleteSuccessMessage = "Оценката беше успешно изтрита.";
        public const string GradeDeleteErrorMessage = "Възникна грешка при изтриване на оценката.";
        public const string AddSuccessMessage = "Студентът беше успешно добавен.";
        public const string AddErrorMessage = "Възникна грешка при добавянето на студента.";
    }

    public static class Teachers
    {
        // ControllerMessages
        public const string InvalidDataMessage = "Невалидни данни.";
        public const string AddSuccessMessage = "Учителят беше успешно добавен.";
        public const string NotFoundMessage = "Учителят не беше намерен.";
        public const string EditSuccessMessage = "Учителят беше успешно редактиран.";
        public const string DeleteSuccessMessage = "Учителя беше успешно изтрит.";
        public const string DeleteErrorMessage = "Възникна проблем при изтриването на учителя.";

        // ServiceMessages
        public const string ImageSizeErrorMessage = "Файлът трябва да е по-малък от 2MB";
        public const string AllowedFormatsMessage = "Позволените формати са само JPG, JPEG и PNG";
        public const string InvalidTeacherIdMessage = "Невалидно ID!";
        public const string TeacherNotFoundMessage = "Учителят не беше намерен!";
        public const string DeleteOldImageErrorMessage = "Възникна грешка при изтриването на старата снимка!";
        public const string AddImageRequiredMessage = "Моля добавете снимка!";
        public const string UpdateErrorMessage = "Възникна грешка при обновяването на учителя!";
    }

    public static class UserRoles
    {
        public const string UserNotFoundMessage = "Потребителят не беше намерен.";
        public const string TeacherLinkUpdateErrorMessage = "Възникна грешка при обновяването на връзката с учител.";
        public const string TeacherLinkUpdateSuccessMessage = "Успешно обновена връзка с учител.";
        public const string RolesUpdateSuccessMessage = "Успешно обновени роли.";
    }

    public static class Diary
    {
        public const string InvalidData = "Невалидни данни.";
        public const string GradesAddSuccess = "Успешно добавихте оценки.";
        public const string AbsencesAddSuccess = "Успешно добавихте отсъствия.";
        public const string RemarkAddSuccess = "Успешно добавихте забележка.";
        public const string AbsenceExcuseSuccess = "Успешно извинихте отсъствието.";
        public const string AbsenceDeleteSuccess = "Успешно изтрихте отсъствието.";
        public const string RemarkNotFound = "Забележката не е намерена.";
        public const string RemarkEditSuccess = "Успешно редактирахте забележката.";
        public const string RemarkDeleteSuccess = "Успешно изтрихте забележката.";
    }

    public static class Home
    {
        public const string MissingRequiredFields = "Моля, попълнете всички задължителни полета.";
        public const string SubmitSuccess = "Вашето съобщение беше изпратено успешно!";
        public const string SubmitError = "Възникна грешка при изпращане на съобщението. Моля, опитайте отново или се свъжете с администратор.";
    }

    public static class News
    {
        public const string InvalidIdMessage = "Невалидно ID на новина.";
        public const string NotFoundMessage = "Новината не беше намерена.";

        public const string ImageSizeError = "Снимката трябва да е по-малка от 2MB!";
        public const string AllowedFormatsMessage = "Позволените формати са само JPG, JPEG и PNG!";
        public const string ImageUploadError = "Възникна грешка при качването на снимката.";
        public const string NewsCreateSuccess = "Новината беше създадена успешно!";
        public const string ImageDeleteError = "Възникна грешка при изтриването на снимката от хранилището!";
        public const string NewsDeleteSuccess = "Новината беше изтрита успешно!";
        public const string NewsDeleteError = "Възникна грешка при изтриването на новината!";

        public const string AnnouncementNotFoundMessage = "Съобщението не беше намерено.";
        public const string AnnouncementCreateSuccess = "Съобщението беше създадено успешно!";
        public const string AnnouncementEditSuccess = "Съобщението беше редактирано успешно!";
        public const string AnnouncementDeleteSuccess = "Съобщението беше изтрито успешно!";

        public const string NewsEditSuccess = "Новината беше редактирана успешно!";
        public const string EditError = "Грешка при редактиране на новина с ID: {newsId}";
    }

}