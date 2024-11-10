using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Diary.New
{
	public class StudentRecordUpdateModel
    {
        public DateTime AddedOn { get; set; }

        public IList<StudentModel> Students { get; set; }
            = new List<StudentModel>();

        [Required]
        public int SubjectId { get; set; }

        public IEnumerable<SubjectModel> Subjects { get; set; }
            = new HashSet<SubjectModel>();
    }
}