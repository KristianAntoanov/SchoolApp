

document.getElementById('deleteGradeModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var gradeId = button.getAttribute('data-grade-id');
    var gradeValue = button.getAttribute('data-grade-value');
    var subjectName = button.getAttribute('data-subject-name');

    document.getElementById('gradeToDelete').textContent = gradeValue;
    document.getElementById('subjectName').textContent = subjectName;
    document.getElementById('gradeIdToDelete').value = gradeId;
});