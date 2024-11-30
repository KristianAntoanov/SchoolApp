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
        public const string TeacherImageRequiredMessage = "Моля изберете снимка";
        public const string TeacherSubjectRequiredMessage = "Моля изберете поне един предмет";
        public const string GalleryImageRequiredMessage = "Моля изберете снимка.";
        public const string GalleryTitleRequiredMessage = "Заглавието е задължително";
        public const string GalleryDescriptionRequiredMessage = "Описанието е задължително";
        public const string AnnouncementTitleRequiredMessage = "Заглавието е задължително";
        public const string AnnouncementContentRequiredMessage = "Съдържанието е задължително";
        public const string NewsTitleRequiredMessage = "Заглавието е задължително";
        public const string NewsContentRequiredMessage = "Съдържание е задължително";


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
    }
}