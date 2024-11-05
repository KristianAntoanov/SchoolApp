using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Web.Controllers
{
	public class DiaryController : BaseController
	{
        private readonly DiaryService _service;

        public DiaryController(DiaryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<DiaryIndexViewModel> model = await _service
                .IndexGetAllClasses();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadClassAndContent(int classId)
        {
            IEnumerable<SubjectsViewModel> model = await _service
                .GetClassContent(classId);

            return PartialView("ContentView", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadGradeContent(int classId, int subjectId)
        {
            IEnumerable<StudentGradesViewModel> model = await _service
                .GetGradeContent(classId, subjectId);

            return PartialView("GradesView", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadRemarksContent(int classId)
        {
            IEnumerable<StudentRemarksViewModel> model = await _service
                .GetRemarksContent(classId);

            return PartialView("RemarksView", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadAbsencesContent(int classId)
        {
            IEnumerable<StudentAbsencesViewModel> model = await _service
                .GetAbsencesContent(classId);

            return PartialView("AbsencesView", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddGrades(int classId, int subjectId)
        {
            DiaryGradeAddViewModel model = await _service
                .GetClassNames(classId, subjectId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGrades(int classId, DiaryGradeAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            //await _service.AddGradesToStudents(classId, model)

            return View(model);
        }

    }
}