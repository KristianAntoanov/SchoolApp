using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Grade;

namespace SchoolApp.Web.ViewModels
{
	public class StudentVewModel
	{
		public int Id { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public int Grade { get; set; }
	}
}