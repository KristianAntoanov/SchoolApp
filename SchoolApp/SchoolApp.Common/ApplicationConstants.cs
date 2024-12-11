namespace SchoolApp.Common;

public class ApplicationConstants
{
    public const string DateFormat = "dd.MM.yyyy";

    public const string AzureNewsContainerName = "newsimages";
    public const string AzureGalleryContainerName = "galleryimages";
    public const string AzureTeacherContainerName = "teachersimages";

    public const int PasswordMinLenght = 6;
    public const int PasswordMaxLenght = 100;

    public const int ContactMessageMinLenght = 10;
    public const int ContactMessageMaxLenght = 300;

    public const int ReleaseYear = 2024;

    public const string AppName = "SchoolApp";

    public const string ParentRole = "Parent";
    public const string TeacherRole = "Teacher";
    public const string AdminRole = "Admin";


    public const string DefaultNewsImageUrl = "/img/default-news-image.jpeg";
}