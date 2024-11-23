namespace SchoolApp.Common
{
	public class ErrorMessages
	{
        //RequiredMessages
        public const string DateRequiredMessage = "Добавянето на дата е задължително.";
        public const string RemarkRequiredMessage = "Моля добавете забележка.";
        public const string StudentNameRequiredMessage = "Името е задължително";
        public const string ClassRequiredMessage = "Класът е задължителен";


        //StringLengthMessages
        public const string RemarkTextStringLengthMessage = "Забележката трябва да е между {2} и {1} символа";
        public const string StudentNameStringLengthMessage = "Името трябва да е между {2} и {1} символа";

        public const string DateAfterMessage = "Данните не могат да бъдат въведени за бъдещи дни";
    }
}