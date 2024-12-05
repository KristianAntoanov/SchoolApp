namespace SchoolApp.Common
{
	public class ErrorMessages
	{
        //RequiredMessages
        public const string DateRequiredMessage = "Добавянето на дата е задължително.";
        public const string RemarkRequiredMessage = "Моля добавете забележка.";
        public const string StudentNameRequiredMessage = "Името е задължително";
        public const string ClassRequiredMessage = "Класът е задължителен";

        public const string TeacherNameRequiredMessage = "Името на учител е задължително";
        public const string TeacherJobTitleRequiredMessage = "Длъжността на учител е задължителнa";
        public const string TeacherImageRequiredMessage = "Моля изберете профилна снимка за учител";
        public const string TeacherSubjectRequiredMessage = "Моля изберете поне един предмет";

        public const string GalleryImageRequiredMessage = "Моля изберете снимка.";
        public const string GalleryTitleRequiredMessage = "Заглавието е задължително";
        public const string GalleryDescriptionRequiredMessage = "Описанието е задължително";

        public const string AnnouncementTitleRequiredMessage = "Заглавието е задължително";
        public const string AnnouncementContentRequiredMessage = "Съдържанието е задължително";

        public const string NewsTitleRequiredMessage = "Заглавието е задължително";
        public const string NewsContentRequiredMessage = "Съдържание е задължително";

        public const string ContactNameRequiredMessage = "Името е задължително";
        public const string ContactEmailRequiredMessage = "Името е задължително";
        public const string ContactSubjectRequiredMessage = "Името е задължително";
        public const string ContactMessageRequiredMessage = "Името е задължително";

        //AccountRequiredMessages
        public const string EmailRequiredMessage = "Имейла е задължителен.";
        public const string PasswordRequiredMessage = "Паролата е задължителна.";


        //StringLengthMessages
        public const string RemarkTextStringLengthMessage = "Забележката трябва да е между {2} и {1} символа";
        public const string NameStringLengthMessage = "Името трябва да е между {2} и {1} символа";
        public const string JobTitleStringLengthMessage = "Длъжността трябва да е между {2} и {1} символа";

        public const string DateAfterMessage = "Данните не могат да бъдат въведени за бъдещи дни";

        public const string GalleryTitleStringLengthMessage = "Заглавието трябва да е между {2} и {1} символа";
        public const string GalleryDescriptionStringLengthMessage = "Описанието трябва да е между {2} и {1} символа";

        public const string AnnouncementTitleStringLengthMessage = "Заглавието трябва да е между {2} и {1} символа";
        public const string AnnouncementContentStringLengthMessage = "Съдържанието трябва да е между {2} и {1} символа";

        public const string NewsTitleStringLengthMessage = "Заглавието трябва да е между {2} и {1} символа";
        public const string NewsContentStringLengthMessage = "Съдържание трябва да е между {2} и {1} символа";

        public const string ContactMessageStringLengthMessage = "Съобщението трябва да e между {2} и {1} символа";


        //AllowedExtensions
        public const string ImageAllowedExtensionJPG = ".jpg";
        public const string ImageAllowedExtensionJPEG = ".jpeg";
        public const string ImageAllowedExtensionPNG = ".png";

        //FileLengthMessages
        public const string ImageFileLengthMessage = "Файлът трябва да е не по-голям от 2MB";

        //AccountValidationMessages
        public const string ValidEmailMessage = "Моля, въведете валиден имейл адрес.";
        public const string PasswordStringLengthMessage = "Паролата трябва да бъде между {2} и {1} символа.";
        public const string PasswordNotMatchMessage = "Паролите не съвпадат.";
    }
}