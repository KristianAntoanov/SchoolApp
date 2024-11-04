﻿using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data
{
	public class DiaryService
	{
		private readonly IRepository<Class, int> _classRepository;
		private readonly IRepository<Student, int> _studentRepository;
		private readonly IRepository<Subject, int> _subjectRepository;

		public DiaryService(IRepository<Class, int> classRepository,
							IRepository<Student, int> studentRepository,
                            IRepository<Subject, int> subjectRepository)
		{
            _classRepository = classRepository;
            _studentRepository = studentRepository;
			_subjectRepository = subjectRepository;
        }

		public async Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses()
		{
			IEnumerable<DiaryIndexViewModel> diaries = await _classRepository
                .GetAllAttached()
				.OrderBy(c => c.GradeLevel)
				.ThenBy(c => c.SectionId)
				.Select(c => new DiaryIndexViewModel()
				{
					ClassId = c.Id,
					ClassName = $"{c.GradeLevel} {c.Section.Name}"
                })
				.ToArrayAsync();

			return diaries;
		}

        public async Task<IEnumerable<StudentsViewModel>> GetGradeContent(int classId, int subjectId)
        {
            IEnumerable<StudentsViewModel> students = await _studentRepository
                .GetAllAttached()
                .Where(s => s.ClassId == classId)
                .Select(s => new StudentsViewModel()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Grades = s.Grades
                                .Where(g => g.SubjectId == subjectId)
                                .Select(g => new GradeViewModel()
                                {
                                    GradeValue = g.GradeValue,
                                    GradeDate = g.AddedOn
                                })
                                .ToArray()
                })
                .ToArrayAsync();

            return students;
        }

        public async Task<IEnumerable<SubjectsViewModel>> GetClassContent(int classId)
        {
            IEnumerable<SubjectsViewModel> subjects = await _subjectRepository
                    .GetAllAttached()
                    .Select(s => new SubjectsViewModel()
                    {
                        Id = s.Id,
                        SubjectName = s.Name
                    })
                    .ToArrayAsync();

            return subjects;
        }

        public async Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId)
        {
            IEnumerable<StudentRemarksViewModel> studentRemarks = await _studentRepository
                    .GetAllAttached()
                    .Where(sr => sr.ClassId == classId)
                    .Select(s => new StudentRemarksViewModel()
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Remarks = s.Remarks
                                .Select(r => new RemarksViewModel()
                                {
                                    Id = r.Id,
                                    SubjectName = r.Subject.Name,
                                    TeacherName = $"{r.Teacher.FirstName} {r.Teacher.LastName}",
                                    RemarkText = r.RemarkText,
                                    AddedOn = r.AddedOn.ToString()
                                })
                    })
                    .ToArrayAsync();

            return studentRemarks;
        }

        public async Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId)
        {
            IEnumerable<StudentAbsencesViewModel> studentAbsences = await _studentRepository
                    .GetAllAttached()
                    .Where(sa => sa.ClassId == classId)
                    .Select(s => new StudentAbsencesViewModel()
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Absences = s.Absences
                                .Select(a => new AbsencesViewModel()
                                {
                                    SubjectName = a.Subject.Name,
                                    AddedOn = a.AddedOn.ToString(),
                                    IsExcused = a.IsExcused
                                })
                    })
                    .ToArrayAsync();

            return studentAbsences;
        }

    }
}