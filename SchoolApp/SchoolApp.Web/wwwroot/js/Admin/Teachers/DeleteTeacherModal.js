

document.getElementById('deleteModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var teacherId = button.getAttribute('data-teacher-id');
    var teacherName = button.getAttribute('data-teacher-name');

    document.getElementById('teacherNameToDelete').textContent = teacherName;
    document.getElementById('deleteForm').action = 'Teachers/Delete/' + teacherId;
});