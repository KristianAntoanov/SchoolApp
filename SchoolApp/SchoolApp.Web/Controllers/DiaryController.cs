﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data.Models;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

namespace SchoolApp.Web.Controllers
{
	public class DiaryController : BaseController
	{
        private readonly IDiaryService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiaryController(IDiaryService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
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
            IEnumerable<SubjectViewModel> model = await _service
                .GetClassContent(classId);

            return PartialView("Content", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadGradeContent(int classId, int subjectId)
        {
            IEnumerable<StudentGradesViewModel> model = await _service
                .GetGradeContent(classId, subjectId);

            return PartialView("Grades", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadRemarksContent(int classId)
        {
            IEnumerable<StudentRemarksViewModel> model = await _service
                .GetRemarksContent(classId);

            return PartialView("Remarks", model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadAbsencesContent(int classId)
        {
            IEnumerable<StudentAbsencesViewModel> model = await _service
                .GetAbsencesContent(classId);

            return PartialView("Absences", model);
        }

        [HttpGet]
        public async Task<IActionResult> AddGrades(int classId, int subjectId)
        {
            GradeFormModel model = await _service
                .GetClassStudentForGrades<GradeFormModel>(classId, subjectId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGrades(GradeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = _service.GetSubjects();
                return View(model);
            }

            string userId = _userManager.GetUserId(User)!;
            if (String.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/Identity/Account/Login");
            }

            bool result = await _service.AddGrades(userId, model);

            if (!result)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), new
            {
                selectedSubjectId = model.SubjectId
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddAbsence(int classId, int subjectId)
        {
            AbsenceFormModel model = await _service
                .GetClassStudentForGrades<AbsenceFormModel>(classId, subjectId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAbsence(AbsenceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = _service.GetSubjects();
                return View(model);
            }

            bool result = await _service.AddAbsence(model);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddRemark(int classId, int subjectId)
        {
            RemarkFormModel model = await _service
                .GetClassStudentForGrades<RemarkFormModel>(classId, subjectId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRemark(RemarkFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = _service.GetSubjects();
                model.Students = _service.GetStudents(model);
                return View(model);
            }

            string userId = _userManager.GetUserId(User)!;
            if (String.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/Identity/Account/Login");
            }

            bool result = await _service.AddRemark(userId, model);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ExcuseAbsence(int id)
        {
            bool result = await _service.ExcuseAbsence(id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            bool result = await _service.DeleteAbsence(id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRemark(int id)
        {
            bool result = await _service.DeleteRemark(id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditRemark(int id)
        {
            EditRemarkViewModel? model = await _service.GetRemarkById(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRemark(EditRemarkViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await _service.EditRemark(model);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}