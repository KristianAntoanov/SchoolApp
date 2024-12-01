namespace SchoolApp.Web.ViewModels.Admin.Students
{
	public class PaginatedList<T>
	{
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public string? SearchTerm { get; set; }

        public IEnumerable<T> Items { get; set; }
            = new HashSet<T>();
    }
}