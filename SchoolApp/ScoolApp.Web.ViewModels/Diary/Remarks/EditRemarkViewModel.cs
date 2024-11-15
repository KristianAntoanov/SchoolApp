using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Diary.Remarks
{
	public class EditRemarkViewModel
	{
		public int Id { get; set; }

		public DateTime AddedOn { get; set; }

        public int SubjectId { get; set; }

        public int StudentId { get; set; }

        public string RemarkText { get; set; } = null!;
    }
}