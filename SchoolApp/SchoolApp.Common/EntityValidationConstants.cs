namespace SchoolApp.Common
{
	public static class EntityValidationConstants
	{
		public static class Absence
		{
            public const string AddedOnDateFormat = "dd-MM-yyyy";
        }

        public static class Grade
        {
            public const int GradeMinValue = 2;
            public const int GradeMaxValue = 6;
            public const string AddedOnDateFormat = "dd-MM-yyyy";
        }

        public static class Remark
        {
            public const int RemarkTextMinLength = 5;
            public const int RemarkTextMaxLength = 300;
            public const string AddedOnDateFormat = "dd-MM-yyyy";
        }

        public static class Section
        {
            public const int NameMinLength = 1;
            public const int NameMaxLength = 1;
        }

        public static class Student
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 50;
        }

        public static class Subject
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;
        }

        public static class Teacher
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;
            public const int ImageUrlMinLength = 7;
            public const int ImageUrlMaxLength = 2083;
            public const int JobTitleMinLength = 5;
            public const int JobTitleMaxLength = 150;
        }
    }
}