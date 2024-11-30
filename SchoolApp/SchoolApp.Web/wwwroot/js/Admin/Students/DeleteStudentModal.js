

document.getElementById('deleteModal').addEventListener('show.bs.modal', function (event) {
    var button = event.relatedTarget;
    var studentId = button.getAttribute('data-student-id');
    var studentName = button.getAttribute('data-student-name');

    document.getElementById('studentNameToDelete').textContent = studentName;
    document.getElementById('deleteForm').action = 'Students/Delete/' + studentId;
});