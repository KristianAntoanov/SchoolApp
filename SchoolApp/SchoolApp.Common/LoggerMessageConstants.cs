namespace SchoolApp.Common
{
	public static class LoggerMessageConstants
	{
        public static class Gallery
        {
            public const string LoadAllError = "Грешка при зареждане на всички албуми";
            public const string CreateError = "Грешка при създаване на албум: {Title}";
            public const string LoadAlbumError = "Грешка при зареждане на албум с ID: {AlbumId}";
            public const string AddImageError = "Грешка при добавяне на изображение към албум: {AlbumId}";
            public const string DeleteImageError = "Грешка при изтриване на изображение: {ImageId} от албум: {AlbumId}";
            public const string DeleteAlbumError = "Грешка при изтриване на албум: {AlbumId}";
        }

        public static class News
        {
            public const string LoadAllError = "Грешка при зареждане на новините";
            public const string CreateError = "Грешка при добавяне на новина / постижение";
            public const string ImageProcessError = "Грешка при обработка на снимката за новина";
            public const string LoadDetailsError = "Грешка при зареждане на детайли за новина с ID: {id}";
            public const string DeleteError = "Грешка при изтриване на новина с ID: {id}";
            public const string LoadMessagesError = "Грешка при зареждане на важни съобщения";
            public const string AddMessageError = "Грешка при добавяне на съобщение";
            public const string EditMessageError = "Грешка при редактиране на съобщение: {id}";
            public const string DeleteMessageError = "Грешка при изтриване на съобщение с ID: {id}";
            public const string LoadAchievementsError = "Грешка при зареждане на постиженията";
            public const string EditNewsError = "Error occurred while editing news with ID: {id}";
        }

        public static class Team
        {
            public const string LoadAllError = "Грешка при зареждане на учителския екип";
        }

        public static class Diary
        {
            public const string LoadClassesError = "Грешка при зареждане на класовете";
            public const string LoadContentError = "Грешка при зареждане на съдържание";
            public const string AddGradesError = "Грешка при добавяне на оценки";
            public const string TeacherIdError = "Невалиден формат на учителско ID";
            public const string AddAbsenceError = "Грешка при добавяне на отсъствия";
            public const string AddRemarkError = "Грешка при добавяне на забележка";
            public const string ExcuseAbsenceError = "Грешка при извиняване на отсъствие с ID: {id}";
            public const string DeleteAbsenceError = "Грешка при изтриване на отсъствие с ID: {id}";
            public const string DeleteRemarkError = "Грешка при изтриване на забележка с ID: {id}";
            public const string EditRemarkError = "Грешка при редактиране на забележка с ID: {id}";
        }

        public static class Roles
        {
            public const string LoadAllError = "Грешка при зареждане на потребителите.";
            public const string UpdateTeacherError = "Грешка при обновяване на учител. Потребител ID: {UserId}, Учител ID: {TeacherId}";
            public const string UpdateRolesError = "Грешка при обновяване на роли. Потребител ID: {UserId}";
        }

        public static class Students
        {
            public const string LoadAllError = "Грешка при зареждане на студенти";
            public const string DeleteError = "Грешка при изтриване на студент с ID: {id}";
            public const string EditLoadError = "Грешка при зареждане на студент за редакция с ID: {id}";
            public const string EditError = "Грешка при редактиране на студент с ID: {id}";
            public const string LoadGradesError = "Грешка при зареждане на оценки за студент с ID: {id}";
            public const string DeleteGradeError = "Грешка при изтриване на оценка с ID: {gradeId} за студент с ID: {studentId}";
            public const string AddError = "Грешка при добавяне на нов студент";
        }

        public static class Teachers
        {
            public const string LoadAllError = "Грешка при зареждане на всички учители";
            public const string AddFormError = "Грешка при зареждане на формата за добавяне на учител";
            public const string AddError = "Грешка при добавяне на учител";
            public const string ImageError = "Грешка при обработка на снимката на учител";
            public const string EditLoadError = "Грешка при зареждане на учител за редакция с ID: {id}";
            public const string EditError = "Грешка при редактиране на учител с ID: {Id}";
            public const string DeleteError = "Грешка при изтриване на учител с ID: {id}";
        }

        public static class Home
        {
            public const string ContactFormError = "Грешка при изпращането на имейл през контактната форма";
            
        }
    }
}