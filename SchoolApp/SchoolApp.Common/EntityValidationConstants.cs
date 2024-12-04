namespace SchoolApp.Common
{
	public static class EntityValidationConstants
	{
        public static class Grade
        {
            public const int GradeMinValue = 0;
            public const int GradeMaxValue = 6;
        }

        public static class Remark
        {
            public const int RemarkTextMinLength = 5;
            public const int RemarkTextMaxLength = 300;
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

        public static class Album
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 500;
        }

        public static class GalleryImage
        {
            public const int ImageUrlMaxLength = 2083;
        }

        public static class News
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 500;

            public const int ImageUrlMaxLength = 2083;
        }

        public static class Announcement
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            public const int ContentMinLength = 5;
            public const int ContentMaxLength = 500;
        }
    }
}